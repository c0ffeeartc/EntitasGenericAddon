using System;

namespace Entitas.Generic
{
public sealed class OnSelf_Flag<TScope,TComp> : OnSelf_Base<TScope>, IEventsFeature2_OnSelf_Flag<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, ICompFlag, IEvent_Self<TScope,TComp>, Scope<TScope>
{
	public					OnSelf_Flag				( Contexts db ) : base ( db ) { }
	public static IEventsFeature2_OnSelf_Flag<TScope,TComp> I;

	public					void					Sub						( Entity<TScope> ent, Action<Entity<TScope>> action, bool triggerAfterSub )
	{
		Sub( ent.creationIndex, action );
		if ( triggerAfterSub )
		{
			action.Invoke( ent );
		}
	}

	public					void					Sub						( Entity<TScope> ent, Action<Entity<TScope>> action, Context<Entity<TScope>> context, bool triggerAfterSub = false )
	{
		Sub( ent.creationIndex, action, context );
		if ( triggerAfterSub )
		{
			action.Invoke( ent );
		}
	}
}

public interface IEventsFeature2_OnSelf_Flag<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, ICompFlag, IEvent_Self<TScope,TComp>, Scope<TScope>
{
	void					Sub						( Entity<TScope> ent, Action<Entity<TScope>> action, Context<Entity<TScope>> context, bool triggerAfterSub = false );
	void					Sub						( Entity<TScope> ent, Action<Entity<TScope>> action, bool triggerAfterSub = false );
	void					Sub						( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Sub						( Int32 id, Action<Entity<TScope>> action );

	void					Unsub					( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Unsub					( Int32 id, Action<Entity<TScope>> action );

	void					Invoke					( Int32 id, Entity<TScope> entity, Context<Entity<TScope>> context );
}
}