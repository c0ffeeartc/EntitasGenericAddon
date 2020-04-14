using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_SelfRemoved_<TScope, TData> : ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TData : struct, IComponent, Scope<TScope>, IEvent_SelfRemoved<TScope,TData>, ICompData
{
	public					EventSystem_SelfRemoved_			( Contexts contexts ) : base( contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly				Contexts				_contexts;
	readonly	List<IOnSelfRemoved<TScope,TData>>	_interfaceBuffer		= new List<IOnSelfRemoved<TScope,TData>>(  );


	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) {
		return context.CreateCollector(
			Matcher<TScope,TData>.I.Removed(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<Event_SelfRemovedComponent<TScope,TData>>(  )
		       && !ent.Has_<TData>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var entCount				= entities.Count;
		for ( var i = 0; i < entCount; i++ )
		{
			var e					= entities[i];
			_interfaceBuffer.Clear(  );
			_interfaceBuffer.AddRange( e.Get<Event_SelfRemovedComponent<TScope,TData>>(   ).Listeners );
			foreach ( var listener in _interfaceBuffer )
			{
				listener.OnSelfRemoved( default(TData), e, _contexts );
			}
		}
	}
}
}