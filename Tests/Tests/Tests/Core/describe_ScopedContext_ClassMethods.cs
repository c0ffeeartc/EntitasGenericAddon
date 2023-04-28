using System;
using Entitas.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_ScopedContext_ClassMethods
	{
		private				Contexts				_contexts;

			[SetUp]
			public				void					BeforeEach()
			{
				_contexts			= new Contexts(  );
				_contexts.AddScopedContexts(  );
			}

			[Test]
			public				void					test_SetComponent(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).Should(  ).BeFalse(  );

				// when
				var entity			= context.Set( new TestCompAUnique(  ) );

				// then
				context.Has<TestCompAUnique>(  ).Should(  ).BeTrue(  );
			}

			[Test]
			public				void					test_SetTwiceThrows(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).Should(  ).BeFalse(  );

				// when
				var entity			= context.Set( new TestCompAUnique(  ) );

				// then
				context.Has<TestCompAUnique>(  ).Should(  ).BeTrue(  );

				// then
				Action act = (  )=>
					{
						context.Set( new TestCompAUnique(  ));
					};
				act.Should().Throw<Exception>(  );
			}

			[Test]
			public				void					test_RemoveComponent(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).Should(  ).BeFalse(  );

				var entity			= context.Set( new TestCompAUnique(  ) );
				context.Has<TestCompAUnique>(  ).Should().BeTrue(  );

				// when
				context.Remove<TestCompAUnique>(  );

				// then
				context.Has<TestCompAUnique>(  ).Should(  ).BeFalse(  );
			}

			[Test]
			public				void					test_RemoveNonexistentThrows(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).Should(  ).BeFalse(  );

				// then
				Action act = (  )=>
					{
						context.Remove<TestCompAUnique>(  );
					};
				act.Should(  ).Throw<Exception>(  );
			}

			[Test]
			public				void					test_ReplaceComponent(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has<TestCompAUnique>(  ).Should().BeFalse(  );

				context.Set( Cache<TestCompAUnique>.I.Set( 1f ) );
				context.Get<TestCompAUnique>(  ).Value.Should().Be( 1f );

				// when
				context.Replace( Cache<TestCompAUnique>.I.Set( 2f ) );
				Cache<TestCompAUnique>.I.Set( 3f );

				// then
				context.Get<TestCompAUnique>(  ).Value.Should(  ).Be( 2f );
			}

			[Test]
			public				void					test_Flag_Is(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Is<TestFlagAUnique>(  ).Should().BeFalse(  );

				// when
				context.Flag<TestFlagAUnique>( true );

				// then
				context.Is<TestFlagAUnique>(  ).Should(  ).BeTrue(  );
				context.GetEntity<TestFlagAUnique>(  ).Should(  ).NotBeNull(  );

				// when
				context.Flag<TestFlagAUnique>( false );

				// then
				context.Is<TestFlagAUnique>(  ).Should(  ).BeFalse(  );
				context.GetEntity<TestFlagAUnique>(  ).Should(  ).BeNull(  );
			}
	}
}
