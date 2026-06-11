using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IInitializable
    {
        #region ================================ METHODS

        UniTask OnInitialize(CancellationToken cancellationToken);

        #endregion
    }
}