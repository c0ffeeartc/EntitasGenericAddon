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
				entity.Add( Cache<TestCompA>.I.Set( 1f ) );
				entity.Get<TestCompA>(  ).Value.should_be( 1f );

				// when
				entity.Replace( Cache<TestCompA>.I.Set( 2f ) );
				Cache<TestCompA>.I.Set( 3f );

				// then
				entity.Get<TestCompA>(  ).Value.should_be( 2f );
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
      
      it["Init (Create or Replace) component"] = ()=>
      {
        // given
        var entity = _contexts.Get<ScopeA>().CreateEntity();

        // when
        var comp1 = entity.Init<TestComp_InitSet>();

        // then - entity has component at once after Init
        comp1.Value.should_be(0f);
        entity.Has<TestComp_InitSet>().should_be_true();

        // when Remove updated component and Init it again
        comp1.Value = 1f;
        entity.Remove<TestComp_InitSet>();
        var comp2 = entity.Init<TestComp_InitSet>();

        // then
        entity.Has<TestComp_InitSet>().should_be_true();
        // ... it is the same component as comp1, it was taken from components pool
        entity.Get<TestComp_InitSet>().should_be_same(comp1);
        entity.Get<TestComp_InitSet>().should_be_same(comp2);
        // ... and its value is not reset after Init !
        // users have to remember to call .Set method
        comp2.Value.should_be(1);
      };
      
      it["Init-Set (Create or Replace + Set) component data"] = ()=>
      {
        // given
        var entity = _contexts.Get<ScopeA>().CreateEntity();

        // when
        var comp1 = entity.Init<TestComp_InitSet>().Set(1);

        // then
        entity.Has<TestComp_InitSet>().should_be_true();
        comp1.Value.should_be(1);
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