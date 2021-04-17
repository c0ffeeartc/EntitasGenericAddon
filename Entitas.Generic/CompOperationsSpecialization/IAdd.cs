#if ENT_GA_SPECIALIZATION
namespace Entitas.Generic
{
public interface IAdd<TScope,TComp>
	where TScope : IScope
	where TComp : class, Scope<TScope>, ICompData, ICopyFrom<TComp>, IComponent
{
	void					Add						(Entity<TScope> ent, TComp comp);
}

public interface IAdd_<TScope,TData>
		where TScope : IScope
		where TData : struct, Scope<TScope>, ICompData, IComponent
{
    void					Add_					(Entity<TScope> ent, TData comp);
}
}
#endif