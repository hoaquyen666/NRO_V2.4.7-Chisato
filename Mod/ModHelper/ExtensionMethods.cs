namespace Assets.Scripts.Assembly_CSharp.Mod.ModHelper
{

	internal static class ExtensionMethods 
	{
        internal static T getValueProperty<T>(this object obj, string name)
        {
            return (T)obj.GetType().GetProperty(name).GetValue(obj, null);
        }

        internal static bool IsCharDead(this Char ch)
        {
            if (!ch.isDie && ch.cHP > 0)
            {
                return ch.statusMe == 14;
            }
            return true;
        }
    }
}