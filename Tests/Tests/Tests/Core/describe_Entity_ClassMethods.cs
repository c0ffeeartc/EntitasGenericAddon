using System;
using Entitas;
using Entitas.Generic;
using FluentAssertions;
using NSpec;

namespace Tests
{
	public class describe_Entity_ClassMethods : nspec
	{
		private				Contexts				_contexts;

		private				void					test_Entity_ClassMethods(  )
		{
			Lookup_ScopeManager.RegisterAll();

			before					= ()=>
			{
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
				entity.Add( Cache<TestCompA>.I.Set( 1 ) );
				entity.Get<TestCompA>(  ).Value.should_be( 1 );

				// when
				entity.Replace( Cache<TestCompA>.I.Set( 2 ) );
				Cache<TestCompA>.I.Set( 3 );

				// then
				entity.Get<TestCompA>(  ).Value.should_be( 2 );
			};

			it["ICreateApply. Create component"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );

				// when
				var comp				= entity.Create<TestComp_CreateApply_A>(  );

				// then
				entity.Has<TestComp_CreateApply_A>(  ).should_be_false(  );
			};

			it["ICreateApply. Apply component"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );

				// when
				var comp				= entity.Create<TestComp_CreateApply_A>(  );
				comp.Value.should_be( 0f );
				comp.Value				= 1f;
				entity.Has<TestComp_CreateApply_A>(  ).should_be_false(  );
				entity.Apply( comp );

				// then
				entity.Has<TestComp_CreateApply_A>(  ).should_be_true(  );
				entity.Get<TestComp_CreateApply_A>(  ).should_be_same( comp );
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