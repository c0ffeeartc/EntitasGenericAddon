using Entitas.Generic;

public sealed class Entity_Is_CompFlag : IPerformanceTest
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
		var middleI = n / 2;
		for ( var i = 0; i < middleI; i++ )
		{
			_e.Is<TestCompAFlag_Scope1>(  );
		}

		_e.Flag<TestCompAFlag_Scope1>( true );
		for ( var i = middleI; i < n; i++ )
		{
			_e.Is<TestCompAFlag_Scope1>(  );
		}
	}
}