using System;

namespace NatureOfCodeTest
{
    public static class UserSession
    {
        public static int CurrentUserID { get; set; } = -1;
        public static string CurrentUsername { get; set; } = "Guest";
        
        public static bool IsLoggedIn => CurrentUserID > 0;
    }
}
