using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_AnyRemoved_<TScope,TData> : ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TData : struct, IComponent, Scope<TScope>, IEvent_AnyRemoved<TScope,TData>, ICompData
{
	public					EventSystem_AnyRemoved_	( Contexts contexts ) : base(contexts.Get<TScope>())
	{
		_contexts					= contexts;
		var context					= contexts.Get<TScope>(  );
		_listeners					= context.GetGroup( Matcher<TScope, Event_AnyRemovedComponent<TScope,TData>>.I );
		_listenersBuffer			= new List<Entity<TScope>>(  );
		_interfaceBuffer			= new List<IOnAnyRemoved<TScope, TData>>(  );
	}

	readonly				Contexts				_contexts;
	readonly				IGroup<Entity<TScope>>	_listeners;
	readonly				List<Entity<TScope>>	_listenersBuffer;
	readonly			List<IOnAnyRemoved<TScope, TData>>	_interfaceBuffer;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TData>.I.Removed(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return !ent.Has_<TData>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var e					= entities[i];
			foreach ( var listenerEntity in _listeners.GetEntities( _listenersBuffer ) )
			{
				_interfaceBuffer.Clear(  );
				_interfaceBuffer.AddRange( listenerEntity.Get<Event_AnyRemovedComponent<TScope,TData>>(   ).Listeners );
				foreach ( var listener in _interfaceBuffer )
				{
					listener.OnAnyRemoved( default(TData), e, _contexts );
				}
			}
		}
	}
}
}