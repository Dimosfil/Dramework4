using System.Threading;

using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces
{
    public interface IStartable
    {
        #region ================================ METHODS

        UniTask OnStart(CancellationToken token);

        #endregion
    }
}