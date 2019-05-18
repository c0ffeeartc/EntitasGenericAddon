using System;

namespace Entitas.Generic
{
	public partial class Entity<TScope>
	{
		public				void					Add_OnAny<TComp, TCompListen>( Action<Contexts, IEntity, TComp> action )
				where TCompListen : Event_Any<TComp>, TScope, new()
				where TComp : TScope, IComponent
		{
			var index				= Lookup<TScope, TCompListen>.Id;

			TCompListen component;
			if ( HasComponent( index ) )
			{
				component			= Get<TCompListen>();
			}
			else
			{
				component			= CreateComponent<TCompListen>(index);
				AddComponent(index, component);
				component.OnAny	= null;
			}

			component.OnAny		+= action;
		}

		public				void					Remove_OnAny<TComp, TCompListen>( Action<Contexts, IEntity, TComp> action )
				where TCompListen : Event_Any<TComp>, TScope, new()
				where TComp : TScope, IComponent
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= Get<TCompListen>();
			component.OnAny			-= action;
		}

		public				void					Remove_OnAny<TComp, TCompListen>(  )
				where TCompListen : Event_Any<TComp>, TScope
				where TComp : TScope, IComponent
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			RemoveComponent( index );
		}

		public				void					Add_OnAnyRemoved<TComp, TCompListen>( Action<Contexts, IEntity, TComp> action )
				where TCompListen : Event_AnyRemoved<TComp>, TScope, new()
				where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;

			TCompListen component;
			if ( HasComponent( index ) )
			{
				component			= (TCompListen)GetComponent(index);
			}
			else
			{
				component			= CreateComponent<TCompListen>(index);
				AddComponent(index, component);
				component.OnAnyRemoved	= null;
			}

			component.OnAnyRemoved	+= action;
		}

		public				void					Remove_OnAnyRemoved<TComp, TCompListen>( Action<Contexts, IEntity, TComp> action )
				where TCompListen : Event_AnyRemoved<TComp>, TScope
				where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= Get<TCompListen>();
			component.OnAnyRemoved	-= action;
		}

		public				void					Remove_OnAnyRemoved<TComp, TCompListen>(  )
				where TCompListen : Event_AnyRemoved<TComp>, TScope
				where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			RemoveComponent( index );
		}

		public				void					Add_OnSelf<TComp, TCompListen>( Action<Contexts, IEntity, TComp> action )
				where TCompListen : Event_Self<TComp>, TScope, new()
				where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;

			TCompListen component;
			if ( HasComponent( index ) )
			{
				component			= (TCompListen)GetComponent(index);
			}
			else
			{
				component			= CreateComponent<TCompListen>(index);
				AddComponent(index, component);
				component.OnSelf	= null;
			}

			component.OnSelf		+= action;
		}

		public				void					Remove_OnSelf<TComp, TCompListen>( Action<Contexts, IEntity, TComp> action )
			where TCompListen: Event_Self<TComp>, TScope, new()
			where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= (TCompListen)GetComponent(index);
			component.OnSelf		-= action;
		}

		public				void					Remove_OnSelf<TComp, TCompListen>(  )
			where TCompListen: Event_Self<TComp>, TScope, IComponent
			where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			RemoveComponent( index );
		}

		public				void					Add_OnSelfRemoved<TComp, TCompListen>( Action<Contexts, IEntity, TComp> action )
			where TCompListen : Event_SelfRemoved<TComp>, TScope, new()
			where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;

			TCompListen component;
			if ( HasComponent( index ) )
			{
				component			= Get<TCompListen>();
			}
			else
			{
				component			= CreateComponent<TCompListen>(index);
				AddComponent(index, component);
				component.OnSelfRemoved	= null;
			}

			component.OnSelfRemoved	+= action;
		}

		public				void					Remove_OnSelfRemoved<TComp, TCompListen>( Action<Contexts, IEntity, TComp> action )
			where TCompListen : Event_SelfRemoved<TComp>, TScope, new()
			where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= (TCompListen)GetComponent(index);
			component.OnSelfRemoved	-= action;
		}

		public				void					Remove_OnSelfRemoved<TComp, TCompListen>(  )
			where TCompListen : Event_SelfRemoved<TComp>, TScope
			where TComp : IComponent, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			RemoveComponent( index );
		}
	}
}