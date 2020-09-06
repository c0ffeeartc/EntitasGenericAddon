namespace Entitas.Generic
{
public class StructComponent<TData> : IComponent where TData : struct
{
	public TData Data;

	// Consider removing ToString here and deal with string formatting in concrete places
	public override string ToString()
	{
		return typeof(TData).ToGenericTypeString(  );
	}
}
}