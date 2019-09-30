using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Any<TScope,TComp> : Entitas.ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : class, IComponent, Scope<TScope>, IEvent_Any<TScope,TComp>
{
	public					EventSystem_Any			( Contexts contexts ) : base(contexts.Get<TScope>())
	{
		_contexts					= contexts;
		var context					= contexts.Get<TScope>(  );
		_listeners					= context.GetGroup( Matcher<TScope, Event_AnyComponent<TScope,TComp>>.I );
		_listenersBuffer			= new List<Entity<TScope>>(  );
		_interfaceBuffer			= new List<IOnAny<TScope, TComp>>(  );
	}

	readonly				Contexts				_contexts;
	readonly				IGroup<Entity<TScope>>	_listeners;
	readonly				List<Entity<TScope>>	_listenersBuffer;
	readonly			List<IOnAny<TScope, TComp>>	_interfaceBuffer;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TComp>.I.Added(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<TComp>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var e					= entities[i];
			var component			= e.Get<TComp>(   );
			foreach ( var listenerEntity in _listeners.GetEntities( _listenersBuffer ) )
			{
				_interfaceBuffer.Clear(  );
				var listenerComp = listenerEntity.Get<Event_AnyComponent<TScope,TComp>>(  );
				_interfaceBuffer.AddRange( listenerComp.Listeners );
				foreach ( var listener in _interfaceBuffer )
				{
					listener.OnAny( component, e, _contexts );
				}
			}
		}
	}
}
}