using System;

namespace Entitas.Generic
{
	public abstract class Event_OnSelf<TScope,TComp>
			: IComponent
			where TScope : IScope
	{
		public Action<Contexts, Entity<TScope>, TComp> OnSelf				= delegate {  };
	}

	public abstract class Event_OnSelfRemoved<TScope,TComp>
			: IComponent
			where TScope : IScope
	{
		public Action<Contexts, Entity<TScope>, TComp> OnSelfRemoved		= delegate {  };
	}

	public abstract class Event_OnAny<TScope,TComp>
			: IComponent
			where TScope : IScope
	{
		public Action<Contexts, Entity<TScope>, TComp> OnAny				= delegate {  };
	}

	public abstract class Event_OnAnyRemoved<TScope,TComp>
			: IComponent
			where TScope : IScope
	{
		public Action<Contexts, Entity<TScope>, TComp> OnAnyRemoved			= delegate {  };
	}
}