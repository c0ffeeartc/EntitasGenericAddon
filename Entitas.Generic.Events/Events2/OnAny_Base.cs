using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
	public abstract class OnAny_Base<TScope>
		where TScope : IScope
	{
		public OnAny_Base(Contexts db)
		{
			_db = db;
		}

		private Contexts _db;

		private Dictionary<Context<Entity<TScope>>, Action<Entity<TScope>>>
			Action = new Dictionary<Context<Entity<TScope>>, Action<Entity<TScope>>>();

		public void Sub(Action<Entity<TScope>> action, Context<Entity<TScope>> context)
		{
			if (!Action.ContainsKey(context))
			{
				Action.Add(context, delegate { });
			}

			Action[context] += action;
		}

		public void Unsub(Action<Entity<TScope>> action, Context<Entity<TScope>> context)
		{
			if (Action.ContainsKey(context))
			{
				Action[context] -= action;
			}
		}

		public void Invoke(Entity<TScope> entity, Context<Entity<TScope>> context)
		{
			if (Action.ContainsKey(context))
			{
				Action[context].Invoke(entity);
			}
		}

		public void Sub(Action<Entity<TScope>> action)
		{
			Sub(action, _db.Get<TScope>());
		}

		public void Unsub(Action<Entity<TScope>> action)
		{
			Unsub(action, _db.Get<TScope>());
		}
	}
}