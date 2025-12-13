using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    /// <summary>
    /// Thuật toán tìm đường
    /// </summary>
    internal class AlgorithmsMap
    {
        internal static DatasMap xmapData;

        /*internal static List<MapNext> FindWay(int mapStart, int mapEnd)
        {
            UnityEngine.Debug.Log($"[xmap] Bắt đầu tìm đường từ {mapStart} tới {mapEnd}");
            if (xmapData == null)
            {
                throw new Exception("xmapData is null");
            }

            

            int length = xmapData.links.Length;
            var prev = new MapNext?[length];
            var visited = new bool[length];
            var dist = new int[length];
            for (int i = 0; i < length; i++)
                dist[i] = int.MaxValue;
            dist[mapStart] = 0;

            for (int _ = 0; _ < length; _++)
            {
                var cmap = -1;
                for (int i = 0; i < length; i++)
                    if (!visited[i] && (cmap == -1 || dist[i] < dist[cmap]))
                        cmap = i;

                if (cmap == -1 || dist[cmap] == int.MaxValue)
                    break;

                var neighbors = xmapData.links[cmap];
                var count = neighbors.Count;
                for (int i = 0; i < count; i++)
                {
                    var mapNext = neighbors[i];
                    var cost = 1;
                    if (mapNext.type == TypeMapNext.NpcMenu && mapNext.info[0] == 38)
                        cost = 100;

                    int tentative = dist[cmap] + cost;
                    if (tentative < dist[mapNext.to])
                    {
                        dist[mapNext.to] = tentative;
                        prev[mapNext.to] = mapNext;
                    }
                }

                visited[cmap] = true;
            }

            var way = new List<MapNext>();
            var index = mapEnd;
            while (index != mapStart)
            {
                way.Add(prev[index]);
                index = prev[index].mapStart;
            }
            way.Reverse();

            if (way[0].mapStart == mapStart)
                return way;
            if (way.Count == 0)
            {
                UnityEngine.Debug.Log($"[xmap][warn] Empty path from {mapStart} to {mapEnd}");
                return null; // Hoặc new List<MapNext>()
            }

            if (way[0].mapStart == mapStart)
                return way;

            UnityEngine.Debug.Log($"[xmap][err] Invalid path: first step {way[0].mapStart} != start {mapStart}");
            return null;

        }*/

        internal static List<MapNext> FindWay(int mapStart, int mapEnd)
        {
            UnityEngine.Debug.Log($"[xmap] Bắt đầu tìm đường từ {mapStart} tới {mapEnd}");
            if (xmapData == null)
            {
                throw new Exception("xmapData is null");
            }

            // Kiểm tra map hợp lệ
            int length = xmapData.links.Length;
            if (mapStart < 0 || mapStart >= length || mapEnd < 0 || mapEnd >= length)
            {
                UnityEngine.Debug.Log($"[xmap][err] Map ID không hợp lệ: start={mapStart}, end={mapEnd}, max={length - 1}");
                return null;
            }

            // Trường hợp đặc biệt: đã ở đích
            if (mapStart == mapEnd)
            {
                UnityEngine.Debug.Log($"[xmap][info] Đã ở map đích {mapEnd}");
                return new List<MapNext>();
            }

            var prev = new MapNext?[length]; // Dùng nullable để phát hiện chưa khởi tạo
            var visited = new bool[length];
            var dist = new int[length];
            for (int i = 0; i < length; i++)
                dist[i] = int.MaxValue;
            dist[mapStart] = 0;

            for (int _ = 0; _ < length; _++)
            {
                var cmap = -1;
                for (int i = 0; i < length; i++)
                    if (!visited[i] && (cmap == -1 || dist[i] < dist[cmap]))
                        cmap = i;

                if (cmap == -1 || dist[cmap] == int.MaxValue)
                    break; // Không còn node nào có thể đến được

                var neighbors = xmapData.links[cmap];
                var count = neighbors.Count;
                for (int i = 0; i < count; i++)
                {
                    var mapNext = neighbors[i];
                    var cost = 1;
                    if (mapNext.type == TypeMapNext.NpcMenu && mapNext.info != null && mapNext.info.Length > 0 && mapNext.info[0] == 38)
                        cost = 100;

                    int tentative = dist[cmap] + cost;
                    if (tentative < dist[mapNext.to])
                    {
                        dist[mapNext.to] = tentative;
                        prev[mapNext.to] = mapNext;
                    }
                }

                visited[cmap] = true;
            }

            // Kiểm tra có tìm được đường không
            if (dist[mapEnd] == int.MaxValue)
            {
                UnityEngine.Debug.Log($"[xmap][err] Không tìm được đường từ {mapStart} đến {mapEnd}");
                return null;
            }

            // Dựng lại đường đi
            var way = new List<MapNext>();
            var index = mapEnd;
            int safetyCounter = 0; // Tránh infinite loop

            while (index != mapStart)
            {
                if (safetyCounter++ > length)
                {
                    UnityEngine.Debug.Log($"[xmap][err] Phát hiện vòng lặp trong đường đi!");
                    return null;
                }

                if (!prev[index].HasValue)
                {
                    UnityEngine.Debug.Log($"[xmap][err] prev[{index}] chưa được khởi tạo!");
                    return null;
                }

                way.Add(prev[index].Value);
                index = prev[index].Value.mapStart;
            }

            way.Reverse();

            // Kiểm tra tính hợp lệ
            if (way.Count == 0)
            {
                UnityEngine.Debug.Log($"[xmap][warn] Empty path from {mapStart} to {mapEnd}");
                return null;
            }

            if (way[0].mapStart != mapStart)
            {
                UnityEngine.Debug.Log($"[xmap][err] Invalid path: first step {way[0].mapStart} != start {mapStart}");
                return null;
            }

            // In ra đường đi để debug
            string pathStr = $"{mapStart}";
            foreach (var step in way)
            {
                pathStr += $" → {step.to}";
            }
            UnityEngine.Debug.Log($"[xmap][info] Tìm thấy đường: {pathStr}");

            return way;
        }
    }
}
