using System.Security.Cryptography;

namespace Caravel.AspNetCore.Authentication;

public sealed class TokenFactory : ITokenFactory
{
    public Task<string> GenerateToken(int size = 32)
    {
        var randomNumber = new byte[size];
        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);
        return Task.FromResult(Convert.ToBase64String(randomNumber));
    }
}