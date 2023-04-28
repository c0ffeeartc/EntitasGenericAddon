using System;
using Entitas;
using Entitas.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_Entity_ClassMethods
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
				entity.Has<TestCompA>(  ).Should().BeFalse(  );

				// when
				entity.Add( new TestCompA(  ) );

				// then
				entity.Has<TestCompA>(  ).Should().BeTrue(  );
			}

			[Test]
			public				void					test_AddTwiceThrows(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Has<TestCompA>(  ).Should(  ).BeFalse(  );

				// when
				entity.Add( new TestCompA(  ) );

				// then
				Action act = (  )=>
					{
						entity.Add( new TestCompA(  ));
					};
				act.Should().Throw<EntityAlreadyHasComponentException>(  );
			}

			[Test]
			public				void					test_RemoveComponent(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add( new TestCompA(  ) );
				entity.Has<TestCompA>(  ).Should(  ).BeTrue(  );

				// when
				entity.Remove<TestCompA>();

				// then
				entity.Has<TestCompA>(  ).Should(  ).BeFalse(  );
			}

			[Test]
			public				void					test_RemoveNonExistentThrows(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );

				// when
				Action act = (  )=>
					{
						entity.Remove<TestCompA>(  );
					};

				// then
				act.Should().Throw<EntityDoesNotHaveComponentException>(  );
			}

			[Test]
			public				void					test_ReplaceComponent(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add( Cache<TestCompA>.I.Set( 1 ) );
				entity.Get<TestCompA>(  ).Value.Should().Be(1);

				// when
				entity.Replace( Cache<TestCompA>.I.Set( 2 ) );
				Cache<TestCompA>.I.Set( 3 );

				// then
				entity.Get<TestCompA>(  ).Value.Should(  ).Be( 2 );
			}

			[Test]
			public				void					test_ICreateApply_CreateComponent(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );

				// when
				var comp				= entity.Create<TestComp_CreateApply_A>(  );

				// then
				entity.Has<TestComp_CreateApply_A>(  ).Should(  ).BeFalse(  );
			}

			[Test]
			public				void					test_ICreateApply_ApplyComponent(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );

				// when
				var comp				= entity.Create<TestComp_CreateApply_A>(  );
				comp.Value.Should(  ).Be( 0f );
				comp.Value				= 1f;
				entity.Has<TestComp_CreateApply_A>(  ).Should(  ).BeFalse(  );
				entity.Apply( comp );

				// then
				entity.Has<TestComp_CreateApply_A>(  ).Should(  ).BeTrue(  );
				entity.Get<TestComp_CreateApply_A>(  ).Should(  ).BeSameAs( comp );
			}

			[Test]
			public				void					test_Flag_Is(  )
			{
				// given
				var entity				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Is<TestFlagA>(  ).Should().BeFalse();

				// when
				entity.Flag<TestFlagA>( true );

				// then
				entity.Is<TestFlagA>(  ).Should().BeTrue();

				// when
				entity.Flag<TestFlagA>( false );

				// then
				entity.Is<TestFlagA>(  ).Should().BeFalse();
			}
	}
}