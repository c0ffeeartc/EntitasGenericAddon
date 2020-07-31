using System;

namespace Entitas.Generic
{
public static class EntExtensions
{
	public static			Boolean					IsValid					( this IEntity entity, Int32 creationIndex )
	{
		return entity != null
			&& entity.isEnabled
			&& entity.creationIndex == creationIndex;
	}
}
}