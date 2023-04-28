using System;
using Entitas.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_ScopedContext_StructMethods
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
				context.Has_<TestDataAUnique>(  ).Should(  ).BeFalse(  );

				// when
				var entity			= context.Set_( new TestDataAUnique(  ) );

				// then
				context.Has_<TestDataAUnique>(  ).Should(  ).BeTrue(  );
			}

			[Test]
			public				void					test_SetTwiceThrows(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has_<TestDataAUnique>(  ).Should(  ).BeFalse(  );

				// when
				var entity			= context.Set_( new TestDataAUnique(  ) );

				// then
				context.Has_<TestDataAUnique>(  ).Should(  ).BeTrue(  );

				// then
				Action act = (  )=>
					{
						context.Set_( new TestDataAUnique(  ));
					};
				act.Should(  ).Throw<Exception>(  );
			}

			[Test]
			public				void					test_RemoveComponent(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has_<TestDataAUnique>(  ).Should(  ).BeFalse(  );

				var entity			= context.Set_( new TestDataAUnique(  ) );
				context.Has_<TestDataAUnique>(  ).Should(  ).BeTrue(  );

				// when
				context.Remove_<TestDataAUnique>(  );

				// then
				context.Has_<TestDataAUnique>(  ).Should(  ).BeFalse(  );
			}

			[Test]
			public				void					test_RemoveNonexistentThrows(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has_<TestDataAUnique>(  ).Should(  ).BeFalse(  );

				// then
				Action act = (  )=>
					{
						context.Remove_<TestDataAUnique>(  );
					};
				act.Should(  ).Throw<Exception>(  );
			}

			[Test]
			public				void					test_ReplaceComponent(  )
			{
				// given
				var context			= _contexts.Get<ScopeA>(  );
				context.Has_<TestDataAUnique>(  ).Should(  ).BeFalse(  );

				context.Set_( new TestDataAUnique( 1f ) );
				context.Get_<TestDataAUnique>(  ).Value.Should(  ).Be( 1f );

				// when
				context.Replace_( new TestDataAUnique( 2f ) );

				// then
				context.Get_<TestDataAUnique>(  ).Value.Should(  ).Be( 2f );
			}
	}
}