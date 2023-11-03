/*using JobHunting.Services.Password;
using Microsoft.AspNetCore.Mvc;

namespace JobHunting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasswordController
{
    private readonly IEnumerable<IPassword> _listMethods;
    public PasswordController(IEnumerable<IPassword> listMethods)
    {
        _listMethods = listMethods;
    }

    [HttpGet]
    public string RandomGeneratePassword()
    {
        string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
        Random random = new Random();
        int length = 15;
        char[] chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = validChars[random.Next(0, validChars.Length)];
        }

*//*        List<string> strings = new List<string>();
        foreach (var elem in _listMethods)
        {
            strings.Add(elem.Encryption("123"));
        }
        return strings;*//*


        //return _listMethods.ToList()[new Random().Next(0,3)].Encryption("321");
        return _listMethods.ToList()[new Random().Next(0, 3)].Encryption(new string(chars));
    }
      
}
*/