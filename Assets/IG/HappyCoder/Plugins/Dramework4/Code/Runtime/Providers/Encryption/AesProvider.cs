using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
    internal static class AesProvider
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte[] Decrypt(byte[] bytes, string key, string iv)
        {
            var decryptor = GetCryptoProvider(key, iv).CreateDecryptor();
            return decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte[] Encrypt(byte[] bytes, string key, string iv)
        {
            var cryptoProvider = GetCryptoProvider(key, iv);
            var encryptor = cryptoProvider.CreateEncryptor(cryptoProvider.Key, cryptoProvider.IV);
            return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static AesCryptoServiceProvider GetCryptoProvider(string key, string iv)
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.BlockSize = 128;
            cryptoProvider.KeySize = 256;
            cryptoProvider.Key = Encoding.UTF8.GetBytes(key);
            cryptoProvider.IV = Encoding.UTF8.GetBytes(iv);
            cryptoProvider.Mode = CipherMode.CBC;
            cryptoProvider.Padding = PaddingMode.PKCS7;
            return cryptoProvider;
        }

        #endregion
    }
}