using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

#pragma warning disable 219

namespace Zenject.Tests.Bindings
{
	public class TestMemoryPoolLeaks : ZenjectIntegrationTestFixture
	{
		[UnityTest]
		public IEnumerator TestFactoryProperties()
		{
			// making GameObjectContext prefab with pool
			var prefab = new GameObject().AddComponent<GameObjectContext>();
			prefab.Installers = new List<MonoInstaller> {
				prefab.gameObject.AddComponent<FooInstallerScript>()
			};

			// install the prefab
			PreInstall();
			Container.Bind<GameObjectContext>().FromComponentInNewPrefab(prefab).AsTransient();
			PostInstall();

			yield return new WaitForEndOfFrame();

			// spawn GameObjectContext
			var context1 = Container.Resolve<GameObjectContext>();
			// get pool
			var pool1 = context1.Container.Resolve<Foo.Pool>();
			// get pools count
			var memoryPoolsCount1 = new List<WeakReference<IMemoryPool>>(StaticMemoryPoolRegistry.Pools).Count;

			// destroy GameObjectContext
			Object.DestroyImmediate(context1);

			// make garbage collection // todo: ensure, that GameObjectContext completely destroyed and Pool.Dispose() has been invoked
			prefab = null;
			context1 = null;
			pool1 = null;
			yield return DestroyEverything();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			GC.Collect(2);
			GC.WaitForPendingFinalizers();

			// waiting
			yield return new WaitForEndOfFrame(); 
			yield return new WaitForEndOfFrame();

			// get pools count
			var memoryPoolsCount2 = new List<WeakReference<IMemoryPool>>(StaticMemoryPoolRegistry.Pools).Count;

			// the memory leak
			Assert.IsEqual(memoryPoolsCount1 - 1, memoryPoolsCount2);

			yield break;
		}

		class Foo
		{
			public string Value
			{
				get;
				private set;
			}

			public int ResetCount
			{
				get; private set;
			}

			public class Pool : MemoryPool<string, Foo>
			{
				protected override void Reinitialize(string value, Foo foo)
				{
					foo.Value = value;
					foo.ResetCount++;
				}
			}
		}

		void TestAbstractMemoryPoolInternal()
		{
			PreInstall();
			Container.BindMemoryPool<IBar, BarPool>()
				.WithInitialSize(3).To<Bar>().NonLazy();

			PostInstall();
		}

		public interface IBar
		{
		}

		public class Bar : IBar
		{
		}

		public class BarPool : MemoryPool<int, IBar>
		{
		}

		public class FooInstallerScript : MonoInstaller<FooInstallerScript>
		{
			public override void InstallBindings()
			{
				Container.BindMemoryPool<Foo, Foo.Pool>();
			}
		}
	}
}

