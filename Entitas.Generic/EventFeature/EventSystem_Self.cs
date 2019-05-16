using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Self<TScope, TComp, TCompListen> : Entitas.ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : IComponent, TScope
		where TCompListen : CompListen_Self<TScope,TComp>, IComponent, TScope
{
	public					EventSystem_Self			( Contexts contexts ) : base((IContext<Entity<TScope>>) contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly				Contexts				_contexts;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) {
		return context.CreateCollector(
			Matcher<TScope,TComp>.Instance.Added(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<TComp>(  )
			&& ent.HasIComponent<TCompListen>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for (var i = 0; i < entities.Count; i++)
		{
			var ent					= entities[i];
			var comp				= ent.Get<TComp>(  );
			ent.Get<TCompListen>(  ).OnSelf.Invoke( _contexts, ent, comp );
		}
	}
}
}