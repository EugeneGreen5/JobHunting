using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JobHunting.Helpers;

public class JWTOptions
{
    public const string ISSUER = "server";
    public const string AUDIENCE = "client";
    private const string KEY = "abccba!!_!!ABCCBA_!!"; 

    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
