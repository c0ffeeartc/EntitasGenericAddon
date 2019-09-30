namespace Entitas.Generic
{
	public class Matcher<TScope, TComponent> where TScope: IScope where TComponent : class, IComponent, Scope<TScope>
	{
		private static		IMatcher<Entity<TScope>>_instance;
		public static		IMatcher<Entity<TScope>>I  // Instance
		{
			get
			{
				if ( _instance != null )
				{
					return _instance;
				}
				var index				= Lookup<TScope, TComponent>.Id;
				var matcher				= (Matcher<Entity<TScope>>) Matcher<Entity<TScope>>.AllOf( index );
				matcher.componentNames	= Lookup_ComponentManager<TScope>.ComponentNamesCache;
				_instance					= matcher;
				return _instance;
			}
		}
	}

	public class Matcher_<TScope, TData>
			where TScope: IScope
			where TData : struct, IComponent, Scope<TScope>
	{
		private static		IMatcher<Entity<TScope>>_instance;
		public static		IMatcher<Entity<TScope>>I  // Instance
		{
			get
			{
				if ( _instance != null )
				{
					return _instance;
				}
				var index				= Lookup<TScope, StructComponent<TData>>.Id;
				var matcher				= (Matcher<Entity<TScope>>) Matcher<Entity<TScope>>.AllOf( index );
				matcher.componentNames	= Lookup_ComponentManager<TScope>.ComponentNamesCache;
				_instance					= matcher;
				return _instance;
			}
		}
	}
}