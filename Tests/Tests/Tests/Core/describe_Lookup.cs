using Entitas.Generic;
using NSpec;

namespace Tests
{
public class describe_Lookup : nspec
{
	private					Contexts				_contexts;

	private					void					test_Lookup				(  )
	{
		Lookup_ScopeManager.RegisterAll();

		before						= ()=>
		{
			_contexts				= new Contexts(  );
			_contexts.AddScopedContexts(  );
		};

		it["Lookup.IsDefined"] = ()=>
		{
			Lookup<ScopeA>.IsDefined(typeof(TestCompA)).should_be_true(  );
			Lookup<ScopeA>.IsDefined(typeof(TestCompB)).should_be_false(  );
		};

		it["Lookup.CompTypeToI"] = ()=>
		{
			(Lookup<ScopeA>.CompTypeToI[typeof(TestCompA)] == Lookup<ScopeA,TestCompA>.Id).should_be_true(  );
			(Lookup<ScopeA>.CompTypeToI[typeof(TestDataA)] == Lookup<ScopeA,TestDataA>.Id).should_be_true(  );
			(Lookup<ScopeA>.CompTypeToI[typeof(TestCompA)] != Lookup<ScopeA,TestDataA>.Id).should_be_true(  );
			(Lookup<ScopeA>.CompTypeToI[typeof(TestDataA)] != Lookup<ScopeA,TestCompA>.Id).should_be_true(  );
		};
	}
}
}