using System;
using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Tools
{
    public static class DW4Extensions
    {
        #region ================================ METHODS

        public static float DotProductBack(this Component source, Component target)
        {
            return DW4.MathTools.DotProductBack(source, target);
        }

        public static float DotProductDown(this Component source, Component target)
        {
            return DW4.MathTools.DotProductDown(source, target);
        }

        public static float DotProductForward(this Component source, Component target)
        {
            return DW4.MathTools.DotProductForward(source, target);
        }

        public static float DotProductLeft(this Component source, Component target)
        {
            return DW4.MathTools.DotProductLeft(source, target);
        }

        public static float DotProductRight(this Component source, Component target)
        {
            return DW4.MathTools.DotProductRight(source, target);
        }

        public static float DotProductUp(this Component source, Component target)
        {
            return DW4.MathTools.DotProductUp(source, target);
        }

        public static Transform FindNearest(this Component source, IReadOnlyCollection<Component> components)
        {
            return DW4.ObjectTools.GetNearestInOverlapSphere(source, components);
        }

        public static T FindNearest<T>(this Component source, IReadOnlyCollection<T> components) where T : Component
        {
            return DW4.ObjectTools.GetNearestInOverlapSphere(source, components);
        }

        public static Transform FindNearest(this Component source, float maxSearchRadius)
        {
            return DW4.ObjectTools.GetNearestInOverlapSphere(source, maxSearchRadius);
        }

        public static T FindNearest<T>(this Component source, float maxSearchRadius) where T : Component
        {
            return DW4.ObjectTools.GetNearestInOverlapSphere<T>(source, maxSearchRadius);
        }

        public static Transform FindNearestNonAlloc(this Component source, float maxSearchRadius, Collider[] colliders)
        {
            return DW4.ObjectTools.GetNearestInOverlapSphereNonAlloc(source, maxSearchRadius, colliders);
        }

        public static T FindNearestNonAlloc<T>(this Component source, float maxSearchRadius, Collider[] colliders) where T : Component
        {
            return DW4.ObjectTools.GetNearestInOverlapSphereNonAlloc<T>(source, maxSearchRadius, colliders);
        }

        public static IReadOnlyList<Transform> GetChildren(this Transform transform)
        {
            return DW4.ObjectTools.GetChildren(transform);
        }

        public static T GetNearest<T>(this IReadOnlyList<T> components, Component point) where T : Component
        {
            return DW4.ObjectTools.GetNearest(components, point);
        }

        public static GameObject GetNearest(this IReadOnlyList<GameObject> gameObjects, GameObject point)
        {
            return DW4.ObjectTools.GetNearest(gameObjects, point);
        }

        public static Component GetNearest(this IReadOnlyList<Component> components, Component point)
        {
            return DW4.ObjectTools.GetNearest(components, point);
        }

        public static Transform GetNearest(this IReadOnlyList<Transform> transforms, Transform point)
        {
            return DW4.ObjectTools.GetNearest(transforms, point);
        }

        public static Vector3 GetNearest(this IReadOnlyList<Vector3> positions, Vector3 position)
        {
            return DW4.ObjectTools.GetNearest(positions, position);
        }

        public static IReadOnlyList<Transform> GetNearestPoints(this IReadOnlyList<Transform> transforms, Transform point, int pointsNumber)
        {
            return DW4.ObjectTools.GetNearestPoints(transforms, point, pointsNumber);
        }

        public static RelativeSide GetTargetRelativeSide(this Component source, Component target)
        {
            return DW4.MathTools.GetTargetRelativeSide(source, target);
        }

        public static bool IsMagnitudeLess(this Vector3 position1, Vector3 position2, float distance)
        {
            return DW4.MathTools.IsMagnitudeLess(position1, position2, distance);
        }

        public static bool IsMagnitudeLessOrEqual(this Vector3 position1, Vector3 position2, float distance)
        {
            return DW4.MathTools.IsMagnitudeLessOrEqual(position1, position2, distance);
        }

        public static bool IsMagnitudeMore(this Vector3 position1, Vector3 position2, float distance)
        {
            return DW4.MathTools.IsMagnitudeMore(position1, position2, distance);
        }

        public static bool IsMagnitudeMoreOrEqual(this Vector3 position1, Vector3 position2, float distance)
        {
            return DW4.MathTools.IsMagnitudeMoreOrEqual(position1, position2, distance);
        }

        public static bool IsRotationEqual(this Quaternion rotation1, Quaternion rotation2)
        {
            return DW4.MathTools.IsRotationEqual(rotation1, rotation2);
        }

        public static bool IsRotationNotEqual(this Quaternion rotation1, Quaternion rotation2)
        {
            return DW4.MathTools.IsRotationNotEqual(rotation1, rotation2);
        }

        public static bool IsSqrMagnitudeLess(this Vector3 position1, Vector3 position2, float distance)
        {
            return DW4.MathTools.IsSqrMagnitudeLess(position1, position2, distance);
        }

        public static bool IsSqrMagnitudeLessOrEqual(this Vector3 position1, Vector3 position2, float distance)
        {
            return DW4.MathTools.IsSqrMagnitudeLessOrEqual(position1, position2, distance);
        }

        public static bool IsSqrMagnitudeMore(this Vector3 position1, Vector3 position2, float distance)
        {
            return DW4.MathTools.IsSqrMagnitudeMore(position1, position2, distance);
        }

        public static bool IsSqrMagnitudeMoreOrEqual(this Vector3 position1, Vector3 position2, float distance)
        {
            return DW4.MathTools.IsSqrMagnitudeMoreOrEqual(position1, position2, distance);
        }

        public static async UniTask<T> LoadAssetAsync<T>(this string value, Action<float> onProgress = null, Action<Exception> onFailed = null, CancellationToken cancellationToken = default) where T : Object
        {
            return await DW4.AddressablesTools.LoadAssetAsync<T>(value, onProgress, onFailed, cancellationToken);
        }

        public static async UniTask<IEnumerable<T>> LoadAssetsAsync<T>(this string value, Action<float> onProgress = null, Action<Exception> onFailed = null, CancellationToken cancellationToken = default) where T : Object
        {
            return await DW4.AddressablesTools.LoadAssetsAsync<T>(value, onProgress, onFailed, cancellationToken);
        }

        public static T RemoveClonePostfix<T>(this T obj) where T : Object
        {
            obj.name = DW4.StringTools.RemoveClonePostfix(obj.name);
            return obj;
        }

        public static string RemoveClonePostfix(this string value)
        {
            return DW4.StringTools.RemoveClonePostfix(value);
        }

        public static void ShuffleArray<T>(this T[] array, out T[] result)
        {
            result = DW4.CollectionTools.ShuffleArray(array);
        }

        public static void ShuffleList<T>(this List<T> list, out List<T> result)
        {
            result = DW4.CollectionTools.ShuffleList(list);
        }

        #endregion
    }
}