#if ENT_GA_SPECIALIZATION
namespace Entitas.Generic
{
public class SpecAdd<TScope, TComp>
	where TScope : IScope
	where TComp : class, Scope<TScope>, ICompData, ICopyFrom<TComp>, IComponent
{
	public static			IAdd<TScope,TComp>		Spec;
}

public class SpecAdd_<TScope, TData>
	where TScope : IScope
	where TData : struct, Scope<TScope>, ICompData, IComponent
{
	public static			IAdd_<TScope,TData>		Spec;
}

public class SpecReplace<TScope, TComp>
	where TScope : IScope
	where TComp : class, Scope<TScope>, ICompData, ICopyFrom<TComp>, IComponent, new()
{
	public static			IReplace<TScope,TComp>	Spec;
}

public class SpecReplace_<TScope, TData>
	where TScope : IScope
	where TData : struct, Scope<TScope>, ICompData, IComponent
{
	public static			IReplace_<TScope,TData>	Spec;
}
}
#endif