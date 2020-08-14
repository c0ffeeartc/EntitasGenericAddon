using Entitas.Generic;

public sealed class Entity_Flag_CompFlag : IPerformanceTest
{
	private const			int						n						= 10000000;
	private					Entity<TestScope1>		_e;
	public					int						Iterations				=> n;

	public					void					Before					(  )
	{
		var contexts				= new Contexts(  );
		contexts.AddScopedContexts(  );

		var context					= contexts.Get<TestScope1>(  );
		_e							= context.CreateEntity();
	}

	public					void					Run						(  )
	{
		for ( int i = 0; i < n; i++ )
		{
			_e.Flag<TestCompAFlag_Scope1>( i % 2 == 0 );
		}
	}
}