using System;
using Entitas;
using Entitas.Generic;
using FluentAssertions;
using NSpec;

namespace Tests
{
	public class describe_Entity_StructMethods : nspec
	{
		private				Contexts				_contexts;

		private				void					test_Entity_StructMethods(  )
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
				entity.Has_<TestDataA>(  ).should_be_false(  );

				// when
				entity.Add_( new TestDataA( 21f ) );

				// then
				entity.Has_<TestDataA>(  ).should_be_true(  );
			};

			it["add twice throws"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Has_<TestDataA>(  ).should_be_false(  );

				// when
				entity.Add_( new TestDataA( 21f ) );

				// then
				Action act = (  )=>
					{
						entity.Add_( new TestDataA( 32f ) );
					};
				act.ShouldThrow<EntityAlreadyHasComponentException>(  );
			};

			it["remove component"] = (  )=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add_( new TestDataA(  ) );
				entity.Has_<TestDataA>(  ).should_be_true(  );

				// when
				entity.Remove_<TestDataA>(  );

				// then
				entity.Has_<TestDataA>(  ).should_be_false(  );
			};

			it["remove inexistent throws"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );

				// when
				Action act = (  )=>
					{
						entity.Remove_<TestDataA>(  );
					};

				// then
				act.ShouldThrow<EntityDoesNotHaveComponentException>(  );
			};

			it["replace component"] = ()=>
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add_( new TestDataA( 1f ) );
				entity.Get_<TestDataA>(  ).Value.should_be( 1f );

				// when
				entity.Replace_( new TestDataA( 2f ) );

				// then
				entity.Get_<TestDataA>(  ).Value.should_be( 2f );
			};
		}
	}
}