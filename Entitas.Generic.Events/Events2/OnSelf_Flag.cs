using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class OnSelf_Flag<TScope, TComp> : IEventsFeature2_OnSelf_Flag<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompFlag, IEvent_Self<TScope, TComp>, Scope<TScope>
{
	public					OnSelf_Flag				( Contexts db )
	{
		_db							= db;
	}

	public static IEventsFeature2_OnSelf_Flag<TScope, TComp> I;
	private					Contexts				_db;
	private					Dictionary<KeyValuePair<Context<Entity<TScope>>, int>, Action<Entity<TScope>>>
													Action					= new Dictionary<KeyValuePair<Context<Entity<TScope>>, int>, Action<Entity<TScope>>>();

	public					void					Sub						( Context<Entity<TScope>> context, Int32 id, Action<Entity<TScope>> action )
	{
		var contextIdKey			= new KeyValuePair<Context<Entity<TScope>>, Int32>(context, id);
		if ( !Action.ContainsKey( contextIdKey ) )
		{
			Action.Add( contextIdKey, delegate{  } );
		}
		Action[contextIdKey]		+= action;
	}

	public					void					Unsub					( Context<Entity<TScope>> context, Int32 id, Action<Entity<TScope>> action ) 
	{ 
		var contextIdKey			= new KeyValuePair<Context<Entity<TScope>>, Int32>(context, id);
		if(Action.ContainsKey(contextIdKey)) 
		{
			Action[contextIdKey]	-= action;
		} 
	}

	public					void					Invoke					( Context<Entity<TScope>> context, Int32 id, Entity<TScope> entity )
	{
		var contextIdKey			= new KeyValuePair<Context<Entity<TScope>>, Int32>(context, id);
		if ( Action.TryGetValue( contextIdKey, out var action ) )
		{
			action.Invoke( entity );
		}
	}

	public					void					Sub						( Int32 id, Action<Entity<TScope>> action ) 
	{ 
		Sub( _db.Get<TScope>(), id, action );
	}

	public					void					Unsub					( Int32 id, Action<Entity<TScope>> action ) 
	{ 
		Unsub( _db.Get<TScope>(), id, action );
	}
}

public interface IEventsFeature2_OnSelf_Flag<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompFlag, IEvent_Self<TScope, TComp>, Scope<TScope>
{
	void					Sub						( Context<Entity<TScope>> context, Int32 id, Action<Entity<TScope>> action );
	void					Unsub					( Context<Entity<TScope>> context, Int32 id, Action<Entity<TScope>> action );
	void					Invoke					( Context<Entity<TScope>> context, Int32 id, Entity<TScope> entity );

	void					Sub						( Int32 id, Action<Entity<TScope>> action );
	void					Unsub					( Int32 id, Action<Entity<TScope>> action );
}
}