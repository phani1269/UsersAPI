using System.Diagnostics.CodeAnalysis;

namespace UsersAPI.Helper
{
    [ExcludeFromCodeCoverage]
    public static class Settings
    {
        public static List<string> Roles
        {
            get
            {
                return new List<string> { "it_admin", "executive_manager", "user" };
            }
        }
    }
}
