using GameServerAlpha.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent.Test
{
    [TestFixture]
    public class TestGlobalEvent
    {
        Mock<IGlobalEventListener> listener;

        AssemblyTypeLoader loader = null;
        [SetUp]
        public async Task SetUp()
        {
            if (loader == null)
            {
                loader = new AssemblyTypeLoader();
                loader.Load();
            }

            await GlobalEventManager.Instance.ClearEvents();
            if (listener != null)
            {
                GlobalEventManager.Instance.RemoveListener(listener.Object);
            }

            listener = new Mock<IGlobalEventListener>();
            listener.Setup(x => x.Filter).Returns(GlobalEventFilter.User);
            GlobalEventManager.Instance.AddListener(listener.Object);
        }

        [Test]
        public void TestLoadCommand()
        {
            foreach (var name in GlobalEventManager.Instance.LoadedCommandNames())
            {
                TestContext.WriteLine(name);
            }
        }

        [Test]
        [TestCase(1500, 1500)]
        [TestCase(500, 1000)]
        [TestCase(3000, 3100)]
        public async Task TestTimer(int step1Time, int step2Time)
        {
            var builder = new GlobalEventSpec.Builder(1)
                .SetFilter(GlobalEventFilter.All)
                .SetTime(DateTime.Now.AddSeconds(1), DateTime.Now.AddSeconds(2))
                .AddParam(new GlobalEventParam() { Type = GlobalEventParamType.Gold, Id = 1, Value = 100 })
                .SetCommandType(GlobalEventCommandType.Test);
            GlobalEventSpec spec = builder.Build();
            await GlobalEventManager.Instance.AddEvent(spec);

            await Task.Delay(step1Time);
            Assert.IsTrue(GlobalEventManager.Instance.GetEvent(1).IsStarted);
            await Task.Delay(step2Time);
            Assert.IsNull(GlobalEventManager.Instance.GetEvent(1));
        }

        [TestCase(GlobalEventFilter.User)]
        public async Task TestFilter(GlobalEventFilter filter)
        {
            var builder = new GlobalEventSpec.Builder(1)
                .SetFilter(filter)
                .SetTime(DateTime.Now.AddSeconds(0.5), DateTime.Now.AddSeconds(1))
                .AddParam(new GlobalEventParam() { Type = GlobalEventParamType.Gold, Id = 1, Value = 100 })
                .SetCommandType(GlobalEventCommandType.Test);
            GlobalEventSpec spec = builder.Build();

            await GlobalEventManager.Instance.AddEvent(spec);


            await Task.Delay(1500);
            Assert.Multiple(() =>
            {
                listener.Verify(foo => foo.DoSomthing("start"));
                listener.Verify(foo => foo.DoSomthing("end"));
                listener.Verify(foo => foo.TakeParam(1, 100));
            });
        }
    }
}
