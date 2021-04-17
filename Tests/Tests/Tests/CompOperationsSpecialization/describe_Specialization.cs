#if ENT_GA_SPECIALIZATION
using System;
using Entitas.Generic;
using NSpec;

namespace Tests
{
public class describe_Specialization : nspec
{
	private					Contexts				_db;

	private					void					test_SpecReplace_		(  )
	{
		Lookup_ScopeManager.RegisterAll(  );

		before						= ()=>
		{
			_db						= new Contexts(  );
			_db.AddScopedContexts(  );
		};

		it["SpecReplace TestDataA"] = () =>
		{
			// given
			var ent = _db.Get<ScopeA>(  ).CreateEntity();

			// when
			var valueSpec = 5f;
			var valueData = 100f;
			SpecReplace_<ScopeA,TestDataA>.Spec = new SpecReplaceA_(valueSpec);
			ent.Replace_( new TestDataA(valueData) );

			// then
			ent.Get_<TestDataA>(  ).Value.should_be( valueSpec );

			// when
			SpecReplace_<ScopeA,TestDataA>.Spec = null; // FIXME: implement some reset
			ent.Replace_( new TestDataA(valueData) );

			// then
			ent.Get_<TestDataA>(  ).Value.should_be( valueData );
		};

		it["SpecReplace TestDataB"] = () =>
		{
			// given
			var ent = _db.Get<ScopeA>(  ).CreateEntity();

			// when
			var valueSpec = "spec";
			var valueData = "data";
			SpecReplace_<ScopeA,TestDataB>.Spec = new SpecReplaceB_(valueSpec);
			ent.Replace_( new TestDataB(valueData) );

			// then
			ent.Get_<TestDataB>(  ).Value.should_be( valueSpec );

			// when
			ent.Replace_( new TestDataA(100f) );

			// then
			ent.Get_<TestDataA>(  ).Value.should_be( 100f );

			SpecReplace_<ScopeA,TestDataB>.Spec = null; // FIXME: implement some reset
		};
	}
}

public sealed class SpecReplaceA_: IReplace_<ScopeA,TestDataA>
{
	public					SpecReplaceA_			(Single value)
	{
		Value						= value;
	}

	public					Single					Value;

	public					void					Replace_				(Entity<ScopeA> ent, TestDataA data )
	{
		var index					= Lookup<ScopeA,TestDataA>.Id;

		var component				= ent.CreateComponent<StructComponent<TestDataA>>( index );
		component.Data				= new TestDataA(Value);
		ent.ReplaceComponent(index, component);
	}
}

public sealed class SpecReplaceB_: IReplace_<ScopeA,TestDataB>
{
	public					SpecReplaceB_			(String value)
	{
		Value						= value;
	}

	public					String					Value;

	public					void					Replace_				(Entity<ScopeA> ent, TestDataB data )
	{
		var index					= Lookup<ScopeA,TestDataB>.Id;

		var component				= ent.CreateComponent<StructComponent<TestDataB>>( index );
		component.Data				= new TestDataB(Value);
		ent.ReplaceComponent(index, component);
	}
}
}
#endif