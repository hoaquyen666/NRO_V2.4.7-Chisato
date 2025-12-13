// File: CommandKey.cs (tạo mới)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Assembly_CSharp.Mod.HotAction
{
    internal class CommandKey
    {
        class NativeInput
        {
            [DllImport("user32.dll")]
            private static extern short GetAsyncKeyState(int vKey);

            private const int VK_SHIFT = 0x10;

            public static bool IsShiftPressed()
            {
                return (GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0;
            }
        }

        private static Dictionary<int, Action> hotkeyActions = new Dictionary<int, Action>();
        private static bool isLoaded = false;

        public static void LoadHotkeys()
        {
            hotkeyActions.Clear();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                foreach (MethodInfo method in methods)
                {
                    object[] attributes = method.GetCustomAttributes(typeof(CommandAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                    {
                        foreach (CommandAttribute attr in attributes)
                        {
                            if (attr.Type == CommandType.Key)
                            {
                                try
                                {
                                    // Chuyển Command thành keycode
                                    if (int.TryParse(attr.Command, out int keyCode))
                                    {
                                        // Command là số (keycode trực tiếp)
                                        Action action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                                        if (!hotkeyActions.ContainsKey(keyCode))
                                        {
                                            hotkeyActions.Add(keyCode, action);
                                        }
                                    }
                                    else if (attr.Command.Length == 1)
                                    {
                                        keyCode = attr.Command[0];
                                        Action action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                                        if (!hotkeyActions.ContainsKey(keyCode))
                                        {
                                            hotkeyActions.Add(keyCode, action);
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            isLoaded = true;
        }

        public static bool KeyPress(int asscicode)
        {
            if (!isLoaded) LoadHotkeys();

            bool isShift = NativeInput.IsShiftPressed();
            if (isShift && asscicode >= 97 && asscicode <= 122)
            {
                asscicode -= 32; // Chuyển lowercase thành uppercase
            }

            if (hotkeyActions.ContainsKey(asscicode))
            {
                hotkeyActions[asscicode].Invoke();
                return true;
            }

            return false;
        }

        // Ví dụ các phương thức hotkey
        [Command("a")]
        public static void a()
        {
            GameScr.info1.addInfo("a", 0);
        }

        [Command("A")]
        public static void A()
        {
            GameScr.info1.addInfo("A", 0);
        }
    }
}