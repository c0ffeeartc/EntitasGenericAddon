using Entitas.Generic;

public sealed class Entity_Has_CompData_Struct : IPerformanceTest
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
		_e.Add_( new TestCompAStruct_Scope1( 5 ) );
		_e.Add_( new TestCompBStruct_Scope1( 5 ) );
		_e.Flag<TestCompAFlag_Scope1>( true );
	}

	public					void					Run						(  )
	{
		for ( int i = 0; i < n; i++ )
		{
			_e.Has_<TestCompAStruct_Scope1>(  );
		}
	}
}