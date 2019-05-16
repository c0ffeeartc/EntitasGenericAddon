using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Any<TScope,TComp,TCompListen> : Entitas.ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : IComponent, TScope
		where TCompListen : CompListen_Any<TScope,TComp>, IComponent, TScope
{
	public					EventSystem_Any			( Contexts contexts ) : base(contexts.Get<TScope>())
	{
		_contexts					= contexts;
		var context					= contexts.Get<TScope>(  );
		_listeners					= context.GetGroup( Matcher<TScope, TCompListen>.Instance );
		_buff						= new List<Entity<TScope>>(  );
	}

	readonly				Contexts				_contexts;
	readonly				IGroup<Entity<TScope>>	_listeners;
	readonly				List<Entity<TScope>>	_buff;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TComp>.Instance.Added(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<TComp>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var e					= entities[i];
			var ents				= _listeners.GetEntities( _buff );
			var comp				= e.Get<TComp>(  );
			for ( var j = 0; j < ents.Count; j++ )
			{
				var listenerEntity	= ents[j];
				var listenerComp	= listenerEntity.Get<TCompListen>(  );
				if ( listenerComp.OnAny != null )
				{
					listenerComp.OnAny.Invoke( _contexts, e, comp );
				}
			}
		}
	}
}
}