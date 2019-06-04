using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_AnyRemoved<TScope,TComp> : ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : class, IComponent, Scope<TScope>, IEvent_AnyRemoved<TScope,TComp>
{
	public					EventSystem_AnyRemoved	( Contexts contexts ) : base(contexts.Get<TScope>())
	{
		_contexts					= contexts;
		var context					= contexts.Get<TScope>(  );
		_listeners					= context.GetGroup( Matcher<TScope, Event_AnyRemovedComponent<TScope,TComp>>.I );
		_listenersBuffer			= new List<Entity<TScope>>(  );
		_interfaceBuffer			= new List<IOnAnyRemoved<TScope, TComp>>(  );
	}

	readonly				Contexts				_contexts;
	readonly				IGroup<Entity<TScope>>	_listeners;
	readonly				List<Entity<TScope>>	_listenersBuffer;
	readonly			List<IOnAnyRemoved<TScope, TComp>>	_interfaceBuffer;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TComp>.I.Removed(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return !ent.HasIComponent<TComp>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var e					= entities[i];
			foreach ( var listenerEntity in _listeners.GetEntities( _listenersBuffer ) )
			{
				_interfaceBuffer.Clear(  );
				_interfaceBuffer.AddRange( listenerEntity.Get<Event_AnyRemovedComponent<TScope,TComp>>(   ).Listeners );
				foreach ( var listener in _interfaceBuffer )
				{
					listener.OnAnyRemoved( null, e, _contexts );
				}
			}
		}
	}
}
}