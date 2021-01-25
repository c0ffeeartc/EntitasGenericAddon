using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Self_Flag2<TScope, TComp> : ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TComp : IComponent, ICompFlag, Scope<TScope>, IEvent_Self<TScope, TComp>
{
	public					EventSystem_Self_Flag2		( Contexts db, Context<Entity<TScope>> context = null) : base( context ?? db.Get<TScope>() )
	{
		if ( OnSelf_Flag<TScope,TComp>.I == null )
		{
			OnSelf_Flag<TScope,TComp>.I	= new OnSelf_Flag<TScope,TComp>( db );
		}
		_context					= context ?? db.Get<TScope>(  );
	}

	private					Context<Entity<TScope>> _context;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TComp>.I.AddedOrRemoved(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return true; }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var entCount				= entities.Count;
		for ( var i = 0; i < entCount; i++ )
		{
			var e					= entities[i];
			OnSelf_Flag<TScope,TComp>.I.Invoke( e.creationIndex, e, _context );
		}
	}
}
}