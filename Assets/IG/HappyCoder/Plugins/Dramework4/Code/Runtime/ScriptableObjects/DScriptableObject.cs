using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.ScriptableObjects
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public partial class DScriptableObject : ScriptableObject
    {
    }
}