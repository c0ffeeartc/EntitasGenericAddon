using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType
namespace Entitas.Generic
{
public class Lookup_ComponentManager<TScope> where TScope : IScope
{
	private static			Boolean					_isInit;

	public static			void					Autoscan				(  )
	{
		if ( _isInit )
		{
			return;
		}
		_isInit = true;

		Scan_IComponents(  );
		Scan_EventAny(  );
		Scan_EventAnyRemoved(  );
		Scan_EventSelf(  );
		Scan_EventSelfRemoved(  );

		// Console.WriteLine( "\n" + typeof(TScope).Name + ": " + Lookup<TScope>.CompCount.ToString(  ) );
		// for ( var i = 0; i < Lookup<TScope>.CompTypes.Count; i++ )
		// {
		// 	var regT = Lookup<TScope>.CompTypes[i];
		// 	Console.WriteLine( regT.ToGenericTypeString( ) );
		// }
	}

	private static			void					Scan_IComponents		(  )
	{
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (var type in assembly.GetTypes())
			{
				if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent)) )
				{
					continue;
				}

				if ( !IsInScope(type))
				{
					continue;
				}

				Register(type);
			}
		}
	}

	private static			List<Type>				Collect_EventAnyComps	(  )
	{
		var eventComps				= new List<Type>(  );
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies(  ) )
		{
			foreach (var type in assembly.GetTypes())
			{
				if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
					|| !IsInScope(type)
					|| !IsEventAnyChild(type) )
				{
					continue;
				}
				eventComps.Add( type );
			}
		}
		return eventComps;
	}

	private static			List<Type>				Collect_EventAnyRemovedComps(  )
	{
		var eventComps = new List<Type>();
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (var type in assembly.GetTypes())
			{
				if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
					|| !IsInScope(type)
					|| !IsEventAnyRemovedChild(type) )
				{
					continue;
				}
				eventComps.Add( type );
			}
		}
		return eventComps;
	}

	private static			List<Type>				Collect_EventSelfComps	(  )
	{
		var eventComps = new List<Type>();
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (var type in assembly.GetTypes())
			{
				if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
					|| !IsInScope(type)
					|| !IsEventSelfChild(type) )
				{
					continue;
				}
				eventComps.Add( type );
			}
		}
		return eventComps;
	}

	private static			List<Type>				Collect_EventSelfRemovedComps(  )
	{
		var eventComps = new List<Type>();
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (var type in assembly.GetTypes())
			{
				if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
					|| !IsInScope(type)
					|| !IsEventSelfRemovedChild(type) )
				{
					continue;
				}
				eventComps.Add( type );
			}
		}
		return eventComps;
	}

	public static			void					Scan_EventAny			(  )
	{
		var eventComps = Collect_EventAnyComps(  );
		var t_Event = typeof(Event_AnyComponent<,>);
		for ( var i = 0; i < eventComps.Count; i++ )
		{
			var t_Inner = eventComps[i];

			var eventType = t_Event.MakeGenericType( typeof(TScope), t_Inner );
			Register( eventType );
		}
	}

	public static			void					Scan_EventAnyRemoved	(  )
	{
		var eventComps = Collect_EventAnyRemovedComps(  );
		var t_Event = typeof(Event_AnyRemovedComponent<,>);
		for ( var i = 0; i < eventComps.Count; i++ )
		{
			var t_Inner = eventComps[i];

			var eventType = t_Event.MakeGenericType( typeof(TScope), t_Inner );
			Register( eventType );
		}
	}

	public static			void					Scan_EventSelf			(  )
	{
		var eventComps = Collect_EventSelfComps(  );
		var t_Event = typeof(Event_SelfComponent<,>);
		for ( var i = 0; i < eventComps.Count; i++ )
		{
			var t_Inner = eventComps[i];

			var eventType = t_Event.MakeGenericType( typeof(TScope), t_Inner );
			Register( eventType );
		}
	}

	public static			void					Scan_EventSelfRemoved	(  )
	{
		var eventComps = Collect_EventSelfRemovedComps(  );
		var t_Event = typeof(Event_SelfRemovedComponent<,>);
		for ( var i = 0; i < eventComps.Count; i++ )
		{
			var t_Inner = eventComps[i];

			var eventType = t_Event.MakeGenericType( typeof(TScope), t_Inner );
			Register( eventType );
		}
	}

	private static			void					Register				( Type dataType )
	{
		try
		{
			var lookupT1T2 = typeof(Lookup<,>);

			// if ( !dataType.IsClass )
			// {
			// 	var structComponentType = typeof(StructComponent<>);
			// 	dataType = structComponentType.MakeGenericType(dataType);
			// }

			var lookupTScopeTComp = lookupT1T2.MakeGenericType(typeof(TScope), dataType);

			if (Lookup<TScope>.CompTypes.Contains(lookupTScopeTComp))
			{
				return;
			}

			Lookup<TScope>.CompTypes.Add( dataType );
			Lookup<TScope>.CompNamesFull.Add( dataType.FullName );
			Lookup<TScope>.CompNamesPretty.Add( dataType.ToGenericTypeString(  ) );

			var fieldInfo = lookupTScopeTComp.GetField("Id" ,
				BindingFlags.Static
				| BindingFlags.SetField
				| BindingFlags.Public
				);

			if (fieldInfo == null)
			{
				throw new Exception(string.Format("Type `{0}' does not contains `Id' field", lookupTScopeTComp.Name));
			}

			fieldInfo.SetValue(null, Lookup<TScope>.CompCount);
			Lookup<TScope>.CompTypeToI[dataType] = Lookup<TScope>.CompCount;
			Lookup<TScope>.CompCount++;
		}
		catch ( Exception )  // when there is unused IComponent in code - iOS may get exception because of missing AOT 
		{
			// Console.WriteLine( ex.Message );
		}
	}

	private static			bool					IsInScope				( Type type )
	{
		return type.GetInterfaces(  )
			.Any( x =>
				x.IsGenericType
				&& x.GetGenericTypeDefinition(  ) == typeof(Scope<>)
				&& x.GetGenericArguments()[0] == typeof(TScope)
				);
	}

	private static			bool					IsEventAnyChild			( Type type )
	{
		return type.GetInterfaces(  )
			.Any( x =>
				x.IsGenericType
				&& x.GetGenericTypeDefinition(  ) == typeof(IEvent_Any<,>)
				&& x.GetGenericArguments()[0] == typeof(TScope)
				&& x.GetGenericArguments()[1] == type
				);
	}

	private static			bool					IsEventAnyRemovedChild	( Type type )
	{
		return type.GetInterfaces(  )
			.Any( x =>
				x.IsGenericType
				&& x.GetGenericTypeDefinition(  ) == typeof(IEvent_AnyRemoved<,>)
				&& x.GetGenericArguments()[0] == typeof(TScope)
				&& x.GetGenericArguments()[1] == type
				);
	}

	private static			bool					IsEventSelfChild		( Type type )
	{
		return type.GetInterfaces(  )
			.Any( x =>
				x.IsGenericType
				&& x.GetGenericTypeDefinition(  ) == typeof(IEvent_Self<,>)
				&& x.GetGenericArguments()[0] == typeof(TScope)
				&& x.GetGenericArguments()[1] == type
				);
	}

	private static			bool					IsEventSelfRemovedChild	( Type type )
	{
		return type.GetInterfaces(  )
			.Any( x =>
				x.IsGenericType
				&& x.GetGenericTypeDefinition(  ) == typeof(IEvent_SelfRemoved<,>)
				&& x.GetGenericArguments()[0] == typeof(TScope)
				&& x.GetGenericArguments()[1] == type
				);
	}
}
}