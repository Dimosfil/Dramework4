using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Providers.Encryption;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Tools
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static partial class DW4
    {
        #region ================================ METHODS

        public static byte[] AesDecrypt(byte[] bytes, string key, string iv)
        {
            return AesProvider.Decrypt(bytes, key, iv);
        }

        public static byte[] AesEncrypt(byte[] bytes, string key, string iv)
        {
            return AesProvider.Encrypt(bytes, key, iv);
        }

        public static byte[] Md5Decrypt(byte[] bytes, string key)
        {
            return MD5Provider.Decrypt(bytes, key);
        }

        public static byte[] Md5Encrypt(byte[] bytes, string key)
        {
            return MD5Provider.Encrypt(bytes, key);
        }

        public static string Md5Hash(string source)
        {
            return MD5Provider.Hash(source);
        }

        public static void Xor(ref byte[] input, byte key)
        {
            XorProvider.Xor(ref input, key);
        }

        public static void Xor(ref string input, byte key)
        {
            XorProvider.Xor(ref input, key);
        }

        #endregion
    }
}