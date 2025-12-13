using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.Mod.ModHelper.Thread
{
    /// <summary>
    /// Kế thừa class này để tạo chức năng sử dụng Thread.
    /// </summary>
    public abstract class ThreadAction<T>
        where T : ThreadAction<T>, new()
    {
        public static T gI { get; } = new T();

        /// <summary>
        /// Kiểm tra hành động còn thực hiện.
        /// </summary>
        public bool IsActing => threadAction?.IsAlive == true;

        /// <summary>
        /// Thread sử dụng để thực thi hành động.
        /// </summary>
        protected System.Threading.Thread threadAction;

        /// <summary>
        /// Hành động cần thực hiện.
        /// </summary>
        protected abstract void action();

        /// <summary>
        /// Thực thi hành động bằng thread của instance.
        /// </summary>
        public void performAction()
        {
            if (IsActing)
                threadAction.Abort();

            executeAction();
        }

        /// <summary>
        /// Sử dụng thread của instance để thực thi hành động.
        /// </summary>
        protected void executeAction()
        {
            // Không thực hiện hành động trong luồng khác
            if (System.Threading.Thread.CurrentThread != threadAction)
            {
                threadAction = new System.Threading.Thread(executeAction)
                {
                    IsBackground = true
                };
                threadAction.Start();
                return;
            }
            action();
        }
    }
}
