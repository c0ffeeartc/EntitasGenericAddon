using System;

namespace Entitas.Generic
{
	public sealed class OnAny<TScope, TComp> : OnAny_Base<TScope>, IEventsFeature2_OnAny<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_Any<TScope, TComp>, Scope<TScope>
	{
		public OnAny(Contexts db) : base(db)
		{
		}

		public static IEventsFeature2_OnAny<TScope, TComp> I;
	}

	public interface IEventsFeature2_OnAny<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_Any<TScope, TComp>, Scope<TScope>
	{
		void Sub(Action<Entity<TScope>> action, Context<Entity<TScope>> context);
		void Unsub(Action<Entity<TScope>> action, Context<Entity<TScope>> context);
		void Invoke(Entity<TScope> entity, Context<Entity<TScope>> context);

		void Sub(Action<Entity<TScope>> action);
		void Unsub(Action<Entity<TScope>> action);
	}
}