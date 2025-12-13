using Assets.Scripts.Assembly_CSharp.Mod.Menu;
using Assets.Scripts.Assembly_CSharp.Mod.ModHelper;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    /// <summary>
    /// Class menu chính của Xmap
    /// </summary>
    internal class MenuMap
    {
        private static string welcomeMessage = $"Xmap by Nishikigi Chisato - Made based on a community mod\nMap hiện tại: {TileMap.mapName} - ID map: {TileMap.mapID}\n";

        public static void showMenuXmap()
        {

            new MenuBuilder().setChatPopup(welcomeMessage + "Xin hãy chọn nơi muốn đến")
            .addItem("Xayda", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Xayda");
                foreach (int mapId in GroupMaps.Xayda.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Namek", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Namek");
                foreach (int mapId in GroupMaps.Namek.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Trái đất", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Trái đất");
                foreach (int mapId in GroupMaps.Earth.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Nappa", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Nappa");
                foreach (int mapId in GroupMaps.Nappa.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Yardat", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Yardat");
                foreach (int mapId in GroupMaps.Yardat.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Tương lai", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Tương lai");
                foreach (int mapId in GroupMaps.Future.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Cold", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Cold");
                foreach (int mapId in GroupMaps.Cold.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Potaufeu", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Potaufeu");
                foreach (int mapId in GroupMaps.Potaufeu.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Doanh trại", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Doanh trại");
                foreach (int mapId in GroupMaps.Barracks.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Ngũ hành sơn", new MenuAction(() =>
            {
                var builder = new MenuBuilder().setChatPopup(welcomeMessage + "Bạn đang chọn nhóm map Ngũ hành sơn");
                foreach (int mapId in GroupMaps.FiveElementsMountain.maps)
                {
                    builder.addItem(TileMap.mapNames[mapId], new MenuAction(() =>
                    {
                        ControllersMap.start(mapId);
                    }));
                }
                builder.start();
            }))
            .addItem("Cài đặt Xmap", new MenuAction(ShowXmapSettings))
            .start();
        }

        private static void ShowXmapSettings()
        {
            /*new MenuBuilder().setChatPopup("|1|Cài đặt Xmap by Nishikigi Chisato")
            .addItem($"Capsule VIP: {(ChisatoMap.isUseCapsuleVip ? "Bật" : "Tắt")}", new MenuAction(() => {
                ChisatoMap.isUseCapsuleVip = !ChisatoMap.isUseCapsuleVip;
                if (ChisatoMap.isUseCapsuleVip) ChisatoMap.isUseCapsuleNormal = false;
                if (!UtilsMap.hasItemCapsuleVip()) GameScr.info1.addInfo("Không có Capsule VIP trong túi!", 0);
                ShowXmapSettings();
            }))
            .addItem($"Capsule Thường: {(ChisatoMap.isUseCapsuleNormal ? "Bật" : "Tắt")}", new MenuAction(() => {
                ChisatoMap.isUseCapsuleNormal = !ChisatoMap.isUseCapsuleNormal;
                if (ChisatoMap.isUseCapsuleNormal) ChisatoMap.isUseCapsuleVip = false;
                if (!UtilsMap.hasItemCapsuleNormal()) GameScr.info1.addInfo("Không có Capsule VIP trong túi!", 0);
                ShowXmapSettings();
            }))
            .addItem($"AStar trong map: {(ChisatoMap.isXmapAStar ? "Bật" : "Tắt")}", new MenuAction(() =>
            {
                ChisatoMap.isXmapAStar = !ChisatoMap.isXmapAStar;
                ShowXmapSettings();
            }))
            .addItem($"Chạy bộ xmap: {(ChisatoMap.isWalk ? "Bật" : "Tắt")}", new MenuAction(() => {
                ChisatoMap.isWalk = !ChisatoMap.isWalk;
                if (ChisatoMap.isWalk) ChisatoMap.isTele = false;
                ShowXmapSettings();
            }))
            .addItem($"Tele xmap: {(ChisatoMap.isTele ? "Bật" : "Tắt")}", new MenuAction(() => {
                ChisatoMap.isTele = !ChisatoMap.isTele;
                if (ChisatoMap.isTele) ChisatoMap.isWalk = false;
                ShowXmapSettings();
            }))
            .addItem($"Ăn đùi gà khi ở nhà: {(Utils.isEatChicken ? "Bật" : "Tắt")}", new MenuAction(() => {
                Utils.isEatChicken = !Utils.isEatChicken;
                Utils.EatChicken();
                ShowXmapSettings();
            }))
            .addItem($"Timeout (giây): {ChisatoMap.aStarTimeout}", new MenuAction(() => {
                // Có thể thêm nhập số nếu bạn có ChatTextField
                GameScr.info1.addInfo("Timeout hiện tại: " + ChisatoMap.aStarTimeout + "s", 0);
            }))
            .addItem("Quay lại", new MenuAction(() => showMenuXmap()))
            .start();*/
            new MenuBuilder()
    .setChatPopup(string.Format("Cài đặt xmap", TileMap.mapName, TileMap.mapID))
    .addItem($"Capsule thường: {(ChisatoMap.isUseCapsuleVip ? "Bật" : "Tắt")}", new MenuAction(UtilsMap.useCapsuleNormal))
    .addItem($"Capsule VIP: {(ChisatoMap.isUseCapsuleVip ? "Bật" : "Tắt")}", new MenuAction(UtilsMap.useCapsuleVip))
    .addItem($"AStar trong map: {(ChisatoMap.isXmapAStar ? "Bật" : "Tắt")}", new MenuAction(() =>
    {
        ChisatoMap.isXmapAStar = !ChisatoMap.isXmapAStar;
        GameScr.info1.addInfo($"AStar trong map: {(ChisatoMap.isXmapAStar ? "Bật" : "Tắt")}", 0);
    }))
    .addItem($"Timeout (giây): {ChisatoMap.aStarTimeout}", new MenuAction(() =>
    {
        ChatTextField.gI().strChat = "Thời gian timeout";
        ChatTextField.gI().tfChat.name = "Giới hạn time out từ 10s - 300s";
        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
        ChatTextField.gI().startChat2(new XmapChatable(), string.Empty);
        ChatTextField.gI().tfChat.setText(ChisatoMap.aStarTimeout.ToString());
    }))
    .start();
        }
    }

    class XmapChatable : IChatable
    {
        public void onChatFromMe(string text, string to)
        {
            if (int.TryParse(text, out int timeout))
            {
                if (timeout < 10 || timeout > 300)
                {
                    GameCanvas.startOKDlg("Vượt quá giá trị" + '!');
                    return;
                }
                else
                {
                    ChisatoMap.aStarTimeout = timeout;
                    GameScr.info1.addInfo("Mặc định thời gian timeout là" + ": " + ChisatoMap.aStarTimeout, 0);
                }
            }
            else
            {
                GameCanvas.startOKDlg("Giá trị không hợp lệ" + '!');
                return;
            }
            onCancelChat();
        }

        public void onCancelChat() => ChatTextField.ResetTF();


    }


}
