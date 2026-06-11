namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Data
{
    public struct StorageResponse<T>
    {
        #region ================================ FIELDS

        public readonly bool Success;
        public readonly T Data;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public StorageResponse(bool success, T data)
        {
            Success = success;
            Data = data;
        }

        #endregion
    }
}