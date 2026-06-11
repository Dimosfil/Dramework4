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
    internal static class MD5Provider
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte[] Decrypt(byte[] bytes, string key)
        {
            var md5 = new MD5CryptoServiceProvider();
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            cryptoProvider.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            cryptoProvider.Mode = CipherMode.ECB;
            var decryptor = cryptoProvider.CreateDecryptor();
            return decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte[] Encrypt(byte[] bytes, string key)
        {
            var md5 = new MD5CryptoServiceProvider();
            var cryptoProvider = new TripleDESCryptoServiceProvider();
            cryptoProvider.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            cryptoProvider.Mode = CipherMode.ECB;
            var encryptor = cryptoProvider.CreateEncryptor();
            return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string Hash(string source)
        {
            using var hash = MD5.Create();
            var data = hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            var builder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
                builder.Append(data[i].ToString("x2"));
            return builder.ToString();
        }

        #endregion
    }
}