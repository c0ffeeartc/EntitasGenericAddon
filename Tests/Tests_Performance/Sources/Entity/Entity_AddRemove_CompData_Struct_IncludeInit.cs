using Entitas.Generic;

#pragma warning disable
public class Entity_AddRemove_CompData_Struct_IncludeInit : IPerformanceTest
{
	private const			int						n						= 10000000;
	private					Entity<TestScope1>		_ent;
	public					int						Iterations				=> n;

	public					void					Before					(  )
	{
		var contexts				= new Contexts(  );
		contexts.AddScopedContexts(  );

		var context					= contexts.Get<TestScope1>();
		_ent						= context.CreateEntity();
	}

	public					void					Run						(  )
	{
		for ( var i = 0; i < n; i++ )
		{
			_ent.Add_( new TestCompAStruct_Scope1() {data = 5});
			_ent.Remove_<TestCompAStruct_Scope1>(  );
		}
	}
}