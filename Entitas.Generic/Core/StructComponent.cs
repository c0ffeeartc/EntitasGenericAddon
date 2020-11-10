namespace Entitas.Generic
{
public class StructComponent<TData> : IComponent where TData : struct
{
	public TData Data;

	public override string ToString() 
  {
    return Data.ToString();
  }
}
}