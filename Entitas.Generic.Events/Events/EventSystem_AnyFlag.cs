using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_AnyFlag<TScope,TCompFlag> : Entitas.ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TCompFlag : class, IComponent, ICompFlag, Scope<TScope>, IEvent_AnyFlag<TScope,TCompFlag>
{
	public					EventSystem_AnyFlag		( Contexts contexts ) : base(contexts.Get<TScope>())
	{
		_contexts					= contexts;
		var context					= contexts.Get<TScope>(  );
		_listeners					= context.GetGroup( Matcher<TScope, Event_AnyFlagComponent<TScope,TCompFlag>>.I );

		_listenersBuffer			= new List<Entity<TScope>>(  );
		_interfaceBuffer			= new List<IOnAnyFlag<TScope, TCompFlag>>(  );
	}

	private readonly		Contexts				_contexts;
	private readonly		IGroup<Entity<TScope>>	_listeners;
	private readonly		List<Entity<TScope>>	_listenersBuffer;
	private readonly	List<IOnAnyFlag<TScope, TCompFlag>> _interfaceBuffer;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TCompFlag>.I.AddedOrRemoved(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent )
		=> true;

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var e					= entities[i];
			foreach ( var listenerEntity in _listeners.GetEntities( _listenersBuffer ) )
			{
				_interfaceBuffer.Clear(  );
				var listenerComp = listenerEntity.Get<Event_AnyFlagComponent<TScope,TCompFlag>>(  );
				_interfaceBuffer.AddRange( listenerComp.Listeners );
				foreach ( var listener in _interfaceBuffer )
				{
					listener.OnAnyFlag( null, e, _contexts );
				}
			}
		}
	}
}
}