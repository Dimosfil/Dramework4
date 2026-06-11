using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Initialization
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class SetOnPlayModeStateChangedAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly object Value;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public SetOnPlayModeStateChangedAttribute(object value)
        {
            Value = value;
        }

        #endregion
    }
}