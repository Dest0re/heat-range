

namespace HeatRange
{
    public static partial class Utils
    {
        public static T Prefab<T>(this string s)
        where T: class
        {
            return Godot.ResourceLoader.Load<Godot.PackedScene>(s).Instance<T>();
        }
    }
}
