using Assets.Scripts.Assembly_CSharp.Mod.Menu;
using Assets.Scripts.Assembly_CSharp.Mod.ModHelper;
using Assets.Scripts.Assembly_CSharp.Mod.Train;
using Assets.Scripts.Assembly_CSharp.Mod.Xmap;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.Mod.HotAction
{
    internal class ChatHandle
    {
        [Command("tsall", CommandType.Chat)]
        public static void tsall()
        {
            try
            {
                AutoTrain.gI().isAutoFind = !AutoTrain.gI().isAutoFind;
                Char.myCharz().addInfo("Find mob: " + (AutoTrain.gI().isAutoFind ? "Bật" : "Tắt"));
                if (AutoTrain.gI().isAutoFind)
                {
                    new Thread(new ThreadStart(AutoTrain.gI().GoFindAttackMob)).Start();
                }
            }
            catch { }
        }

        [Command("k", true)] // true = IsPrefix
        public static void ChangeZone(string args)
        {
            try
            {
                if (string.IsNullOrEmpty(args))
                {
                    GameScr.info1.addInfo("Chưa nhập khu!", 0);
                    return;
                }

                int zoneId = int.Parse(args);
                Service.gI().requestChangeZone(zoneId, -1);
            }
            catch { }
        }

        [Command("as")]
        public static void FindWayAStar()
        {
        }

        [Command("xmap")]
        public static void OpenMenuXmap()
        {
            MenuMap.showMenuXmap();
        }

        [Command("test")]
        public static void testcreateMenu()
        {
            new MenuBuilder().setChatPopup("test")
            .addItem("test 1", new MenuAction(() =>
            {
                GameScr.info1.addInfo("test 1", 0);
            }))
            .addItem("test 2", new MenuAction(() =>
            {
                GameScr.info1.addInfo("test 2", 0);
            }))
            .addItem("test 3", new MenuAction(() =>
            {
                GameScr.info1.addInfo("test 3", 0);
            }))
            .start();
        }

        [Command("checkcolor")]
        public static void checkColor()
        {
            GameScr.info1.addInfo("|0| màu 0\n|1| màu 1\n|2| màu 2\n|3| màu 3\n|4| màu 4\n|5| màu 5\n|6| màu 6\n|7| màu 7", 0);
        }


        [Command("checkmap")]
        public static void CheckMap()
        {
            try
            {
                string filePath = "DebugData\\Log_Map_Waypoints.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("--------------------------------------------------");
                    writer.WriteLine($"Thời gian: {System.DateTime.Now}");
                    writer.WriteLine($"Map hiện tại: [{TileMap.mapID}] {TileMap.mapNames[TileMap.mapID]}");

                    if (TileMap.vGo == null || TileMap.vGo.size() == 0)
                    {
                        writer.WriteLine(" -> Map này không có Waypoint.");
                        GameScr.info1.addInfo("Map này không có Waypoint!", 0);
                    }
                    else
                    {
                        for (int i = 0; i < TileMap.vGo.size(); i++)
                        {
                            Waypoint wp = (Waypoint)TileMap.vGo.elementAt(i);

                            string wpName = "Unknown";
                            if (wp.popup != null && wp.popup.says != null)
                            {
                                wpName = string.Join(" ", wp.popup.says).Trim();
                            }

                            int destinationMapID = -1;
                            for (int m = 0; m < TileMap.mapNames.Length; m++)
                            {
                                if (TileMap.mapNames[m].Equals(wpName, System.StringComparison.OrdinalIgnoreCase))
                                {
                                    destinationMapID = m;
                                    break;
                                }
                            }

                            int x = (wp.minX + wp.maxX) / 2;
                            int y = wp.maxY;

                            string infoID = destinationMapID != -1 ? destinationMapID.ToString() : "???";
                            writer.WriteLine($" -> Cổng {i}: [{wpName}] -> Map ID: {infoID} | Tọa độ ({x}, {y})");
                        }
                        GameScr.info1.addInfo($"Đã xuất {TileMap.vGo.size()} cổng ra file log!", 0);
                    }
                    writer.WriteLine("--------------------------------------------------");
                }
            }
            catch (System.Exception ex)
            {
                GameScr.info1.addInfo("Lỗi: " + ex.Message, 0);
            }
            return;
        }
    }
}