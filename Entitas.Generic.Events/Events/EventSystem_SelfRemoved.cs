using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_SelfRemoved<TScope, TComp, TCompListen> : ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : class, IComponent, TScope
		where TCompListen : Event_SelfRemoved<TComp, TCompListen>, IComponent, TScope
{
	public					EventSystem_SelfRemoved			( Contexts contexts ) : base( contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly				Contexts				_contexts;
	readonly	List<IOnSelfRemoved<TComp, TCompListen>>_interfaceBuffer = new List<IOnSelfRemoved<TComp, TCompListen>>(  );


	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) {
		return context.CreateCollector(
			Matcher<TScope,TComp>.I.Removed(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<TCompListen>(  )
			&& !ent.HasIComponent<TComp>(  );
	}

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var entCount				= entities.Count;
		for ( var i = 0; i < entCount; i++ )
		{
			var e					= entities[i];
			_interfaceBuffer.Clear(  );
			_interfaceBuffer.AddRange( e.Get<TCompListen>(   ).Listeners );
			foreach ( var listener in _interfaceBuffer )
			{
				listener.OnSelfRemoved( null, e, _contexts );
			}
		}
	}
}
}
