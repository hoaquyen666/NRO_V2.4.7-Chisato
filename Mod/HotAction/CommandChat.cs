// File: CommandChat.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Assembly_CSharp.Mod.HotAction
{
    internal class CommandChat
    {
        private static Dictionary<string, Action> exactActions = new Dictionary<string, Action>();
        private static Dictionary<string, Action<string>> prefixActions = new Dictionary<string, Action<string>>();
        private static bool isLoaded = false;

        public static void LoadCommands()
        {
            exactActions.Clear();
            prefixActions.Clear();

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
                            if (attr.Type == CommandType.Chat)
                            {
                                string cmd = attr.Command.ToLower();

                                try
                                {
                                    ParameterInfo[] parameters = method.GetParameters();

                                    if (attr.IsPrefix)
                                    {
                                        // Command có tham số (prefix)
                                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
                                        {
                                            Action<string> action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), method);
                                            if (!prefixActions.ContainsKey(cmd))
                                                prefixActions.Add(cmd, action);
                                        }
                                    }
                                    else
                                    {
                                        // Command không tham số (exact)
                                        if (parameters.Length == 0)
                                        {
                                            Action action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                                            if (!exactActions.ContainsKey(cmd))
                                                exactActions.Add(cmd, action);
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

        public static bool OnChat(string text)
        {
            if (!isLoaded) LoadCommands();

            string input = text.Trim();
            if (string.IsNullOrEmpty(input))
                return false;

            string cmdLower = input.ToLower();

            // Kiểm tra exact command trước
            if (exactActions.ContainsKey(cmdLower))
            {
                exactActions[cmdLower].Invoke();
                return true;
            }

            // Kiểm tra prefix command
            foreach (var kvp in prefixActions)
            {
                string prefix = kvp.Key;
                if (cmdLower.StartsWith(prefix))
                {
                    string argument = input.Substring(prefix.Length).Trim();
                    kvp.Value.Invoke(argument);
                    return true;
                }
            }

            return false;
        }

        // Bạn cũng có thể thêm các phương thức chat mẫu ở đây nếu muốn
        [Command("help", CommandType.Chat)]
        public static void ShowHelp()
        {
            GameScr.info1.addInfo("Available commands: tsall, as, test, k [zone], capsule, zone, potara", 0);
        }

        [Command("about", CommandType.Chat)]
        public static void ShowAbout()
        {
            GameScr.info1.addInfo("Mod by You - Version 1.0", 0);
        }
    }
}