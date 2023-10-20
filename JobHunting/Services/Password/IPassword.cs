namespace JobHunting.Services.Password;

public interface IPassword
{
    string Encryption(string password);
}
