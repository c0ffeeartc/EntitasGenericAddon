using System;

namespace Entitas.Generic
{
public sealed class OnAny_Removed<TScope,TComp> : OnAny_Base<TScope>, IEventsFeature2_OnAny_Removed<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_AnyRemoved<TScope,TComp>, Scope<TScope>
{
	public					OnAny_Removed			( Contexts db ) : base( db ) {  }
	public static IEventsFeature2_OnAny_Removed<TScope,TComp> I;
}

public interface IEventsFeature2_OnAny_Removed<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_AnyRemoved<TScope, TComp>, Scope<TScope>
{
	void					Sub						( Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Unsub					( Action<Entity<TScope>> action, Context<Entity<TScope>> context );
	void					Invoke					( Entity<TScope> entity, Context<Entity<TScope>> context );

	void					Sub						( Action<Entity<TScope>> action );
	void					Unsub					( Action<Entity<TScope>> action );
}
}