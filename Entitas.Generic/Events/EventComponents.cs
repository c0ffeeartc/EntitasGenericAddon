using System.Collections.Generic;

namespace Entitas.Generic
{
	public sealed class Event_AnyComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_Any<TScope, TComp>
	{
		public List<IOnAny<TScope, TComp>> Listeners = new List<IOnAny<TScope, TComp>>(  );
	}

	public sealed class Event_SelfComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_Self<TScope, TComp>
	{
		public List<IOnSelf<TScope, TComp>> Listeners = new List<IOnSelf<TScope, TComp>>(  );
	}

	public sealed class Event_AnyRemovedComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_AnyRemoved<TScope,TComp>
	{
		public List<IOnAnyRemoved<TScope,TComp>> Listeners = new List<IOnAnyRemoved<TScope,TComp>>(  );
	}

	public sealed class Event_SelfRemovedComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_SelfRemoved<TScope, TComp>
	{
		public List<IOnSelfRemoved<TScope, TComp>> Listeners = new List<IOnSelfRemoved<TScope, TComp>>(  );
	}
}