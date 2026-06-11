#if UNITY_EDITOR

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Configs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public partial class StorageDataConfigAsset
    {
        #region ================================ METHODS

        private void OnEnable()
        {
            HideSaveButton = true;
        }

        #endregion
    }
}

#endif