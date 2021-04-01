using System;

namespace Entitas.Generic
{
	public partial class Entity<TScope>
		: Entitas.Entity
		where TScope : IScope
	{
		public				void					Add_<TData>				( TData data )
				where TData : struct, Scope<TScope>, ICompData, IComponent
		{
			var index				= Lookup<TScope, TData>.Id;
			var component			= CreateComponent<StructComponent<TData>>( index );
			component.Data			= data;
			AddComponent(index, component);
		}

		public				void					Replace_<TData>			( TData data )
				where TData : struct, Scope<TScope>, ICompData, IComponent
		{
			var index				= Lookup<TScope, TData>.Id;
			var component			= CreateComponent<StructComponent<TData>>( index );
			component.Data			= data;
			ReplaceComponent(index, component);
		}

		public				void					Remove_<TData>			(  )
				where TData : struct, Scope<TScope>, ICompData, IComponent
		{
			RemoveComponent( Lookup<TScope, TData>.Id );
		}

		public				void					RemoveSafe_<TData>		(  )
				where TData : struct, Scope<TScope>, ICompData, IComponent
		{
			var index				= Lookup<TScope,TData>.Id;
			if (HasComponent(index))
			{
				RemoveComponent(index);
			}
		}

		public				TData					Get_<TData>				(  )
				where TData : struct, Scope<TScope>, IComponent
		{
			return ((StructComponent<TData>) GetComponent(Lookup<TScope, TData>.Id)).Data;
		}

		public				TData					GetOrAdd_<TData>		(  )
				where TData : struct, Scope<TScope>, IComponent, ICompData
		{
			var index				= Lookup<TScope,TData>.Id;
			if ( HasComponent( index ) )
			{
				return ((StructComponent<TData>) GetComponent( index )).Data;
			}

			var component			= CreateComponent<StructComponent<TData>>( index );
			AddComponent(index, component);
			return component.Data;
		}

		public				Boolean					Has_<TData>				(  )
				where TData : struct, Scope<TScope>, IComponent, ICompData
		{
			return HasComponent(Lookup<TScope, TData>.Id);
		}
	}
}
