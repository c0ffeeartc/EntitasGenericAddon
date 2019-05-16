# EntitasGenericAddon
Lightweight wrapper of Entitas functions into generic methods replicating code generator

## Goal
Make Entitas extensible by separate dll

## Main Concepts
  - Code Generator is optional
  - Type inference allows only valid components and gives access only to needed methods
  - Interfaces serve as markers for type inference
      - `IScope` - base interface for context scope
      - `IComponent` - allows class to be managed by **Entitas**, gives access to `Get<T>` extension methods
      - `ICompData` - gives access to `Add`, `Remove`, `Replace`, `Has` requires `ICopyFrom<TSelf>`
      - `ICompFlag` - gives access to `Flag<T>`, `Is<T>`
      - `IUnique` - provides context `Add`, `Replace` etc methods for unique components or flags

  - Generic Events Feature
      - `Event_*<TScope, TComp>` - abstract listener classes
      - `EventSystem_*<TScope, TComp, TCompListen>` - event system classes
 
  - Can be used together with regular Entitas components
  - Manual `EntityIndex` registration

## Installation
Copy `Entitas.Generic` sources into same assembly as generated `Contexts` class

## Usage

```csharp
public void Example()
{
    // initialization must be called before new Contexts( ) or accessing Contexts.sharedInstance
    Lookup_ScopeManager.RegisterAll( );

    var contexts = Contexts.sharedInstance;
    var game = contexts.Get<Game>( );

    var entity = game.CreateEntity( );
    entity.Add( Cache<Position>.I.Set(3f, 10f) );
    entity.Replace( Cache<Position>.I.Set( 20f, 1f ) );
    entity.Has<Position>( );
    entity.Remove<Position>( );

    entity.Flag<Moving>( true );
    entity.Is<Moving>( );
    entity.Flag<Moving>( false );

    game.Flag<GameActive>( true );  // provides unique component interfaces
    var gameActiveEntity = game.GetEntity<GameActive>( );
}
```

Scope
```csharp
public interface Game : IScope { }
```

Component
```csharp
public sealed class A : IComponent, ICompData, ICopyFrom<A>, Game
{
    public int Value;

    public A Set(int value) // optional, allows using Cache<T>.I.Set()
    {
        Value = value;
        return this;
    }

    public void CopyFrom(A other)
    {
        Value = other.Value;
    }
}
```

FlagComponent
```csharp
public sealed class FlagA : IComponent, ICompFlag, Game { }
```

Events
```csharp
// Step 1. Provide listener components
public sealed class FlagA_OnAny : Event_OnAny<Game, FlagA>, Game { }
public sealed class B_OnSelfRemoved : Event_OnSelfRemoved<Game, B>, Game { }

    
// Step 2. Add event system per event listener component to Systems
    systems.Add( new EventSystem_SelfRemoved<Game, B, B_OnSelfRemoved>(  ) );
    systems.Add( new EventSystem_Any<Game, FlagA, FlagA_OnAny>(  ) );

// Step 3. Add listener components to entity
public void AddListenersToEntity()
{
    var contexts = Contexts.sharedInstance;
    var entity = contexts.Get<Game>( ).CreateEntity( );
    entity.Add_OnSelfRemoved<B, B_OnSelfRemoved>( ( contexts_, ent, comp) => { } );

    entity.Add_OnAny<FlagA, FlagA_OnAny>( FlagA_OnAnyAdded ); // subscribe
    entity.Remove_OnAny<FlagA, FlagA_OnAny>( FlagA_OnAnyAdded ); // unsubscribe
    entity.Remove_OnAny<FlagA, FlagA_OnAny>(  ); // removes listener component
}

private void FlagA_OnAnyAdded( Contexts contexts_, Entity<SGame> ent_, FlagA comp_ )
{
    // some code
}

```

## FAQ
**Q: What `Cache<T>.I` does?**

A: `Cache<T>.I`creates and reuses static copy of component for passing values to Entitas component through manually created `Component.Set` method. `I` is shortened `Instance`. Check [CacheT.cs](./Entitas.Generic/Lookup/CacheT.cs) for implementation.

**Q: I have better implementation of some interface/feature

A: Improvements are great, please write your suggestion in Issues section
