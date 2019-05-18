using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_SelfRemoved<TScope, TComp, TCompListen> : ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : class, IComponent, TScope
		where TCompListen : Event_SelfRemoved<TComp>, IComponent, TScope
{
	public					EventSystem_SelfRemoved			( Contexts contexts ) : base( contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly				Contexts				_contexts;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) {
		return context.CreateCollector(
			Matcher<TScope,TComp>.I.Removed(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<TCompListen>(  )
			&& !ent.HasIComponent<TComp>(  );
	}

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		for (var i = 0; i < entities.Count; i++)
		{
			var ent					= entities[i];
			var compListen			= ent.Get<TCompListen>(  );

			if ( compListen.OnSelfRemoved != null )
			{
				compListen.OnSelfRemoved.Invoke( _contexts, ent, null );
			}
		}
	}
}
}
