using Assets.Scripts.Assembly_CSharp.Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.Mod.ModHelper
{
    internal class Utils
    {
        private static Utils select;

        public static float lastWaitTime;

        public static bool isEatChicken = false;

        public static Utils gI()
        {
            if (select == null)
            {
                select = new Utils();
            }
            return select;
        }

        internal static int getIndexItemBag(int idItem)
        {
            Item[] bag = Char.myCharz().arrItemBag;
            for (int i = 0; i < bag.Length; i++)
            {
                if (bag[i] != null && bag[i].template.id == idItem)
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool CheckItem(int id)
        {
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                if (Char.myCharz().arrItemBag[i] != null && Char.myCharz().arrItemBag[i].template.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        internal static string getTextPopup(PopUp popUp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < popUp.says.Length; i++)
            {
                stringBuilder.Append(popUp.says[i]);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString().Trim();
        }

        internal Item FindItemBag(int id)
        {
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                if (Char.myCharz().arrItemBag[i] != null && Char.myCharz().arrItemBag[i].template.id == id)
                {
                    return Char.myCharz().arrItemBag[i];
                }
            }
            return null;
        }

        
        internal static bool EatChicken()
        {
            if (!isEatChicken || (TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23))
            {
                return false;
            }
            for (int i = 0; i < GameScr.vItemMap.size(); i++)
            {
                ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                if ((itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1) && itemMap.template.id == 74)
                {
                    Char.myCharz().itemFocus = itemMap;
                    if (mSystem.currentTimeMillis() - lastWaitTime > 0.6f)
                    {
                        lastWaitTime = Time.realtimeSinceStartup;
                        Service.gI().pickItem(Char.myCharz().itemFocus.itemMapID);
                    }
                    return true;
                }
            }
            return false;
        }

        internal static bool isUsingTDLT() =>
            ItemTime.isExistItem(4387);

        internal static double Distance(double x1, double y1, double x2, double y2) => System.Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

    }
}
