using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Entitas;

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
                    if ( !((IList) type.GetInterfaces()).Contains(typeof(IComponent))
                         || !IsInScope(type))
                        continue;

                    Register(type);
                }
            }
        }

        private static void Register(Type dataType)
        {
            var componentType = typeof(Lookup<,>);
            var genericType = componentType.MakeGenericType(typeof(TScope), dataType);

            if (_registeredTypes.Contains(genericType))
                return;

            _registeredTypes.Add(genericType);

            var fieldInfo = genericType.GetField("Id", BindingFlags.Static | BindingFlags.SetField | BindingFlags.Public);

            if (fieldInfo == null)
                throw new Exception(string.Format("Type `{0}' does not contains `Id' field", genericType.Name));

            fieldInfo.SetValue(null, _registeredCount);
            _registeredCount++;
        }

        private static bool IsInScope(Type type)
        {
            return ((IList)type.GetInterfaces()).Contains(typeof(TScope));
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