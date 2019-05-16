using System;

namespace Entitas.Generic
{
	public partial class Entity<TScope>
	{
		public				void					Add_OnAny<TComp, TCompListen>( Action<Contexts, Entity<TScope>, TComp> action )
			where TCompListen
				: Event_OnAny<TScope, TComp>
				, TScope
				, IComponent
				, new()
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

		public				void					Remove_OnAny<TComp, TCompListen>( Action<Contexts, Entity<TScope>, TComp> action )
			where TCompListen
				: Event_OnAny<TScope, TComp>
				, TScope
				, IComponent
				, new()
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= Get<TCompListen>();
			component.OnAny			-= action;
		}

		public				void					Remove_OnAny<TComp, TCompListen>(  )
			where TCompListen
				: Event_OnAny<TScope, TComp>
				, TScope
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			RemoveComponent( index );
		}

		public				void					Add_OnAnyRemoved<TComp, TCompListen>( Action<Contexts, Entity<TScope>, TComp> action )
			where TCompListen
				: Event_OnAnyRemoved<TScope, TComp>
				, IComponent
				, TScope
				, new()
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

		public				void					Remove_OnAnyRemoved<TComp, TCompListen>( Action<Contexts, Entity<TScope>, TComp> action )
			where TCompListen
				: Event_OnAnyRemoved<TScope, TComp>
				, TScope
				, IComponent
				, new()
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= Get<TCompListen>();
			component.OnAnyRemoved	-= action;
		}

		public				void					Remove_OnAnyRemoved<TComp, TCompListen>(  )
			where TCompListen : Event_OnAnyRemoved<TScope, TComp>
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			RemoveComponent( index );
		}

		public				void					Add_OnSelf<TComp, TCompListen>( Action<Contexts, Entity<TScope>, TComp> action )
			where TCompListen
				: Event_OnSelf<TScope, TComp>
				, TScope
				, IComponent
				, new()
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

		public				void					Remove_OnSelf<TComp, TCompListen>( Action<Contexts, Entity<TScope>, TComp> action )
			where TCompListen
				: Event_OnSelf<TScope, TComp>
				, TScope
				, IComponent
				, new()
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= (TCompListen)GetComponent(index);
			component.OnSelf		-= action;
		}

		public				void					Remove_OnSelf<TComp, TCompListen>(  )
			where TCompListen
				: Event_OnSelf<TScope, TComp>
				, TScope
				, IComponent
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			RemoveComponent( index );
		}

		public				void					Add_OnSelfRemoved<TComp, TCompListen>( Action<Contexts, Entity<TScope>, TComp> action )
			where TCompListen
				: Event_OnSelfRemoved<TScope, TComp>
				, TScope
				, IComponent
				, new()
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

		public				void					Remove_OnSelfRemoved<TComp, TCompListen>( Action<Contexts, Entity<TScope>, TComp> action )
			where TCompListen
				: Event_OnSelfRemoved<TScope, TComp>
				, TScope
				, IComponent
				, new()
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			var component			= (TCompListen)GetComponent(index);
			component.OnSelfRemoved	-= action;
		}

		public				void					Remove_OnSelfRemoved<TComp, TCompListen>(  )
			where TCompListen : Event_OnSelfRemoved<TScope, TComp>
		{
			var index				= Lookup<TScope, TCompListen>.Id;
			RemoveComponent( index );
		}
	}
}