using Entitas;
using Entitas.Generic;

public class Entity_AddRemove_CompData_Class_WithGroups : IPerformanceTest
{
	private const			int						n						= 1000000;
	private					Entity<TestScope1>		_ent;
	private					TestCompA_Scope1		_testCompAScope1;
	public					int						Iterations				=> n;

	public					void					Before					(  )
	{
		var contexts				= new Contexts(  );
		contexts.AddScopedContexts(  );

		var _context				= contexts.Get<TestScope1>(  );
		var compAId					= Lookup<TestScope1,TestCompA_Scope1>.Id;
		var compBId					= Lookup<TestScope1,TestCompB_Scope1>.Id;
		var compAFlagId				= Lookup<TestScope1,TestCompAFlag_Scope1>.Id;

		_context.GetGroup(Matcher<Entity<TestScope1>>.AllOf( compAId ));
		_context.GetGroup(Matcher<Entity<TestScope1>>.AllOf( compBId ));
		_context.GetGroup(Matcher<Entity<TestScope1>>.AllOf( compAFlagId ));
		_context.GetGroup(Matcher<Entity<TestScope1>>.AllOf( compAId ));

		_context.GetGroup(Matcher<Entity<TestScope1>>
			.AllOf(
				compAId,
				compBId
			) );

		_context.GetGroup(Matcher<Entity<TestScope1>>
			.AllOf(
				compAId,
				compAFlagId
			) );

		_context.GetGroup(Matcher<Entity<TestScope1>>
			.AllOf(
				compBId,
				compAFlagId
			) );

		_context.GetGroup(Matcher<Entity<TestScope1>>
			.AllOf(
				compAId,
				compBId,
				compAFlagId
			) );

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