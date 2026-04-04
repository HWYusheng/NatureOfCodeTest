using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    public static class UserSession
    {
        public static int CurrentUserID { get; set; } = -1;
        public static string CurrentUsername { get; set; } = "Guest";
        
        public static bool IsLoggedIn => CurrentUserID > 0;
    }
}
