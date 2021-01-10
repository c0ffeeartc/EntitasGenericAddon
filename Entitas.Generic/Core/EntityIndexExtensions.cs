using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public interface IGetSingleEntByIndex<TKey> {}
public interface IGetAllEntsByIndex<TKey> {}

public static class EntityIndexExtensions
{
	public static		void					AddEntityIndex<TScope, TKey>			( this Context<Entity<TScope>> context
			, String indexKey
			, IGroup<Entity<TScope>> group
			, Func<Entity<TScope>, IComponent, TKey> getKey )
				where TScope : IScope
	{
		context.AddEntityIndex( new EntityIndex<Entity<TScope>, TKey>( indexKey, group, getKey ) );
	}

	public static		void					AddPrimaryEntityIndex<TScope, TKey>		( this Context<Entity<TScope>> context
			, String indexKey
			, IGroup<Entity<TScope>> group
			, Func<Entity<TScope>, IComponent, TKey> getKey )
				where TScope : IScope
	{
		context.AddEntityIndex( new PrimaryEntityIndex<Entity<TScope>, TKey>( indexKey, group, getKey ) );
	}

	public static		HashSet<Entity<TScope>>	GetEntities<TScope, TKey>				( this Context<Entity<TScope>> context, String indexKey, TKey entityKey )
			where TScope : IScope
	{
		return ((EntityIndex<Entity<TScope>, TKey>)context.GetEntityIndex( indexKey )).GetEntities( entityKey );
	}

	public static		Entity<TScope>			GetEntity<TScope, TKey>					( this Context<Entity<TScope>> context, String indexKey, TKey entityKey )
			where TScope : IScope
	{
		return ((PrimaryEntityIndex<Entity<TScope>, TKey>)context.GetEntityIndex( indexKey )).GetEntity( entityKey );
	}

	// for compile time type inference
	public static		HashSet<Entity<TScope>>	GetAllEntsBy<TScope, TComp, TKey>		( this ScopedContext<TScope> context, String indexName, TKey key )
			where TScope : IScope
			where TComp : Scope<TScope>, IGetAllEntsByIndex<TKey>
	{
		return context.GetEntities( indexName, key );
	}

	// for compile time type inference
	public static		Entity<TScope>			GetSingleEntBy<TScope, TComp, TKey>		( this ScopedContext<TScope> context, String indexName, TKey key )
			where TScope : IScope
			where TComp : Scope<TScope>, IGetSingleEntByIndex<TKey>
	{
		return context.GetEntity( indexName, key );
	}
}
}