using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.Mod.CustomPanel
{
    internal class SetTabPanelTemplates
    {
        internal static void setTabListTemplate(Panel panel, params int[] lengths)
        {
            // Set the item height
            panel.ITEM_HEIGHT = 24;
            // Set the current list length
            if (lengths.Length > 1)
                panel.currentListLength = lengths[panel.currentTabIndex];
            else
                panel.currentListLength = lengths[0];
            // Set the selected index
            panel.selected = GameCanvas.isTouch ? (-1) : 0;
            // Set the scroll limit
            panel.cmyLim = panel.currentListLength * panel.ITEM_HEIGHT - panel.hScroll;
            if (panel.cmyLim < 0)
                panel.cmyLim = 0;
            // Set the scroll position
            panel.cmy = panel.cmtoY = panel.cmyLast[panel.currentTabIndex];
            if (panel.cmy < 0)
                panel.cmy = panel.cmtoY = 0;
            if (panel.cmy > panel.cmyLim)
                panel.cmy = panel.cmtoY = panel.cmyLim;
        }

        internal static void setTabListTemplate(Panel panel, params ICollection[] collections)
        {
            var lengths = collections.Select(x => x.Count).ToArray();
            setTabListTemplate(panel, lengths);
        }
    }
}
