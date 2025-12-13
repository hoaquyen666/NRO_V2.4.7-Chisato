using Assets.Scripts.Assembly_CSharp.Mod.ModHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    /// <summary>
    /// Thực thi hành động
    /// </summary>
    internal class ChisatoMap
    {
        internal static bool isUseCapsuleNormal = false;
        internal static bool isUseCapsuleVip = false;
        internal static bool isXmapAStar = true;
        static bool isChangingMap;
        static bool isMovingMyChar;
        internal static int aStarTimeout = 60;

        public static bool isWalk = false;

        public static bool isTele = true;

        internal static bool IsActing = false;

        // Kiểm tra có thể dùng capsule không
        internal static bool CanUseCapsuleVip() => isUseCapsuleVip && UtilsMap.hasItemCapsuleVip();
        internal static bool CanUseCapsuleNormal() => isUseCapsuleNormal && UtilsMap.hasItemCapsuleNormal();

        internal static void NextMap(MapNext mapNext)
        {
            switch (mapNext.type)
            {
                case TypeMapNext.AutoWaypoint:
                    NextMapAutoWaypoint(mapNext);
                    break;
                case TypeMapNext.NpcMenu:
                    NextMapNpcMenu(mapNext);
                    break;
                case TypeMapNext.NpcPanel:
                    NextMapNpcPanel(mapNext);
                    break;
                case TypeMapNext.Position:
                    NextMapPosition(mapNext);
                    break;
                case TypeMapNext.Capsule:
                    NextMapCapsule(mapNext);
                    break;
            }
        }


        static System.Random random = new System.Random();

        /// <summary>
        /// Tập trung vào chuyển map tự động qua waypoint
        /// Tìm và kích hoạt waypoint để nhảy sang map mới
        /// </summary>
        /// <param name="mapNext"></param>
        internal static void NextMapAutoWaypoint(MapNext mapNext)
        {
            var waypoint = UtilsMap.findWaypoint(mapNext.to);
            ChangeMap(waypoint);
        }
        /// <summary>
        /// Di chuyển nhân vật đến vị trí của một NPC cụ thể trên map
        /// Sau đó mở menu của NPC và chọn một option để thực hiện hành động
        /// Đơn giản hơn NpcPanel
        /// </summary>
        /// <param name="mapNext"></param>
        internal static void NextMapNpcMenu(MapNext mapNext)
        {
            var npcId = mapNext.info[0];
            if (npcId == 38)
            {
                var flag = false;
                int vNpcSize = GameScr.vNpc.size();
                for (int i = 0; i < vNpcSize; i++)
                {
                    var npc = (Npc)GameScr.vNpc.elementAt(i);
                    if (npc.template.npcTemplateId == npcId)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    Waypoint waypoint;
                    if (TileMap.mapID == 27 || TileMap.mapID == 29)
                        waypoint = UtilsMap.findWaypoint(28);
                    else
                    {
                        if (random.Next(27, 29) == 27)
                            waypoint = UtilsMap.findWaypoint(27);
                        else
                            waypoint = UtilsMap.findWaypoint(29);
                    }

                    ChangeMap(waypoint);
                    return;
                }
            }
            Service.gI().openMenu(npcId);
            for (int i = 1; i < mapNext.info.Length; i++)
            {
                int select = mapNext.info[i];
                Service.gI().confirmMenu((short)npcId, (sbyte)select);
            }
            Char.chatPopup = null;
        }

        /// <summary>
        /// Di chuyển nhân vật đến vị trí của một NPC cụ thể trên map
        /// Sau đó mở menu của NPC và chọn một option để thực hiện hành động
        /// Phức tap hơn NpcMenu
        /// </summary>
        /// <param name="mapNext"></param>
        internal static void NextMapNpcPanel(MapNext mapNext)
        {
            var idNpc = mapNext.info[0];
            var selectMenu = mapNext.info[1];
            var selectPanel = mapNext.info[2];
            Service.gI().openMenu(idNpc);
            Service.gI().confirmMenu((short)idNpc, (sbyte)selectMenu);
            Service.gI().requestMapSelect(selectPanel);
        }

        /// <summary>
        /// Tập trung vào di chuyển nội bộ trong map hiện tại đến một tọa độ cụ thể
        ///  Không chuyển map, chỉ thay đổi vị trí nhân vật
        /// </summary>
        /// <param name="mapNext"></param>
        internal static void NextMapPosition(MapNext mapNext)
        {
            var xPos = mapNext.info[0];
            var yPos = mapNext.info[1];
            MoveMyChar(xPos, yPos);
            if (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, xPos, yPos) <= TileMap.size)
            {
                Service.gI().requestChangeMap();
                Service.gI().getMapOffline();
            }
        }

        internal static void NextMapCapsule(MapNext mapNext)
        {
            UtilsMap.mapCapsuleReturn = TileMap.mapID;
            var select = mapNext.info[0];
            Service.gI().requestMapSelect(select);
        }

        private static void MoveMyChar(int x, int y)
        {
            if (isXmapAStar)
            {
                if (isMovingMyChar)
                    return;
                isMovingMyChar = true;
                new Thread(() =>
                {
                    try
                    {
                        long startTime = mSystem.currentTimeMillis();
                        int size = TileMap.size;
                        Tile start = new Tile(Char.myCharz().cx / size, Char.myCharz().cy / size - 1);
                        Tile destination = new Tile(x / size, y / size);
                        var path = XmapAStar.FindPath(start, destination);
                        if (path.Count == 0)
                        {
                            ControllersMap.finishXmap();
                            GameScr.info1.addInfo("Không tìm được đường", 0);
                            isMovingMyChar = false;
                        }
                        while (path.Count > 0)
                        {
                            if (!ControllersMap.gI.IsActing)
                                break;
                            if (mSystem.currentTimeMillis() - startTime > aStarTimeout * 1000)
                            {
                                isMovingMyChar = false;
                                return;
                            }
                            var tile = path.Pop();
                            int sleep = 0;
                            int xEnd = tile.x * size;
                            int yEnd = tile.y * size;
                            Char.myCharz().currentMovePoint = new MovePoint(xEnd, yEnd);
                            while (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, xEnd, yEnd) > size * 2)
                            {
                                if (!ControllersMap.gI.IsActing)
                                    break;
                                if (mSystem.currentTimeMillis() - startTime > aStarTimeout * 1000)
                                {
                                    isMovingMyChar = false;
                                    return;
                                }
                                if (sleep % 500 == 0)
                                {
                                    if (sleep >= 2000)
                                    {
                                        xEnd = tile.x * size + random.Next(size / -2, size / 2);
                                        yEnd = tile.y * size + random.Next(size / -2, size / 2);
                                        sleep = 0;
                                    }
                                    if (Char.myCharz().currentMovePoint == null || Char.myCharz().currentMovePoint.xEnd != xEnd || Char.myCharz().currentMovePoint.yEnd != tile.y * size + yEnd)
                                        Char.myCharz().currentMovePoint = new MovePoint(xEnd, yEnd);
                                }
                                Thread.Sleep(100);
                                sleep += 100;
                            }
                        }
                        Thread.Sleep(500);
                    }
                    catch (Exception ex) { UnityEngine.Debug.LogException(ex); }
                    isMovingMyChar = false;
                })
                { IsBackground = true }.Start();
            }
            else
                TeleportMyChar(x, y);
        }
        internal static void TeleportMyChar(int x, int y)
        {
            if (TileMap.tileTypeAtPixel(x, y) == TileMap.T_SOLIDGROUND || TileMap.tileTypeAtPixel(x, y) == TileMap.T_TREE || TileMap.tileTypeAtPixel(x, y) == TileMap.T_WATERFALL)
            {
                x += random.Next(-12, 13);
                y += random.Next(-12, 13);
                UnityEngine.Debug.Log($"Né địa hình: Tele to {x},{y}");
            }

            Char.myCharz().currentMovePoint = null;
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();

            if (Utils.isUsingTDLT())
                return;

            Char.myCharz().cx = x;
            Char.myCharz().cy = y + 1;
            Service.gI().charMove();
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
        }

        static void ChangeMap(Waypoint waypoint)
        {
            var GetXInsideMap = waypoint.maxX < TileMap.size ? TileMap.size : waypoint.minX > TileMap.pxw - TileMap.size ? TileMap.pxw - TileMap.size : waypoint.minX + ((waypoint.maxX - waypoint.minX) / 2);
            if (isXmapAStar)
            {
                if (isChangingMap)
                    return;
                isChangingMap = true;
                new Thread(() =>
                {
                    try
                    {
                        int size = TileMap.size;
                        Tile start = new Tile(Char.myCharz().cx / size, Char.myCharz().cy / size - 1);
                        Tile destination = new Tile(GetXInsideMap / size, waypoint.minY / size);
                        var path = XmapAStar.FindPath(start, destination);
                        if (path.Count == 0)
                        {
                            ControllersMap.finishXmap();
                            GameScr.info1.addInfo("Không tìm được đường", 0);
                            isChangingMap = false;
                            return;
                        }
                        long startTime = mSystem.currentTimeMillis();
                        int mapId = TileMap.mapID;
                        while (path.Count > 0)
                        {
                            if (!ControllersMap.gI.IsActing)
                                break;
                            if (TileMap.mapID != mapId)
                                break;
                            if (mSystem.currentTimeMillis() - startTime > aStarTimeout * 1000)
                            {
                                UtilsMap.ChangeMap(waypoint);
                                isChangingMap = false;
                                return;
                            }
                            var tile = path.Pop();
                            int sleep = 0;
                            int xEnd = tile.x * size;
                            int yEnd = tile.y * size;
                            Char.myCharz().currentMovePoint = new MovePoint(xEnd, yEnd);
                            while (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, xEnd, yEnd) > size * 2)
                            {
                                if (!ControllersMap.gI.IsActing)
                                    break;
                                if (TileMap.mapID != mapId)
                                    break;
                                if (mSystem.currentTimeMillis() - startTime > aStarTimeout * 1000)
                                {
                                    UtilsMap.ChangeMap(waypoint);
                                    isChangingMap = false;
                                    return;
                                }
                                if (sleep % 500 == 0)
                                {
                                    if (sleep >= 1500)
                                    {
                                        int mutation;
                                        do
                                            mutation = random.Next(-2, 3);
                                        while (mutation == 0);
                                        xEnd = tile.x * size + mutation * size / 2;
                                        do
                                            mutation = random.Next(-2, 3);
                                        while (mutation == 0);
                                        yEnd = tile.y * size + mutation * size / 2;
                                    }
                                    if (sleep >= 5000)
                                    {
                                        start = new Tile(Char.myCharz().cx / size, Char.myCharz().cy / size - 1);
                                        path = XmapAStar.FindPath(start, destination);
                                        if (path.Count == 0)
                                        {
                                            ControllersMap.finishXmap();
                                            GameScr.info1.addInfo("Không tìm thấy đường", 0);
                                            isChangingMap = false;
                                            return;
                                        }
                                        sleep = 0;
                                        break;
                                    }
                                    if (Char.myCharz().currentMovePoint == null || Char.myCharz().currentMovePoint.xEnd != xEnd || Char.myCharz().currentMovePoint.yEnd != tile.y * size + yEnd)
                                        Char.myCharz().currentMovePoint = new MovePoint(xEnd, yEnd);
                                }
                                Thread.Sleep(100);
                                sleep += 100;
                            }
                        }
                        if (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, GetXInsideMap, waypoint.minY) <= size * 2)
                            waypoint.popup.command.performAction();
                        Thread.Sleep(500);
                        //Utils.requestChangeMap(waypoint);
                    }
                    catch (Exception ex) { UnityEngine.Debug.LogException(ex); }
                    isChangingMap = false;
                })
                { IsBackground = true }.Start();
            }
            else
                UtilsMap.ChangeMap(waypoint);
        }

    }
}