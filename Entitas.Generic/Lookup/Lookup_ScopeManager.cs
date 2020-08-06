using System;
using System.Collections;
using System.Reflection;

namespace Entitas.Generic
{
public class Lookup_ScopeManager
{
	public const			int						MaxScopes				= 32;

	private static readonly	Type[]					_contextTypes			= new Type[MaxScopes];
	private static readonly	Func<Func<IEntity, IAERC>, IContext>[] _factories = new Func<Func<IEntity, IAERC>, IContext>[MaxScopes];

	public static			IContext				CreateContext			( int index, Func<IEntity, IAERC> aercFactory )
	{
		return _factories[index](aercFactory);
	}

	public static			void					RegisterScope<TScope>	(  ) where TScope : IScope
	{
		var scopeType				= typeof(TScope);

		if ( IsScopeRegistered( scopeType ) )
		{
			return;
		}

		_contextTypes[ScopeCount.Value] = typeof(TScope);
		_factories[ScopeCount.Value] = (aercFactory) => new ScopedContext<TScope>(aercFactory);
		Lookup<TScope>.Id			= ScopeCount.Value;
		ScopeCount.Value++;
	}

	public static			void					RegisterAll				(  )
	{
		if ( ScopeCount.Value > 0 )
		{
			return;
		}

		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (var type in assembly.GetTypes())
			{
				if ( !type.IsInterface
					|| !((IList) type.GetInterfaces()).Contains(typeof(IScope))
					// || IsScopeRegistered( type )
					)
				{
					continue;
				}

				{
					MethodInfo methodInfo  = typeof(Lookup_ScopeManager).GetMethod( "RegisterScope", BindingFlags.Static | BindingFlags.Public );
					var generic			= methodInfo.MakeGenericMethod( type );
					generic.Invoke( null, null );
				}

				{
					var managerType = typeof(Lookup_ComponentManager<>);
					var genericManagerType = managerType.MakeGenericType(type);
					MethodInfo methodInfo  = genericManagerType.GetMethod( "Autoscan", BindingFlags.Static | BindingFlags.Public );
					methodInfo.Invoke( null, null );
				}
			}
		}
	}

	private static			bool					IsScopeRegistered		( Type type )
	{
		for (var i = 0; i < ScopeCount.Value; i++)
		{
			if ( type == _contextTypes[i] )
			{
				return true;
			}
		}
		return false;
	}
}
}