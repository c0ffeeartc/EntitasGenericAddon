using System;

namespace Entitas.Generic
{
public partial class ScopedContext<TScope> : Context<Entity<TScope>> where TScope : IScope
{
	public					ScopedContext			(
			int totalComponents,
			int startCreationIndex,
			ContextInfo contextInfo,
			Func<IEntity, IAERC> aercFactory,
			Func<Entity<TScope>> entityFactory)
				: base( totalComponents, startCreationIndex, contextInfo, aercFactory, entityFactory )
	{
	}
}
}