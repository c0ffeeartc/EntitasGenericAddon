using System;

namespace Entitas.Generic
{
public partial class ScopedContext<TScope>
{
	public					Entity<TScope>			GetEntity<TComp>		(  )
			where TComp : class, Scope<TScope>, IComponent, IUnique
	{
		return GetGroup( Matcher<TScope, TComp>.I ).GetSingleEntity(  );
	}

	public					TComp					Get<TComp>				(  )
			where TComp : class, Scope<TScope>, IComponent, ICompData, IUnique
	{
		return GetEntity<TComp>( ).Get<TComp>( );
	}

	public					Boolean					Has<TComp>				(  )
			where TComp : class, Scope<TScope>, IComponent, ICompData, IUnique
	{
		return GetGroup( Matcher<TScope, TComp>.I ).GetSingleEntity(  ) != null;
	}

	public					Entity<TScope>			Set<TComp>				( TComp component )
			where TComp : class, Scope<TScope>, IComponent, ICompData, IUnique, ICopyFrom<TComp>, new(  )
	{
		if ( Has<TComp>(  ) )
		{
			var typename = typeof(TComp).ToString(  );
			throw new Entitas.EntitasException("Could not set " + typename + "!\n" + this + " already has an entity with " + typename + "!",
				"You should check if the context already has a " + typename + "Entity before setting it or use context.Replace<" + typename + ">().");
		}
		var entity = CreateEntity(  );
		entity.Add( component );
		return entity;
	}

	public					void					Remove<TComp>			(  )
			where TComp : class, Scope<TScope>, IComponent, ICompData, IUnique
	{
		GetEntity<TComp>(  ).Remove<TComp>( );
	}

	public					void					Replace<TComp>			( TComp component )
			where TComp : class, Scope<TScope>, IComponent, ICompData, IUnique, ICopyFrom<TComp>, new(  )
	{
		var entity					= GetEntity<TComp>(  );
		if ( entity == null )
		{
			entity					= Set( component );
		}
		else
		{
			entity.Replace( component );
		}
	}

	public					void					Flag<TComp>				( Boolean value )
			where TComp : class, Scope<TScope>, IComponent, ICompFlag, IUnique, new(  )
	{
		var ent = GetGroup( Matcher<TScope, TComp>.I ).GetSingleEntity(  );
		if ( value == ( ent != null ) )
		{
			return;
		}

		if ( value )
		{ 
			CreateEntity(  ).Flag<TComp>( true );
		}
		else
		{
			ent.Destroy(  );
		}
	}

	public					Boolean					Is<TComp>				(  )
			where TComp : class, Scope<TScope>, IComponent, ICompFlag, IUnique
	{
		return GetGroup( Matcher<TScope, TComp>.I ).GetSingleEntity(  ) != null;
	}
}
}
