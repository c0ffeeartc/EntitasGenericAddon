using System;
using Entitas.Generic;
using FluentAssertions;
using NSpec;

namespace Tests
{
	public class describe_ScopedContext_StructMethods : nspec
	{
		private				Contexts				_contexts;

		private				void					test_StructMethods		(  )
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
				context.Has_<TestDataAUnique>(  ).should_be_false(  );

				// when
				var entity			= context.Set_( new TestDataAUnique(  ) );

				// then
				context.Has_<TestDataAUnique>(  ).should_be_true(  );
			};

			it["Set twice throws"] = ()=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has_<TestDataAUnique>(  ).should_be_false(  );

				// when
				var entity			= context.Set_( new TestDataAUnique(  ) );

				// then
				context.Has_<TestDataAUnique>(  ).should_be_true(  );

				// then
				Action act = (  )=>
					{
						context.Set_( new TestDataAUnique(  ));
					};
				act.ShouldThrow<Exception>(  );
			};

			it["Remove component"]	= ()=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has_<TestDataAUnique>(  ).should_be_false(  );

				var entity			= context.Set_( new TestDataAUnique(  ) );
				context.Has_<TestDataAUnique>(  ).should_be_true(  );

				// when
				context.Remove_<TestDataAUnique>(  );

				// then
				context.Has_<TestDataAUnique>(  ).should_be_false(  );
			};

			it["Remove inexistent throws"]	= ()=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has_<TestDataAUnique>(  ).should_be_false(  );

				// then
				Action act = (  )=>
					{
						context.Remove_<TestDataAUnique>(  );
					};
				act.ShouldThrow<Exception>(  );
			};

			it["Replace component"] = (  )=>
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has_<TestDataAUnique>(  ).should_be_false(  );

				context.Set_( new TestDataAUnique( 1f ) );
				context.Get_<TestDataAUnique>(  ).Value.should_be( 1f );

				// when
				context.Replace_( new TestDataAUnique( 2f ) );

				// then
				context.Get_<TestDataAUnique>(  ).Value.should_be( 2f );
			};
		}
	}
}