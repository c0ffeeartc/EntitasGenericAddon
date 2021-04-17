#if ENT_GA_SPECIALIZATION
namespace Entitas.Generic
{
public interface IReplace<TScope,TComp>
	where TScope : IScope
	where TComp : class, Scope<TScope>, ICompData, ICopyFrom<TComp>, IComponent, new()
{
	void					Replace					(Entity<TScope> ent, TComp comp);
}

public interface IReplace_<TScope,TData>
		where TScope : IScope
		where TData : struct, Scope<TScope>, ICompData, IComponent
{
    void					Replace_				(Entity<TScope> ent, TData data);
}
}
#endif
