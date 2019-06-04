using System;
using Entitas;
using Entitas.Generic;
using FluentAssertions;
using NSpec;

namespace Tests
{
	public class describe_EntityCoreMethods : nspec
	{
		private				Contexts				_contexts;

		private				void					test_CoreMethods		(  )
		{
			before					= ()=>
			{
				Lookup_ScopeManager.RegisterAll();

				_contexts			= new Contexts(  );
				_contexts.AddScopedContexts(  );
			};

			it["add component"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Has<TestCompA>(  ).should_be_false(  );

				// when
				entity.Add( new TestCompA(  ) );

				// then
				entity.Has<TestCompA>(  ).should_be_true(  );
			};

			it["add twice throws"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Has<TestCompA>(  ).should_be_false(  );

				// when
				entity.Add( new TestCompA(  ) );

				// then
				Action act = (  )=>
					{
						entity.Add( new TestCompA(  ));
					};
				act.ShouldThrow<EntityAlreadyHasComponentException>(  );
			};

			it["remove component"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add( new TestCompA(  ) );
				entity.Has<TestCompA>(  ).should_be_true(  );

				// when
				entity.Remove<TestCompA>();

				// then
				entity.Has<TestCompA>(  ).should_be_false(  );
			};

			it["remove inexistent throws"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );

				// when
				Action act = (  )=>
					{
						entity.Remove<TestCompA>(  );
					};

				// then
				act.ShouldThrow<EntityDoesNotHaveComponentException>(  );
			};

			it["replace component"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add( Cache<TestCompA>.I.Set( 1f ) );
				entity.Get<TestCompA>(  ).Value.should_be( 1f );

				// when
				entity.Replace( Cache<TestCompA>.I.Set( 2f ) );
				Cache<TestCompA>.I.Set( 3f );

				// then
				entity.Get<TestCompA>(  ).Value.should_be( 2f );
			};

			it["flag, is"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Is<TestFlagA>(  ).should_be( false );

				// when
				entity.Flag<TestFlagA>( true );

				// then
				entity.Is<TestFlagA>(  ).should_be( true );

				// when
				entity.Flag<TestFlagA>( false );

				// then
				entity.Is<TestFlagA>(  ).should_be( false );
			};
		}
	}
}