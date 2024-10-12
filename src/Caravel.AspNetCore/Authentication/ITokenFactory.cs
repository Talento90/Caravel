namespace Caravel.AspNetCore.Authentication;

public interface ITokenFactory
{
    Task<string> GenerateToken(int size = 32);
}