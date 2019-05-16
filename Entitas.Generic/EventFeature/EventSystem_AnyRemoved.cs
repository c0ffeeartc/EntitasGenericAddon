using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_AnyRemoved<TScope,TComp,TCompListen> : ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TComp : class, IComponent, TScope
	where TCompListen : Event_OnAnyRemoved<TScope,TComp>, IComponent, TScope
{
	public					EventSystem_AnyRemoved	( Contexts contexts ) : base(contexts.Get<TScope>())
	{
		_contexts					= contexts;
		_groupListen				= contexts.Get<TScope>(  ).GetGroup(Matcher<TScope, TCompListen>.Instance);
		_buff						= new List<Entity<TScope>>(  );
	}

	readonly				Contexts				_contexts;
	readonly				IGroup<Entity<TScope>>	_groupListen;
	readonly				List<Entity<TScope>>	_buff;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) { return context.CreateCollector(
		Matcher<TScope,TComp>.Instance.Removed(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return !ent.HasIComponent<TComp>(); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var listenerEnts			= _groupListen.GetEntities(_buff);
		for (var i = 0; i < entities.Count; i++)
		{
			var e					= entities[i];
			for (var j = 0; j < listenerEnts.Count; j++)
			{
				var listenerEntity	= listenerEnts[j];
				listenerEntity.Get<TCompListen>(  ).OnAnyRemoved.Invoke( _contexts, e, null );
			}
		}
	}
}
}