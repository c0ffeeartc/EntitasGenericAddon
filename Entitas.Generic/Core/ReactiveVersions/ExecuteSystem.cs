using System;

namespace Entitas.Generic
{
public abstract class ExecuteSystem : IExecuteSystem, IVersionLast
{
	public					Int32					VersionLast				{ get; set; }

	public					void					Execute					(  )
	{
		Version.Global++;
		Exec(  );
		VersionLast					= Version.Global;
	}

	public abstract			void					Exec					(  );
}
}