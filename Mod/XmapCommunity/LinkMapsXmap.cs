using System.Collections.Generic;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    /// <summary>
    /// Liên kết NPC Menu / Cổng dịch chuyển
    /// </summary>
    internal static class LinkMapsXmap
    {
        internal static readonly List<MapNext> All = new List<MapNext>();

        static LinkMapsXmap()
        {
            LoadAllNpcLinks();
        }

        private static void LoadAllNpcLinks()
        {
            // Làng Aru - Ngũ hành sơn
            All.Add(new MapNext(0, 122, TypeMapNext.NpcMenu, new[] { 49, 0 }));

            // Trái Đất - Namek
            All.Add(new MapNext(24, 25, TypeMapNext.NpcMenu, new[] { 10, 0 }));
            All.Add(new MapNext(25, 24, TypeMapNext.NpcMenu, new[] { 11, 0 }));

            // Trái Đất - Xayda
            All.Add(new MapNext(24, 26, TypeMapNext.NpcMenu, new[] { 10, 1 }));
            All.Add(new MapNext(26, 24, TypeMapNext.NpcMenu, new[] { 12, 1 }));

            // Namec - Xayda
            All.Add(new MapNext(25, 26, TypeMapNext.NpcMenu, new[] { 11, 1 }));
            All.Add(new MapNext(26, 25, TypeMapNext.NpcMenu, new[] { 12, 0 }));

            // Hành tinh - siêu thị
            All.Add(new MapNext(24, 84, TypeMapNext.NpcMenu, new int[] { 10, 2 }));
            All.Add(new MapNext(25, 84, TypeMapNext.NpcMenu, new int[] { 11, 2 }));
            All.Add(new MapNext(26, 84, TypeMapNext.NpcMenu, new int[] { 12, 2 }));

            // Thành phố Vegeta - Thành phố Santa
            All.Add(new MapNext(19, 126, TypeMapNext.NpcMenu, new int[] { 53, 0 }));
            All.Add(new MapNext(126, 19, TypeMapNext.NpcMenu, new[] { 53, 0 }));

            // Hành tinh Potaufeu - các hành tinh còn lại
            All.Add(new MapNext(24, 139, TypeMapNext.NpcMenu, new[] { 63, 0 }));
            All.Add(new MapNext(139, 24, TypeMapNext.NpcMenu, new[] { 63, 0 }));
            All.Add(new MapNext(139, 25, TypeMapNext.NpcMenu, new[] { 63, 1 })); // Namek
            All.Add(new MapNext(139, 26, TypeMapNext.NpcMenu, new[] { 63, 2 })); // Xayda

            // Rừng Bamboo - Tường thành 1
            All.Add(new MapNext(27, 53, TypeMapNext.NpcMenu, new[] { 25, 0 }));

            // Thần điện → Hành tinh Kaio
            All.Add(new MapNext(45, 48, TypeMapNext.NpcMenu, new[] { 19, 3 }));

            // Thánh địa Kaio → Hành tinh Kaio
            All.Add(new MapNext(50, 48, TypeMapNext.NpcMenu, new[] { 44, 0 }));
        }
    }
}