using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Creating
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string SceneName;
        public readonly int Order;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public CreateAttribute(string sceneName, int order = 0)
        {
            SceneName = sceneName;
            Order = order;
        }

        #endregion
    }
}