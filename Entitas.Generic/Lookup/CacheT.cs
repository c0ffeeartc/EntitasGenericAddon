namespace Entitas.Generic
{
	public class Cache<T> where T : new(  )
	{
		public static		T						I = new T();
	}
}