using Assets.Scripts.Assembly_CSharp.Mod.ModHelper;
using Assets.Scripts.Assembly_CSharp.Mod.Xmap;

namespace Assets.Scripts.Assembly_CSharp.Mod.HotAction
{
    internal class KeyHandle
    {
        [Command("c", CommandType.Key, false)]
        public static void UseCapsule()
        {
            int capsuleVip = 194;
            int capsule = 193;

            int indexVip = Utils.getIndexItemBag(capsuleVip);
            if (indexVip != -1)
            {
                Service.gI().useItem(0, 1, (sbyte)indexVip, -1);
                return;
            }

            int indexNormal = Utils.getIndexItemBag(capsule);
            if (indexNormal != -1)
            {
                Service.gI().useItem(0, 1, (sbyte)indexNormal, -1);
                return;
            }
        }

        [Command("x", CommandType.Key, false)]
        public static void OpenMenuXmap()
        {
            //MenuMap.showMenuXmap();
        }

        [Command("m", CommandType.Key, false)]
        public static void OpenChangeZonePanel()
        {
            Service.gI().openUIZone();
        }

        [Command("f", CommandType.Key, false)]
        public static void UsePotara()
        {
            int btc3 = 1884;
            int btc2 = 921;
            int btc1 = 454;

            int index3 = Utils.getIndexItemBag(btc3);
            if (index3 != -1)
            {
                Service.gI().useItem(0, 1, (sbyte)index3, -1);
                return;
            }

            int index2 = Utils.getIndexItemBag(btc2);
            if (index2 != -1)
            {
                Service.gI().useItem(0, 1, (sbyte)index2, -1);
                return;
            }

            int index1 = Utils.getIndexItemBag(btc1);
            if (index1 != -1)
            {
                Service.gI().useItem(0, 1, (sbyte)index1, -1);
                return;
            }
        }
    }
}