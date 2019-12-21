namespace Caravel.Exceptions
{
    public static class Errors
    {
        // Generic errors
        public static readonly Error Error = new Error(10000, "An error has occured", Severity.High);

        // Cancellation errors
        public static readonly Error OperationWasCancelled = new Error(20000, "Operation was cancelled");

        // Not found errors
        public static readonly Error NotFound = new Error(30000, "Not found");

        // Conflict errors
        public static readonly Error Conflict = new Error(40000, "Conflict");

        // Validation errors
        public static readonly Error Validation = new Error(50000, "Validation error");
        public static readonly Error FieldsValidation = new Error(50001, "Validation fields error");
        public static readonly Error InvalidOperation = new Error(50002, "Invalid operation error");

        // Unauthorized errors
        public static readonly Error Unauthorized = new Error(60000, "Unauthorized");

        // Permission errors
        public static readonly Error Permission = new Error(70000, "No permission");
    }
}