using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IPreInitializable
    {
        #region ================================ METHODS

        public void OnPreInitialize();

        #endregion
    }
}