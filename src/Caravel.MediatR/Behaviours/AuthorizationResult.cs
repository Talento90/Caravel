namespace Caravel.MediatR.Behaviours
{
    public class AuthorizationResult
    {
        public bool IsAuthorized { get; }
        public string FailureMessage { get; }
        
        private AuthorizationResult(bool isAuthorized, string failureMessage)
        {
            IsAuthorized = isAuthorized;
            FailureMessage = failureMessage;
        }

        public static AuthorizationResult Fail()
        {
            return new AuthorizationResult(false, null);
        }

        public static AuthorizationResult Fail(string failureMessage)
        {
            return new AuthorizationResult(false, failureMessage);
        }

        public static AuthorizationResult Succeed()
        {
            return new AuthorizationResult(true, null);
        }
    }
}