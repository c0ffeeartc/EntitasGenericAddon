using System;

namespace Entitas.Generic
{
public partial class ScopedContext<TScope>
{
	public					Entity<TScope>			GetEntity_<TData>		(  )
			where TData : struct, Scope<TScope>, IComponent, IUnique
	{
		return GetGroup( Matcher<TScope, TData>.I ).GetSingleEntity(  );
	}

	public					TData					Get_<TData>				(  )
			where TData : struct, Scope<TScope>, IComponent, ICompData, IUnique
	{
		return GetEntity_<TData>( ).Get_<TData>( );
	}

	public					Boolean					Has_<TData>				(  )
			where TData : struct, Scope<TScope>, IComponent, ICompData, IUnique
	{
		return GetGroup( Matcher<TScope, TData>.I ).GetSingleEntity(  ) != null;
	}

	public					Entity<TScope>			Set_<TData>				( TData data )
			where TData : struct, Scope<TScope>, IComponent, ICompData, IUnique
	{
		if ( Has_<TData>(  ) )
		{
			var typename = typeof(TData).ToString(  );
			throw new Entitas.EntitasException("Could not set " + typename + "!\n" + this + " already has an entity with " + typename + "!",
				"You should check if the context already has a " + typename + "Entity before setting it or use context.Replace<" + typename + ">().");
		}
		var entity = CreateEntity(  );
		entity.Add_( data );
		return entity;
	}

	public					void					Remove_<TData>			(  )
			where TData : struct, Scope<TScope>, IComponent, ICompData, IUnique
	{
		GetEntity_<TData>(  ).Remove_<TData>(  );
	}

	public					void					Replace_<TData>			( TData data )
			where TData : struct, Scope<TScope>, IComponent, ICompData, IUnique
	{
		var entity					= GetEntity_<TData>(  );
		if ( entity == null )
		{
			entity					= Set_( data );
		}
		else
		{
			entity.Replace_( data );
		}
	}
}
}
