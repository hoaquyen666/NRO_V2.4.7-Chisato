using Assets.Scripts.Assembly_CSharp.Mod.CustomPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.Mod.ModMenu
{
    internal class ModMenuMain
    {
        internal static readonly int TYPE_CUSTOM_PANEL_MENU = 27;

        internal static Command cmdOpenModMenu;
        internal static Panel currentPanel
        {
            get => GameCanvas.panel2;
            set => GameCanvas.panel2 = value;
        }

        internal static void SetTabModMenu(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, -1);
        }  

        internal static void UpdateTouch()
        {
            if (cmdOpenModMenu == null)
                return;
            if (Char.isLoadingMap)
                return;
            if (ChatTextField.gI().isShow)
                return;
            if (GameCanvas.menu.showMenu)
                return;
            if (GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                return;
            if (GameCanvas.isPointerHoldIn((int)(cmdOpenModMenu.x - cmdOpenModMenu.w * 1.5), cmdOpenModMenu.y, (int)(cmdOpenModMenu.w * 2.5), cmdOpenModMenu.h) && GameCanvas.isPointerClick)
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                cmdOpenModMenu.performAction();
                Char.myCharz().currentMovePoint = null;
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }


    }
}
