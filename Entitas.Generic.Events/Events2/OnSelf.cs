using System;

namespace Entitas.Generic
{
public sealed class OnSelf<TScope,TComp> : OnSelf_Base<TScope>, IEventsFeature2_OnSelf<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent,ICompData, IEvent_Self<TScope,TComp>, Scope<TScope>
{
	public					OnSelf					( Contexts db ) : base ( db ) { }
	public static IEventsFeature2_OnSelf<TScope, TComp> I;

	public					void					Sub						( Entity<TScope> ent, Action<Entity<TScope>> action, bool triggerIfHas )
	{
		Sub( ent.creationIndex, action );
		if ( triggerIfHas && ent.HasComponent( Lookup<TScope,TComp>.Id ) )
		{
			action.Invoke( ent );
		}
	}

	public					void					Sub						( Entity<TScope> ent, Action<Entity<TScope>> action, Context<Entity<TScope>> context, bool triggerIfHas = false )
	{
		Sub( ent.creationIndex, action, context );
		if ( triggerIfHas && ent.HasComponent( Lookup<TScope,TComp>.Id ) )
		{
			action.Invoke( ent );
		}
	}
}

public interface IEventsFeature2_OnSelf<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_Self<TScope, TComp>, Scope<TScope>
{
	void					Sub						( Entity<TScope> ent, Action<Entity<TScope>> action, Context<Entity<TScope>> context, bool triggerIfHas = false );
	void					Sub						( Entity<TScope> ent, Action<Entity<TScope>> action, bool triggerIfHas = false );
	void					Sub						( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Sub						( Int32 id, Action<Entity<TScope>> action );

	void					Unsub					( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Unsub					( Int32 id, Action<Entity<TScope>> action );

	void					Invoke					( Int32 id, Entity<TScope> entity, Context<Entity<TScope>> context );
}
}