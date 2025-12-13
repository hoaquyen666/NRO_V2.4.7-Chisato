using Assets.Scripts.Assembly_CSharp.Mod.ModHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    /// <summary>
    /// Tiện ích cho Xmap
    /// </summary>
    internal class UtilsMap
    {
        internal static int mapCapsuleReturn = -1;

        internal static readonly short ID_ITEM_CAPSULE_VIP = 194;

        internal static readonly short ID_ITEM_CAPSULE_NORMAL = 193;

        internal static readonly int ID_MAP_HOME_BASE = 21;

        internal static readonly int ID_MAP_LANG_BASE = 7;

        internal static readonly int ID_MAP_TTVT_BASE = 24;

        internal static int getIdMapHome(int cgender)
        {
            return ID_MAP_HOME_BASE + cgender;
        }

        internal static int getIdMapLang(int cgender)
        {
            return ID_MAP_LANG_BASE * cgender;
        }

        internal static bool hasItemCapsuleNormal()
        {
            if (Utils.getIndexItemBag(ID_ITEM_CAPSULE_NORMAL) != -1)
            {
                return true;
            }
            return false;
        }

        internal static bool hasItemCapsuleVip()
        {
            if (Utils.getIndexItemBag(ID_ITEM_CAPSULE_VIP) != -1)
            {
                return true;
            }
            return false;
        }

        public static bool isUseCapsuleNormal = false;
        public static void useCapsuleNormal()
        {
            if (isUseCapsuleNormal == true)
            {
                int capsule = 193;
                int indexNormal = Utils.getIndexItemBag(capsule);
                if (indexNormal != -1)
                {
                    Service.gI().useItem(0, 1, (sbyte)indexNormal, -1);
                    return;
                }
            }
        }

        public static void useCapsuleVip()
        {
            int capsule = 194;
            int indexVip = Utils.getIndexItemBag(capsule);
            if (indexVip != -1)
            {
                Service.gI().useItem(0, 1, (sbyte)indexVip, -1);
                return;
            }
        }

        internal static bool CanNextMap() => !Char.isLoadingMap && !Char.ischangingMap && !Controller.isStopReadMessage;


        internal static bool useCapsuleXmap()
        {
            if (Utils.getIndexItemBag(ID_ITEM_CAPSULE_VIP) != -1)
            {
                return true;
            }
            if (Utils.getIndexItemBag(ID_ITEM_CAPSULE_NORMAL) != -1)
            {
                return true;
            }
            return false;
        }

        private static string GetTextPopup(PopUp popUp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < popUp.says.Length; i++)
            {
                stringBuilder.Append(popUp.says[i]);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString().Trim();
        }

        internal static Waypoint FindWaypoint(int targetMapId)
        {
            Waypoint bestWp = null;
            string targetName = getMapName(targetMapId);

            // Lấy danh sách tất cả Waypoint trong map hiện tại
            var waypoints = new List<Waypoint>();
            try
            {
                for (int i = 0; i < TileMap.vGo.size(); i++)
                {
                    waypoints.Add((Waypoint)TileMap.vGo.elementAt(i));
                }
            }
            catch { }

            // --- CÁCH 1: Tìm theo tên (So sánh mềm) ---
            foreach (var wp in waypoints)
            {
                if (wp.popup != null && wp.popup.command != null)
                {
                    // FIX: Nối says[] thành string đầy đủ (sử dụng GetTextPopup đã có)
                    string wpName = GetTextPopup(wp.popup);

                    // Chỉ cần tên chứa từ khóa là nhận (Ví dụ: "Về làng Aru" chứa "làng Aru")
                    if (wpName.Equals(targetName) || targetName.Equals(wpName))
                    {
                        UnityEngine.Debug.Log($"[XMAP DEBUG] Match waypoint by name: '{wpName}' for target '{targetName}'");
                        return wp;  // Trả về ngay cái match đầu tiên
                    }
                }
            }

            if (bestWp != null)
            {
                string selectedName = GetTextPopup(bestWp.popup).ToLower().Trim();
                UnityEngine.Debug.Log($"[XMAP DEBUG] Fallback to position: Selected '{selectedName}' (ID guess for target {targetMapId})");
                return bestWp;
            }

            if (waypoints.Count > 0)
            {
                bestWp = waypoints[0];
                string selectedName = GetTextPopup(bestWp.popup).ToLower().Trim();
                UnityEngine.Debug.Log($"[XMAP DEBUG] Random fallback: Selected '{selectedName}'");
                return bestWp;
            }

            UnityEngine.Debug.Log($"[XMAP ERROR] No waypoint found for target {targetMapId}");
            return null;
        }

        internal static string getMapName(int mapId)
        {
            if (mapId >= 0 && mapId < TileMap.mapNames.Length)
            {
                return TileMap.mapNames[mapId];
            }
            return null;
        }

        internal string getMapName(PopUp popup)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string[] says = popup.says;
            string[] array = says;
            string[] array2 = array;
            string[] array3 = array2;
            foreach (string value in array3)
            {
                stringBuilder.Append(value).Append(' ');
            }
            return stringBuilder.ToString().Trim();
        }

        internal static int getMapIdFromName(string mapName)
        {
            int cgender = Char.myCharz().cgender;
            if (mapName.Equals(LocalizedStringsMap.goHome))
            {
                return ID_MAP_HOME_BASE + cgender;
            }
            if (mapName.Equals(LocalizedStringsMap.spaceshipStation))
            {
                return ID_MAP_TTVT_BASE + cgender;
            }
            if (LocalizedStringsMap.backTo.ContainsReversed(mapName))
            {
                mapName = LocalizedStringsMap.backTo.Replace(mapName, "");
                if (TileMap.mapNames[mapCapsuleReturn].Equals(mapName))
                {
                    return mapCapsuleReturn;
                }
                if (mapName == LocalizedStringsMap.stoneForest)
                {
                    return -1;
                }
            }
            for (int i = 0; i < TileMap.mapNames.Length; i++)
            {
                if (mapName.Equals(TileMap.mapNames[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        internal static void LoadLinkMapCapsule()
        {
            if (!ChisatoMap.CanUseCapsuleVip() && !ChisatoMap.CanUseCapsuleNormal())
                return;

            // Bắt buộc phải mở panel trước để lấy dữ liệu
            if (!GameCanvas.panel.isShow)
            {
                // Dùng item capsule để mở panel
                int itemIndex = Utils.getIndexItemBag(
                    ChisatoMap.CanUseCapsuleVip() ? UtilsMap.ID_ITEM_CAPSULE_VIP : UtilsMap.ID_ITEM_CAPSULE_NORMAL
                );
                if (itemIndex != -1)
                {
                    Service.gI().useItem(0, 1, (sbyte)itemIndex, -1);
                    Thread.Sleep(800); // Đợi panel hiện
                }
                else return;
            }

            if (GameCanvas.panel.mapNames == null || GameCanvas.panel.mapNames.Length == 0)
                return;

            int currentMap = TileMap.mapID;

            for (int i = 0; i < GameCanvas.panel.mapNames.Length; i++)
            {
                string mapName = GameCanvas.panel.mapNames[i];
                int mapId = getMapIdFromName(mapName);

                if (mapId != -1 && mapId != currentMap && mapId != 21 + Char.myCharz().cgender) // tránh về nhà
                {
                    // Thêm liên kết capsule
                    AlgorithmsMap.xmapData.links[currentMap].Add(
                        new MapNext(currentMap, mapId, TypeMapNext.Capsule, new int[] { i })
                    );
                }
            }

            GameCanvas.panel.hide(); // Đóng panel sau khi quét xong
        }

        internal static Waypoint findWaypoint(int idMap)
        {
            Waypoint waypoint;
            string textPopup;
            for (int i = 0; i < TileMap.vGo.size(); i++)
            {
                waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                textPopup = Utils.getTextPopup(waypoint.popup);
                if (textPopup.Equals(TileMap.mapNames[idMap]))
                    return waypoint;
            }
            return null;
        }

        internal static void ChangeMap(Waypoint waypoint)
        {
            if (waypoint != null)
            {
                ChisatoMap.TeleportMyChar((waypoint.maxX < 60 ? 15 :
                waypoint.minX > TileMap.pxw - 60 ? TileMap.pxw - 15 :
                waypoint.minX + ((waypoint.maxX - waypoint.minX) / 2)), waypoint.maxY);
                requestChangeMap(waypoint);
            }
        }

        internal static void requestChangeMap(Waypoint waypoint)
        {
            if (waypoint.isOffline)
            {
                Service.gI().getMapOffline();
            }
            else
            {
                Service.gI().requestChangeMap();
            }
        }

        internal static void addInfo(string info, int type)
        {
            GameScr.info1.addInfo(info, type);

            if (type == 0 && ControllersMap.isRunning)
            {
                foreach (string keyword in LocalizedStringsMap.StopKeywords)
                {
                    if (info.Contains(keyword))
                    {
                        GameScr.info1.addInfo("Xmap dừng: " + info, 0);
                        ControllersMap.finishXmap();
                        return;
                    }
                }
            }
        }
    }
}
