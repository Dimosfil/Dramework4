using System;
using System.Diagnostics.CodeAnalysis;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Injecting
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Method)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class InjectAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string ID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public InjectAttribute()
        {
        }

        public InjectAttribute(string id)
        {
            ID = id;
        }

        #endregion
    }
}