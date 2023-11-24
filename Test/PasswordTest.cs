using JobHunting.Services.Password;

namespace Test;

public class PasswordTest
{

    [Theory]
    [InlineData("default", "37a8eec1ce19687d132fe29051dca629d164e2c4958ba141d5f4133a33f0688f")]
    [InlineData("qwerty12345", "f6ee94ecb014f74f887b9dcc52daecf73ab3e3333320cadd98bcb59d895c52f5")]
    [InlineData("lololoshka", "7c91b7c7cdf82fa9b61cadd8f47ad2e5a931fa25debe43791d5cb72188ab1ad0")]
    public void CheckResult_SHA256EncryptionPassword(string password, string expected)
    {
        SHA256EncryptionPassword encryptionPassword = new SHA256EncryptionPassword();
        var result = encryptionPassword.Encryption(password);
        Assert.Equal(expected, result);
        Assert.IsType<string>(result);
    }

    [Theory]
    [InlineData("default", "c21f969b5f03d33d43e04f8f136e7682")]
    [InlineData("qwerty12345", "85064efb60a9601805dcea56ec5402f7")]
    [InlineData("lololoshka", "25231f9c2376aac401ddae60e9934694")]
    public void CheckResult_MD5EncryptionPassword(string password, string expected)
    {
        MD5EncryptionPassword encryptionPassword = new MD5EncryptionPassword();
        var result = encryptionPassword.Encryption(password);
        Assert.Equal(expected, result);
        Assert.IsType<string>(result);
    }

    [Theory]
    [InlineData("default", "1625cdb75d25d9f699fd2779f44095b6e320767f606f095eb7edab5581e9e3441adbb0d628832f7dc4574a77a382973ce22911b7e4df2a9d2c693826bbd125bc")]
    [InlineData("qwerty12345", "a4d4483fe1604a52d0b5ae1a717f7391f08e490afeb4e06e03dcc045eb175e17f491433eb7ca26796b0aedb58ac0dddc50fc30aa4c8ed52211dc1a71dc8de3af")]
    [InlineData("lololoshka", "00c103735a6d47c69106f84f2b27bf667bcc674053a23df199c055c697afd68a5c164b7805eeab65e220e65c4e0d24ab986dbc04f76dcb5ca0ce63d8cb2a8c32")]
    public void CheckResult_SHA512EncryptionPassword(string password, string expected)
    {
        SHA512EncryptionPassword encryptionPassword = new SHA512EncryptionPassword();
        var result = encryptionPassword.Encryption(password);
        Assert.Equal(expected, result);
        Assert.IsType<string>(result);
    }
}