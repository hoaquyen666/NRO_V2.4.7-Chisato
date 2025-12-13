using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.Mod.ModHelper.Thread
{
    public class MainThreadDispatcher
    {
        private static readonly Queue<Action> Queue = new Queue<Action>();

        public static void Dispatch(Action action)
        {
            Queue.Enqueue(action);
        }

        public static void update()
        {
            while (Queue.Count > 0)
            {
                Queue.Dequeue()();
            }
        }
    }
}
