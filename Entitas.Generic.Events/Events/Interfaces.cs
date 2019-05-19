namespace Entitas.Generic
{
	public interface IOnSelf<TComp, TCompListen>
			where TComp : IComponent
			where TCompListen: Event_Self<TComp, TCompListen>
	{
		void				OnSelf					( TComp component, IEntity entity, Contexts contexts );
	}

	public interface IOnSelfRemoved<TComp, TCompListen>
			where TComp : IComponent
			where TCompListen: Event_SelfRemoved<TComp, TCompListen>
	{
		void				OnSelfRemoved			( TComp component, IEntity entity, Contexts contexts );
	}

	public interface IOnAny<TComp, TCompListen>
			where TComp : IComponent
			where TCompListen: Event_Any<TComp, TCompListen>
	{
		void				OnAny					( TComp component, IEntity entity, Contexts contexts );
	}

	public interface IOnAnyRemoved<TComp, TCompListen>
			where TComp : IComponent
			where TCompListen: Event_AnyRemoved<TComp, TCompListen>
	{
		void				OnAnyRemoved			( TComp component, IEntity entity, Contexts contexts );
	}
}