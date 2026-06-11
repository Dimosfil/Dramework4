using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Initialization;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Injecting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Updating;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Testing
{
    public sealed class DWTestContainer : IDisposable
    {
        #region ================================ FIELDS

        private readonly List<Binding> _bindings = new List<Binding>();
        private readonly List<object> _trackedObjects = new List<object>();

        #endregion

        #region ================================ METHODS

        public DWTestContainer Bind<T>(T instance, string id = "")
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return Bind(typeof(T), instance, id);
        }

        public DWTestContainer Bind(Type contractType, object instance, string id = "")
        {
            if (contractType == null) throw new ArgumentNullException(nameof(contractType));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (contractType.IsInstanceOfType(instance) == false)
                throw new InvalidOperationException($"Instance type '{instance.GetType()}' is not assignable to '{contractType}'.");

            _bindings.Add(Binding.FromInstance(contractType, instance, GetID(instance, id)));
            Track(instance);
            return this;
        }

        public DWTestContainer Bind<TContract, TImplementation>(string id = "", bool asSingle = true)
            where TImplementation : class, TContract
        {
            _bindings.Add(Binding.FromImplementation(typeof(TContract), typeof(TImplementation), id, asSingle));
            return this;
        }

        public DWTestContainer Bind<TImplementation>(string id = "", bool asSingle = true)
            where TImplementation : class
        {
            _bindings.Add(Binding.FromImplementation(typeof(TImplementation), typeof(TImplementation), id, asSingle));
            return this;
        }

        public T Resolve<T>(string id = "")
        {
            return (T)Resolve(typeof(T), id);
        }

        public object Resolve(Type type, string id = "")
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var binding = FindBinding(type, id);
            if (binding != null)
                return ResolveBinding(binding);

            if (type.IsAbstract || type.IsInterface)
                throw new InvalidOperationException($"No binding found for '{type}' with id '{id}'.");

            return Create(type);
        }

        public T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        public object Create(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var instance = CreateInstance(type);
            Inject(instance);
            Track(instance);
            return instance;
        }

        public void Inject(object target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            var type = target.GetType();
            InjectFields(target, type);
            InjectProperties(target, type);
        }

        public async UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            foreach (var obj in Ordered<IPreInitializable>(_ => 0))
                obj.Object.OnPreInitialize();

            foreach (var obj in Ordered<IInitializable>(target => target.GetType().GetCustomAttribute<InitializeOrderAttribute>()?.Order ?? 0))
            {
                await obj.Object.OnInitialize(cancellationToken);
                if (cancellationToken.IsCancellationRequested) return;
            }
        }

        public async UniTask StartAsync(CancellationToken cancellationToken = default)
        {
            foreach (var obj in Ordered<IStartable>(target => target.GetType().GetCustomAttribute<StartOrderAttribute>()?.Order ?? 0))
            {
                await obj.Object.OnStart(cancellationToken);
                if (cancellationToken.IsCancellationRequested) return;
            }
        }

        public async UniTask InitializeAndStartAsync(CancellationToken cancellationToken = default)
        {
            await InitializeAsync(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;

            await StartAsync(cancellationToken);
        }

        public void Dispose()
        {
            for (var i = _trackedObjects.Count - 1; i >= 0; i--)
            {
                if (_trackedObjects[i] is IDisposable disposable)
                    disposable.Dispose();
            }

            _trackedObjects.Clear();
            _bindings.Clear();
        }

        private object ResolveBinding(Binding binding)
        {
            if (binding.Instance != null)
                return binding.Instance;

            var instance = Create(binding.ImplementationType);
            if (binding.AsSingle)
                binding.Instance = instance;

            return instance;
        }

        private object CreateInstance(Type type)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var constructors = type.GetConstructors(flags);
            var injectConstructor = constructors.FirstOrDefault(constructor => constructor.GetCustomAttribute<InjectAttribute>() != null);

            if (injectConstructor == null)
                return Activator.CreateInstance(type, flags, null, new object[0], CultureInfo.InvariantCulture);

            var parameters = injectConstructor.GetParameters();
            var values = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var id = parameter.GetCustomAttribute<IDAttribute>()?.ID ?? string.Empty;
                values[i] = Resolve(parameter.ParameterType, id);
            }

            return injectConstructor.Invoke(values);
        }

        private void InjectFields(object target, Type type)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var field in type.GetFields(flags))
            {
                var inject = field.GetCustomAttribute<InjectAttribute>();
                if (inject != null)
                    field.SetValue(target, ResolveValue(field.FieldType, inject.ID));

                if (field.GetCustomAttribute<InjectInsideAttribute>() == null) continue;

                var nested = field.GetValue(target);
                if (nested != null)
                    Inject(nested);
            }
        }

        private void InjectProperties(object target, Type type)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var property in type.GetProperties(flags))
            {
                if (property.GetIndexParameters().Length > 0) continue;

                var inject = property.GetCustomAttribute<InjectAttribute>();
                if (inject != null)
                {
                    var setter = property.GetSetMethod(true);
                    if (setter == null)
                        throw new InvalidOperationException($"Property '{property.Name}' on '{type}' has [Inject] but no setter.");

                    setter.Invoke(target, new[] { ResolveValue(property.PropertyType, inject.ID) });
                }

                if (property.GetCustomAttribute<InjectInsideAttribute>() == null) continue;

                var getter = property.GetGetMethod(true);
                if (getter == null) continue;

                var nested = getter.Invoke(target, null);
                if (nested != null)
                    Inject(nested);
            }
        }

        private object ResolveValue(Type type, string id)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                if (elementType == null) return null;

                var objects = ResolveAll(elementType, id).ToArray();
                var array = Array.CreateInstance(elementType, objects.Length);
                for (var i = 0; i < objects.Length; i++)
                    array.SetValue(objects[i], i);

                return array;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = type.GetGenericArguments()[0];
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                foreach (var obj in ResolveAll(elementType, id))
                    list.Add(obj);

                return list;
            }

            return Resolve(type, id);
        }

        private IEnumerable<object> ResolveAll(Type type, string id)
        {
            var normalizedID = id ?? string.Empty;
            var bindings = _bindings
                .Where(binding => Matches(binding.ContractType, type))
                .Where(binding => string.IsNullOrEmpty(normalizedID) || binding.ID == normalizedID)
                .ToList();

            foreach (var binding in bindings)
                yield return ResolveBinding(binding);
        }

        private Binding FindBinding(Type type, string id)
        {
            var normalizedID = id ?? string.Empty;
            return _bindings.LastOrDefault(binding => Matches(binding.ContractType, type) && binding.ID == normalizedID);
        }

        private static bool Matches(Type bindingType, Type requestedType)
        {
            return requestedType.IsAssignableFrom(bindingType) || bindingType == requestedType;
        }

        private static string GetID(object instance, string explicitID)
        {
            if (string.IsNullOrEmpty(explicitID) == false)
                return explicitID;

            return instance is IIdentifiable identifiable ? identifiable.ID : string.Empty;
        }

        private IEnumerable<OrderedObject<T>> Ordered<T>(Func<T, int> orderSelector) where T : class
        {
            return _trackedObjects
                .OfType<T>()
                .Distinct()
                .Select(obj => new OrderedObject<T>(obj, orderSelector(obj)))
                .OrderBy(obj => obj.Order);
        }

        private void Track(object obj)
        {
            if (_trackedObjects.Contains(obj)) return;
            _trackedObjects.Add(obj);
        }

        #endregion

        private sealed class Binding
        {
            #region ================================ FIELDS

            internal readonly Type ContractType;
            internal readonly Type ImplementationType;
            internal readonly string ID;
            internal readonly bool AsSingle;
            internal object Instance;

            #endregion

            #region ================================ CONSTRUCTORS AND DESTRUCTOR

            private Binding(Type contractType, Type implementationType, object instance, string id, bool asSingle)
            {
                ContractType = contractType;
                ImplementationType = implementationType;
                Instance = instance;
                ID = id ?? string.Empty;
                AsSingle = asSingle;
            }

            #endregion

            #region ================================ METHODS

            internal static Binding FromInstance(Type contractType, object instance, string id)
            {
                return new Binding(contractType, instance.GetType(), instance, id, true);
            }

            internal static Binding FromImplementation(Type contractType, Type implementationType, string id, bool asSingle)
            {
                return new Binding(contractType, implementationType, null, id, asSingle);
            }

            #endregion
        }

        private readonly struct OrderedObject<T>
        {
            #region ================================ FIELDS

            internal readonly T Object;
            internal readonly int Order;

            #endregion

            #region ================================ CONSTRUCTORS AND DESTRUCTOR

            internal OrderedObject(T obj, int order)
            {
                Object = obj;
                Order = order;
            }

            #endregion
        }
    }
}
