namespace Entitas.Generic
{
	public class Matcher<TScope, TComponent> where TScope: IScope where TComponent : IComponent, TScope
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
				var matcher				= (Matcher<Entity<TScope>>) Matcher<Entity<TScope>>.AllOf(Lookup<TScope, TComponent>.Id);
				matcher.componentNames	= Lookup_ComponentManager<TScope>.ComponentNamesCache;
				_instance					= matcher;
				return _instance;
			}
		}
	}
}