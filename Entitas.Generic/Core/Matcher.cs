namespace Entitas.Generic
{
	public class Matcher<TScope, TComponent>
			where TScope: IScope
			where TComponent : IComponent, Scope<TScope>
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
				matcher.componentNames	= Lookup<TScope>.CompNamesPrettyArray;
				_instance				= matcher;
				return _instance;
			}
		}
	}
}