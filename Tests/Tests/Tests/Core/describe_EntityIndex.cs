using System;
using Entitas.Generic;
using NSpec;

namespace Tests
{
public class describe_EntityIndex : nspec
{
	private					Contexts				_contexts;

	private					void					test_EntityIndex_GetAllEntsBy(  )
	{
		Lookup_ScopeManager.RegisterAll();

		before						= ()=>
		{
			_contexts				= new Contexts(  );
			_contexts.AddScopedContexts(  );
		};

		it["gets 0 entity by non-matching index"] = ()=>
		{
			// given
			var indexKey			= "indexKey";
			var contextA			= _contexts.Get<ScopeA>(  );
			contextA.AddEntityIndex(
				indexKey
				, contextA.GetGroup( Matcher<ScopeA,TestCompA>.I )
				, ( e,  c ) => ((TestCompA )c).Value );

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 1 ) );
			}

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 2 ) );
			}

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 2 ) );
			}

			// when
			var ents = contextA.GetAllEntsBy<ScopeA, TestCompA, Int32>(indexKey, 100);

			// then
			ents.Count.should_be( 0 );
		};

		it["gets correct amount of entities by matching index"] = ()=>
		{
			// given
			var indexKey			= "indexKey";
			var contextA			= _contexts.Get<ScopeA>(  );
			contextA.AddEntityIndex(
				indexKey
				, contextA.GetGroup( Matcher<ScopeA,TestCompA>.I )
				, ( e,  c ) => ((TestCompA )c).Value );

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 1 ) );
			}

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 2 ) );
			}

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 2 ) );
			}

			{
				// when
				var ents = contextA.GetAllEntsBy<ScopeA, TestCompA, Int32>(indexKey, 1);

				// then
				ents.Count.should_be( 1 );
			}

			{
				// when
				var ents = contextA.GetAllEntsBy<ScopeA, TestCompA, Int32>(indexKey, 2);

				// then
				ents.Count.should_be( 2 );
			}
		};
	}
}
}