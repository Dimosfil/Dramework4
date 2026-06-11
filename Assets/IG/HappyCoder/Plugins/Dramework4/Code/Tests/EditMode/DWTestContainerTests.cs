using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Initialization;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Injecting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Updating;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Testing;

using NUnit.Framework;


namespace IG.HappyCoder.Plugins.Dramework4.Tests.EditMode
{
    public sealed class DWTestContainerTests
    {
        #region ================================ FIELDS

        private DWTestContainer _container;

        #endregion

        #region ================================ METHODS

        [SetUp]
        public void SetUp()
        {
            _container = new DWTestContainer();
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

        [Test]
        public void Resolve_ReturnsBoundInstance()
        {
            var service = new ServiceA();

            _container.Bind<IService>(service);

            Assert.AreSame(service, _container.Resolve<IService>());
        }

        [Test]
        public void Create_UsesInjectConstructorAndID()
        {
            var primary = new IdentifiedService("primary");
            var secondary = new IdentifiedService("secondary");

            _container.Bind<IService>(primary);
            _container.Bind<IService>(secondary);

            var target = _container.Create<ConstructorTarget>();

            Assert.AreSame(secondary, target.Service);
        }

        [Test]
        public void Inject_FillsPrivateFieldsAndProperties()
        {
            var service = new ServiceA();
            var target = new FieldPropertyTarget();

            _container.Bind<IService>(service);
            _container.Inject(target);

            Assert.AreSame(service, target.FieldService);
            Assert.AreSame(service, target.PropertyService);
        }

        [Test]
        public void Inject_FillsListDependencies()
        {
            _container.Bind<IService>(new IdentifiedService("first"), "first");
            _container.Bind<IService>(new IdentifiedService("second"), "second");

            var target = new ListTarget();
            _container.Inject(target);

            Assert.AreEqual(2, target.Services.Count);
        }

        [Test]
        public void Inject_FillsArrayDependencies()
        {
            _container.Bind<IService>(new IdentifiedService("first"), "first");
            _container.Bind<IService>(new IdentifiedService("second"), "second");

            var target = new ArrayTarget();
            _container.Inject(target);

            Assert.AreEqual(2, target.Services.Length);
        }

        [Test]
        public void InjectInside_FillsNestedObject()
        {
            var service = new ServiceA();
            var target = new NestedTarget();

            _container.Bind<IService>(service);
            _container.Inject(target);

            Assert.AreSame(service, target.Nested.Service);
        }

        [Test]
        public async Task InitializeAndStartAsync_RunsLifecycleInOrder()
        {
            var log = new LifecycleLog();

            _container.Bind(log);
            _container.Bind<FirstLifecycle>();
            _container.Bind<SecondLifecycle>();

            _container.Resolve<FirstLifecycle>();
            _container.Resolve<SecondLifecycle>();

            await _container.InitializeAndStartAsync().AsTask();

            CollectionAssert.AreEqual(
                new[]
                {
                    "pre:first",
                    "pre:second",
                    "init:first",
                    "init:second",
                    "start:first",
                    "start:second"
                },
                log.Entries);
        }

        #endregion

        private interface IService
        {
        }

        private sealed class ServiceA : IService
        {
        }

        private sealed class IdentifiedService : IService, IIdentifiable
        {
            #region ================================ CONSTRUCTORS AND DESTRUCTOR

            public IdentifiedService(string id)
            {
                ID = id;
            }

            #endregion

            #region ================================ PROPERTIES AND INDEXERS

            public string ID { get; }

            #endregion
        }

        private sealed class ConstructorTarget
        {
            #region ================================ CONSTRUCTORS AND DESTRUCTOR

            [Inject]
            public ConstructorTarget([ID("secondary")] IService service)
            {
                Service = service;
            }

            #endregion

            #region ================================ PROPERTIES AND INDEXERS

            public IService Service { get; }

            #endregion
        }

        private sealed class FieldPropertyTarget
        {
            #region ================================ FIELDS

            [Inject] private IService _fieldService;

            #endregion

            #region ================================ PROPERTIES AND INDEXERS

            public IService FieldService => _fieldService;
            public IService PropertyService => PropertyServiceInternal;

            [Inject] private IService PropertyServiceInternal { get; set; }

            #endregion
        }

        private sealed class ListTarget
        {
            #region ================================ PROPERTIES AND INDEXERS

            [Inject] public List<IService> Services { get; private set; }

            #endregion
        }

        private sealed class ArrayTarget
        {
            #region ================================ PROPERTIES AND INDEXERS

            [Inject] public IService[] Services { get; private set; }

            #endregion
        }

        private sealed class NestedTarget
        {
            #region ================================ PROPERTIES AND INDEXERS

            public NestedServiceTarget Nested { get; } = new NestedServiceTarget();

            [InjectInside] private NestedServiceTarget NestedInternal => Nested;

            #endregion
        }

        private sealed class NestedServiceTarget
        {
            #region ================================ FIELDS

            [Inject] private IService _service;

            #endregion

            #region ================================ PROPERTIES AND INDEXERS

            public IService Service => _service;

            #endregion
        }

        private sealed class LifecycleLog
        {
            #region ================================ FIELDS

            public readonly List<string> Entries = new List<string>();

            #endregion
        }

        [InitializeOrder(10)]
        [StartOrder(10)]
        private sealed class SecondLifecycle : IPreInitializable, IInitializable, IStartable
        {
            #region ================================ FIELDS

            [Inject] private LifecycleLog _log;

            #endregion

            #region ================================ METHODS

            public void OnPreInitialize()
            {
                _log.Entries.Add("pre:second");
            }

            public UniTask OnInitialize(CancellationToken cancellationToken)
            {
                _log.Entries.Add("init:second");
                return UniTask.CompletedTask;
            }

            public UniTask OnStart(CancellationToken token)
            {
                _log.Entries.Add("start:second");
                return UniTask.CompletedTask;
            }

            #endregion
        }

        [InitializeOrder(1)]
        [StartOrder(1)]
        private sealed class FirstLifecycle : IPreInitializable, IInitializable, IStartable
        {
            #region ================================ FIELDS

            [Inject] private LifecycleLog _log;

            #endregion

            #region ================================ METHODS

            public void OnPreInitialize()
            {
                _log.Entries.Add("pre:first");
            }

            public UniTask OnInitialize(CancellationToken cancellationToken)
            {
                _log.Entries.Add("init:first");
                return UniTask.CompletedTask;
            }

            public UniTask OnStart(CancellationToken token)
            {
                _log.Entries.Add("start:first");
                return UniTask.CompletedTask;
            }

            #endregion
        }
    }
}
