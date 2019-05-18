using System;

namespace Entitas.Generic
{
	public abstract class Event_Self<TComp> : IComponent where TComp : IComponent
	{
		public Action<Contexts, IEntity, TComp> OnSelf;
	}

	public abstract class Event_SelfRemoved<TComp> : IComponent where TComp : IComponent
	{
		public Action<Contexts, IEntity, TComp> OnSelfRemoved;
	}

	public abstract class Event_Any<TComp> : IComponent where TComp : IComponent
	{
		public Action<Contexts, IEntity, TComp> OnAny;
	}

	public abstract class Event_AnyRemoved<TComp> : IComponent where TComp : IComponent
	{
		public Action<Contexts, IEntity, TComp> OnAnyRemoved;
	}
}