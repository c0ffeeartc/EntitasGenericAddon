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

		_all				= new List<IContext>(Scopes.Count);

		for ( var i = 0; i < Scopes.Count; i++ )
		{
			_all.Add( Scopes.CreateContext[i].Invoke(  ) );
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
