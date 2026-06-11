using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Constants;

using Unity.IL2CPP.CompilerServices;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.LowLevel;
using UnityEngine.ResourceManagement.AsyncOperations;

using Random = System.Random;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Tools
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static partial class DW4
    {
        #region ================================ FIELDS

        private static readonly Random _random = new Random();
        private static readonly Dictionary<Type, object> Signals = new Dictionary<Type, object>();

        #endregion

        #region ================================ METHODS

        public static int GetRandom(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static float GetRandom(float min, float max)
        {
            return (float)_random.NextDouble() * (max - min) + min;
        }

        #endregion

        #region ================================ NESTED TYPES

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static partial class AddressablesTools
        {
            #region ================================ METHODS

            public static async UniTask<T> LoadAssetAsync<T>(string key, Action<float> onProgress, Action<Exception> onFailed, CancellationToken cancellationToken = default) where T : class
            {
                var handle = Addressables.LoadAssetAsync<T>(key);
                onProgress?.Invoke(0);
                while (handle.IsDone == false)
                {
                    onProgress?.Invoke(handle.PercentComplete);
                    await UniTask.Yield(cancellationToken);
                }
                onProgress?.Invoke(1);
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    return handle.Result;

                onFailed?.Invoke(handle.OperationException);
                return null;
            }

            public static async UniTask<IEnumerable<T>> LoadAssetsAsync<T>(string key, Action<float> onProgress, Action<Exception> onFailed, CancellationToken cancellationToken = default)
            {
                var handle = Addressables.LoadAssetsAsync<T>(key, null);
                onProgress?.Invoke(0);
                while (handle.IsDone == false)
                {
                    onProgress?.Invoke(handle.PercentComplete);
                    await UniTask.Yield(cancellationToken);
                }
                onProgress?.Invoke(1);
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    return handle.Result;

                onFailed?.Invoke(handle.OperationException);
                return null;
            }

            #endregion
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static class CollectionTools
        {
            #region ================================ METHODS

            public static IList<T> GetInsertionSorted<T>(IEnumerable<T> collection) where T : IComparable<T>
            {
                var copy = collection.ToList();
                InsertionSort(copy);
                return copy;
            }

            public static T[] GetInsertionSorted<T>(T[] collection) where T : IComparable<T>
            {
                var copy = new T[collection.Length];
                for (var i = 0; i < copy.Length; i++)
                {
                    copy[i] = collection[i];
                }
                InsertionSort(copy);
                return copy;
            }

            public static List<T> GetRandomItems<T>(List<T> list, int count = 1)
            {
                count = Mathf.Clamp(count, 0, list.Count);
                var result = new List<T>();
                while (result.Count < count)
                {
                    var index = _random.Next(0, list.Count);
                    var item = list[index];
                    if (result.Contains(item)) continue;
                    result.Add(item);
                }

                return result;
            }

            public static IReadOnlyList<T> GetRandomItems<T>(IReadOnlyList<T> list, int count = 1, IReadOnlyList<T> excluding = null, int tryCount = 1000)
            {
                count = Mathf.Clamp(count, 0, list.Count);
                var result = new List<T>();
                while (result.Count < count)
                {
                    var index = _random.Next(0, list.Count);
                    var item = list[index];
                    if (result.Contains(item) || (excluding != null && excluding.Contains(item)))
                    {
                        tryCount--;
                        if (tryCount == 0)
                            return result;

                        continue;
                    }
                    result.Add(item);
                }

                return result;
            }

            public static T[] GetRandomItems<T>(T[] array, int count = 1)
            {
                count = Mathf.Clamp(count, 0, array.Length);
                var result = new List<T>();
                while (result.Count < count)
                {
                    var index = _random.Next(0, array.Length);
                    var item = array[index];
                    if (result.Contains(item)) continue;
                    result.Add(item);
                }

                return result.ToArray();
            }

            public static void InsertionSort<T>(IList<T> collection) where T : IComparable<T>
            {
                for (var i = 1; i < collection.Count; i++)
                {
                    var index = i;
                    var current = collection[i];

                    while (index > 0 && collection[index - 1].CompareTo(current) > 0)
                    {
                        (collection[index], collection[index - 1]) = (collection[index - 1], collection[index]);
                        index--;
                    }

                    collection[index] = current;
                }
            }

            public static void InsertionSort<T1, T2>(IList<T1> collection, Func<T1, T2> getComparable) where T2 : IComparable<T2>
            {
                for (var i = 1; i < collection.Count; i++)
                {
                    var index = i;
                    var current = collection[i];

                    while (index > 0 && getComparable.Invoke(collection[index - 1]).CompareTo(getComparable.Invoke(current)) > 0)
                    {
                        (collection[index], collection[index - 1]) = (collection[index - 1], collection[index]);
                        index--;
                    }

                    collection[index] = current;
                }
            }

            public static void InsertionSort<T>(T[] collection) where T : IComparable<T>
            {
                for (var i = 1; i < collection.Length; i++)
                {
                    var index = i;
                    var current = collection[i];

                    while (index > 0 && collection[index - 1].CompareTo(current) > 0)
                    {
                        (collection[index], collection[index - 1]) = (collection[index - 1], collection[index]);
                        index--;
                    }

                    collection[index] = current;
                }
            }

            public static T[] ShuffleArray<T>(T[] array)
            {
                for (var i = array.Length - 1; i >= 1; i--)
                {
                    var j = _random.Next(i + 1);
                    (array[j], array[i]) = (array[i], array[j]);
                }
                return array;
            }

            public static List<T> ShuffleList<T>(List<T> list)
            {
                for (var i = list.Count - 1; i >= 1; i--)
                {
                    var j = _random.Next(i + 1);
                    (list[j], list[i]) = (list[i], list[j]);
                }
                return list;
            }

            #endregion
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static class IOTools
        {
            #region ================================ METHODS

            public static void AddFileAttributes(string path, FileAttributes attributes)
            {
                File.SetAttributes(path, File.GetAttributes(path) | attributes);
            }

            public static void ClearDirectory(string path, IReadOnlyCollection<string> excludedFiles = null, IReadOnlyCollection<string> excludedDirectories = null)
            {
                if (Directory.Exists(path) == false) return;

                var directory = new DirectoryInfo(path);

                foreach (var file in directory.GetFiles())
                {
                    if (excludedFiles != null && excludedFiles.Any(f => file.Name.Contains(f))) continue;
                    file.Delete();
                }

                foreach (var subdirectory in directory.GetDirectories())
                {
                    if (excludedDirectories != null && excludedDirectories.Any(d => subdirectory.Name.Contains(d))) continue;
                    ClearDirectory(subdirectory.FullName);
                    subdirectory.Delete();
                }
            }

            public static void DeleteFiles(string path, IReadOnlyCollection<string> excludedFiles = null)
            {
                if (Directory.Exists(path) == false) return;

                var directory = new DirectoryInfo(path);

                foreach (var file in directory.GetFiles())
                {
                    if (excludedFiles != null && excludedFiles.Any(fileName => fileName == file.Name)) continue;
                    file.Delete();
                }
            }

            public static List<string> GetDirectoriesInFolder(string folderPath)
            {
                var directories = new List<string>();

                // Проверяем, существует ли папка
                if (Directory.Exists(folderPath) == false)
                {
                    Debug.LogError($"Папка не найдена: {folderPath}");
                    return directories;
                }

                // Получаем все подпапки
                var subDirectories = Directory.GetDirectories(folderPath);

                // Добавляем подпапки в список
                foreach (var subDirectory in subDirectories)
                {
                    directories.Add(subDirectory);
                }

                return directories;
            }

            public static string GetRelativePath(string absolutePath)
            {
                return absolutePath.Remove(0, absolutePath.IndexOf("Assets", StringComparison.Ordinal));
            }

            public static void RemoveFileAttributes(string path, FileAttributes attributesToRemove)
            {
                File.SetAttributes(path, File.GetAttributes(path) & ~attributesToRemove);
            }

            #endregion
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static class MathTools
        {
            #region ================================ METHODS

            public static float DotProductBack(Component source, Component target)
            {
                var dirToTarget = (target.transform.position - source.transform.position).normalized;
                return Vector3.Dot(-source.transform.forward, dirToTarget);
            }

            public static float DotProductDown(Component source, Component target)
            {
                var dirToTarget = (target.transform.position - source.transform.position).normalized;
                return Vector3.Dot(-source.transform.up, dirToTarget);
            }

            public static float DotProductForward(Component source, Component target)
            {
                var dirToTarget = (target.transform.position - source.transform.position).normalized;
                return Vector3.Dot(source.transform.forward, dirToTarget);
            }

            public static float DotProductLeft(Component source, Component target)
            {
                var dirToTarget = (target.transform.position - source.transform.position).normalized;
                return Vector3.Dot(-source.transform.right, dirToTarget);
            }

            public static float DotProductRight(Component source, Component target)
            {
                var dirToTarget = (target.transform.position - source.transform.position).normalized;
                return Vector3.Dot(source.transform.right, dirToTarget);
            }

            public static float DotProductUp(Component source, Component target)
            {
                var dirToTarget = (target.transform.position - source.transform.position).normalized;
                return Vector3.Dot(source.transform.up, dirToTarget);
            }

            public static Vector3 GetNearestPointOnLine(Vector3 point, Vector3 lineStart, Vector3 lineDir)
            {
                var pointToStart = point - lineStart;
                var projection = Vector3.Project(pointToStart, lineDir.normalized);
                return lineStart + projection;
            }

            public static RelativeSide GetTargetRelativeSide(Component source, Component target)
            {
                var result = RelativeSide.Center;
                var dotProduct = DotProductForward(source, target);
                result = dotProduct switch
                {
                    > 0 => RelativeSide.Front,
                    < 0 => RelativeSide.Back,
                    _ => result
                };

                dotProduct = DotProductRight(source, target);
                switch (dotProduct)
                {
                    case > 0 when result == RelativeSide.Center:
                        result = RelativeSide.Right;
                        break;
                    case > 0:
                        result |= RelativeSide.Right;
                        break;
                    case < 0 when result == RelativeSide.Center:
                        result = RelativeSide.Left;
                        break;
                    case < 0:
                        result |= RelativeSide.Left;
                        break;
                }
                return result;
            }

            public static bool IsMagnitudeLess(Vector3 position1, Vector3 position2, float distance)
            {
                return (position1 - position2).magnitude < distance;
            }

            public static bool IsMagnitudeLessOrEqual(Vector3 position1, Vector3 position2, float distance)
            {
                return (position1 - position2).magnitude <= distance;
            }

            public static bool IsMagnitudeMore(Vector3 position1, Vector3 position2, float distance)
            {
                return (position1 - position2).magnitude > distance;
            }

            public static bool IsMagnitudeMoreOrEqual(Vector3 position1, Vector3 position2, float distance)
            {
                return (position1 - position2).magnitude >= distance;
            }

            public static bool IsRotationEqual(Quaternion rotation1, Quaternion rotation2)
            {
                return rotation1 == rotation2;
            }

            public static bool IsRotationNotEqual(Quaternion rotation1, Quaternion rotation2)
            {
                return rotation1 != rotation2;
            }

            public static bool IsSqrMagnitudeLess(Vector3 position1, Vector3 position2, float distance)
            {
                return (position1 - position2).sqrMagnitude < distance;
            }

            public static bool IsSqrMagnitudeLessOrEqual(Vector3 position1, Vector3 position2, float distance)
            {
                return (position1 - position2).sqrMagnitude <= distance;
            }

            public static bool IsSqrMagnitudeMore(Vector3 position1, Vector3 position2, float distance)
            {
                return (position1 - position2).sqrMagnitude > distance;
            }

            public static bool IsSqrMagnitudeMoreOrEqual(Vector3 position1, Vector3 position2, float distance)
            {
                return (position1 - position2).sqrMagnitude >= distance;
            }

            #endregion
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static class ObjectTools
        {
            #region ================================ METHODS

            public static IReadOnlyList<Transform> GetChildren(Transform transform)
            {
                var result = new List<Transform>();
                for (var i = 0; i < transform.childCount; i++)
                    result.Add(transform.GetChild(i));
                return result;
            }

            public static Transform GetNearest(IReadOnlyList<Transform> transforms, Transform point)
            {
                var result = transforms[0];
                var minDistance = float.MaxValue;
                for (var i = 0; i < transforms.Count; i++)
                {
                    var current = transforms[i];
                    var distance = (point.position - current.position).sqrMagnitude;
                    if (distance > minDistance) continue;
                    minDistance = distance;
                    result = current;
                }

                return result;
            }

            public static GameObject GetNearest(IReadOnlyList<GameObject> gameObjects, GameObject point)
            {
                var result = gameObjects[0];
                var minDistance = float.MaxValue;
                for (var i = 0; i < gameObjects.Count; i++)
                {
                    var current = gameObjects[i];
                    var distance = (point.transform.position - current.transform.position).sqrMagnitude;
                    if (distance > minDistance) continue;
                    minDistance = distance;
                    result = current;
                }

                return result;
            }

            public static Component GetNearest(IReadOnlyList<Component> components, Component point)
            {
                var result = components[0];
                var minDistance = float.MaxValue;
                for (var i = 0; i < components.Count; i++)
                {
                    var current = components[i];
                    var distance = (point.transform.position - current.transform.position).sqrMagnitude;
                    if (distance > minDistance) continue;
                    minDistance = distance;
                    result = current;
                }

                return result;
            }

            public static T GetNearest<T>(IReadOnlyList<T> components, Component point) where T : Component
            {
                var result = components[0];
                var minDistance = float.MaxValue;
                for (var i = 0; i < components.Count; i++)
                {
                    var current = components[i];
                    var distance = (point.transform.position - current.transform.position).sqrMagnitude;
                    if (distance > minDistance) continue;
                    minDistance = distance;
                    result = current;
                }

                return result;
            }

            public static Vector3 GetNearest(IReadOnlyList<Vector3> positions, Vector3 position)
            {
                var result = positions[0];
                var minDistance = float.MaxValue;
                for (var i = 0; i < positions.Count; i++)
                {
                    var current = positions[i];
                    var distance = (position - current).sqrMagnitude;
                    if (distance > minDistance) continue;
                    minDistance = distance;
                    result = current;
                }

                return result;
            }

            public static Transform GetNearestInOverlapSphere(Component source, float maxSearchRadius)
            {
                if (source == null) return null;

                Collider nearest = null;
                var minSqrDistance = Mathf.Infinity;

                // ReSharper disable once Unity.PreferNonAllocApi
                foreach (var collider in Physics.OverlapSphere(source.transform.position, maxSearchRadius))
                {
                    var sqrDistance = (source.transform.position - collider.transform.position).sqrMagnitude;
                    if (sqrDistance >= minSqrDistance) continue;
                    minSqrDistance = sqrDistance;
                    nearest = collider;
                }

                return nearest?.transform;
            }

            public static T GetNearestInOverlapSphere<T>(Component source, float maxSearchRadius) where T : Component
            {
                if (source == null) return null;

                Collider nearest = null;
                var minSqrDistance = Mathf.Infinity;

                // ReSharper disable once Unity.PreferNonAllocApi
                foreach (var collider in Physics.OverlapSphere(source.transform.position, maxSearchRadius))
                {
                    var sqrDistance = (source.transform.position - collider.transform.position).sqrMagnitude;
                    if (sqrDistance >= minSqrDistance) continue;
                    minSqrDistance = sqrDistance;
                    nearest = collider;
                }

                return nearest?.GetComponent<T>();
            }

            public static Transform GetNearestInOverlapSphere(Component source, IReadOnlyCollection<Component> components)
            {
                if (source == null || components == null || components.Any() == false) return null;

                Component nearest = null;
                var minSqrDistance = Mathf.Infinity;

                foreach (var component in components)
                {
                    var sqrDistance = (source.transform.position - component.transform.position).sqrMagnitude;
                    if (sqrDistance >= minSqrDistance) continue;
                    minSqrDistance = sqrDistance;
                    nearest = component;
                }

                return nearest?.transform;
            }

            public static T GetNearestInOverlapSphere<T>(Component source, IReadOnlyCollection<T> components) where T : Component
            {
                if (source == null || components == null || components.Any() == false) return null;

                T nearest = null;
                var minSqrDistance = Mathf.Infinity;

                foreach (var component in components)
                {
                    var sqrDistance = (source.transform.position - component.transform.position).sqrMagnitude;
                    if (sqrDistance >= minSqrDistance) continue;
                    minSqrDistance = sqrDistance;
                    nearest = component;
                }

                return nearest;
            }

            public static Transform GetNearestInOverlapSphereNonAlloc(Component source, float maxSearchRadius, Collider[] colliders)
            {
                if (source == null || colliders == null || colliders.Length == 0) return null;

                var hitsCount = Physics.OverlapSphereNonAlloc(source.transform.position, maxSearchRadius, colliders);

                Collider nearest = null;
                var minSqrDistance = Mathf.Infinity;

                for (var i = 0; i < hitsCount; i++)
                {
                    var collider = colliders[i];
                    var sqrDistance = (source.transform.position - collider.transform.position).sqrMagnitude;
                    if (sqrDistance >= minSqrDistance) continue;
                    minSqrDistance = sqrDistance;
                    nearest = collider;
                }

                return nearest?.transform;
            }

            public static T GetNearestInOverlapSphereNonAlloc<T>(Component source, float maxSearchRadius, Collider[] colliders) where T : Component
            {
                if (source == null || colliders == null || colliders.Length == 0) return null;

                var hitsCount = Physics.OverlapSphereNonAlloc(source.transform.position, maxSearchRadius, colliders);

                Collider nearest = null;
                var minSqrDistance = Mathf.Infinity;

                for (var i = 0; i < hitsCount; i++)
                {
                    var collider = colliders[i];
                    var sqrDistance = (source.transform.position - collider.transform.position).sqrMagnitude;
                    if (sqrDistance >= minSqrDistance) continue;
                    minSqrDistance = sqrDistance;
                    nearest = collider;
                }

                return nearest?.GetComponent<T>();
            }

            public static IReadOnlyList<Transform> GetNearestPoints(IReadOnlyList<Transform> transforms, Transform point, int pointsNumber)
            {
                var distances = new float[transforms.Count];
                for (var i = 0; i < transforms.Count; i++)
                    distances[i] = (transforms[i].position - point.position).sqrMagnitude;

                var nearestIndices = new int[pointsNumber];
                for (var i = 0; i < pointsNumber; i++)
                {
                    var minDistance = float.MaxValue;
                    var minIndex = -1;

                    for (var j = 0; j < transforms.Count; j++)
                    {
                        var alreadySelected = false;

                        for (var k = 0; k < i; k++)
                        {
                            if (nearestIndices[k] != j) continue;
                            alreadySelected = true;
                            break;
                        }

                        if (alreadySelected || distances[j] >= minDistance) continue;
                        minDistance = distances[j];
                        minIndex = j;
                    }

                    nearestIndices[i] = minIndex;
                }

                return nearestIndices.Select(index => transforms[index]).ToList();
            }

            #endregion
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static class StringTools
        {
            #region ================================ FIELDS

            private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            #endregion

            #region ================================ METHODS

            public static int CharCount(string source, char c)
            {
                return source.Count(ch => ch == c);
            }

            public static string ClearText(string text)
            {
                return Regex.Replace(text, "[^a-zA-Z0-9]", string.Empty);
            }

            public static string FirstCharToUpper(string text)
            {
                switch (text)
                {
                    case null: throw new ArgumentNullException(nameof(text));
                    case "": throw new ArgumentException($"{nameof(text)} cannot be empty", nameof(text));
                    default: return text.First().ToString().ToUpper() + text[1..];
                }
            }

            public static string GetRandomString(int length)
            {
                return new string(Enumerable.Repeat(CHARS, length)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            }

            public static string InsertSymbolBetweenUppercaseWords(string text, string symbol)
            {
                var words = SplitByUppercase(text);
                return words.Aggregate(string.Empty, (current, word) =>
                {
                    if (word != words.Last())
                        return current + $"{word}{symbol}";

                    return current + word;
                });
            }

            public static string RemoveClonePostfix(string value)
            {
                const string clone = "(Clone)";
                if (value.EndsWith(clone) == false) return value;
                return value.Replace(clone, "");
            }

            public static string[] SplitByUppercase(string text)
            {
                var words = Regex.Split(text, @"(?<!^)(?=[A-Z])");
                var result = new List<string>();
                var word = string.Empty;
                for (var i = 0; i < words.Length; i++)
                {
                    if (words[i].All(char.IsUpper))
                    {
                        word += words[i];
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(word))
                        {
                            result.Add(words[i]);
                        }
                        else
                        {
                            result.Add(word);
                            result.Add(words[i]);
                            word = null;
                        }
                    }
                }

                if (string.IsNullOrEmpty(word) == false)
                    result.Add(word);

                return result.ToArray();
            }

            public static int StringToProduct(string input)
            {
                return input.Aggregate(0, (current, c) => current * c);
            }

            public static int StringToSum(string input)
            {
                return input.Aggregate(0, (current, c) => current + c);
            }

            public static int SubstringCount(string source, string subString)
            {
                var split = source.Split(new[] { subString }, StringSplitOptions.None);
                return split.Length - 1;
            }

            public static string UrlEncode(string url)
            {
                var strRdr = new StringReader(url);
                var strWtr = new StringWriter();
                var charValue = strRdr.Read();
                while (charValue != -1)
                {
                    if (charValue is >= 48 and <= 57 // 0-9
                        || charValue is >= 65 and <= 90 // A-Z
                        || charValue is >= 97 and <= 122) // a-z
                    {
                        strWtr.Write((char)charValue);
                    }
                    else if (charValue == 32) // Space
                    {
                        strWtr.Write("+");
                    }
                    else
                    {
                        strWtr.Write("%{0:x2}", charValue);
                    }
                    charValue = strRdr.Read();
                }
                return strWtr.ToString();
            }

            #endregion
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public static class UnityPlayerLoopTools
        {
            #region ================================ FIELDS

            private static string _logFile;
            private static string _tag;

            #endregion

            #region ================================ PROPERTIES AND INDEXERS

            private static string TAG
            {
                get
                {
                    if (string.IsNullOrEmpty(_tag) == false) return _tag;
                    return _tag = $"[{nameof(UnityPlayerLoopTools)}] :";
                }
            }

            #endregion

            #region ================================ METHODS

            public static void AddSystem<T>(PlayerLoopSystem system) where T : struct
            {
                var current = PlayerLoop.GetCurrentPlayerLoop();

                for (var i = 0; i < current.subSystemList.Length; i++)
                {
                    if (current.subSystemList[i].type != typeof(T)) continue;

                    var subSystems = current.subSystemList[i].subSystemList.ToList();
                    if (subSystems.All(s => s.type != system.type))
                        subSystems.Add(system);
                    current.subSystemList[i].subSystemList = subSystems.ToArray();
                    PlayerLoop.SetPlayerLoop(current);
                    return;
                }
            }

            public static void InsertSystem<T>(int index, PlayerLoopSystem system) where T : struct
            {
                var current = PlayerLoop.GetCurrentPlayerLoop();

                for (var i = 0; i < current.subSystemList.Length; i++)
                {
                    if (current.subSystemList[i].type != typeof(T)) continue;

                    var subSystems = current.subSystemList[i].subSystemList.ToList();
                    if (subSystems.All(s => s.type != system.type))
                        subSystems.Insert(index, system);
                    current.subSystemList[i].subSystemList = subSystems.ToArray();
                    PlayerLoop.SetPlayerLoop(current);
                    return;
                }
            }

            public static void LogCurrentPlayerLoop(string path)
            {
                _logFile = string.Empty;
                var rootSystem = PlayerLoop.GetCurrentPlayerLoop();
                LogSubSystems(rootSystem, "");
                File.WriteAllText(path, _logFile);
            }

            public static void LogDefaultPlayerLoop(string path)
            {
                _logFile = string.Empty;
                var rootSystem = PlayerLoop.GetDefaultPlayerLoop();
                LogSubSystems(rootSystem, "");
                File.WriteAllText(path, _logFile);
            }

            public static void RemoveSystem<T>(Type systemType) where T : struct
            {
                var current = PlayerLoop.GetCurrentPlayerLoop();

                for (var i = 0; i < current.subSystemList.Length; i++)
                {
                    if (current.subSystemList[i].type != typeof(T)) continue;

                    var subSystems = current.subSystemList[i].subSystemList.ToList();
                    subSystems.RemoveAll(s => s.type == systemType);
                    current.subSystemList[i].subSystemList = subSystems.ToArray();
                    PlayerLoop.SetPlayerLoop(current);
                    return;
                }
            }

            public static bool TryRemoveTypeFrom(ref PlayerLoopSystem currentSystem, Type type)
            {
                var subSystems = currentSystem.subSystemList;
                if (subSystems == null)
                {
                    return false;
                }

                for (var i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].type == type)
                    {
                        var newSubSystems = new PlayerLoopSystem[subSystems.Length - 1];

                        Array.Copy(subSystems, newSubSystems, i);
                        Array.Copy(subSystems, i + 1, newSubSystems, i, subSystems.Length - i - 1);

                        currentSystem.subSystemList = newSubSystems;

                        Log($"{TAG} Subsystem - «{type}» was removed from player loop");

                        return true;
                    }

                    if (TryRemoveTypeFrom(ref subSystems[i], type))
                    {
                        return true;
                    }
                }

                return false;
            }

            public static bool TryRemoveTypeFrom(ref PlayerLoopSystem currentSystem, string typeFullName)
            {
                var subSystems = currentSystem.subSystemList;
                if (subSystems == null)
                {
                    return false;
                }

                for (var i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].type.FullName == typeFullName)
                    {
                        var newSubSystems = new PlayerLoopSystem[subSystems.Length - 1];

                        Array.Copy(subSystems, newSubSystems, i);
                        Array.Copy(subSystems, i + 1, newSubSystems, i, subSystems.Length - i - 1);

                        currentSystem.subSystemList = newSubSystems;

                        Log($"{TAG} Subsystem - «{typeFullName}» was removed from player loop");

                        return true;
                    }

                    if (TryRemoveTypeFrom(ref subSystems[i], typeFullName))
                    {
                        return true;
                    }
                }

                return false;
            }

            private static void LogSubSystems(PlayerLoopSystem playerLoopSystem, string indent)
            {
                _logFile = $"{_logFile}{indent}{playerLoopSystem.type}\n".Replace("UnityEngine.PlayerLoop.", "");
                if (playerLoopSystem.subSystemList is { Length: > 0 })
                {
                    indent = $"    {indent}";
                    foreach (var subSystem in playerLoopSystem.subSystemList)
                    {
                        LogSubSystems(subSystem, indent);
                    }
                }
            }

            #endregion
        }

        #endregion
    }
}