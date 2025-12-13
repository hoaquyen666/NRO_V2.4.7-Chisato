// File: CommandAttribute.cs
using System;

namespace Assets.Scripts.Assembly_CSharp.Mod.HotAction
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class CommandAttribute : Attribute
    {
        public string Command { get; set; }
        public CommandType Type { get; set; }
        public bool IsPrefix { get; set; }

        // Constructor 1: Cho chat command (không prefix)
        public CommandAttribute(string command)
        {
            Command = command;
            Type = CommandType.Chat;
            IsPrefix = false;
        }

        // Constructor 2: Cho chat command với prefix
        public CommandAttribute(string command, bool isPrefix)
        {
            Command = command;
            Type = CommandType.Chat;
            IsPrefix = isPrefix;
        }

        // Constructor 3: Cho chat command với type rõ ràng
        public CommandAttribute(string command, CommandType type)
        {
            Command = command;
            Type = type;
            IsPrefix = false;
        }

        // Constructor 4: Cho key command (dùng string key)
        public CommandAttribute(string command, CommandType type, bool dummy = false)
        {
            Command = command;
            Type = type;
            IsPrefix = false;
        }

        // Constructor cho key command (dùng int keycode)
        public CommandAttribute(int keyCode)
        {
            Command = keyCode.ToString();
            Type = CommandType.Key;
            IsPrefix = false;
        }
    }

    internal enum CommandType
    {
        Chat,
        Key
    }
}