namespace Caravel.AspNetCore.Authentication
{
    public class AccessToken
    {
        public string Token { get; }
        public int ExpiresIn { get; }
        
        public AccessToken(string token, int expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}