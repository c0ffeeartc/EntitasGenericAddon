using System;

namespace Entitas.Generic
{
public class StructComponent<TData>
		: IComponent
		, IVersionChanged
			where TData : struct
{
	public					TData					Data;
	public					Int32					VersionChanged			{ get; set; }
	public					VersionChangeId			VersionChangedId		{ get; set; }

	// Slow. Don't use in release builds
	public override			string					ToString				(  )
	{
		var tData					= typeof(TData);
		if ( tData.IsGenericType )
		{
			var s					= Data.ToString(  );
			if ( s == tData.FullName )
			{
				return tData.ToGenericTypeString(  );
			}
		}

		return Data.ToString(  );
	}
}
}