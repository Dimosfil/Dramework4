using System;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Configs;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using NUnit.Framework;


namespace IG.HappyCoder.Plugins.Dramework4.Tests.EditMode
{
    public sealed class DW4StorageTests
    {
        #region ================================ METHODS

        [Test]
        public void Save_UnknownStorageType_Throws()
        {
            var config = CreateConfig((StorageType)999);

            Assert.Throws<ArgumentOutOfRangeException>(() => DW4.Save("data", config));
        }

        [Test]
        public void Load_UnknownStorageType_Throws()
        {
            var config = CreateConfig((StorageType)999);

            Assert.Throws<ArgumentOutOfRangeException>(() => DW4.Load<string>(config));
        }

        [Test]
        public void SaveAsync_UnknownStorageType_Throws()
        {
            var config = CreateConfig((StorageType)999);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await DW4.SaveAsync("data", config).AsTask());
        }

        [Test]
        public void LoadAsync_UnknownStorageType_Throws()
        {
            var config = CreateConfig((StorageType)999);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await DW4.LoadAsync<string>(config, null).AsTask());
        }

        private static StorageDataConfig CreateConfig(StorageType storageType)
        {
            return new StorageDataConfig(
                "invalid-storage",
                "unused-path",
                storageType,
                SerializationType.JSON);
        }

        #endregion
    }
}
