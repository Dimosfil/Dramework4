using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Encryption
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal static class XorProvider
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Xor(ref byte[] input, byte key)
        {
            var length = input.Length;
            for (var i = 0; i < length; i++)
                input[i] = (byte)(input[i] ^ key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Xor(ref string input, byte key)
        {
            var length = input.Length;
            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var c = (char)(input[i] ^ key);
                sb.Append(c);
            }
            input = sb.ToString();
        }

        #endregion
    }
}