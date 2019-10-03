namespace Entitas.Generic
{
	public static class Event_Entity_StructMethods
	{
		public static		void					Add_OnAny_<TScope, TData>( this Entity<TScope> entity, IOnAny<TScope, TData> listener )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_Any<TScope, TData>
		{
			var index				= Lookup<TScope, Event_AnyComponent_<TScope,TData>>.Id;

			Event_AnyComponent_<TScope,TData> component;
			if ( entity.HasComponent( index ) )
			{
				component			= (Event_AnyComponent_<TScope,TData>)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<Event_AnyComponent_<TScope,TData>>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnAny_<TScope, TData>( this Entity<TScope> entity, IOnAny<TScope,TData> listener )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_Any<TScope, TData>
		{
			var index				= Lookup<TScope, Event_AnyComponent_<TScope,TData>>.Id;

			var component			= (Event_AnyComponent_<TScope,TData>)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnAny_<TScope, TData>( this Entity<TScope> entity )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_Any<TScope, TData>
		{
			var index				= Lookup<TScope, Event_AnyComponent_<TScope,TData>>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnAnyRemoved_<TScope, TData>( this Entity<TScope> entity, IOnAnyRemoved<TScope,TData> listener )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_AnyRemoved<TScope, TData>
		{
			var index				= Lookup<TScope, Event_AnyRemovedComponent_<TScope,TData>>.Id;

			Event_AnyRemovedComponent_<TScope,TData> component;
			if ( entity.HasComponent( index ) )
			{
				component			= (Event_AnyRemovedComponent_<TScope,TData>)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<Event_AnyRemovedComponent_<TScope,TData>>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnAnyRemoved_<TScope, TData>( this Entity<TScope> entity, IOnAnyRemoved<TScope,TData> listener )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_AnyRemoved<TScope, TData>
		{
			var index				= Lookup<TScope, Event_AnyRemovedComponent_<TScope,TData>>.Id;
			var component			= (Event_AnyRemovedComponent_<TScope,TData>)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnAnyRemoved_<TScope, TData>( this Entity<TScope> entity )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_AnyRemoved<TScope, TData>
		{
			var index				= Lookup<TScope, Event_AnyRemovedComponent_<TScope,TData>>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnSelf_<TScope, TData>( this Entity<TScope> entity, IOnSelf<TScope,TData> listener )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_Self<TScope, TData>
		{
			var index				= Lookup<TScope, Event_SelfComponent_<TScope,TData>>.Id;

			Event_SelfComponent_<TScope,TData> component;
			if ( entity.HasComponent( index ) )
			{
				component			= (Event_SelfComponent_<TScope,TData>)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<Event_SelfComponent_<TScope,TData>>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnSelf_<TScope, TData>( this Entity<TScope> entity, IOnSelf<TScope,TData> listener )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_Self<TScope, TData>
		{
			var index				= Lookup<TScope, Event_SelfComponent_<TScope,TData>>.Id;
			var component			= (Event_SelfComponent_<TScope,TData>)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnSelf_<TScope,TData>( this Entity<TScope> entity )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_Self<TScope, TData>
		{
			var index				= Lookup<TScope, Event_SelfComponent_<TScope,TData>>.Id;
			entity.RemoveComponent( index );
		}

		public static		void					Add_OnSelfRemoved_<TScope, TData>( this Entity<TScope> entity, IOnSelfRemoved<TScope, TData> listener )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_SelfRemoved<TScope, TData>
		{
			var index				= Lookup<TScope, Event_SelfRemovedComponent_<TScope,TData>>.Id;

			Event_SelfRemovedComponent_<TScope,TData> component;
			if ( entity.HasComponent( index ) )
			{
				component			= (Event_SelfRemovedComponent_<TScope,TData>)entity.GetComponent(index);
			}
			else
			{
				component			= entity.CreateComponent<Event_SelfRemovedComponent_<TScope,TData>>(index);
				entity.AddComponent(index, component);
				component.Listeners.Clear(  );
			}

			component.Listeners.Add( listener );
		}

		public static		void					Remove_OnSelfRemoved_<TScope, TData>( this Entity<TScope> entity, IOnSelfRemoved<TScope,TData> listener )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_SelfRemoved<TScope, TData>
		{
			var index				= Lookup<TScope, Event_SelfRemovedComponent_<TScope,TData>>.Id;
			var component			= (Event_SelfRemovedComponent_<TScope,TData>)entity.GetComponent(index);
			component.Listeners.Remove( listener );
		}

		public static		void					Remove_OnSelfRemoved_<TScope,TData>( this Entity<TScope> entity )
				where TScope : IScope
				where TData : struct, Scope<TScope>, IComponent, IEvent_SelfRemoved<TScope, TData>
		{
			var index				= Lookup<TScope, Event_SelfRemovedComponent_<TScope,TData>>.Id;
			entity.RemoveComponent( index );
		}

	}
}