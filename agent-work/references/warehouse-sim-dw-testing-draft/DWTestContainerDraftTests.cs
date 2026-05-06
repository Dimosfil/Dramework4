using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Initialization;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Injecting;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Attributes.Updating;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Interfaces;

using NUnit.Framework;

namespace TestsProject.DrameworkTesting
{
    public class DWTestContainerDraftTests
    {
        private DWTestContainerDraft _container;

        [SetUp]
        public void SetUp()
        {
            _container = new DWTestContainerDraft();
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

        private interface IService
        {
        }

        private sealed class ServiceA : IService
        {
        }

        private sealed class IdentifiedService : IService, IIdentifiable
        {
            public IdentifiedService(string id)
            {
                ID = id;
            }

            public string ID { get; }
        }

        private sealed class ConstructorTarget
        {
            [Inject]
            public ConstructorTarget([ID("secondary")] IService service)
            {
                Service = service;
            }

            public IService Service { get; }
        }

        private sealed class FieldPropertyTarget
        {
            [Inject] private IService _fieldService;

            [Inject] private IService PropertyServiceInternal { get; set; }

            public IService FieldService => _fieldService;
            public IService PropertyService => PropertyServiceInternal;
        }

        private sealed class ListTarget
        {
            [Inject] public List<IService> Services { get; private set; }
        }

        private sealed class LifecycleLog
        {
            public readonly List<string> Entries = new List<string>();
        }

        [InitializeOrder(10)]
        [StartOrder(10)]
        private sealed class SecondLifecycle : IPreInitializable, IInitializable, IStartable
        {
            [Inject] private LifecycleLog _log;

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
        }

        [InitializeOrder(1)]
        [StartOrder(1)]
        private sealed class FirstLifecycle : IPreInitializable, IInitializable, IStartable
        {
            [Inject] private LifecycleLog _log;

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
        }
    }
}
