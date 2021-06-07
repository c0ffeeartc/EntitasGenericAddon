using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_SelfFlag<TScope, TCompFlag> : Entitas.ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TCompFlag : class, IComponent, ICompFlag, Scope<TScope>, IEvent_SelfFlag<TScope,TCompFlag>
{
	public					EventSystem_SelfFlag	( Contexts contexts ) : base((IContext<Entity<TScope>>) contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly		List<IOnSelfFlag<TScope,TCompFlag>>	_interfaceBuffer	= new List<IOnSelfFlag<TScope,TCompFlag>>(  );

	readonly				Contexts				_contexts;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TCompFlag>.I.AddedOrRemoved(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) { return
		ent.HasIComponent<Event_SelfFlagComponent<TScope, TCompFlag>>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var entCount				= entities.Count;
		for ( var i = 0; i < entCount; i++ )
		{
			var e					= entities[i];
			_interfaceBuffer.Clear(  );
			_interfaceBuffer.AddRange( e.Get<Event_SelfFlagComponent<TScope,TCompFlag>>(   ).Listeners );
			for ( var j = 0; j < _interfaceBuffer.Count; j++ )
			{
				var listener		= _interfaceBuffer[j];
				listener.OnSelfFlag( null, e, _contexts );
			}
		}
	}
}
}