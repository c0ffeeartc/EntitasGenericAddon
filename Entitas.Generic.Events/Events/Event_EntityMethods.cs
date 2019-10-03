namespace Entitas.Generic
{
	public static class Event_EntityMethods
	{
		public static		void					Add_OnAny<TScope, TComp>( this Entity<TScope> entity, IOnAny<TScope, TComp> listener )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_Any<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_AnyComponent<TScope,TComp>>.Id;

			Event_AnyComponent<TScope,TComp> component;
			if ( entity.HasComponent( index ) )
			{
				component			= (Event_AnyComponent<TScope,TComp>)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<Event_AnyComponent<TScope,TComp>>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnAny<TScope, TComp>( this Entity<TScope> entity, IOnAny<TScope,TComp> listener )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_Any<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_AnyComponent<TScope,TComp>>.Id;

			var component			= (Event_AnyComponent<TScope,TComp>)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnAny<TScope, TComp>( this Entity<TScope> entity )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_Any<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_AnyComponent<TScope,TComp>>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnAnyRemoved<TScope, TComp>( this Entity<TScope> entity, IOnAnyRemoved<TScope,TComp> listener )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_AnyRemoved<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_AnyRemovedComponent<TScope,TComp>>.Id;

			Event_AnyRemovedComponent<TScope,TComp> component;
			if ( entity.HasComponent( index ) )
			{
				component			= (Event_AnyRemovedComponent<TScope,TComp>)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<Event_AnyRemovedComponent<TScope,TComp>>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnAnyRemoved<TScope, TComp>( this Entity<TScope> entity, IOnAnyRemoved<TScope,TComp> listener )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_AnyRemoved<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_AnyRemovedComponent<TScope,TComp>>.Id;
			var component			= (Event_AnyRemovedComponent<TScope,TComp>)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnAnyRemoved<TScope, TComp>( this Entity<TScope> entity )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_AnyRemoved<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_AnyRemovedComponent<TScope,TComp>>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnSelf<TScope, TComp>( this Entity<TScope> entity, IOnSelf<TScope,TComp> listener )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_Self<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_SelfComponent<TScope,TComp>>.Id;

			Event_SelfComponent<TScope,TComp> component;
			if ( entity.HasComponent( index ) )
			{
				component			= (Event_SelfComponent<TScope,TComp>)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<Event_SelfComponent<TScope,TComp>>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnSelf<TScope, TComp>( this Entity<TScope> entity, IOnSelf<TScope,TComp> listener )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_Self<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_SelfComponent<TScope,TComp>>.Id;
			var component			= (Event_SelfComponent<TScope,TComp>)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnSelf<TScope,TComp>( this Entity<TScope> entity )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_Self<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_SelfComponent<TScope,TComp>>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnSelfRemoved<TScope, TComp>( this Entity<TScope> entity, IOnSelfRemoved<TScope, TComp> listener )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_SelfRemoved<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_SelfRemovedComponent<TScope,TComp>>.Id;

			Event_SelfRemovedComponent<TScope,TComp> component;
			if ( entity.HasComponent( index ) )
			{
				component			= (Event_SelfRemovedComponent<TScope,TComp>)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<Event_SelfRemovedComponent<TScope,TComp>>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnSelfRemoved<TScope, TComp>( this Entity<TScope> entity, IOnSelfRemoved<TScope,TComp> listener )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_SelfRemoved<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_SelfRemovedComponent<TScope,TComp>>.Id;
			var component			= (Event_SelfRemovedComponent<TScope,TComp>)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnSelfRemoved<TScope,TComp>( this Entity<TScope> entity )
				where TScope : IScope
				where TComp : Scope<TScope>, IComponent, IEvent_SelfRemoved<TScope, TComp>
		{
			var index				= Lookup<TScope, Event_SelfRemovedComponent<TScope,TComp>>.Id;
			entity.RemoveComponent( index );
		}

	}
}