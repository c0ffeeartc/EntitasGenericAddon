using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Any2<TScope, TComp> : ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : IComponent, ICompData, Scope<TScope>, IEvent_Any<TScope, TComp>
{
	public					EventSystem_Any2		( Contexts db, Context<Entity<TScope>> context = null ) : base( context ?? db.Get<TScope>( ) )
	{
		if ( OnAny<TScope,TComp>.I == null )
		{
			OnAny<TScope,TComp>.I	= new OnAny<TScope,TComp>( db );
		}
		_context					= context ?? db.Get<TScope>();
	}

	private					Context<Entity<TScope>>	_context;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope, TComp>.I.Added(   ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) { return
		ent.HasComponent( Lookup<TScope, TComp>.Id ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var e					= entities[i];
			OnAny<TScope, TComp>.I.Invoke( e, _context );
		}
	}
}
}