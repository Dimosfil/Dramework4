using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Injecting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Data;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Tools;

using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class Dispatcher
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object GetArray(Type fieldType, string objID)
        {
            var elementType = fieldType.GetElementType();
            if (elementType == null) return null;
            var objects = GetObjects(elementType, objID).ToArray();
            if (objects.Any() == false) return null;
            var array = (IList)Array.CreateInstance(elementType, objects.Length);
            for (var i = 0; i < array.Count; i++)
                array[i] = objects[i];
            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object GetList(Type fieldType, string objID)
        {
            var elementType = fieldType.GetGenericArguments()[0];
            var objects = GetObjects(elementType, objID).ToArray();
            if (objects.Any() == false) return null;
            var constructedListType = typeof(List<>).MakeGenericType(elementType);
            var list = (IList)Activator.CreateInstance(constructedListType);
            for (var i = 0; i < objects.Length; i++)
                list.Add(objects[i]);
            return list;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object GetObject(string sceneName, Type objType, string objID)
        {
            if (_objects.TryGetValue(sceneName, out var objects))
            {
                var obj = FindObject(objects);
                if (obj != null) return obj;
            }

            foreach (var containerObjects in _objects.Values)
            {
                var result = FindObject(containerObjects);
                if (result != null) return result;
            }

            return null;

            object FindObject(IReadOnlyList<ContainerObject<object>> containerObjects)
            {
                for (var i = 0; i < containerObjects.Count; i++)
                {
                    var containerObject = containerObjects[i];
                    if (string.IsNullOrEmpty(objID))
                    {
                        if (objType.IsInterface)
                        {
                            if (objType.IsAssignableFrom(containerObject.Type))
                                return containerObject.Object;
                        }
                        else
                        {
                            if (containerObject.Type == objType)
                                return containerObject.Object;
                        }
                    }
                    else
                    {
                        if (objType.IsInterface)
                        {
                            if (objType.IsAssignableFrom(containerObject.Type) && containerObject.ID == objID)
                                return containerObject.Object;
                        }
                        else
                        {
                            if (containerObject.Type == objType && containerObject.ID == objID)
                                return containerObject.Object;
                        }
                    }
                }
                return null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<object> GetObjects(Type objType, string objID)
        {
            var result = new List<object>();
            foreach (var containerObjects in _objects.Values)
            {
                var obj = FindObject(containerObjects);
                if (obj != null) result.Add(obj);
            }
            return result;

            object FindObject(IReadOnlyList<ContainerObject<object>> containerObjects)
            {
                for (var i = 0; i < containerObjects.Count; i++)
                {
                    var containerObject = containerObjects[i];
                    if (string.IsNullOrEmpty(objID) && containerObject.Type == objType)
                        return containerObject.Object;
                    if (string.IsNullOrEmpty(objID) == false && containerObject.Type == objType && containerObject.ID == objID)
                        return containerObject.Object;
                }

                return null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InjectToFields(string sceneName, object obj, Type objType)
        {
            var fieldInfos = objType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var fieldInfo in fieldInfos)
            {
                var injectAttribute = fieldInfo.GetCustomAttribute<InjectAttribute>();
                if (injectAttribute != null)
                    Inject(fieldInfo, injectAttribute);

                var injectInsideAttribute = fieldInfo.GetCustomAttribute<InjectInsideAttribute>();
                if (injectInsideAttribute != null)
                    InjectInside(fieldInfo);
            }
            return;

            void Inject(FieldInfo fieldInfo, InjectAttribute injectAttribute)
            {
                if (fieldInfo.FieldType.IsArray)
                {
                    var array = GetArray(fieldInfo.FieldType, injectAttribute.ID);
                    if (array == null)
                    {
                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.LogError($"{TAG} Inject NULL to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.LogError($"{TAG} Inject NULL to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                    else
                    {
                        fieldInfo.SetValue(obj, array);

                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.Log($"{TAG} Inject object type of \"Array<{fieldInfo.FieldType.GetElementType()}>\" to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.Log($"{TAG} Inject object type of \"Array<{fieldInfo.FieldType.GetElementType()}>\" to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                }
                else if (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var list = GetList(fieldInfo.FieldType, injectAttribute.ID);
                    if (list == null)
                    {
                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.LogError($"{TAG} Inject NULL to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.LogError($"{TAG} Inject NULL to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                    else
                    {
                        fieldInfo.SetValue(obj, list);

                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.Log($"{TAG} Inject object type of \"List<{fieldInfo.FieldType.GetElementType()}>\" to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.Log($"{TAG} Inject object type of \"List<{fieldInfo.FieldType.GetElementType()}>\" to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                }
                else
                {
                    var target = GetObject(sceneName, fieldInfo.FieldType, injectAttribute.ID);
                    if (target == null)
                    {
                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.LogError($"{TAG} Inject NULL to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.LogError($"{TAG} Inject NULL to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                    else
                    {
                        fieldInfo.SetValue(obj, target);
                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                            {
                                if (target is IIdentifiable identifiableTarget)
                                    DW4.Log($"{TAG} Inject object type of \"{fieldInfo.FieldType}\", ID: \"{identifiableTarget.ID}\" to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                                else
                                    DW4.Log($"{TAG} Inject object type of \"{fieldInfo.FieldType}\" to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            }
                            else
                            {
                                if (target is IIdentifiable identifiableTarget)
                                    DW4.Log($"{TAG} Inject object type of \"{fieldInfo.FieldType}\", ID: \"{identifiableTarget.ID}\" to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\"");
                                else
                                    DW4.Log($"{TAG} Inject object type of \"{fieldInfo.FieldType}\" to field \"{fieldInfo.Name}\" of object type of \"{obj.GetType()}\"");
                            }
                        }
                    }
                }
            }

            void InjectInside(FieldInfo fieldInfo)
            {
                var field = fieldInfo.GetValue(obj);
                if (field == null) return;
                var type = field.GetType();
                InjectToFields(sceneName, field, type);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InjectToProperties(string sceneName, object obj, Type objType)
        {
            var propertyInfos = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var propertyInfo in propertyInfos)
            {
                var injectAttribute = propertyInfo.GetCustomAttribute<InjectAttribute>();
                if (injectAttribute != null)
                    Inject(propertyInfo, injectAttribute);

                var injectInsideAttribute = propertyInfo.GetCustomAttribute<InjectInsideAttribute>();
                if (injectInsideAttribute != null)
                    InjectInside(propertyInfo);
            }
            return;

            void Inject(PropertyInfo propertyInfo, InjectAttribute injectAttribute)
            {
                if (propertyInfo.PropertyType.IsArray)
                {
                    var array = GetArray(propertyInfo.PropertyType, injectAttribute.ID);
                    if (array == null)
                    {
                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.LogError($"{TAG} Inject NULL to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.LogError($"{TAG} Inject NULL to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, array);

                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.Log($"{TAG} Inject object type of \"Array<{propertyInfo.PropertyType.GetElementType()}>\" to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.Log($"{TAG} Inject object type of \"Array<{propertyInfo.PropertyType.GetElementType()}>\" to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                }
                else if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var list = GetList(propertyInfo.PropertyType, injectAttribute.ID);
                    if (list == null)
                    {
                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.LogError($"{TAG} Inject NULL to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.LogError($"{TAG} Inject NULL to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, list);

                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.Log($"{TAG} Inject object type of \"List<{propertyInfo.PropertyType.GetElementType()}>\" to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.Log($"{TAG} Inject object type of \"List<{propertyInfo.PropertyType.GetElementType()}>\" to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                }
                else
                {
                    var target = GetObject(sceneName, propertyInfo.PropertyType, injectAttribute.ID);
                    if (target == null)
                    {
                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                                DW4.LogError($"{TAG} Inject NULL to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            else
                                DW4.LogError($"{TAG} Inject NULL to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\"");
                        }
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, target);
                        if (DW4.AppConfig.LogDispatcher)
                        {
                            if (obj is IIdentifiable identifiable)
                            {
                                if (target is IIdentifiable identifiableTarget)
                                    DW4.Log($"{TAG} Inject object type of \"{propertyInfo.PropertyType}\", ID: \"{identifiableTarget.ID}\" to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                                else
                                    DW4.Log($"{TAG} Inject object type of \"{propertyInfo.PropertyType}\" to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\", ID: \"{identifiable.ID}\"");
                            }
                            else
                            {
                                if (target is IIdentifiable identifiableTarget)
                                    DW4.Log($"{TAG} Inject object type of \"{propertyInfo.PropertyType}\", ID: \"{identifiableTarget.ID}\" to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\"");
                                else
                                    DW4.Log($"{TAG} Inject object type of \"{propertyInfo.PropertyType}\" to property \"{propertyInfo.Name}\" of object type of \"{obj.GetType()}\"");
                            }
                        }
                    }
                }
            }

            void InjectInside(PropertyInfo propertyInfo)
            {
                var property = propertyInfo.GetValue(obj);
                if (property == null) return;
                var type = property.GetType();
                InjectToProperties(sceneName, property, type);
            }
        }

        #endregion
    }
}