# Writing Automated Tests

## Table of contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

- [Writing Automated Unit Tests and Integration Tests](#writing-automated-unit-tests-and-integration-tests)
  - [Unit Tests](#unit-tests)
  - [Integration Tests](#integration-tests)
  - [Scene Tests](#scene-tests)
  - [User Driven Test Beds](#user-driven-test-beds)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->


## Writing Automated Unit Tests and Integration Tests

When writing properly loosely coupled code using dependency injection, it is much easier to isolate specific areas of your code base for the purposes of running tests on them without needing to fire up your entire project.  This can take the form of user-driven test-beds or fully automated tests using NUnit.  Automated tests are especially useful when used with a continuous integration server.  This allows you to automatically run the tests whenever new commits are pushed to source control.

There are three basic helper classes included with Zenject that can make it easier to write automated tests for your game.  One is for Unit Tests, the other is for Integration Tests, and the third is for Scene Tests.  All approaches are run via Unity's built in Test Runner (which also has a command line interface that you can hook up to a continuous integration server).  The main differences are that Unit Tests are much smaller in scope and meant for testing a small subset of the classes in your application, whereas Integration Tests can be more expansive and can involve firing up many different systems.  And Scene Tests are used to fire up entire scenes and then probe the state of the scene as part of the test.

This is best shown with some examples.

### Unit Tests

As an example, let's add the following class to our Unity project:

```csharp
using System;

public class Logger
{
    public Logger()
    {
        Log = "";
    }

    public string Log
    {
        get;
        private set;
    }

    public void Write(string value)
    {
        if (value == null)
        {
            throw new ArgumentException();
        }

        Log += value;
    }
}
```

To test the class do the following:
- Open up Unity's Test Runner by clicking Window -> General -> Test Runner
- Underneath the EditMode tab click "Create EditMode Test Assembly Folder". This will create a folder that contains the necessary asmdef file that is needed to get access to the Nunit namespace.
- Select the newly created asmdef file and add a reference to Zenject-TestFramework in the Unity inspector (Tests Import Settings -> Assembly Definition References)
- Right click inside the folder in the Project tab and select `Create -> Zenject -> Unit Test`.  Name it `TestLogger`.cs.  This will create a basic template that we can fill in with our tests
- Copy and paste the following:

```csharp
using System;
using Zenject;
using NUnit.Framework;

[TestFixture]
public class TestLogger : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        Container.Bind<Logger>().AsSingle();
    }

    [Test]
    public void TestInitialValues()
    {
        var logger = Container.Resolve<Logger>();

        Assert.That(logger.Log == "");
    }

    [Test]
    public void TestFirstEntry()
    {
        var logger = Container.Resolve<Logger>();

        logger.Write("foo");
        Assert.That(logger.Log == "foo");
    }

    [Test]
    public void TestAppend()
    {
        var logger = Container.Resolve<Logger>();

        logger.Write("foo");
        logger.Write("bar");

        Assert.That(logger.Log == "foobar");
    }

    [Test]
    public void TestNullValue()
    {
        var logger = Container.Resolve<Logger>();

        Assert.Throws(() => logger.Write(null));
    }
}

```

To run it, open up Unity's test runner by selecting `Window -> Test Runner`.  Then make sure the EditMode tab is selected, then click `Run All` or right click on the specific test you want to run.

As you can see above, this approach is very basic and just involves inheriting from the `ZenjectUnitTestFixture` class.  All `ZenjectUnitTestFixture` does is ensure that a new Container is re-created before each test method is called.   That's it.  This is the entire code for it:

```csharp
public abstract class ZenjectUnitTestFixture
{
    DiContainer _container;

    protected DiContainer Container
    {
        get
        {
            return _container;
        }
    }

    [SetUp]
    public virtual void Setup()
    {
        _container = new DiContainer();
    }
}
```

So typically you run installers from within `[SetUp]` methods and then directly call `Resolve<>` to retrieve instances of the classes you want to test.

You could also avoid all the calls to `Container.Resolve` by injecting into the unit test itself after finishing the install, by changing your unit test to this:

```csharp
using System;
using Zenject;
using NUnit.Framework;

[TestFixture]
public class TestLogger : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        Container.Bind<Logger>().AsSingle();
        Container.Inject(this);
    }

    [Inject]
    Logger _logger;

    [Test]
    public void TestInitialValues()
    {
        Assert.That(_logger.Log == "");
    }

    [Test]
    public void TestFirstEntry()
    {
        _logger.Write("foo");
        Assert.That(_logger.Log == "foo");
    }

    [Test]
    public void TestAppend()
    {
        _logger.Write("foo");
        _logger.Write("bar");

        Assert.That(_logger.Log == "foobar");
    }

    [Test]
    public void TestNullValue()
    {
        Assert.Throws(() => _logger.Write(null));
    }
}
```

### Integration Tests

Integration tests, on the other hand, are executed in a similar environment to the scenes in your project.  Unlike unit tests, integration tests involve a `SceneContext` and `ProjectContext`, and any bindings to IInitializable, ITickable, and IDisposable will be executed just like when running your game normally.  It achieves this by using Unity's support for 'playmode tests'.

As a very simple example, let's say we have the following class we want to test:

```csharp
public class SpaceShip : MonoBehaviour
{
    [InjectOptional]
    public Vector3 Velocity
    {
        get; set;
    }

    public void Update()
    {
        transform.position += Velocity * Time.deltaTime;
    }
}
```

To test the class do the following:
- Open up Unity's Test Runner by clicking Window -> General -> Test Runner
- Underneath the PlayMode tab click "Create PlayMode Test Assembly Folder". This will create a folder that contains the necessary asmdef file that is needed to get access to the Nunit namespace.
- Select the newly created asmdef file and add a reference to Zenject-TestFramework
- Right click inside the folder in the Project tab and select `Create -> Zenject -> Integration Test`.  Name it `SpaceShipTests`.cs.  This will create a basic template that we can fill in with our tests
- This will create the following template code with everything you need to start writing your test:

```csharp
public class SpaceShipTests : ZenjectIntegrationTestFixture
{
    [UnityTest]
    public IEnumerator RunTest1()
    {
        // Setup initial state by creating game objects from scratch, loading prefabs/scenes, etc

        PreInstall();

        // Call Container.Bind methods

        PostInstall();

        // Add test assertions for expected state
        // Using Container.Resolve or [Inject] fields
        yield break;
    }
}
```

Let's fill in some test code for our `SpaceShip` class:

```csharp
public class SpaceShipTests : ZenjectIntegrationTestFixture
{
    [UnityTest]
    public IEnumerator TestVelocity()
    {
        PreInstall();

        Container.Bind<SpaceShip>().FromNewComponentOnNewGameObject()
            .AsSingle().WithArguments(new Vector3(1, 0, 0));

        PostInstall();

        var spaceShip = Container.Resolve<SpaceShip>();

        Assert.IsEqual(spaceShip.transform.position, Vector3.zero);

        yield return null;

        // Should move in the direction of the velocity
        Assert.That(spaceShip.transform.position.x > 0);
    }
}
```

All we're doing here is ensuring that the space ship moves in the same direction as its velocity.  If we had many tests to run on `SpaceShip` we could also change it to this:

```csharp
public class SpaceShipTests : ZenjectIntegrationTestFixture
{
    void CommonInstall()
    {
        PreInstall();

        Container.Bind<SpaceShip>().FromNewComponentOnNewGameObject()
            .AsSingle().WithArguments(new Vector3(1, 0, 0));

        PostInstall();
    }

    [Inject]
    SpaceShip _spaceship;

    [UnityTest]
    public IEnumerator TestInitialState()
    {
        CommonInstall();

        Assert.IsEqual(_spaceship.transform.position, Vector3.zero);
        Assert.IsEqual(_spaceship.Velocity, new Vector3(1, 0, 0));
        yield break;
    }

    [UnityTest]
    public IEnumerator TestVelocity()
    {
        CommonInstall();

        // Wait one frame to allow update logic for SpaceShip to run
        yield return null;

        // Should move in the direction of the velocity
        Assert.That(_spaceship.transform.position.x > 0);
    }
}
```

After `PostInstall()` is called, our integration test is injected, so we can define `[Inject]` fields on it like above if we don't want to call `Container.Resolve` for every test.

Note that we can yield our coroutine to test behaviour across time.  If you are unfamiliar with how Unity's test runner works (and in particular how 'playmode test' work) please see the [unity documentation](https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html).

Every zenject integration test is broken up into three phases:

- Before PreInstall - Set up the initial scene how you want for your test. This could involve loading prefabs from the Resources directory, creating new GameObject's from scratch, etc.
- After PreInstall - Install all the bindings to the Container that you need for your test
- After PostInstall - At this point, all the non-lazy objects that we've bound to the container have been instantiated, all objects in the scene have been injected, and all `IInitializable.Initialize` methods have been called.  So we can now start adding Assert's to test the state, manipulate the runtime state of the objects, etc.  Note that you will have to yield once immediately after PostInstall if you want the MonoBehaviour start methods to run as well.

### Scene Tests

Scene tests work by actually running the scene alongside the test, then accessing dependencies in the scene through the scene DiContainer, then making changes to it or simply verifying expected state.  To take the included SpaceFighter game as an example, we might want to ensure that our simple AI for the enemy ships runs as expected:

```csharp
public class SpaceFighterTests : SceneTestFixture
{
    [UnityTest]
    public IEnumerator TestEnemyStateChanges()
    {
        // Override settings to only spawn one enemy to test
        StaticContext.Container.BindInstance(
            new EnemySpawner.Settings()
            {
                SpeedMin = 50,
                SpeedMax = 50,
                AccuracyMin = 1,
                AccuracyMax = 1,
                NumEnemiesIncreaseRate = 0,
                NumEnemiesStartAmount = 1,
            });

        yield return LoadScene("SpaceFighter");

        var enemy = SceneContainer.Resolve<EnemyRegistry>().Enemies.Single();

        // Should always start by chasing the player
        Assert.IsEqual(enemy.State, EnemyStates.Follow);

        // Wait a frame for AI logic to run
        yield return null;

        // Our player mock is always at position zero, so if we move the enemy there then the enemy
        // should immediately go into attack mode
        enemy.Position = Vector3.zero;

        // Wait a frame for AI logic to run
        yield return null;

        Assert.IsEqual(enemy.State, EnemyStates.Attack);

        enemy.Position = new Vector3(100, 100, 0);

        // Wait a frame for AI logic to run
        yield return null;

        // The enemy is very far away now, so it should return to searching for the player
        Assert.IsEqual(enemy.State, EnemyStates.Follow);
    }
}
```

Note that you can add your own scene tests through the right click menu in the Projects tab by choosing `Create -> Zenject -> Scene Test`.  Note that they will require an asmdef file set up in a similar way to integration tests as described above.

Every scene test should inherit from SceneTestFixture, and then at some point in each test method it should call `yield return LoadScene(NameOfScene)`

Before calling LoadScene it is sometimes useful to configure some settings that will get injected into our scene.  We can do this by adding bindings to the StaticContext.   StaticContext is the parent context of ProjectContext, and so will be inherited by all dependencies.  In this case, we want to configure the EnemySpawner class to only spawn one enemy because that's all we need for our simple test.

Note that the test will fail if any errors are logged to the console during the execution of the test method, or if any exceptions are thrown.

Scene tests can be particularly useful when combined with a continuous integration test server.  Even tests as simple as the following can be invaluable to ensure that each scene starts without errors:

```csharp
public class TestSceneStartup : SceneTestFixture
{
    [UnityTest]
    public IEnumerator TestSpaceFighter()
    {
        yield return LoadScene("SpaceFighter");

        // Wait a few seconds to ensure the scene starts correctly
        yield return new WaitForSeconds(2.0f);
    }

    [UnityTest]
    public IEnumerator TestAsteroids()
    {
        yield return LoadScene("Asteroids");

        // Wait a few seconds to ensure the scene starts correctly
        yield return new WaitForSeconds(2.0f);
    }
}
```

Note that the scene name that you pass to the LoadScene method must be added to the build settings for it to be loaded properly

If you want to test multiple scenes being loaded at once, you can do that too, by using LoadScenes instead of LoadScene, like this:

```csharp
public class TestSceneStartup : SceneTestFixture
{
    [UnityTest]
    public IEnumerator TestSpaceFighter()
    {
        yield return LoadScenes("SpaceFighterMenu", "SpaceFighterEnvironment");

        // Wait a few seconds to ensure the scene starts correctly
        yield return new WaitForSeconds(2.0f);
    }
}
```

In this case, it will inject your SceneTestFixture derived class with the last loaded SceneContext container which will also be set to the SceneContainer property.  If you want to access the other scene containers you can do that too using the SceneContainers property.

Note that if you are executing a particularly long test, you might have to increase the timeout value which defaults to 30 seconds.  For example:

```csharp
public class LongTestExample : SceneTestFixture
{
    [UnityTest]
    [Timeout(60000)]
    public IEnumerator ExecuteSoakTest()
    {
        ...
    }
}
```

### User Driven Test Beds

A fourth common approach to testing worth mentioning is User Driven Test Beds.  This just involves creating a new scene with a SceneContext etc. just as you do for production scenes, except installing only a subset of the bindings that you would normally include in the production scenes, and possibly mocking out certain parts that you don't need to test.  Then, by iterating on the system you are working on using this test bed, it can be much faster to make progress rather than needing to fire up your normal production scene.

This might also be necessary if the functionality you want to test is too complex for a unit test or an integration test.

The only drawback with this approach is that it isn't automated and requires a human to run - so you can't have these tests run as part of a continuous integration server

