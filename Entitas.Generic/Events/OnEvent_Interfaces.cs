namespace Entitas.Generic
{
	public interface IOnSelf<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, IEvent_Self<TScope, TComp>, Scope<TScope>, ICompData
	{
		void				OnSelf					( TComp component, Entity<TScope> entity, Contexts contexts );
	}

	public interface IOnSelfRemoved<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, IEvent_SelfRemoved<TScope, TComp>, Scope<TScope>, ICompData
	{
		void				OnSelfRemoved			( TComp component, Entity<TScope> entity, Contexts contexts );
	}

	public interface IOnAny<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, IEvent_Any<TScope, TComp>, Scope<TScope>, ICompData
	{
		void				OnAny					( TComp component, Entity<TScope> entity, Contexts contexts );
	}

	public interface IOnAnyRemoved<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, IEvent_AnyRemoved<TScope, TComp>, Scope<TScope>, ICompData
	{
		void				OnAnyRemoved			( TComp component, Entity<TScope> entity, Contexts contexts );
	}

	public interface IOnAnyFlag<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, IEvent_AnyFlag<TScope, TComp>, Scope<TScope>, ICompFlag
	{
		void				OnAnyFlag				( TComp component, Entity<TScope> entity, Contexts contexts );
	}

	public interface IOnSelfFlag<TScope, TComp>
			where TScope : IScope
			where TComp : IComponent, IEvent_SelfFlag<TScope, TComp>, Scope<TScope>, ICompFlag
	{
		void				OnSelfFlag				( TComp component, Entity<TScope> entity, Contexts contexts );
	}
}