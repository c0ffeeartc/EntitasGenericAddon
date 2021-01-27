using System;

namespace Entitas.Generic
{
public sealed class OnSelf_Removed<TScope,TComp> : OnSelf_Base<TScope> ,IEventsFeature2_OnSelf_Removed<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_SelfRemoved<TScope,TComp>, Scope<TScope>
{
	public					OnSelf_Removed				( Contexts db ) : base ( db ) { }
	public static IEventsFeature2_OnSelf_Removed<TScope,TComp> I;
}

public interface IEventsFeature2_OnSelf_Removed<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_SelfRemoved<TScope, TComp>, Scope<TScope>
{
	void					Sub						( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Sub						( Int32 id, Action<Entity<TScope>> action );

	void					Unsub					( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Unsub					( Int32 id, Action<Entity<TScope>> action );

	void					Invoke					( Int32 id, Entity<TScope> entity, Context<Entity<TScope>> context );
}
}