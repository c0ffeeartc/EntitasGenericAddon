using System;

namespace Entitas.Generic
{
public sealed class OnSelf_Flag<TScope,TComp> : OnSelf_Base<TScope>, IEventsFeature2_OnSelf_Flag<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, ICompFlag, IEvent_Self<TScope,TComp>, Scope<TScope>
{
	public					OnSelf_Flag				( Contexts db ) : base ( db ) { }
	public static IEventsFeature2_OnSelf_Flag<TScope,TComp> I;
}

public interface IEventsFeature2_OnSelf_Flag<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, ICompFlag, IEvent_Self<TScope,TComp>, Scope<TScope>
{
	void					Sub						( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Unsub					( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Invoke					( Int32 id, Entity<TScope> entity, Context<Entity<TScope>> context );

	void					Sub						( Int32 id, Action<Entity<TScope>> action );
	void					Unsub					( Int32 id, Action<Entity<TScope>> action );
}
}