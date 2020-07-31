using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Generic;

namespace Custom.Scripts
{
public sealed class EventSystem_Any_Flag2<TScope, TComp> : ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TComp : IComponent, ICompFlag, Scope<TScope>, IEvent_Any<TScope, TComp>
{
	public					EventSystem_Any_Flag2 ( Contexts db ) : base( db.Get<TScope>( ) ) { }

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope, TComp>.I.AddedOrRemoved(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) { return true; }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var e					= entities[i];
			OnAny_Flag<TScope, TComp>.Action.Invoke( e );
		}
	}
}
}