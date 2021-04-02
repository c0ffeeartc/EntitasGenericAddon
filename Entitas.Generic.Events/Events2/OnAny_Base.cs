using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public abstract class OnAny_Base<TScope>
		: IUnsubAll
		where TScope : IScope
{
	public					OnAny_Base				( Contexts db )
	{
		_db							= db;
		Events2.I.Add( this );
	}

	private					Contexts				_db;
	private					Dictionary<Context<Entity<TScope>>,Action<Entity<TScope>>>
													ActionDict				= new Dictionary<Context<Entity<TScope>>,Action<Entity<TScope>>>();

	public					void					Sub						( Action<Entity<TScope>> action, Context<Entity<TScope>> context )
	{
		if ( !ActionDict.ContainsKey( context ) )
		{
			ActionDict.Add( context, null );
		}
		ActionDict[context]			+= action;
	}

	public					void					Unsub					( Action<Entity<TScope>> action, Context<Entity<TScope>> context ) 
	{ 
		if ( ActionDict.ContainsKey( context ) )
		{
			ActionDict[context]		-= action;
		}
	}

	public					void					Invoke					( Entity<TScope> entity, Context<Entity<TScope>> context )
	{
		if ( ActionDict.ContainsKey( context ) )
		{
			ActionDict[context]?.Invoke( entity );
		}
	}

	public					void					Sub						( Action<Entity<TScope>> action ) 
	{ 
		Sub( action, _db.Get<TScope>() );
	}

	public					void					Unsub					( Action<Entity<TScope>> action ) 
	{ 
		Unsub( action, _db.Get<TScope>() );
	}

	public					void					UnsubAll				(  ) 
	{ 
		ActionDict.Clear(  );
	}
}
}