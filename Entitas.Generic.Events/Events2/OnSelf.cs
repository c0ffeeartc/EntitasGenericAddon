using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class OnSelf<TScope, TComp> : IEventsFeature2_OnSelf<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_Self<TScope, TComp>, Scope<TScope>
{
	public					OnSelf					( Contexts db )
	{
		_db							= db;
	}

	public static IEventsFeature2_OnSelf<TScope, TComp> I;
	private					Contexts				_db;
	private					Dictionary<KeyValuePair<Context<Entity<TScope>>, int>, Action<Entity<TScope>>>
													Action					= new Dictionary<KeyValuePair<Context<Entity<TScope>>, int>, Action<Entity<TScope>>>();

	public					void					Sub						( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context )
	{
		var contextIdKey			= new KeyValuePair<Context<Entity<TScope>>, Int32>(context, id);
		if ( !Action.ContainsKey( contextIdKey ) )
		{
			Action.Add( contextIdKey, delegate{  } );
		}
		Action[contextIdKey]		+= action;
	}

	public					void					Unsub					( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context ) 
	{ 
		var contextIdKey			= new KeyValuePair<Context<Entity<TScope>>, Int32>(context, id);
		if(Action.ContainsKey(contextIdKey)) 
		{
			Action[contextIdKey]	-= action;
		} 
	}

	public					void					Invoke					( Int32 id, Entity<TScope> entity, Context<Entity<TScope>> context )
	{
		var contextIdKey			= new KeyValuePair<Context<Entity<TScope>>, Int32>(context, id);
		if ( Action.TryGetValue( contextIdKey, out var action ) )
		{
			action.Invoke( entity );
		}
	}

	public					void					Sub						( Int32 id, Action<Entity<TScope>> action ) 
	{ 
		Sub(  id, action, _db.Get<TScope>() );
	}

	public					void					Unsub					( Int32 id, Action<Entity<TScope>> action ) 
	{ 
		Unsub(  id, action, _db.Get<TScope>() );
	}
}

public interface IEventsFeature2_OnSelf<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_Self<TScope, TComp>, Scope<TScope>
{
	void					Sub						( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Unsub					( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Invoke					( Int32 id, Entity<TScope> entity, Context<Entity<TScope>> context );

	void					Sub						( Int32 id, Action<Entity<TScope>> action );
	void					Unsub					( Int32 id, Action<Entity<TScope>> action );
}
}