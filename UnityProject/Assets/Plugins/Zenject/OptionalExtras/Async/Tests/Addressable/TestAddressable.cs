#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
using System;
using Zenject;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Assert = NUnit.Framework.Assert;

public class TestAddressable : ZenjectIntegrationTestFixture
{
    private AssetReferenceT<GameObject> addressablePrefabReference;

    [TearDown]
    public void Teardown()
    {
        addressablePrefabReference = null;
        Resources.UnloadUnusedAssets();
    }
    
    [UnityTest]
    public IEnumerator TestAddressableAsyncLoad()
    {
        yield return ValidateTestDependency();
        
        PreInstall();
        AsyncOperationHandle<GameObject> handle = default;
        Container.BindAsync<GameObject>().FromMethod(async () =>
        {
            try
            {
                var locationsHandle = Addressables.LoadResourceLocationsAsync("TestAddressablePrefab");
                await locationsHandle.Task;
                Assert.Greater(locationsHandle.Result.Count, 0, "Key required for test is not configured. Check Readme.txt in addressable test folder");

                IResourceLocation location = locationsHandle.Result[0];
                handle = Addressables.LoadAsset<GameObject>(location);
                await handle.Task;
                return handle.Result;
            }
            catch (InvalidKeyException)
            {
                
            }
            return null;
        }).AsCached();
        PostInstall();

        yield return null;
            
        AsyncInject<GameObject> asycFoo = Container.Resolve<AsyncInject<GameObject>>();

        int frameCounter = 0;
        while (!asycFoo.HasResult && !asycFoo.IsFaulted)
        {
            frameCounter++;
            if (frameCounter > 10000)
            {
                Addressables.Release(handle);
                Assert.Fail();
            }
            yield return null;    
        }
            
        Addressables.Release(handle);
        Assert.Pass();
    }
    
    [UnityTest]
    public IEnumerator TestAssetReferenceTMethod()
    {
        yield return ValidateTestDependency();

        PreInstall();

        Container.BindAsync<GameObject>()
            .FromAssetReferenceT(addressablePrefabReference)
            .AsCached();
        PostInstall();

        AsyncInject<GameObject> asycFoo = Container.Resolve<AsyncInject<GameObject>>();

        int frameCounter = 0;
        while (!asycFoo.HasResult && !asycFoo.IsFaulted)
        {
            frameCounter++;
            if (frameCounter > 10000)
            {
                Assert.Fail();
            }
            yield return null;    
        }
            
        Addressables.Release(asycFoo.Result);
        Assert.Pass();
    }

    private IEnumerator ValidateTestDependency()
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = default(AsyncOperationHandle<IList<IResourceLocation>>);
        try
        {
            locationsHandle = Addressables.LoadResourceLocationsAsync("TestAddressablePrefab");
        }
        catch (Exception e)
        {
            Assert.Inconclusive("You need to set TestAddressablePrefab key to run this test");
            yield break;
        }
        
        while (!locationsHandle.IsDone)
        {
            yield return null;
        }

        var locations = locationsHandle.Result;
        if (locations == null || locations.Count == 0)
        {
            Assert.Inconclusive("You need to set TestAddressablePrefab key to run this test");
        }

        var resourceLocation = locations[0];

        if (resourceLocation.ResourceType != typeof(GameObject))
        {
            Assert.Inconclusive("TestAddressablePrefab should be a GameObject");
        }

        addressablePrefabReference = new AssetReferenceT<GameObject>(resourceLocation.PrimaryKey);
    }
}
#endif