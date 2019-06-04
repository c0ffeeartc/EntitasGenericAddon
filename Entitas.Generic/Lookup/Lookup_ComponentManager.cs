using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType
namespace Entitas.Generic
{
    /// <summary>
    /// Component type manager: manages component types per scope
    /// </summary>
    /// <typeparam name="TScope">Scope</typeparam>
    public class Lookup_ComponentManager<TScope> where TScope : IScope
    {
        private static int _registeredCount;
        private static readonly List<Type> _registeredTypes = new List<Type>();
        private static string[] _componentNamesCache;
        private static Type[] _typesCache;

        public static void Autoscan()
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

            Scan_EventAny(  );
            Scan_EventAnyRemoved(  );
            Scan_EventSelf(  );
            Scan_EventSelfRemoved(  );
        }

        public static void Scan_EventAny()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var event_anyComps = new List<Type>();
                foreach (var type in assembly.GetTypes())
                {
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
                        || !IsInScope(type)
                    || !IsEventAnyChild(type) )
                    {
                        continue;
                    }
                    event_anyComps.Add( type );
                }

                foreach (var type in assembly.GetTypes())
                {
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent)) )
                    {
                        continue;
                    }

                    if ( type.IsGenericType && type.GetGenericTypeDefinition(  ) == typeof(Event_AnyComponent<,>) )
                    {
                        foreach (var t in event_anyComps )
                        {
                            var eventType = type.MakeGenericType(typeof(TScope),t);
                            Register( eventType );
                        }
                    }
                }
            }
        }

        public static void Scan_EventAnyRemoved()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var event_anyComps = new List<Type>();
                foreach (var type in assembly.GetTypes())
                {
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
                        || !IsInScope(type)
                        || !IsEventAnyRemovedChild(type) )
                    {
                        continue;
                    }
                    event_anyComps.Add( type );
                }

                foreach (var type in assembly.GetTypes())
                {
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent)) )
                    {
                        continue;
                    }

                    if ( type.IsGenericType && type.GetGenericTypeDefinition(  ) == typeof(Event_AnyRemovedComponent<,>) )
                    {
                        foreach (var t in event_anyComps )
                        {
                            var eventType = type.MakeGenericType(typeof(TScope),t);
                            Register( eventType );
                        }
                    }
                }
            }
        }

        public static void Scan_EventSelf()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var event_anyComps = new List<Type>();
                foreach (var type in assembly.GetTypes())
                {
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
                        || !IsInScope(type)
                    || !IsEventSelfChild(type) )
                    {
                        continue;
                    }
                    event_anyComps.Add( type );
                }

                foreach (var type in assembly.GetTypes())
                {
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent)) )
                    {
                        continue;
                    }

                    if ( type.IsGenericType && type.GetGenericTypeDefinition(  ) == typeof(Event_SelfComponent<,>) )
                    {
                        foreach (var t in event_anyComps )
                        {
                            var eventType = type.MakeGenericType(typeof(TScope),t);
                            Register( eventType );
                        }
                    }
                }
            }
        }

        public static void Scan_EventSelfRemoved()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var event_anyComps = new List<Type>();
                foreach (var type in assembly.GetTypes())
                {
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
                        || !IsInScope(type)
                    || !IsEventSelfRemovedChild(type) )
                    {
                        continue;
                    }
                    event_anyComps.Add( type );
                }

                foreach (var type in assembly.GetTypes())
                {
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent)) )
                    {
                        continue;
                    }

                    if ( type.IsGenericType && type.GetGenericTypeDefinition(  ) == typeof(Event_SelfRemovedComponent<,>) )
                    {
                        foreach (var t in event_anyComps )
                        {
                            var eventType = type.MakeGenericType(typeof(TScope),t);
                            Register( eventType );
                        }
                    }
                }
            }
        }

        private static void Register(Type dataType)
        {
            var componentType = typeof(Lookup<,>);

            var genericType = componentType.MakeGenericType(typeof(TScope), dataType);

            if (_registeredTypes.Contains(genericType))
            {
                return;
            }

            _registeredTypes.Add(genericType);

            var fieldInfo = genericType.GetField("Id" ,
                BindingFlags.Static
                | BindingFlags.SetField
                | BindingFlags.Public
                );

            if (fieldInfo == null)
                throw new Exception(string.Format("Type `{0}' does not contains `Id' field", genericType.Name));

            fieldInfo.SetValue(null, _registeredCount);
            _registeredCount++;
        }

        private static bool IsInScope(Type type)
        {
            return type.GetInterfaces(  )
                .Any( x =>
                    x.IsGenericType
                    && x.GetGenericTypeDefinition(  ) == typeof(Scope<>)
                    && x.GetGenericArguments()[0] == typeof(TScope)
                    );
        }

        private static bool IsEventAnyChild( Type type )
        {
            return type.GetInterfaces(  )
                    .Any( x =>
                        x.IsGenericType
                        && x.GetGenericTypeDefinition(  ) == typeof(IEvent_Any<,>)
                        && x.GetGenericArguments()[0] == typeof(TScope)
                        && x.GetGenericArguments()[1] == type
                        );
        }

        private static bool IsEventAnyRemovedChild( Type type )
        {
            return type.GetInterfaces(  )
                    .Any( x =>
                        x.IsGenericType
                        && x.GetGenericTypeDefinition(  ) == typeof(IEvent_AnyRemoved<,>)
                        && x.GetGenericArguments()[0] == typeof(TScope)
                        && x.GetGenericArguments()[1] == type
                        );
        }

        private static bool IsEventSelfChild( Type type )
        {
            return type.GetInterfaces(  )
                    .Any( x =>
                        x.IsGenericType
                        && x.GetGenericTypeDefinition(  ) == typeof(IEvent_Self<,>)
                        && x.GetGenericArguments()[0] == typeof(TScope)
                        && x.GetGenericArguments()[1] == type
                        );
        }

        private static bool IsEventSelfRemovedChild( Type type )
        {
            return type.GetInterfaces(  )
                    .Any( x =>
                        x.IsGenericType
                        && x.GetGenericTypeDefinition(  ) == typeof(IEvent_SelfRemoved<,>)
                        && x.GetGenericArguments()[0] == typeof(TScope)
                        && x.GetGenericArguments()[1] == type
                        );
        }

        public static void Register<TComp>()
        {
            var type = typeof(TComp);

            if (_registeredTypes.Contains(type))
                return;

            _registeredTypes.Add(type);
            Lookup<TScope, TComp>.Id = _registeredCount;
            _registeredCount++;
        }

        public static int TotalComponents
        {
            get { return _registeredTypes.Count; }
        }

        public static string[] ComponentNamesCache
        {
            get
            {
                if (_componentNamesCache == null)
                {
                    _componentNamesCache = new string[_registeredTypes.Count];

                    for (var i = 0; i < _componentNamesCache.Length; i++)
                    {
                        _componentNamesCache[i] = _registeredTypes[i].Name;
                    }
                }
                return _componentNamesCache;
            }
        }

        public static Type[] ComponentTypesCache
        {
            get
            {
                if (_typesCache == null)
                {
                    _typesCache = _registeredTypes.ToArray();
                }
                return _typesCache;
            }
        }
    }
}