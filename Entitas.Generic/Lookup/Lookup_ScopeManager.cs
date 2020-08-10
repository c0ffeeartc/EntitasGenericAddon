using System;
using System.Collections;
using System.Reflection;

namespace Entitas.Generic
{
public class Lookup_ScopeManager
{
	public const			int						MaxScopes				= 32;

	private static readonly	Type[]					_contextTypes			= new Type[MaxScopes];

	public static			void					RegisterScope<TScope>	(  ) where TScope : IScope
	{
		var scopeType				= typeof(TScope);

		if ( IsScopeRegistered( scopeType ) )
		{
			return;
		}

		_contextTypes[Scopes.Count] = typeof(TScope);
		Scopes.CreateContext.Add( () => new ScopedContext<TScope>
			(
				Lookup<TScope>.CompCount,
				1,
				new ContextInfo(
					typeof(TScope).Name,
					Lookup<TScope>.CompNamesPrettyArray,
					Lookup<TScope>.CompTypesArray
					),
				#if (ENTITAS_FAST_AND_UNSAFE)
				AERCFactories.UnsafeAERCFactory
				#else
				AERCFactories.SafeAERCFactory
				#endif
					,
				() => new Entity<TScope>(  )
			) );

		Lookup<TScope>.Id			= Scopes.Count;
		Scopes.Count++;
	}

	public static			void					RegisterAll				(  )
	{
		if ( Scopes.Count > 0 )
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
		for (var i = 0; i < Scopes.Count; i++)
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