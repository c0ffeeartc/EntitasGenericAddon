using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Generic;

//namespace Entitas.Generic
//{
public partial class Contexts
{
	private					List<IContext>			_all;
	public					List<IContext>			All						=> _all;

	public					void					AddScopedContexts		(  )
	{
		if ( _all != null )
		{
			return;
		}

		_all				= new List<IContext>(ScopeCount.Value);

		for ( var i = 0; i < ScopeCount.Value; i++ )
		{
			_all.Add( Lookup_ScopeManager.CreateContext(i,
				#if (ENTITAS_FAST_AND_UNSAFE)
				AERCFactories.UnsafeAERCFactory
				#else
				AERCFactories.SafeAERCFactory
				#endif
				) );
		}
	}

	public					ScopedContext<TScope>	Get<TScope>				(  ) where TScope : IScope
	{
		return (ScopedContext<TScope>) _all[Lookup<TScope>.Id];
	}

	public					IContext				Get						( Int32 index )
	{
		return _all[index];
	}
}
//}
