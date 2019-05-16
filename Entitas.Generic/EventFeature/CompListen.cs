using System;

namespace Entitas.Generic
{
	public abstract class CompListen_Self<TScope,TComp>
			: IComponent
			where TScope : IScope
	{
		public Action<Contexts, Entity<TScope>, TComp> OnSelf				= delegate {  };
	}

	public abstract class CompListen_SelfRemoved<TScope,TComp>
			: IComponent
			where TScope : IScope
	{
		public Action<Contexts, Entity<TScope>, TComp> OnSelfRemoved		= delegate {  };
	}

	public abstract class CompListen_Any<TScope,TComp>
			: IComponent
			where TScope : IScope
	{
		public Action<Contexts, Entity<TScope>, TComp> OnAny				= delegate {  };
	}

	public abstract class CompListen_AnyRemoved<TScope,TComp>
			: IComponent
			where TScope : IScope
	{
		public Action<Contexts, Entity<TScope>, TComp> OnAnyRemoved			= delegate {  };
	}
}