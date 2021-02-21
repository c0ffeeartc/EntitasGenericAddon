using System;
using System.Collections;
using System.Reflection;

namespace Entitas.Generic
{
public class Lookup_ScopeManager
{
	public static			void					RegisterScope<TScope>	(  ) where TScope : IScope
	{
		if ( Lookup<TScope>.Id >= 0 )
		{
			return;
		}

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
		Scopes.IScopeTypes.Add(typeof(TScope));
		Scopes.ScopedContextTypes.Add(typeof(ScopedContext<TScope>));
		Scopes.CompScopeTypes.Add(typeof(Scope<TScope>));
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
					MethodInfo methodInfo  = typeof(Lookup_ScopeManager).GetMethod( nameof(Lookup_ScopeManager.RegisterScope), BindingFlags.Static | BindingFlags.Public );
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
}
}