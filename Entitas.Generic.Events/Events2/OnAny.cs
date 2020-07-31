using System;
using Entitas;
using Entitas.Generic;

namespace Custom.Scripts
{
public sealed class OnAny<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_Any<TScope, TComp>, Scope<TScope>
{
	public static Action<Entity<TScope>> Action = delegate {  };
}

public sealed class OnAny_Removed<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_AnyRemoved<TScope, TComp>, Scope<TScope>
{
	public static Action<Entity<TScope>> Action = delegate {  };
}

public sealed class OnAny_Flag<TScope, TComp>
		where TScope : IScope
		where TComp : IComponent, ICompFlag, IEvent_Any<TScope, TComp>, Scope<TScope>
{
	public static Action<Entity<TScope>> Action = delegate {  };
}
}