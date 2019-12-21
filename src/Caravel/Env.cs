using System;

namespace Caravel
{
    public static class Env
    {
        public static bool IsLocalEnvironment()
        {
            return GetEnv() == "Local";
        }

        public static bool IsDevelopmentEnvironment()
        {
            return GetEnv() == "Development";
        }

        public static bool IsTestEnvironment()
        {
            return GetEnv() == "Test";
        }

        public static bool IsSandboxEnvironment()
        {
            return GetEnv() == "Sandbox";
        }

        public static bool IsProductionEnvironment()
        {
            return GetEnv() == "Production";
        }

        public static string GetEnv()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
        }

        public static string GetAppVersion()
        {
            return Environment.GetEnvironmentVariable("VERSION") ?? string.Empty;
        }
    }
}