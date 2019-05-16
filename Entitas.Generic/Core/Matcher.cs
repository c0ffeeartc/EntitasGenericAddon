namespace Entitas.Generic
{
	public class Matcher<TScope, TComponent> where TScope: IScope where TComponent : IComponent
	{
		private static		IMatcher<Entity<TScope>>_cached;
		public static		IMatcher<Entity<TScope>>Instance
		{
			get
			{
				if ( _cached != null )
				{
					return _cached;
				}
				var matcher				= (Matcher<Entity<TScope>>) Matcher<Entity<TScope>>.AllOf(Lookup<TScope, TComponent>.Id);
				matcher.componentNames	= Lookup_ComponentManager<TScope>.ComponentNamesCache;
				_cached					= matcher;
				return _cached;
			}
		}
	}
}