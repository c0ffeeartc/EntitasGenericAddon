using Entitas.Generic;

#pragma warning disable
public class Entity_AddRemove_CompData_Class : IPerformanceTest
{
	private const			int						n						= 10000000;
	private					Entity<TestScope1>		_ent;
	private					TestCompA_Scope1		_testCompAScope1;
	public					int						Iterations				=> n;

	public					void					Before					(  )
	{
		var contexts				= new Contexts(  );
		contexts.AddScopedContexts(  );

		var context					= contexts.Get<TestScope1>();
		_ent						= context.CreateEntity();
		_testCompAScope1			= new TestCompA_Scope1( 5 );
	}

	public					void					Run						(  )
	{
		for ( var i = 0; i < n; i++ )
		{
			_ent.Add( _testCompAScope1 );
			_ent.Remove<TestCompA_Scope1>(  );
		}
	}
}
