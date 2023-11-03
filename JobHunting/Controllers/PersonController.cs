using JobHunting.Helpers;
using JobHunting.Models.DTO;
using JobHunting.Models.Entity;
using JobHunting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace JobHunting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IResumeService _resumeService;

    public PersonController(IPersonService personService, IResumeService resumeService)
    {
        _personService = personService;
        _resumeService = resumeService;
    }

    /// <summary>
    /// Метод для получения всех пользователей
    /// </summary>
    /// <returns code="200">Возвращает список пользователей</returns>
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<IEnumerable<Person>>> GetAllPersonsAsync() =>
        Ok(await _personService.GetAllPersonsAsync());

    /// <summary>
    /// Метод для получения пользователя по id
    /// </summary>
    /// <param name="id">Guid пользователя</param>
    /// <returns code="404">Пользователь не найден</returns>
    /// <returns code="200">Возвращает пользователя</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "admin, user")]
    public async Task<ActionResult<Person>> GetPersonByIdAsync(Guid id)
    {
        if (TakeGuidFromToken(HttpContext.User) != id) return Forbid();
        var result = await _personService.GetPersonByIdAsync(id);
        if (result.Item1.Code == 200) return Ok(result.Item2);
        return NotFound(result.Item1.Information);
    }

    /// <summary>
    /// Метод для добавления пользователя
    /// </summary>
    /// <param name="newPerson">Объект пользователя, которого нужно добавить</param>
    /// <returns code="422">Пришёл бракованный объект</returns>
    /// <returns code="201">Пользователь успешно добавлен</returns>
    [HttpPost]
    public async Task<ActionResult> AddPersonAsync(Person newPerson)
    {
        if (newPerson is null) return UnprocessableEntity();

        await _personService.AddPersonAsync(newPerson);
        return Created($"/person/{newPerson.Id}", newPerson);
    }

    /// <summary>
    /// Метод для удаления пользователя
    /// </summary>
    /// <param name="id">Guid пользователя</param>
    /// <returns code="404">Пользователь не найден</returns>
    /// <returns code="204">Пользователь удалён</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin, user")]
    public async Task<ActionResult> DeletePersonByIdAsync(Guid id)
    {
        if (TakeGuidFromToken(HttpContext.User) != id) return Forbid();
        var result = await _personService.DeletePersonByIdAsync(id);
        if (result.Code == 404) return BadRequest(result.Information);
        return NoContent();
    }

    /// <summary>
    /// Метод для обновления основных полей у пользователя
    /// </summary>
    /// <param name="id">Guid пользователя</param>
    /// <param name="newPerson">DTO для обновления данных</param>
    /// <returns code="404">Пользователь не найден</returns>
    /// <returns code="200">Пользователь обновлён</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "admin, user")]
    public async Task<ActionResult> UpdatePersonByIdAsync(Guid id, PersonDTO newPerson)
    {
        if (TakeGuidFromToken(HttpContext.User) != id) return Forbid();
        var result = await _personService.UpdatePersonByIdAsync(id, newPerson);

        if (result.Code == 404) return NotFound(result.Information);
        return Ok();
    }

    /// <summary>
    /// Метод для добавления резюме пользователю
    /// </summary>
    /// <param name="id">Guid пользователя</param>
    /// <param name="resume">Новое резюме</param>
    /// <returns code="404">Пользователь не найден</returns>
    /// <returns code="200">Резюме добавлено</returns>
    [HttpPatch("{id}")]
    [Authorize(Roles = "admin, user")]
    public async Task<ActionResult> AddResumeAsync(Guid id, Resume resume)
    {
        if (TakeGuidFromToken(HttpContext.User) != id) return Forbid();
        var result = await _resumeService.AddResumeAsync(id, resume);
        if (result.Code == 404) return NotFound(result.Information);

        return Ok(result.Information);
    }

    /// <summary>
    /// Метод для удаления резюме у пользователя
    /// </summary>
    /// <param name="idPerson">Guid пользователя</param>
    /// <param name="idResume">Guid резюме</param>
    /// <returns code="404">Пользователь не найден</returns>
    /// <returns code="204">Резюме удалёно</returns>
    [HttpDelete("{idPerson}/{idResume}")]
    [Authorize(Roles = "admin, user")]
    public async Task<ActionResult> DeleteResumeAsync(Guid idPerson, Guid idResume)
    {
        if (TakeGuidFromToken(HttpContext.User) != idPerson) return Forbid();
        var result = await _resumeService.DeleteResumeAsync(idPerson, idResume);

        if (result.Code == 404) return NotFound(result.Information);

        return NoContent();
    }

    [HttpPost("auth")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<string>> Auth(String name, string password)
    {
        var person = (await _personService.GetPersonForAuth(name, password));
        if (person is null) return Unauthorized();

        var claims = new List<Claim>{
            new Claim(ClaimsIdentity.DefaultNameClaimType, person.Id.ToString()),
            new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
        };

        var jwt = new JwtSecurityToken(
            issuer: JWTOptions.ISSUER,
            audience: JWTOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(
                JWTOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
         );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        var response = new
        {
            token = encodedJwt,
            email = person.Email
        };

        return Ok(response);
    }

    private Guid TakeGuidFromToken(ClaimsPrincipal user)
    {
        var identity = user.Identity as ClaimsIdentity;
        return new Guid(identity.Name);
    }
}
