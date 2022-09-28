using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHole.Services
{
    public static class Constants
    {
        public const string PATH_PREFIX = "whitehole";
        public const string PATH_LAST_KEY = "[last]";
        public const string PATH_OBJ_COUNT_KEY = "[objcount]";
        public const string PATH_LAST_OBJ = "obj";
        public const string PATH_LAST_ID = "id";
        public static readonly string[] OBJECT_IGNORED_KEYS = new string[] { "id" };

        public const int OBJECT_1 = 1;
        public const int OBJECT_2 = 2;
        public const int OBJECT_3 = 3;

        public const int PATH_TYPE_OBJECT_1 = 1;
        public const int PATH_TYPE_OBJECT_1_ID = 1;
        public const int PATH_TYPE_OBJECT_2 = 1;
        public const int PATH_TYPE_OBJECT_2_ID = 1;
    }
}
