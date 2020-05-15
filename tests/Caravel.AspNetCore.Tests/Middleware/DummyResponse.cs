namespace Caravel.AspNetCore.Tests.Middleware
{
    public class DummyResponse
    {
        public string Name { get; set; }

        public DummyResponse(string name)
        {
            Name = name;
        }
    }
}