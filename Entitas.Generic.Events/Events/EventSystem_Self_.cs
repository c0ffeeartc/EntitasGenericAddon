using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class EventSystem_Self_<TScope, TData> : Entitas.ReactiveSystem<Entity<TScope>>
	where TScope : IScope
	where TData : struct, IComponent, Scope<TScope>, IEvent_Self<TScope,TData>, ICompData
{
	public					EventSystem_Self_		( Contexts contexts ) : base((IContext<Entity<TScope>>) contexts.Get<TScope>())
	{
		_contexts					= contexts;
	}

	readonly		List<IOnSelf<TScope,TData>>		_interfaceBuffer		= new List<IOnSelf<TScope,TData>>(  );

	readonly				Contexts				_contexts;

	protected override	ICollector<Entity<TScope>>	GetTrigger				( IContext<Entity<TScope>> context ) {
		return context.CreateCollector(
			Matcher<TScope,TData>.I.Added(  ) ); }

	protected override		Boolean					Filter					( Entity<TScope> ent ) {
		return ent.HasIComponent<Event_SelfComponent<TScope,TData>>(  )
		       && ent.Has_<TData>(  ); }

	protected override		void					Execute					( List<Entity<TScope>> entities )
	{
		var entCount				= entities.Count;
		for ( var i = 0; i < entCount; i++ )
		{
			var e					= entities[i];
			var component			= e.Get_<TData>(   );
			_interfaceBuffer.Clear(  );
			_interfaceBuffer.AddRange( e.Get<Event_SelfComponent<TScope,TData>>(   ).Listeners );
			foreach ( var listener in _interfaceBuffer )
			{
				listener.OnSelf( component, e, _contexts );
			}
		}
	}
}
}