using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static partial class DWSignalAsync
    {
        #region ================================ METHODS

        public static void Fire(string name)
        {
            SignalAsync.Fire(name);
        }

        public static void Fire<T>()
        {
            SignalAsync.Fire<T>();
        }

        public static void Fire<T>(T data)
        {
            SignalAsync<T>.Fire("", data);
        }

        public static void Fire<T>(string name, T data)
        {
            SignalAsync<T>.Fire(name, data);
        }

        #endregion
    }
}