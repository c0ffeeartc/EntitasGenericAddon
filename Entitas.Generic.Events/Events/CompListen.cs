using System.Collections.Generic;

namespace Entitas.Generic
{
	public abstract class Event_Self<TComp, TCompListen>
			: IComponent
				where TComp : IComponent
				where TCompListen : Event_Self<TComp, TCompListen>
	{
		public List<IOnSelf<TComp, TCompListen>> Listeners = new List<IOnSelf<TComp, TCompListen>>(  );
	}

	public abstract class Event_SelfRemoved<TComp, TCompListen>
			: IComponent
				where TComp : IComponent
				where TCompListen : Event_SelfRemoved<TComp, TCompListen>
	{
		public List<IOnSelfRemoved<TComp, TCompListen>> Listeners = new List<IOnSelfRemoved<TComp, TCompListen>>(  );
	}

	public abstract class Event_Any<TComp, TCompListen>
			: IComponent
				where TComp : IComponent
				where TCompListen : Event_Any<TComp, TCompListen>
	{
		public List<IOnAny<TComp, TCompListen>> Listeners = new List<IOnAny<TComp, TCompListen>>(  );
	}

	public abstract class Event_AnyRemoved<TComp, TCompListen>
			: IComponent
				where TComp : IComponent
				where TCompListen : Event_AnyRemoved<TComp, TCompListen>
	{
		public List<IOnAnyRemoved<TComp, TCompListen>> Listeners = new List<IOnAnyRemoved<TComp, TCompListen>>(  );
	}
}