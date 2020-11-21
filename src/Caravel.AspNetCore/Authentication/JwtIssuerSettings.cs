namespace Caravel.AspNetCore.Authentication
{
    public sealed record JwtIssuerSettings
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpirationInMinutes { get; set; }
        public string IssuerSigningKey { get; set; } = null!;

        public JwtIssuerSettings()
        {
        }

        public JwtIssuerSettings(string issuer, string audience, string issuerSigningKey, int expirationInMinutes) =>
            (Issuer, Audience, ExpirationInMinutes, IssuerSigningKey) =
            (issuer, audience, expirationInMinutes, issuerSigningKey);
    }
}