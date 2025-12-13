using Assets.Scripts.Assembly_CSharp.Mod.ModHelper;
using Assets.Scripts.Assembly_CSharp.Mod.ModHelper.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    /// <summary>
    /// Bộ điều khiển trung tâm (Engine)
    /// </summary>
    class ControllersMap : ModHelper.Thread.ThreadActionUpdate<ControllersMap>
    {
        internal override int Interval => 100;

        private static int mapEnd;
        private static List<MapNext> way;
        private static int indexWay = 0;
        public static bool isRunning = false;
        static bool isNextMapFailed;

        internal static void start(int mapId)
        {
            if (gI.IsActing)
            {
                finishXmap();
                UnityEngine.Debug.Log($"[xmap][info] Hủy xmap tới {TileMap.mapNames[mapEnd]} để thực hiện xmap mới");
            }
            mapEnd = mapId;
            gI.toggle(true);
            UnityEngine.Debug.Log($"[xmap][info] Bắt đầu xmap tới {TileMap.mapNames[mapEnd]}");
        }

        /*private static int restartCount = 0;
        private static int lastRestartMap = -1;
        private static void RestartCurrentPath()
        {
            // Tránh restart liên tục tại cùng 1 map
            if (lastRestartMap == TileMap.mapID)
            {
                restartCount++;
                if (restartCount > 3)
                {
                    UnityEngine.Debug.Log("Restart quá nhiều lần tại map này. Dừng Xmap.");
                    Finish();
                    return;
                }
            }
            else
            {
                restartCount = 0;
                lastRestartMap = TileMap.mapID;
            }

            // Tìm lại đường từ map hiện tại đến đích
            AlgorithmsMap.xmapData = new DatasMap();
            AlgorithmsMap.xmapData.Load();

            List<MapNext> newWay = AlgorithmsMap.FindWay(TileMap.mapID, mapEnd);

            if (newWay != null && newWay.Count > 0)
            {
                way = newWay;
                indexWay = 0;

                // DEBUG: In ra đường đi mới
                string pathStr = "";
                foreach (var step in newWay)
                {
                    pathStr += $"{step.mapStart}→{step.to}, ";
                }
                UnityEngine.Debug.Log($"Đường mới: {pathStr}");
            }
            else
            {
                UnityEngine.Debug.Log("Không tìm được đường đi. Dừng Xmap.");
                Finish();
            }
        }*/

        protected override void update()
        {
            UnityEngine.Debug.Log($"[xmap][dbg] update {mapEnd}");

            if (way == null)
            {
                // CRITICAL: Khởi tạo xmapData NẾU chưa có
                if (AlgorithmsMap.xmapData == null)
                {
                    AlgorithmsMap.xmapData = new DatasMap();
                }

                if (!isNextMapFailed)
                {
                    string mapName = TileMap.mapNames[mapEnd];
                    MainThreadDispatcher.Dispatch(() =>
                        GameScr.info1.addInfo("Đi đến: " + mapName, 0));
                }

                UnityEngine.Debug.Log($"[xmap][dbg] Đang tạo dữ liệu map");

                // Gọi Load trên main thread
                bool loadSuccess = false;
                MainThreadDispatcher.Dispatch(() =>
                {
                    try
                    {
                        AlgorithmsMap.xmapData.Load();
                        loadSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.Log($"[xmap][err] Lỗi Load(): {ex}");
                    }
                });

                // Đợi load xong
                int timeout = 0;
                while (!AlgorithmsMap.xmapData.isLoaded && timeout < 50)
                {
                    Thread.Sleep(100);
                    timeout++;
                }

                if (!loadSuccess || !AlgorithmsMap.xmapData.isLoaded)
                {
                    UnityEngine.Debug.Log($"[xmap][err] Không thể load dữ liệu map");
                    finishXmap();
                    return;
                }

                AlgorithmsMap.xmapData.LoadLinkMapCapsule();

                try
                {
                    way = AlgorithmsMap.FindWay(TileMap.mapID, mapEnd);

                    if (way == null || way.Count == 0)
                    {
                        UnityEngine.Debug.Log($"[xmap][err] Không tìm thấy đường từ {TileMap.mapID} đến {mapEnd}");
                        MainThreadDispatcher.Dispatch(() =>
                            GameScr.info1.addInfo("Không tìm thấy đường!", 0));
                        finishXmap();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log($"[xmap][err] Lỗi tìm đường đi\n{ex}");
                    finishXmap();
                    return;
                }
                indexWay = 0;
            }

            // Tạo bản sao local để tránh race condition
            var currentWay = way;

            if (currentWay == null || currentWay.Count == 0)
            {
                UnityEngine.Debug.Log($"[xmap][err] Way is null or empty");
                finishXmap();
                return;
            }

            int lastIndex = currentWay.Count - 1;
            if (lastIndex < 0 || lastIndex >= currentWay.Count)
            {
                UnityEngine.Debug.Log($"[xmap][err] Invalid index: {lastIndex}");
                finishXmap();
                return;
            }

            // Chỉ check 1 lần duy nhất, xóa code duplicate
            if (TileMap.mapID == currentWay[lastIndex].to && !Char.myCharz().IsCharDead())
            {
                MainThreadDispatcher.Dispatch(() =>
                    GameScr.info1.addInfo("Đã đến nơi!", 0));
                finishXmap();
                return;
            }

            if (TileMap.mapID == currentWay[indexWay].mapStart)
            {
                if (Char.myCharz().IsCharDead())
                {
                    Service.gI().returnTownFromDead();
                    isNextMapFailed = true;
                    way = null;
                }
                else if (UtilsMap.CanNextMap())
                {
                    MainThreadDispatcher.Dispatch(() =>
                        ChisatoMap.NextMap(currentWay[indexWay]));
                    UnityEngine.Debug.Log($"[xmap][dbg] nextMap: {currentWay[indexWay].to}");
                }
                Thread.Sleep(500);
                return;
            }
            else if (TileMap.mapID == currentWay[indexWay].to)
            {
                indexWay++;
                return;
            }
            else
            {
                UnityEngine.Debug.Log($"[xmap][warn] Map hiện tại ({TileMap.mapID}) không khớp với đường đi (cần: {currentWay[indexWay].mapStart})");
                isNextMapFailed = true;
                way = null;
            }
        }

        internal static void finishXmap()
        {
            isRunning = false;
            way = null;
            indexWay = 0;
            gI.toggle(false);
            UtilsMap.addInfo("Đã đến nơi hoặc hủy Xmap!", 0);
        }
    }
}