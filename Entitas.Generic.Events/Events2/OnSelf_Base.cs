using System;
using System.Collections.Generic;

namespace Entitas.Generic 
{
public abstract class OnSelf_Base<TScope>
		: IUnsubAll
		where TScope : IScope
{
	public					OnSelf_Base				( Contexts db )
	{
		_db							= db;
		Events2.I.Add( this );
	}

	private					Contexts				_db;
	private					Dictionary<KeyValuePair<Context<Entity<TScope>>, int>, Action<Entity<TScope>>>
													Action					= new Dictionary<KeyValuePair<Context<Entity<TScope>>, int>, Action<Entity<TScope>>>();

	public					void					Sub						( Int32 id, Action<Entity<TScope>> action, Context<Entity<TScope>> context )
	{
		var contextIdKey			= new KeyValuePair<Context<Entity<TScope>>, Int32>(context, id);
		if ( !Action.ContainsKey( contextIdKey ) )
		{
			Action.Add( contextIdKey, null );
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
			action?.Invoke( entity );
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

	public					void					UnsubAll				(  ) 
	{ 
		Action.Clear(  );
	}
}
}