namespace Caravel.AspNetCore.Authentication
{
    public record AccessToken
    {
        public string Token { get; }
        public int ExpiresIn { get; }

        public AccessToken(string token, int expiresIn) => (Token, ExpiresIn) = (token, expiresIn);
    }
}