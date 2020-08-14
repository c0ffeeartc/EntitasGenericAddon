using Entitas;
using Entitas.Generic;

public sealed class Entity_Replace_CompData_Class_WithGroups : IPerformanceTest
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

		_testCompAScope1			= new TestCompA_Scope1( 5 );
		_ent						= _context.CreateEntity();
		_ent.Add( _testCompAScope1 );
	}

	public					void					Run						(  )
	{
		for ( int i = 0; i < n; i++ )
		{
			_ent.Replace( _testCompAScope1 );
		}
	}
}