using System;
using Entitas.Generic;
using FluentAssertions;
using NSpec;

namespace Tests
{
	public class describe_ScopedContext_ClassMethods : nspec
	{
		private				Contexts				_contexts;

		private				void					test_CoreMethods		(  )
		{
			Lookup_ScopeManager.RegisterAll();
			before					= ()=>
			{
				_contexts			= new Contexts(  );
				_contexts.AddScopedContexts(  );
			};

			it["Set component"]		= ()=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).should_be_false(  );

				// when
				var entity			= context.Set( new TestCompAUnique(  ) );

				// then
				context.Has<TestCompAUnique>(  ).should_be_true(  );
			};

			it["Set twice throws"] = ()=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).should_be_false(  );

				// when
				var entity			= context.Set( new TestCompAUnique(  ) );

				// then
				context.Has<TestCompAUnique>(  ).should_be_true(  );

				// then
				Action act = (  )=>
					{
						context.Set( new TestCompAUnique(  ));
					};
				act.ShouldThrow<Exception>(  );
			};

			it["Remove component"]	= ()=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).should_be_false(  );

				var entity			= context.Set( new TestCompAUnique(  ) );
				context.Has<TestCompAUnique>(  ).should_be_true(  );

				// when
				context.Remove<TestCompAUnique>(  );

				// then
				context.Has<TestCompAUnique>(  ).should_be_false(  );
			};

			it["Remove inexistent throws"]	= ()=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).should_be_false(  );

				// then
				Action act = (  )=>
					{
						context.Remove<TestCompAUnique>(  );
					};
				act.ShouldThrow<Exception>(  );
			};

			it["Replace component"] = (  )=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).should_be_false(  );

				context.Set( Cache<TestCompAUnique>.I.Set( 1f ) );
				context.Get<TestCompAUnique>(  ).Value.should_be( 1f );

				// when
				context.Replace( Cache<TestCompAUnique>.I.Set( 2f ) );
				Cache<TestCompAUnique>.I.Set( 3f );

				// then
				context.Get<TestCompAUnique>(  ).Value.should_be( 2f );
			};

			it["Flag, Is"] = ()=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Is<TestFlagAUnique>(  ).should_be_false(  );

				// when
				context.Flag<TestFlagAUnique>( true );

				// then
				context.Is<TestFlagAUnique>(  ).should_be_true(  );
				context.GetEntity<TestFlagAUnique>(  ).should_not_be_null(  );

				// when
				context.Flag<TestFlagAUnique>( false );

				// then
				context.Is<TestFlagAUnique>(  ).should_be_false(  );
				context.GetEntity<TestFlagAUnique>(  ).should_be_null(  );
			};
		}
	}
}