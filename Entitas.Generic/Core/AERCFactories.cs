using System;
using Entitas;

namespace Entitas.Generic
{
    public class AERCFactories
    {
        public static readonly Func<IEntity, IAERC> UnsafeAERCFactory = e => new UnsafeAERC();
        public static readonly Func<IEntity, IAERC> SafeAERCFactory = e => new SafeAERC(e);
    }
}