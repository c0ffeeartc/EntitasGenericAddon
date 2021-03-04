﻿using System;

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

		public				void					AddV_<TData>	( TData data )
				where TData : struct, Scope<TScope>, ICompData, IComponent, IVersionChanged
		{
			var index				= Lookup<TScope, TData>.Id;
            var component			= GetOrCreate_<TData>();
			component.Data			= data;
			component.Data.VersionChanged = Version.Global;
			component.Data.VersionChangedId = VersionChangeId.Add;
			ReplaceComponent(index, component);
		}

		public				void					ReplaceV_<TData>( TData data )
				where TData : struct, Scope<TScope>, ICompData, IComponent, IVersionChanged
		{
			var index				= Lookup<TScope, TData>.Id;
            var component			= GetOrCreate_<TData>();
			component.Data			= data;
			component.Data.VersionChanged = Version.Global;
			component.Data.VersionChangedId = VersionChangeId.Replace;
			ReplaceComponent(index, component);
		}

		public				void					RemoveV_<TData>	(  )
				where TData : struct, Scope<TScope>, ICompData, IComponent, IVersionChanged
		{
			var comp = ((StructComponent<TData>) GetComponent(Lookup<TScope, TData>.Id));
			comp.Data.VersionChanged = Version.Global;
			comp.Data.VersionChangedId = VersionChangeId.Remove;
		}

		public				TData					Get_<TData>				(  )
				where TData : struct, Scope<TScope>, IComponent
		{
			return ((StructComponent<TData>) GetComponent(Lookup<TScope, TData>.Id)).Data;
		}

		public				Boolean					Has_<TData>				(  )
				where TData : struct, Scope<TScope>, IComponent, ICompData
		{
			return HasComponent(Lookup<TScope, TData>.Id);
		}

		private 			StructComponent<TData>	GetOrCreate_<TData>		(  )
				where TData : struct, Scope<TScope>, IComponent, ICompData
		{
			var index				= Lookup<TScope, TData>.Id;
			if (Has_<TData>())
			{
				return (StructComponent<TData>) GetComponent( index );
			}
			return CreateComponent<StructComponent<TData>>( index );
		}
	}
}
