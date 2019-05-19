using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Self<TScope, TComp, TCompListen> : Entitas.ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : IComponent, TScope
		where TCompListen : Event_Self<TComp, TCompListen>, IComponent, TScope
{
	public					EventSystem_Self			( Contexts contexts ) : base((IContext<Entity<TScope>>) contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly	List<IOnSelf<TComp, TCompListen>>	_interfaceBuffer		= new List<IOnSelf<TComp, TCompListen>>(  );

	readonly				Contexts				_contexts;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) {
		return context.CreateCollector(
			Matcher<TScope,TComp>.I.Added(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<TComp>(  )
			&& ent.HasIComponent<TCompListen>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var entCount				= entities.Count;
		for ( var i = 0; i < entCount; i++ )
		{
			var e					= entities[i];
			var component			= e.Get<TComp>(   );
			_interfaceBuffer.Clear(  );
			_interfaceBuffer.AddRange( e.Get<TCompListen>(   ).Listeners );
			foreach ( var listener in _interfaceBuffer )
			{
				listener.OnSelf( component, e, _contexts );
			}
		}
	}
}
}