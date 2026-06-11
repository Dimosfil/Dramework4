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
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class GetComponentOnSceneAttribute : GetComponentAttribute
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public GetComponentOnSceneAttribute()
        {
        }

        public GetComponentOnSceneAttribute(bool log = true) : base(log)
        {
        }

        public GetComponentOnSceneAttribute(bool ignoreName = false, bool log = true) : base(ignoreName, log)
        {
        }

        public GetComponentOnSceneAttribute(bool includingThisObject, bool ignoreName = false, bool log = true) : base(includingThisObject, ignoreName, log)
        {
        }

        public GetComponentOnSceneAttribute(string objectName, bool log = true) : base(objectName, log)
        {
        }

        public GetComponentOnSceneAttribute(bool includingThisObject, string objectName, bool log = true) : base(includingThisObject, objectName, log)
        {
        }

        #endregion
    }
}