using System;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Data
{
    internal class ContainerObject<T>
    {
        #region ================================ FIELDS

        internal readonly string ID;
        internal readonly string SceneName;
        internal readonly int Order;
        internal readonly T Object;
        internal readonly Type Type;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal ContainerObject(string id, string sceneName, int order, T obj)
        {
            ID = id;
            SceneName = sceneName;
            Order = order;
            Object = obj;
            Type = obj.GetType();
        }

        #endregion
    }
}