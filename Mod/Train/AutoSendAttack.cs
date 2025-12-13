using Assets.Scripts.Assembly_CSharp.Mod.HotAction;
using Assets.Scripts.Assembly_CSharp.Mod.ModHelper.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.Mod.Train
{
    internal class AutoSendAttack : ThreadActionUpdate<AutoSendAttack>
    {
        internal override int Interval => 100;

        protected override void update()
        {
            var vMob = new MyVector();
            var vChar = new MyVector();
            var myChar = Char.myCharz();
            if (myChar.mobFocus != null)
                vMob.addElement(myChar.mobFocus);
            else if (myChar.charFocus != null)
                vChar.addElement(myChar.charFocus);
            if (vMob.size() > 0 || vChar.size() > 0)
            {
                var myskill = myChar.myskill;
                long currentTimeMillis = mSystem.currentTimeMillis();

                if (currentTimeMillis - myskill.lastTimeUseThisSkill > myskill.coolDown)
                {
                    Service.gI().sendPlayerAttack(vMob, vChar, -1); // type = -1 -> auto
                    myskill.lastTimeUseThisSkill = currentTimeMillis;
                }
            }
        }

        [Command("ak")]
        internal static void toggleAutoAttack()
        {
            gI.toggle();
        }
    }
}
