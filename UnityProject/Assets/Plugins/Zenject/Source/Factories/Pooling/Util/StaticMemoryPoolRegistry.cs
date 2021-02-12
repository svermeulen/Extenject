using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
#if UNITY_EDITOR
	public static class StaticMemoryPoolRegistry
	{
		public static event Action<IMemoryPool> PoolAdded = delegate { };
		public static event Action<IMemoryPool> PoolRemoved = delegate { };

		readonly static List<WeakReference<IMemoryPool>> _pools = new List<WeakReference<IMemoryPool>>();

		public static IEnumerable<WeakReference<IMemoryPool>> Pools
		{
			get { return _pools; }
		}

		public static void Add(IMemoryPool memoryPool)
		{
			_pools.Add(new WeakReference<IMemoryPool>(memoryPool));
			PoolAdded(memoryPool);
		}

		public static void Remove(IMemoryPool memoryPool)
		{
			_pools.RemoveWithConfirm(new WeakReference<IMemoryPool>(memoryPool));
			PoolRemoved(memoryPool);
		}
	}
#endif
}