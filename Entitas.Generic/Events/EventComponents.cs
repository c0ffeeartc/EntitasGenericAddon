using System.Collections.Generic;

namespace Entitas.Generic
{
	public sealed class Event_AnyComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_Any<TScope, TComp>, ICompData
	{
		public List<IOnAny<TScope, TComp>> Listeners = new List<IOnAny<TScope, TComp>>(  );
	}

	public sealed class Event_SelfComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_Self<TScope, TComp>, ICompData
	{
		public List<IOnSelf<TScope, TComp>> Listeners = new List<IOnSelf<TScope, TComp>>(  );
	}

	public sealed class Event_AnyRemovedComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_AnyRemoved<TScope,TComp>, ICompData
	{
		public List<IOnAnyRemoved<TScope,TComp>> Listeners = new List<IOnAnyRemoved<TScope,TComp>>(  );
	}

	public sealed class Event_SelfRemovedComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_SelfRemoved<TScope, TComp>, ICompData
	{
		public List<IOnSelfRemoved<TScope, TComp>> Listeners = new List<IOnSelfRemoved<TScope, TComp>>(  );
	}

	public sealed class Event_AnyFlagComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_AnyFlag<TScope, TComp>, ICompFlag
	{
		public List<IOnAnyFlag<TScope, TComp>> Listeners = new List<IOnAnyFlag<TScope, TComp>>(  );
	}

	public sealed class Event_SelfFlagComponent<TScope, TComp>
			: IComponent, Scope<TScope>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, IEvent_SelfFlag<TScope, TComp>, ICompFlag
	{
		public List<IOnSelfFlag<TScope, TComp>> Listeners = new List<IOnSelfFlag<TScope, TComp>>(  );
	}
}