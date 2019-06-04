using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_SelfRemoved<TScope, TComp> : ReactiveSystem<Entity<TScope>>
		where TScope : IScope
		where TComp : class, IComponent, Scope<TScope>, IEvent_SelfRemoved<TScope,TComp>
{
	public					EventSystem_SelfRemoved			( Contexts contexts ) : base( contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly				Contexts				_contexts;
	readonly	List<IOnSelfRemoved<TScope,TComp>>	_interfaceBuffer		= new List<IOnSelfRemoved<TScope,TComp>>(  );


	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) {
		return context.CreateCollector(
			Matcher<TScope,TComp>.I.Removed(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<Event_SelfRemovedComponent<TScope,TComp>>(  )
			&& !ent.HasIComponent<TComp>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var entCount				= entities.Count;
		for ( var i = 0; i < entCount; i++ )
		{
			var e					= entities[i];
			_interfaceBuffer.Clear(  );
			_interfaceBuffer.AddRange( e.Get<Event_SelfRemovedComponent<TScope,TComp>>(   ).Listeners );
			foreach ( var listener in _interfaceBuffer )
			{
				listener.OnSelfRemoved( null, e, _contexts );
			}
		}
	}
}
}
