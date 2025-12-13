using System.Collections.Generic;
using UnityEditor.PackageManager;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    /// <summary>
    /// Class chứa các nhóm map để hiển thị khi sử dụng Xmap
    /// </summary>
    internal static class GroupMaps
    {
        public static readonly List<GroupMap> AllGroups = new List<GroupMap>();

        public static readonly GroupMap Xayda = new GroupMap( new string[] { "xayda" },
            new List<int> { 44, 23, 14, 15, 16, 17, 18, 20, 19, 35, 36, 37, 38, 26, 84, 52 }
        );

        public static GroupMap Namek = new GroupMap( new string[] { "namek" },
            new List<int> { 43, 22, 7, 8, 9, 11, 12, 13, 10, 31, 32, 33, 34, 25 }
        );

        public static readonly GroupMap Earth = new GroupMap( new string[] { "traidat" },
            new List<int> { 42, 21, 0, 1, 2, 3, 4, 5, 6, 27, 28, 29, 30, 47, 24 }
        );

        public static readonly GroupMap Nappa = new GroupMap( new string[] { "nappa" },
            new List<int> { 68, 69, 70, 71, 72, 64, 65, 63, 66, 67, 73, 74, 75, 76, 77, 81, 82, 83, 79, 80 }
        );

        public static readonly GroupMap Yardat = new GroupMap( new string[] { "yardat" },
            new List<int> { 131, 132, 133 }
        );

        public static readonly GroupMap Future = new GroupMap( new string[] { "tuonglai" },
            new List<int> { 102, 92, 93, 94, 96, 97, 98, 99, 100, 103 }
        );

        public static readonly GroupMap Cold = new GroupMap( new string[] { "cold" },
            new List<int> { 109, 108, 107, 110, 106, 105 }
        );

        public static readonly GroupMap Potaufeu = new GroupMap( new string[] { "potaufeu" },
            new List<int> { 139, 140 }
        );

        public static readonly GroupMap Barracks = new GroupMap( new string[] { "doanhtrai" },
            new List<int> { 53, 58, 59, 60, 61, 62, 55, 56, 54, 57 }
        );

        public static readonly GroupMap FiveElementsMountain = new GroupMap( new string[] { "nguhanhson" },
            new List<int> { 122, 123, 124 }
        );

        public const int ID_MAP_SIEU_THI = 84;
        public const int ID_MAP_TPVGT = 19;
        public const int ID_MAP_TO_COLD = 109;
        public const int ID_MAP_TO_SA_MAC_HOANG_VU = 181;

        static GroupMaps()
        {
            LoadAllGroups();
        }

        private static void LoadAllGroups()
        {
            AllGroups.Add(Xayda);
            AllGroups.Add(Namek);
            AllGroups.Add(Earth);
            AllGroups.Add(Nappa);
            AllGroups.Add(Yardat);
            AllGroups.Add(Future);
            AllGroups.Add(Cold);
            AllGroups.Add(Potaufeu);
            AllGroups.Add(Barracks);
            AllGroups.Add(FiveElementsMountain);
        }
    }
}