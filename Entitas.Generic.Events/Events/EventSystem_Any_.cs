using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Any_<TScope,TData> : Entitas.ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TData : struct, IComponent, Scope<TScope>, IEvent_Any<TScope,TData>, ICompData
{
	public					EventSystem_Any_		( Contexts contexts ) : base(contexts.Get<TScope>())
	{
		_contexts					= contexts;
		var context					= contexts.Get<TScope>(  );
		_listeners					= context.GetGroup( Matcher<TScope, Event_AnyComponent<TScope,TData>>.I );
		_listenersBuffer			= new List<Entity<TScope>>(  );
		_interfaceBuffer			= new List<IOnAny<TScope, TData>>(  );
	}

	readonly				Contexts				_contexts;
	readonly				IGroup<Entity<TScope>>	_listeners;
	readonly				List<Entity<TScope>>	_listenersBuffer;
	readonly			List<IOnAny<TScope, TData>>	_interfaceBuffer;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TData>.I.Added(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.Has_<TData>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var e					= entities[i];
			var component			= e.Get_<TData>(   );
			foreach ( var listenerEntity in _listeners.GetEntities( _listenersBuffer ) )
			{
				_interfaceBuffer.Clear(  );
				var listenerComp = listenerEntity.Get<Event_AnyComponent<TScope,TData>>(  );
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