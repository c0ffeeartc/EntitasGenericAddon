using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Self<TScope, TComp> : Entitas.ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : class, IComponent, ICompData, Scope<TScope>, IEvent_Self<TScope,TComp>
{
	public					EventSystem_Self		( Contexts contexts ) : base((IContext<Entity<TScope>>) contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly		List<IOnSelf<TScope,TComp>>		_interfaceBuffer		= new List<IOnSelf<TScope,TComp>>(  );

	readonly				Contexts				_contexts;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) {
		return context.CreateCollector(
			Matcher<TScope,TComp>.I.Added(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<Event_SelfComponent<TScope,TComp>>(  )
			&& ent.HasIComponent<TComp>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var entCount				= entities.Count;
		for ( var i = 0; i < entCount; i++ )
		{
			var e					= entities[i];
			var component			= e.Get<TComp>(   );
			_interfaceBuffer.Clear(  );
			_interfaceBuffer.AddRange( e.Get<Event_SelfComponent<TScope,TComp>>(   ).Listeners );
			foreach ( var listener in _interfaceBuffer )
			{
				listener.OnSelf( component, e, _contexts );
			}
		}
	}
}
}