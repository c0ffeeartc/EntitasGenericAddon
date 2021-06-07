namespace Entitas.Generic
{
	public interface IEvent_Any<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, ICompData
	{
	}

	public interface IEvent_AnyRemoved<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, ICompData
	{
	}

	public interface IEvent_Self<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, ICompData
	{
	}

	public interface IEvent_SelfRemoved<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, ICompData
	{
	}

	public interface IEvent_AnyFlag<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, ICompFlag
	{
	}

	public interface IEvent_SelfFlag<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, Scope<TScope>, ICompFlag
	{
	}
}