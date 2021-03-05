using System;

namespace Entitas.Generic
{
	public partial class Entity<TScope>
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
				where TData : struct, Scope<TScope>, ICompData, IComponent
		{
			var index				= Lookup<TScope, TData>.Id;
            var component			= GetOrCreate_<TData>();
			component.Data			= data;
			component.VersionChanged = Version.Global;
			component.VersionChangedId = VersionChangeId.Add;
			ReplaceComponent(index, component);
		}

		public				void					ReplaceV_<TData>( TData data )
				where TData : struct, Scope<TScope>, ICompData, IComponent
		{
			var index				= Lookup<TScope, TData>.Id;
            var component			= GetOrCreate_<TData>();
			component.Data			= data;
			component.VersionChanged = Version.Global;
			component.VersionChangedId = VersionChangeId.Replace;
			ReplaceComponent(index, component);
		}

		public				void					RemoveV_<TData>	(  )
				where TData : struct, Scope<TScope>, ICompData, IComponent
		{
			var comp = (StructComponent<TData>) GetComponent(Lookup<TScope, TData>.Id);
			comp.VersionChanged		= Version.Global;
			comp.VersionChangedId	= VersionChangeId.Remove;
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

		public				Boolean					HasV_<TData>				(  )
				where TData : struct, Scope<TScope>, IComponent, ICompData
		{
			var component			= GetComponentNoCheck(Lookup<TScope, TData>.Id);
			return component != null
				&& ((StructComponent<TData>)component).VersionChangedId != VersionChangeId.Remove;
		}

		public				Boolean					HasChanged_<TData>		( Int32 systemLastVersion )
				where TData : struct, Scope<TScope>, IComponent, ICompData
		{
			var component			= GetComponentNoCheck(Lookup<TScope, TData>.Id);
			return component != null
				&& ((StructComponent<TData>)component).VersionChangedId - systemLastVersion > 0;
		}

		private 			StructComponent<TData>	GetOrCreate_<TData>		(  )
				where TData : struct, Scope<TScope>, IComponent, ICompData
		{
			var index				= Lookup<TScope, TData>.Id;
			if (HasComponent(index))
			{
				return (StructComponent<TData>)GetComponentNoCheck( index );
			}
			return CreateComponent<StructComponent<TData>>( index );
		}

		private 			IComponent				GetComponentNoCheck		( Int32 index )
		{
			return _components[index];
		}
	}
}
