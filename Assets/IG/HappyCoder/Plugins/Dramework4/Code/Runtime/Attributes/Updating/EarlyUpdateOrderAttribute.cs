using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Updating
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class EarlyUpdateOrderAttribute : Attribute
    {
        #region ================================ FIELDS

        private readonly int _order;
        private readonly int _offset;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public EarlyUpdateOrderAttribute(int order, int offset = 0)
        {
            _order = order;
            _offset = offset;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public int Order => _order + _offset;

        #endregion
    }
}