# EntitasGenericAddon
Generic addon to Entitas

## Goal
Make Entitas extensible by separate dll


## Main Concepts
  - interfaces serve as markers for type inference
      - `IScope` - base interface for context scope
      - `IComponent` - allows class to be managed by **Entitas**, gives access to `Get<T>` extension methods
      - `ICompData` - gives access to `Add`, `Remove`, `Replace`, `Has` requires `ICopyFrom<TSelf>`
      - `ICompFlag` - gives access to `Flag<T>`, `Is<T>`

  - Generic Events Feature
      - `CompListen_*<TScope, TComp>` - abstract listener classes
      - `EventSystem_*<TScope, TComp, TCompListen>` - event system classes
      
  - Manual `EntityIndex` registration

## Examples
Scope
```csharp
public interface Game : IScope { }
```

Component
```csharp
public sealed class A : IComponent, ICompData, ICopyFrom<A>, Game
{
    public int Value;

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
