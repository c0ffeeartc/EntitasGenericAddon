using System;
using Entitas;
using Entitas.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_Entity_StructMethods
	{
		private				Contexts				_contexts;

			[SetUp]
			public				void					BeforeEach()
			{
				_contexts			= new Contexts(  );
				_contexts.AddScopedContexts(  );
			}

			[Test]
			public				void					test_AddComponent(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Has_<TestDataA>(  ).Should(  ).BeFalse(  );

				// when
				entity.Add_( new TestDataA( 21f ) );

				// then
				entity.Has_<TestDataA>(  ).Should(  ).BeTrue(  );
			}

			[Test]
			public				void					test_AddTwiceThrows(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Has_<TestDataA>(  ).Should(  ).BeFalse(  );

				// when
				entity.Add_( new TestDataA( 21f ) );

				// then
				Action act = (  )=>
					{
						entity.Add_( new TestDataA( 32f ) );
					};
				act.Should().Throw<EntityAlreadyHasComponentException>(  );
			}

			[Test]
			public				void					test_RemoveComponent(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add_( new TestDataA(  ) );
				entity.Has_<TestDataA>(  ).Should(  ).BeTrue(  );

				// when
				entity.Remove_<TestDataA>(  );

				// then
				entity.Has_<TestDataA>(  ).Should(  ).BeFalse(  );
			}

			[Test]
			public				void					test_RemoveNonExistentThrows(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );

				// when
				Action act = (  )=>
					{
						entity.Remove_<TestDataA>(  );
					};

				// then
				act.Should().Throw<EntityDoesNotHaveComponentException>(  );
			}

			[Test]
			public				void					test_ReplaceComponent(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add_( new TestDataA( 1f ) );
				entity.Get_<TestDataA>(  ).Value.Should(  ).Be( 1f );

				// when
				entity.Replace_( new TestDataA( 2f ) );

				// then
				entity.Get_<TestDataA>(  ).Value.Should(  ).Be( 2f );
		}
	}
}