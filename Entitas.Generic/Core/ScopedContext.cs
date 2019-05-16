using System;

namespace Entitas.Generic
{
    public partial class ScopedContext<TScope> : Context<Entity<TScope>> where TScope : IScope
    {
        public ScopedContext(Func<IEntity, IAERC> aercFactory) : base(
            Lookup_ComponentManager<TScope>.TotalComponents,
            1,
            new ContextInfo(
                typeof(TScope).Name,
                Lookup_ComponentManager<TScope>.ComponentNamesCache,
                Lookup_ComponentManager<TScope>.ComponentTypesCache
            ),
            aercFactory,
            () => new Entity<TScope>(  ) )
        {
        }
    }
}