using System;
using System.Diagnostics.CodeAnalysis;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Getting
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public class GetComponentAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly bool IncludingThisObject;
        public readonly string ObjectName;
        public readonly bool IgnoreName;

        public readonly bool Log;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public GetComponentAttribute()
        {
        }

        public GetComponentAttribute(bool log = true)
        {
            Log = log;
        }

        public GetComponentAttribute(bool ignoreName = false, bool log = true)
        {
            IgnoreName = ignoreName;
            Log = log;
        }

        public GetComponentAttribute(bool includingThisObject, bool ignoreName = false, bool log = true)
        {
            IncludingThisObject = includingThisObject;
            IgnoreName = ignoreName;
            Log = log;
        }

        public GetComponentAttribute(string objectName, bool log = true)
        {
            ObjectName = objectName;
            Log = log;
        }

        public GetComponentAttribute(bool includingThisObject, string objectName, bool log = true)
        {
            IncludingThisObject = includingThisObject;
            ObjectName = objectName;
            Log = log;
        }

        #endregion
    }
}