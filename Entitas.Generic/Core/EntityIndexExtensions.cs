using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
	public interface IListen<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, IScope
	{
	}
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

	public static		HashSet<Entity<TScope>>	GetEntities<TScope, TKey>				( this Context<Entity<TScope>> context, String indexKey, TKey entityKey )
			where TScope : IScope
	{
		return ((EntityIndex<Entity<TScope>, TKey>)context.GetEntityIndex( indexKey )).GetEntities( entityKey );
	}
}
}