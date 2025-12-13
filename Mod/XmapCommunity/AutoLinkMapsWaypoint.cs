using System.Collections.Generic;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    /// <summary>
    /// Các liên kết Auto Waypoint
    /// </summary>
    internal static class AutoLinkMapsWaypoint
    {
        // Danh sách các nhóm map có thể đi lại tự do với nhau qua waypoint

        /// <summary>
        /// Danh sách tất cả các liên kết waypoint tự động: từ mapStart → mapTo
        /// info = null vì chỉ dùng AutoWaypoint
        /// </summary>
        public static readonly List<MapNext> Links = new List<MapNext>();

        static AutoLinkMapsWaypoint()
        {
            var earthChain = new List<int> { 42, 0, 1, 2, 3, 4, 5, 6 };
            CreateChainLinks(earthChain);
            Create(0, 21, true);
            Create(1, 47, true);
            Create(2, 24, true);
            Create(3, 27, true);
            Create(27, 28, true);
            Create(28, 29, true);
            Create(5, 29, true);
            Create(29, 30, true);

            var namekChain = new List<int> { 43, 7, 8, 9, 11, 12, 13, 10 };
            CreateChainLinks(namekChain);
            Create(7, 22, true);
            Create(9, 25, true);
            Create(11, 31, true);
            Create(31, 32, true);
            Create(32, 33, true);
            Create(13, 33, true);
            Create(33, 34, true);

            var xaydaChain = new List<int> { 44, 14, 15, 16, 17, 18, 20, 19 };
            CreateChainLinks(xaydaChain);
            Create(44, 52, true);
            Create(14, 23, true);
            Create(16, 26, true);
            Create(17, 35, true);
            Create(35, 36, true);
            Create(36, 37, true);
            Create(20, 37, true);
            Create(37, 38, true);

            var nappaChain = new List<int> { 68, 69, 70, 71, 72, 64, 65, 63, 66, 67, 73, 74, 75, 76, 77, 81, 82, 83, 79, 80 };
            CreateChainLinks(nappaChain);

            var yardatChain = new List<int> { 131, 132, 133 };
            CreateChainLinks(yardatChain);

            var futureChain = new List<int> { 102, 92, 93, 94, 96, 97, 98, 99, 100, 103 };
            CreateChainLinks(futureChain);

            var coldChain = new List<int> { 106, 109, 108 };
            CreateChainLinks(coldChain);
            Create(106, 110, true);
            Create(110, 107, true);
            Create(106, 107, true);
            Create(109, 105, true);
            Create(108, 105, true);
            Create(108, 107, true);
            // Create(105, 80, true); TaskId > 30 mới mở

            var plantplanetChain = new List<int> { 160, 161, 162, 163 };
            CreateChainLinks(plantplanetChain);

            var potaufeuChain = new List<int> { 139, 140 };
            CreateChainLinks(potaufeuChain);

            var barracksChain = new List<int> { 53, 58, 59, 60, 61, 62, 55, 56, 54, 57 };
            CreateChainLinks(barracksChain);

            var fiveElementsMountainChain = new List<int> { 122, 123, 124 };
            CreateChainLinks(fiveElementsMountainChain);
        }

        public static MapNext Create(int from, int to, bool bidirectional = false)
        {
            var link = new MapNext(from, to, TypeMapNext.AutoWaypoint, null);

            if (bidirectional)
            {
                // Tạo liên kết ngược lại
                var reverseLink = new MapNext(to, from, TypeMapNext.AutoWaypoint, null);

                // Thêm cả 2 nếu chưa tồn tại
                if (!Links.Exists(l => l.mapStart == link.mapStart && l.to == link.to))
                    Links.Add(link);

                if (!Links.Exists(l => l.mapStart == reverseLink.mapStart && l.to == reverseLink.to))
                    Links.Add(reverseLink);
            }
            else
            {
                if (!Links.Exists(l => l.mapStart == link.mapStart && l.to == link.to))
                    Links.Add(link);
            }

            return link;
        }

        private static void CreateChainLinks(List<int> chain)
        {
            for (int i = 0; i < chain.Count - 1; i++)
            {
                Create(chain[i], chain[i + 1], true);
            }
        }
    }
}
