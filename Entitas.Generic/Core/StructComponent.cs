namespace Entitas.Generic
{
public class StructComponent<TData> : IComponent where TData : struct
	{
		public TData Data;
	}
}