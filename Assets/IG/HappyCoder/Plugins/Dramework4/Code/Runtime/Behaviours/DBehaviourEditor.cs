#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Getting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Sirenix.OdinInspector;

using Unity.IL2CPP.CompilerServices;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Behaviours
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public partial class DBehaviour
    {
        #region ================================ FIELDS

        [SerializeField] [HideInInspector]
        private bool _isInitializedInEditor;

        [SerializeField] [HideInInspector]
        private Texture _logoTexture;

        private string _behaviourTag;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private Texture LogoTexture
        {
            get
            {
                if (_logoTexture != null) return _logoTexture;
                return _logoTexture = Resources.Load<Texture>(EditorGUIUtility.isProSkin ? "D Framework 4 Logo Dark" : "D Framework 4 Logo Light");
            }
        }

        private string TAG
        {
            get
            {
                if (string.IsNullOrEmpty(_behaviourTag) == false) return _behaviourTag;
                return _behaviourTag = $"[{GetType().Name}] :";
            }
        }

        #endregion

        #region ================================ METHODS

        [ContextMenu("Initialize")]
        protected void InitializeFields()
        {
            InitializeObject(GetType());
            OnEditorInitialize();
            ApplyPrefabInstance();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        protected virtual void OnEditorInitialize()
        {
        }

        private void ApplyPrefabInstance()
        {
            if (gameObject == null
                || gameObject.activeInHierarchy == false
                || PrefabUtility.IsPartOfAnyPrefab(gameObject) == false) return;

            PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.UserAction);
        }

        [OnInspectorGUI]
        [PropertyOrder(-10000)]
        private void DrawHeader()
        {
            GUILayout.Label(LogoTexture);
        }

        private void InitializeArray(FieldInfo fieldInfo, Type fieldType, GetComponentAttribute attribute)
        {
            var elementType = fieldType.GetElementType();
            if (elementType == null) return;
            var components = attribute switch
            {
                GetComponentInChildrenAttribute => GetComponentsInChildren(elementType, true)
                    .Where(component => (attribute.IncludingThisObject
                                         || component.gameObject != gameObject)
                                        && (attribute.IgnoreName
                                            ? component
                                            : string.Equals(DW4.StringTools.ClearText(component.name), string.IsNullOrEmpty(attribute.ObjectName)
                                                ? DW4.StringTools.ClearText(fieldInfo.Name)
                                                : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray(),
                GetComponentInParentAttribute => GetComponentsInParent(elementType, true)
                    .Where(component => (attribute.IncludingThisObject
                                         || component.gameObject != gameObject)
                                        && (attribute.IgnoreName
                                            ? component
                                            : string.Equals(DW4.StringTools.ClearText(component.name), string.IsNullOrEmpty(attribute.ObjectName)
                                                ? DW4.StringTools.ClearText(fieldInfo.Name)
                                                : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray(),
                GetComponentOnSceneAttribute => gameObject.activeInHierarchy
                    ? gameObject.scene.GetRootGameObjects()
                        .SelectMany(ro => ro.GetComponentsInChildren(elementType, true))
                        .Where(component => (attribute.IncludingThisObject
                                             || component.gameObject != gameObject)
                                            && (attribute.IgnoreName
                                                ? component
                                                : string.Equals(DW4.StringTools.ClearText(component.name), string.IsNullOrEmpty(attribute.ObjectName)
                                                    ? DW4.StringTools.ClearText(fieldInfo.Name)
                                                    : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase)))
                        .ToArray()
                    : null,
                not null => GetComponents(elementType),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute), null, null)
            };

            if (components == null) return;

            if (components.Length == 0)
            {
                if (attribute.Log)
                    DW4.LogError($"{TAG} No one component type of \"{elementType}\" for field \"{fieldInfo.Name}\" in object type of \"{GetType()}\" is not found", this);

                fieldInfo.SetValue(this, null);
                return;
            }

            var array = (IList)Array.CreateInstance(elementType, components.Length);
            for (var i = 0; i < array.Count; i++)
                array[i] = components[i];
            fieldInfo.SetValue(this, array);
        }

        private void InitializeList(FieldInfo fieldInfo, Type fieldType, GetComponentAttribute attribute)
        {
            var elementType = fieldType.GetGenericArguments()[0];
            var components = attribute switch
            {
                GetComponentInChildrenAttribute => GetComponentsInChildren(elementType, true)
                    .Where(component => (attribute.IncludingThisObject
                                         || component.gameObject != gameObject)
                                        && (attribute.IgnoreName
                                            ? component
                                            : string.Equals(DW4.StringTools.ClearText(component.name), string.IsNullOrEmpty(attribute.ObjectName)
                                                ? DW4.StringTools.ClearText(fieldInfo.Name)
                                                : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray(),
                GetComponentInParentAttribute => GetComponentsInParent(elementType, true)
                    .Where(component => (attribute.IncludingThisObject
                                         || component.gameObject != gameObject)
                                        && (attribute.IgnoreName
                                            ? component
                                            : string.Equals(DW4.StringTools.ClearText(component.name), string.IsNullOrEmpty(attribute.ObjectName)
                                                ? DW4.StringTools.ClearText(fieldInfo.Name)
                                                : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray(),
                GetComponentOnSceneAttribute => gameObject.activeInHierarchy
                    ? gameObject.scene.GetRootGameObjects()
                        .SelectMany(ro => ro.GetComponentsInChildren(elementType, true))
                        .Where(component => (attribute.IncludingThisObject
                                             || component.gameObject != gameObject)
                                            && (attribute.IgnoreName
                                                ? component
                                                : string.Equals(DW4.StringTools.ClearText(component.name), string.IsNullOrEmpty(attribute.ObjectName)
                                                    ? DW4.StringTools.ClearText(fieldInfo.Name)
                                                    : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase)))
                        .ToArray()
                    : Array.Empty<Component>(),
                not null => GetComponents(elementType),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute), null, null)
            };

            if (components.Length == 0)
            {
                if (attribute.Log)
                    DW4.LogError($"{TAG} No one component type of \"{fieldInfo.FieldType}\" for field \"{fieldInfo.Name}\" in object type of \"{GetType()}\" is not found", this);

                fieldInfo.SetValue(this, null);
                return;
            }

            var constructedListType = typeof(List<>).MakeGenericType(elementType);
            var list = (IList)Activator.CreateInstance(constructedListType);
            for (var i = 0; i < components.Length; i++)
                list.Add(components[i]);
            fieldInfo.SetValue(this, list);
        }

        private void InitializeObject()
        {
            if (this == null || (gameObject != null && gameObject.scene.name == null) || _isInitializedInEditor) return;
            InitializeObject(GetType());
            OnEditorInitialize();
            _isInitializedInEditor = true;
        }

        private void InitializeObject(Type type)
        {
            var baseType = type.BaseType;
            if (baseType != null && baseType != typeof(DBehaviour))
                InitializeObject(baseType);

            var fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fieldInfo in fieldInfos)
            {
                // инициализируем и валидируем только приватные, сериализуемые поля
                if (fieldInfo.IsPrivate && fieldInfo.GetCustomAttribute<SerializeField>() == null) continue;

                // пробуем получить любой атрибут, унаследованный от FindOnAttribute
                var findOnAttribute = fieldInfo.GetCustomAttribute<GetComponentAttribute>();
                if (findOnAttribute != null)
                {
                    var fieldType = fieldInfo.FieldType;
                    if (fieldType.IsArray)
                        InitializeArray(fieldInfo, fieldType, findOnAttribute);
                    else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                        InitializeList(fieldInfo, fieldType, findOnAttribute);
                    else
                        InitializeObject(fieldInfo, findOnAttribute);
                }
            }
        }

        private void InitializeObject(FieldInfo fieldInfo, GetComponentAttribute attribute)
        {
            if (gameObject == null) return;

            var component = attribute switch
            {
                GetComponentInChildrenAttribute => GetComponentsInChildren(fieldInfo.FieldType, true)
                    .Where(c =>
                    {
                        if (attribute.IncludingThisObject) return true;
                        return c.gameObject != gameObject;
                    })
                    .FirstOrDefault(c =>
                    {
                        if (attribute.IgnoreName) return c;
                        return string.Equals(DW4.StringTools.ClearText(c.name), string.IsNullOrEmpty(attribute.ObjectName)
                            ? DW4.StringTools.ClearText(fieldInfo.Name)
                            : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase);
                    }),

                GetComponentInParentAttribute => GetComponentsInParent(fieldInfo.FieldType, true)
                    .Where(c =>
                    {
                        if (attribute.IncludingThisObject) return true;
                        return c.gameObject != gameObject;
                    })
                    .FirstOrDefault(c =>
                    {
                        if (attribute.IgnoreName) return c;
                        return string.Equals(DW4.StringTools.ClearText(c.name), string.IsNullOrEmpty(attribute.ObjectName)
                            ? DW4.StringTools.ClearText(fieldInfo.Name)
                            : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase);
                    }),

                GetComponentOnSceneAttribute => gameObject.scene.GetRootGameObjects()
                    .SelectMany(ro => ro.GetComponentsInChildren(fieldInfo.FieldType, true))
                    .Where(c =>
                    {
                        if (attribute.IncludingThisObject) return true;
                        return c.gameObject != gameObject;
                    })
                    .FirstOrDefault(c =>
                    {
                        if (attribute.IgnoreName) return c;
                        return string.Equals(DW4.StringTools.ClearText(c.name), string.IsNullOrEmpty(attribute.ObjectName)
                            ? DW4.StringTools.ClearText(fieldInfo.Name)
                            : DW4.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase);
                    }),

                not null => GetComponent(fieldInfo.FieldType),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute), null, null)
            };

            if (component == null)
            {
                if (attribute.Log)
                    DW4.LogError($"{TAG} Component type of \"{fieldInfo.FieldType}\" for field \"{fieldInfo.Name}\" " +
                                 $"in object type of \"{GetType()}\" is not found " +
                                 $"by name \"{(string.IsNullOrEmpty(attribute.ObjectName) ? fieldInfo.Name : attribute.ObjectName)}\"", this);
                return;
            }
            fieldInfo.SetValue(this, component);
        }

        private void Reset()
        {
            InitializeFields();
        }

        #endregion
    }
}

#endif