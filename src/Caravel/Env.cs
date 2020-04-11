using System;

namespace Caravel
{
    /// <summary>
    /// Utility class to manage Environment variables.
    /// </summary>
    public static class Env
    {
        /// <summary>
        /// Check if the environment is "Local"
        /// </summary>
        /// <returns>True if is "Local</returns>
        public static bool IsLocalEnvironment()
        {
            return GetEnv() == "Local";
        }

        /// <summary>
        /// Check if the environment is "Development"
        /// </summary>
        /// <returns>True if is "Development</returns>
        public static bool IsDevelopmentEnvironment()
        {
            return GetEnv() == "Development";
        }

        /// <summary>
        /// Check if the environment is "Test"
        /// </summary>
        /// <returns>True if is "Test</returns>
        public static bool IsTestEnvironment()
        {
            return GetEnv() == "Test";
        }

        /// <summary>
        /// Check if the environment is "Sandbox"
        /// </summary>
        /// <returns>True if is "Sandbox</returns>
        public static bool IsSandboxEnvironment()
        {
            return GetEnv() == "Sandbox";
        }

        /// <summary>
        /// Check if the environment is "Production"
        /// </summary>
        /// <returns>True if is "Production</returns>
        public static bool IsProductionEnvironment()
        {
            return GetEnv() == "Production";
        }

        /// <summary>
        /// Get the value of the environment variable "ASPNETCORE_ENVIRONMENT"
        /// </summary>
        /// <returns>Return the environment or "Local" if not present.</returns>
        public static string GetEnv()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
        }

        /// <summary>
        /// Get the value of the environment variable "VERSION"
        /// </summary>
        /// <returns>Return the application version or string.Empty</returns>
        public static string GetAppVersion()
        {
            return Environment.GetEnvironmentVariable("VERSION") ?? string.Empty;
        }
    }
}