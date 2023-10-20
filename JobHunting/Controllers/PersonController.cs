using JobHunting.Models;
using JobHunting.Models.DTO;
using JobHunting.Models.Entity;
using JobHunting.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<IEnumerable<Person>>> GetAllPersonsAsync() =>
        Ok(await _personService.GetAllPersonsAsync());

    /// <summary>
    /// Метод для получения пользователя по id
    /// </summary>
    /// <param name="id">Guid пользователя</param>
    /// <returns code="404">Пользователь не найден</returns>
    /// <returns code="200">Возвращает пользователя</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPersonByIdAsync(Guid id)
    {
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
    public async Task<ActionResult> DeletePersonByIdAsync(Guid id)
    {
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
    public async Task<ActionResult> UpdatePersonByIdAsync(Guid id, PersonDTO newPerson)
    {
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
    public async Task<ActionResult> AddResumeAsync(Guid id, Resume resume)
    {
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
    public async Task<ActionResult> DeleteResumeAsync(Guid idPerson, Guid idResume)
    {
        var result = await _resumeService.DeleteResumeAsync(idPerson, idResume);

        if (result.Code == 404 ) return NotFound(result.Information);

        return NoContent();
    }

}
