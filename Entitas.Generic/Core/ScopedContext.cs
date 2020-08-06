using System;

namespace Entitas.Generic
{
    public partial class ScopedContext<TScope> : Context<Entity<TScope>> where TScope : IScope
    {
        public ScopedContext(Func<IEntity, IAERC> aercFactory) : base(
            Lookup<TScope>.CompCount,
            1,
            new ContextInfo(
                typeof(TScope).Name,
                Lookup<TScope>.CompNamesPrettyArray,
                Lookup<TScope>.CompTypesArray
                ),
            aercFactory,
            () => new Entity<TScope>(  ) )
        {
        }
    }
}