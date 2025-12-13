using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    internal class DatasMap
    {
        internal List<MapNext>[] links;
        internal bool isLoaded;
        internal static List<GroupMap> groups; // = GroupMaps.AllGroups;

        internal DatasMap()
        {
            links = new List<MapNext>[TileMap.mapNames.Length];
            for (int i = 0; i < links.Length; i++)
            {
                links[i] = new List<MapNext>();
            }
        }

        internal void Load()
        {
            try
            {
                UnityEngine.Debug.Log("[DatasMap] Bước 1: Bắt đầu Load");

                groups = new List<GroupMap>(GroupMaps.AllGroups);
                UnityEngine.Debug.Log($"[DatasMap] Bước 2: Đã copy {groups.Count} groups");

                UnityEngine.Debug.Log($"[DatasMap] Bước 3: Load AutoLinkMapsWaypoint ({AutoLinkMapsWaypoint.Links.Count} links)");
                foreach (var link in AutoLinkMapsWaypoint.Links)
                {
                    if (link.mapStart >= links.Length || link.to >= links.Length)
                    {
                        UnityEngine.Debug.Log($"[ERROR] Link không hợp lệ: {link.mapStart}→{link.to}");
                        continue;
                    }
                    links[link.mapStart].Add(link);
                }

                UnityEngine.Debug.Log($"[DatasMap] Bước 4: Load LinkMapsXmap ({LinkMapsXmap.All.Count} links)");
                foreach (var link in LinkMapsXmap.All)
                {
                    if (link.mapStart >= links.Length || link.to >= links.Length)
                    {
                        UnityEngine.Debug.Log($"[ERROR] Link không hợp lệ: {link.mapStart}→{link.to}");
                        continue;
                    }
                    links[link.mapStart].Add(link);
                }

                UnityEngine.Debug.Log("[DatasMap] Bước 5: RemoveMapsHomeInGroupMaps");
                RemoveMapsHomeInGroupMaps();

                UnityEngine.Debug.Log("[DatasMap] Bước 6: AddLinksHome");
                AddLinksHome();

                UnityEngine.Debug.Log("[DatasMap] Bước 7: LoadLinkSieuThi");
                LoadLinkSieuThi();

                UnityEngine.Debug.Log($"[DatasMap] Bước 8: Check taskId={Char.myCharz().taskMaint.taskId}");
                if (Char.myCharz().taskMaint.taskId > 30)
                {
                    UnityEngine.Debug.Log("[DatasMap] Bước 8a: LoadLinkToCold");
                    LoadLinkToCold();
                }

                isLoaded = true;

                int totalLinks = 0;
                for (int i = 0; i < links.Length; i++)
                {
                    totalLinks += links[i].Count;
                }
                UnityEngine.Debug.Log($"[DatasMap] HOÀN TẤT: Đã load {totalLinks} liên kết");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log($"[DatasMap] LỖI LOAD: {ex.Message}\n{ex.StackTrace}");
                isLoaded = false;
            }
        }

        internal void LoadLinkMapCapsule()
        {
            if (!ChisatoMap.isUseCapsuleVip && !ChisatoMap.isUseCapsuleNormal) return;

            short capsuleId = ChisatoMap.CanUseCapsuleVip() ? UtilsMap.ID_ITEM_CAPSULE_VIP :
                             ChisatoMap.CanUseCapsuleNormal() ? UtilsMap.ID_ITEM_CAPSULE_NORMAL : (short)-1;

            if (capsuleId == -1) return;

            UtilsMap.mapCapsuleReturn = TileMap.mapID;

            Service.gI().useItem(0, 1, -1, capsuleId);

            string[] mapNames = GameCanvas.panel.mapNames;
            for (int i = 0; i < mapNames.Length; i++)
            {
                int mapId = UtilsMap.getMapIdFromName(mapNames[i]);
                if (mapId != -1 && mapId != TileMap.mapID)
                {
                    links[TileMap.mapID].Add(new MapNext(TileMap.mapID, mapId, TypeMapNext.Capsule, new int[] { i }));
                }
            }

            GameScr.info1.addInfo("Đã dùng Capsule - Chọn map để dịch chuyển!", 0);
        }

        private static void RemoveMapsHomeInGroupMaps()
        {
            foreach (GroupMap group in groups)
            {
                switch (Char.myCharz().cgender)
                {
                    case 0:
                        group.maps.Remove(22);
                        group.maps.Remove(23);
                        break;
                    case 1:
                        group.maps.Remove(21);
                        group.maps.Remove(23);
                        break;
                    default:
                        group.maps.Remove(21);
                        group.maps.Remove(22);
                        break;
                }
            }
        }

        private void AddLinksHome()
        {
            int cgender = Char.myCharz().cgender;
            int idMapHome = UtilsMap.getIdMapHome(cgender);
            int idMapLang = UtilsMap.getIdMapLang(cgender);
            links[idMapHome].Add(new MapNext(idMapHome, idMapLang, TypeMapNext.AutoWaypoint, null));
            links[idMapLang].Add(new MapNext(idMapLang, idMapHome, TypeMapNext.AutoWaypoint, null));
        }

        private void LoadLinkSieuThi()
        {
            int cgender = Char.myCharz().cgender;
            int to = 24 + cgender;
            int[] info = new int[2] { 10, 0 };
            links[84].Add(new MapNext(84, to, TypeMapNext.NpcMenu, info));
        }

        private void LoadLinkToCold()
        {
            if (Char.myCharz().taskMaint.taskId > 30)
            {
                int[] info = new int[2] { 12, 0 };
                links[19].Add(new MapNext(19, 109, TypeMapNext.NpcMenu, info));
                AutoLinkMapsWaypoint.Create(105, 80, true);
            }
        }
    }
}
