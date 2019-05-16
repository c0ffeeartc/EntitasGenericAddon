using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public static class EntityIndexExtensions
{
	public static		HashSet<Entity<TScope>>	GetEntities<TScope, TKey>				( this Context<Entity<TScope>> context, String indexKey, TKey entityKey )
			where TScope : IScope
	{
		return ((EntityIndex<Entity<TScope>, TKey>)context.GetEntityIndex( indexKey )).GetEntities( entityKey );
	}
}
}