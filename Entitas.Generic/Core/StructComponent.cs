namespace Entitas.Generic
{
public class StructComponent<TData> : IComponent where TData : struct
{
	public TData Data;

	// Slow. Don't use in release builds
	public override string ToString()
	{
		var tData = typeof(TData);
		if ( tData.IsGenericType )
		{
			var s = Data.ToString(  );
			if ( s == tData.FullName )
			{
				return tData.ToGenericTypeString(  );
			}
		}

		return Data.ToString(  );
	}
}
}