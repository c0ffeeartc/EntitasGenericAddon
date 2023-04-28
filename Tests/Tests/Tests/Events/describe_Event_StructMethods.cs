using Entitas.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_Event_StructMethods
	{
		private				Contexts				_contexts;

			[SetUp]
			public				void					BeforeEach()
			{
				_contexts			= new Contexts(  );
				_contexts.AddScopedContexts(  );
			}

			[Test]
			public				void					test_OnAnyListensOther	(  )
			{
				// given
				var system			= new EventSystem_Any_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnAny<ScopeA,TestDataA>>(  );
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnAny( listener );
				}
				listener.DidNotReceiveWithAnyArgs(  ).OnAny( default, null, null );

				// when
				var entity			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add_( new TestDataA(  ) );

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnAny( default, null, null );
			}

			[Test]
			public				void					test_OnAnyRemovedListensOther(  )
			{
				// given
				var system			= new EventSystem_AnyRemoved_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnAnyRemoved<ScopeA,TestDataA>>(  );
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnAnyRemoved( listener );
				}
				listener.DidNotReceiveWithAnyArgs(  ).OnAnyRemoved( default, null, null );

				// when
				var entity			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add_( new TestDataA(  ) );
				entity.Remove_<TestDataA>(  );

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnAnyRemoved( default, null, null );
			}

			[Test]
			public				void					test_OnSelfListensSelf(  )
			{
				// given
				var system			= new EventSystem_Self_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnSelf<ScopeA,TestDataA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( default, null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_( new TestDataA(  ) );
					ent.Add_OnSelf( listener );
				}

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnSelf( default, null, null );
			}

			[Test]
			public				void					test_OnSelfNotListensOther(  )
			{
				// given
				var system			= new EventSystem_Self_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnSelf<ScopeA,TestDataA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( default, null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnSelf( listener );
				}
				{
					var entity		= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					entity.Add_( new TestDataA(  ) );
				}

				system.Execute(  );

				// then
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( default, null, null );
			}

			[Test]
			public				void					test_OnSelfRemovedListensSelf(  )
			{
				// given
				var system			= new EventSystem_SelfRemoved_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnSelfRemoved<ScopeA,TestDataA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( default, null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_( new TestDataA(  ) );
					ent.Add_OnSelfRemoved( listener );
					ent.Remove_<TestDataA>(  );
				}

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnSelfRemoved( default, null, null );
			}

			[Test]
			public				void					test_OnSelfRemovedNotListensOther(  )
			{
				// given
				var system			= new EventSystem_SelfRemoved_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnSelfRemoved<ScopeA,TestDataA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( default, null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnSelfRemoved( listener );
				}
				{
					var entity		= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					entity.Add_( new TestDataA(  ) );
				}

				system.Execute(  );

				// then
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( default, null, null );
			}
	}
}