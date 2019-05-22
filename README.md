# EntitasGenericAddon
Addon to [Entitas](https://github.com/sschmid/Entitas-CSharp) that allows using generic methods instead of code generator

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
      - `Event_*<TComp>` - abstract listener classes
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

Matcher
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

Events
```csharp
// Step 1. Provide listener components
public sealed class FlagA_Any : Event_Any<FlagA>, Game { }
public sealed class B_SelfRemoved : Event_SelfRemoved<B>, Game, Settings { }

    
// Step 2. Add event system per scoped components to Systems. This step could be automated in future
    systems.Add( new EventSystem_SelfRemoved<Game, B, B_SelfRemoved>(  ) );
    systems.Add( new EventSystem_Any<Game, FlagA, FlagA_Any>(  ) );


// Step 3. Inherit and implement event interface
public class Some : MonoBehaviour
    , IOnAny<FlagA, FlagA_Any>
    , IOnSelfRemoved<B, B_SelfRemoved>
{
private void OnAny( FlagA component, IEntity entity, Contexts contexts )
{
    var ent = (Entity<Game>)entity;
    // some code
}

private void OnSelfRemoved( B component, IEntity entity, Contexts contexts )
{
    var ent = (Entity<Game>)entity;
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
    entity.Add_OnAny<Game, FlagA, FlagA_Any>( this );
    entity.Remove_OnAny<Game, FlagA, FlagA_Any>(  ); // removes listener component
}
}

```

EntityIndex
```csharp
// Step 1(Optional). Create const string key for accessing entity index
public static class EntIndex
{
    public const string B = "B";
}


// Step 2. Add EntityIndex during initialization stage
var context = contexts.Get<Game>( );
context.AddEntityIndex( EntIndex.B
    , context.GetGroup( Matcher<Game, B>.I )
    , ( e, c ) => ( (B)c ).Value );


// Step 3. Get entities at runtime
var entities = context.GetEntities( EntIndex.B, 23 );
```
## FAQ
**Q: What `Cache<T>.I` does?**

A: `Cache<T>.I`creates and reuses static copy of component for passing values to Entitas component through manually created `Component.Set` method. `I` is shortened `Instance`. Check [CacheT.cs](./Entitas.Generic/Lookup/CacheT.cs) for implementation.

**Q: I have better implementation of some interface/feature**

A: Improvements are great! Please write your suggestion in Issues section
