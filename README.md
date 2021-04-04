# EntitasGenericAddon
Addon to [Entitas](https://github.com/sschmid/Entitas-CSharp) that allows using generic methods instead of code generator and uses type inference to ensure compile time correctness

This project is based on [Entitas.Generic](https://github.com/yosadchyi/Entitas.Generic)

## Goal
Make Entitas extensible by separate dll

## Main Concepts
  - Code Generator is optional
  - Type inference forces only valid type combinations during development and gives access only to needed methods
  - Interfaces serve as markers for type inference
      - `IScope` - base interface for context scope
      - `Scope<T>` - context scope of IComponent
      - `IComponent` - allows class to be managed by **Entitas**, gives access to `Get<T>` extension methods
      - `ICompData` - gives access to `Add`, `Remove`, `Replace`, `Has` (class componenents require `ICopyFrom<TSelf>`).
      - `ICreateApply` - alternative workflow for class components that doesn't require `ICopyFrom<TSelf>`
      - `ICompFlag` - gives access to `Flag<T>`, `Is<T>`
      - `IUnique` - provides context `Add`, `Replace` etc methods for unique components or flags

  - Generic Events Feature
      - `IEvent_*<TScope, TComp>` - interface marker for `IComponent` classes
      - `IOn*<TScope, TComp>` - interface to implement by listener classes
      - `EventSystem_*<TScope, TComp>` - event system classes

  - Struct components API method names end with underscore (`Add_`, `Remove_`, `Replace_`, `Has_`, `Event_System_*_`)

  - Manual `EntityIndex` registration

## Installation
  - Install [Entitas](https://github.com/sschmid/Entitas-CSharp) framework into your project (tested with Entitas v1.13.0 but more recent may be also compatible)
  - Install EntitasGenericAddon into your project:
    - Preferred. Without using Entitas generator - copy `Entitas.Generic`, `Entitas.Generic.Events` sources into `Assets` folder somewhere
    - Not recommended. With existing generated Contexts - copy `Entitas.Generic`, `Entitas.Generic.Events` sources into same assembly as generated `Contexts` class. For now it only adds new generic contexts, generated and generic context instances have different workflows. Improvements are welcome

Warning: Please test project on target devices as soon as possible, and then regularly to avoid any pitfalls and show stoppers.

## Examples
For more examples see [EntitasGenericAddon.Examples](https://github.com/c0ffeeartc/EntitasGenericAddon.Examples)

## Usage

```csharp
public void Example()
{
    // initialization must be called before new Contexts( ) or accessing Contexts.sharedInstance
    Lookup_ScopeManager.RegisterAll( );  // <-- 1

    var contexts = Contexts.sharedInstance;  // <-- 2
    // var contexts = new Contexts(  ); // <-- 2. If generator is not used in project

    contexts.AddScopedContexts(  ); // <-- 3

    var game = contexts.Get<Game>( );

    var entity = game.CreateEntity( );
    entity.Add( Cache<Position>.I.Set(3f, 10f) );
    entity.Replace( new Position( 20f, 1f ) );
    entity.Has<Position>( );
    entity.Remove<Position>( );

    entity.Flag<Moving>( true );
    entity.Is<Moving>( );
    entity.Flag<Moving>( false );

    game.Flag<GameActive>( true );  // provides unique component interfaces
    var gameActiveEntity = game.GetEntity<GameActive>( );
}
```

#### Scope
```csharp
public interface Game : IScope { }
```

#### Component
```csharp
public struct B : IComponent, ICompData, Scope<Game>
{
    public int Value;
}

public sealed class A
        : IComponent
        , ICompData
        , ICopyFrom<A>  // Not needed for struct components
        , Scope<Game>
{
    public int Value;

    public A Set(int value)  // optional, allows using Cache<T>.I.Set(). Not needed for struct components
    {
        Value = value;
        return this;
    }

    public void CopyFrom(A other)  // Not needed for struct components
    {
        Value = other.Value;
    }
}
```

`ICreateApply` alternative workflow for class components doesn't require to implement `ICopyFrom<T>`.

> It's still recommended to create `Set` method that ensures initializing all values even after refactoring(in rider it's as easy as writing `ctorf`, pressing tab and renaming method to Set)
  
```csharp
// Step1
public sealed class A
        : IComponent
        , ICompData
        , ICreateApply
        , Scope<Game>
{
    public int Value;
    
    public A Set (value)  // Optional
    {
        Value = value;
        return this;
    }
}

// Step2
var comp = entity.Create<A>(  );
comp.Value = 1f;
entity.Apply( comp );

// or recommended using Set method
entity.Apply( entity.Create<A>(  )
    .Set( 1f ) );
```

#### FlagComponent
```csharp
public sealed class FlagA : IComponent, ICompFlag, Scope<Game> { }
```

#### Matcher
```csharp
Matcher<Entity<Game>>
    .AllOf(
        Matcher<Game, Position>.I,
        Matcher<Game, Velocity>.I )
    .AnyOf(
        Matcher<Game, Moving>.I )
    .NoneOf(
        Matcher<Game, Destroy>.I ) );
```

#### Events
```csharp
// Step 1. Add event markers to components
public sealed class FlagA : IComponent, ICompFlag, Scope<Game>
    , IEvent_Any<Game, FlagA> // <---
{ }
public sealed class B : IComponent, ICompData, ICopyFrom<B>, Scope<Game>
    , IEvent_SelfRemoved<Game, B>  // <---
{
    // some code
}

    
// Step 2. Add event systems to Systems. This step could be automated in future
    systems.Add( new EventSystem_SelfRemoved<Game, B>(  ) );
    systems.Add( new EventSystem_Any<Game, FlagA>(  ) );


// Step 3. Inherit and implement event interface
public class Some
    : MonoBehaviour
    , IOnAny<Game, FlagA>
    , IOnSelfRemoved<Game, B>
{
private void OnAny( FlagA component, Entity<Game> entity, Contexts contexts )
{
    // some code
}

private void OnSelfRemoved( B component, Entity<Game> entity, Contexts contexts )
{
    // some code
}


// Step 4. Add listeners to entity
private void OnEnable()
{
    var contexts = Contexts.sharedInstance;
    var entity = contexts.Get<Game>( ).CreateEntity( );

    entity.Add_OnSelfRemoved( this );

    entity.Add_OnAny( this ); // subscribe
    entity.Remove_OnAny( this ); // unsubscribe

    // writing types explicitly is required when implicit inference is impossible
    entity.Add_OnAny<Game, FlagA>( this );
    entity.Remove_OnAny<Game, FlagA>(  ); // removes listener component
}
}

```

#### EventsFeature2
Alternative to original EventsFeature.
Uses `Action` to subscribe/unsubscribe.
```csharp
// Step 1. Add event markers to components. Same as with original EventsFeature
public sealed class FlagA : IComponent, ICompFlag, Scope<Game>
    , IEvent_Any<Game, FlagA>          // <---
    , IEvent_Self<Game, FlagA>         // <---
{ }
public struct CompB : IComponent, ICompData, Scope<Game>
    , IEvent_Any<Game, CompB>         // <---
    , IEvent_AnyRemoved<Game, CompB>  // <---
    , IEvent_Self<Game, CompB>        // <---
    , IEvent_SelfRemoved<Game, CompB> // <---
{
    // some code
}


// Step 2. Add event systems to Systems. New EventSystem must be created before calling Sub/Unsub, otherwise NullReferenceException will be thrown
    systems.Add( new EventSystem_Any2<Game,CompB>(  ) );
    systems.Add( new EventSystem_Any_Removed2<Game,CompB>(  ) );
    systems.Add( new EventSystem_Any_Flag2<Game,FlagA>(  ) );  // Flag, callback on True and False unlike original EventsFeature

    systems.Add( new EventSystem_Self2<Game,CompB>(  ) );
    systems.Add( new EventSystem_Self_Removed2<Game,CompB>(  ) );
    systems.Add( new EventSystem_Self_Flag2<Game,CompB>(  ) );  // Flag, callback on True and False unlike original EventsFeature


// Step 3. Subscribe/Unsubscribe callback
private void Awake()
{
    OnAny<Game,CompB>.I.Sub( OnCompB );
    OnAny_Removed<Game,CompB>.I.Sub( OnCompB_Removed );
    OnAny_Flag<Game,FlagA>.I.Sub( OnFlagA );

    OnSelf<Game,CompB>.I.Sub( entityCreationIndex, OnCompB );
    OnSelf_Removed<Game,CompB>.I.Sub( entityCreationIndex, OnCompB_Removed );
    OnSelf_Flag<Game,FlagA>.I.Sub( entityCreationIndex, OnFlagA );
}

private void OnDestroy()
{
    OnAny<Game,CompB>.I.Unsub( OnCompB );
    OnAny_Removed<Game,CompB>.I.Unsub( OnCompB_Removed );
    OnAny_Flag<Game,FlagA>.I.Unsub( OnFlagA );

    OnSelf<Game,CompB>.I.Unsub( entityCreationIndex, OnCompB );
    OnSelf_Removed<Game,CompB>.I.Unsub( entityCreationIndex, OnCompB_Removed );
    OnSelf_Flag<Game,FlagA>.I.Unsub( entityCreationIndex, OnFlagA );
}

// Events2.I.UnsubAll() will remove all subscriptions across all contexts and components
public static void UnsubscribeAll()
{
    Events2.I.UnsubAll();
}

```

#### EntityIndex
```csharp
// Step 1(Optional). Create const string key for accessing entity index
public static class EntIndex
{
    public const string B = "B";
}


// Step 2. Add EntityIndex during initialization stage
var context = contexts.Get<Game>( );

// for Class Component
context.AddEntityIndex( EntIndex.B
    , context.GetGroup( Matcher<Game, B>.I )
    , ( e, c ) => ( (B)c ).Value );

// for Struct Component
context.AddEntityIndex( EntIndex.S
    , context.GetGroup( Matcher<Game, SStruct>.I )
    , ( e, c ) => ( (StructComponent<SStruct>) c).Data.value );


// Step 3. Get entities at runtime
var entities = context.GetEntities( EntIndex.B, 23 );
// preferred, compile time error checked variation(can be wrapped into extension method for simplicity)
var sameEntities = context.GetAllEntsBy<Game, B, int>( EntIndex.B, 23 );
```

#### PrimaryEntityIndex
```csharp
// Step 1(Optional). Create const string key for accessing entity index
public static class EntIndex
{
    public const string B = "B";
}


// Step 2. Add PrimaryEntityIndex during initialization stage
var context = contexts.Get<Game>( );

// for Class Component
context.AddPrimaryEntityIndex( EntIndex.B
    , context.GetGroup( Matcher<Game, B>.I )
    , ( e, c ) => ( (B)c ).Value );

// for Struct Component
context.AddPrimaryEntityIndex( EntIndex.S
    , context.GetGroup( Matcher<Game, SStruct>.I )
    , ( e, c ) => ( (StructComponent<SStruct>) c).Data.value );


// Step 3. Get entity at runtime
var entity = context.GetEntity( EntIndex.B, 23 );
// preferred, compile time error checked variation(can be wrapped into extension method for simplicity)
var sameEntity = context.GetSingleEntBy<Game, B, int>( EntIndex.B, 23 );
```

#### Visual Debugging
```csharp
public static void InitVisualDebugging ( Contexts contexts )
{
    #if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
    try
    {
        foreach ( var context in contexts.All )
        {
            var observer = new Entitas.VisualDebugging.Unity.ContextObserver(context);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);
        }
    }
    catch(System.Exception)
    {
    }
    #endif
}
```

Copy somewhere into your project and use `var systems = new Feature();` to visually debug `Systems`.
> Because `Feature` class uses `#if` preprocessor directives it must be present in unity project in source form and not in a precompiled EntitasGenericAddon dll.

```csharp
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

public class Feature : Entitas.VisualDebugging.Unity.DebugSystems {

    public Feature(string name) : base(name) {
    }

    public Feature() : base(true) {
        var readableType = GetType().ToGenericTypeString();
        initialize(readableType);
    }
}

#elif (!ENTITAS_DISABLE_DEEP_PROFILING && DEVELOPMENT_BUILD)

public class Feature : Entitas.Systems {

    System.Collections.Generic.List<string> _initializeSystemNames;
    System.Collections.Generic.List<string> _executeSystemNames;
    System.Collections.Generic.List<string> _cleanupSystemNames;
    System.Collections.Generic.List<string> _tearDownSystemNames;

    public Feature(string name) : this() {
    }

    public Feature() {
        _initializeSystemNames = new System.Collections.Generic.List<string>();
        _executeSystemNames = new System.Collections.Generic.List<string>();
        _cleanupSystemNames = new System.Collections.Generic.List<string>();
        _tearDownSystemNames = new System.Collections.Generic.List<string>();
    }

    public override Entitas.Systems Add(Entitas.ISystem system) {
        var systemName = system.GetType().ToGenericTypeString();

        if (system is Entitas.IInitializeSystem) {
            _initializeSystemNames.Add(systemName);
        }

        if (system is Entitas.IExecuteSystem) {
            _executeSystemNames.Add(systemName);
        }

        if (system is Entitas.ICleanupSystem) {
            _cleanupSystemNames.Add(systemName);
        }

        if (system is Entitas.ITearDownSystem) {
            _tearDownSystemNames.Add(systemName);
        }

        return base.Add(system);
    }

    public override void Initialize() {
        for (int i = 0; i < _initializeSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_initializeSystemNames[i]);
            _initializeSystems[i].Initialize();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void Execute() {
        for (int i = 0; i < _executeSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_executeSystemNames[i]);
            _executeSystems[i].Execute();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void Cleanup() {
        for (int i = 0; i < _cleanupSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_cleanupSystemNames[i]);
            _cleanupSystems[i].Cleanup();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void TearDown() {
        for (int i = 0; i < _tearDownSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_tearDownSystemNames[i]);
            _tearDownSystems[i].TearDown();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}

#else

public class Feature : Entitas.Systems {

    public Feature(string name) {
    }

    public Feature() {
    }
}

#endif
```

## FAQ
**Q: What `Cache<T>.I` does?**

A: `Cache<T>.I`creates and reuses static copy of class component for passing values to Entitas component through manually created `Component.Set` method. `I` is shortened `Instance`. Check [CacheT.cs](./Entitas.Generic/Lookup/CacheT.cs) for implementation.
There is no need to use `Cache<T>.I` with struct components.

**Q: I have better implementation of some interface/feature**

A: Improvements are great! Please write your suggestion in Issues section

