using System;

namespace Entitas.Generic
{
	public static class EntityTScope_EventsActionExtensions
	{
		public static		void					Add_OnAny<TScope, TComp, TCompListen>( this Entity<TScope> entity, IOnAny<TComp, TCompListen> listener )
				where TCompListen : Event_Any<TComp, TCompListen>, TScope, new()
				where TComp : TScope, IComponent
				where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;

			TCompListen component;
			if ( entity.HasComponent( index ) )
			{
				component			= entity.Get<TCompListen>();
			}
			else
			{
				component			= entity.CreateComponent<TCompListen>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnAny<TScope, TComp, TCompListen>( this Entity<TScope> entity, IOnAny<TComp, TCompListen> listener )
				where TCompListen : Event_Any<TComp, TCompListen>, TScope, new()
				where TComp : IComponent
				where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= entity.Get<TCompListen>();
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnAny<TScope, TComp, TCompListen>( this Entity<TScope> entity )
				where TCompListen : Event_Any<TComp,TCompListen>, TScope
				where TScope : IScope
				where TComp : IComponent
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnAnyRemoved<TScope, TComp, TCompListen>( this Entity<TScope> entity, IOnAnyRemoved<TComp, TCompListen> listener )
				where TCompListen : Event_AnyRemoved<TComp, TCompListen>, TScope, new()
				where TComp : IComponent, TScope
				where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;

			TCompListen component;
			if ( entity.HasComponent( index ) )
			{
				component			= (TCompListen)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<TCompListen>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnAnyRemoved<TScope, TComp, TCompListen>( this Entity<TScope> entity, IOnAnyRemoved<TComp, TCompListen> listener )
				where TCompListen : Event_AnyRemoved<TComp, TCompListen>, TScope
				where TComp : IComponent, TScope
				where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= entity.Get<TCompListen>();
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnAnyRemoved<TScope, TComp, TCompListen>( this Entity<TScope> entity )
				where TCompListen : Event_AnyRemoved<TComp, TCompListen>, TScope
				where TComp : IComponent, TScope
				where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnSelf<TScope, TComp, TCompListen>( this Entity<TScope> entity, IOnSelf<TComp, TCompListen> listener )
				where TCompListen : Event_Self<TComp, TCompListen>, TScope, new()
				where TComp : IComponent, TScope
				where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;

			TCompListen component;
			if ( entity.HasComponent( index ) )
			{
				component			= (TCompListen)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<TCompListen>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnSelf<TScope, TComp, TCompListen>( this Entity<TScope> entity, IOnSelf<TComp, TCompListen> listener )
			where TCompListen: Event_Self<TComp, TCompListen>, TScope, new()
			where TComp : IComponent, TScope
			where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= (TCompListen)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnSelf<TScope,TComp, TCompListen>( this Entity<TScope> entity )
			where TCompListen: Event_Self<TComp, TCompListen>, TScope, IComponent
			where TComp : IComponent, TScope
			where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnSelfRemoved<TScope, TComp, TCompListen>( this Entity<TScope> entity, IOnSelfRemoved<TComp, TCompListen> listener )
			where TCompListen : Event_SelfRemoved<TComp, TCompListen>, TScope, new()
			where TComp : IComponent, TScope
			where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;

			TCompListen component;
			if ( entity.HasComponent( index ) )
			{
				component			= entity.Get<TCompListen>();
			}
			else
			{
				component			= entity.CreateComponent<TCompListen>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnSelfRemoved<TScope, TComp, TCompListen>( this Entity<TScope> entity, IOnSelfRemoved<TComp, TCompListen> listener )
			where TCompListen : Event_SelfRemoved<TComp,TCompListen>, TScope, new()
			where TComp : IComponent, TScope
			where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= (TCompListen)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnSelfRemoved<TScope,TComp, TCompListen>( this Entity<TScope> entity )
			where TCompListen : Event_SelfRemoved<TComp, TCompListen>, TScope
			where TComp : IComponent, TScope
			where TScope : IScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			entity.RemoveComponent( index );
		}
	}
}