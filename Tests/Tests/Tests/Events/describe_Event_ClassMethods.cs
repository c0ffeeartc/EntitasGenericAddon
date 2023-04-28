using Entitas.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_Event_ClassMethods
	{
		private				Contexts				_contexts;

			[SetUp]
			public					void					BeforeEach()
			{
				_contexts			= new Contexts(  );
				_contexts.AddScopedContexts(  );
			}

			[Test]
			public				void					test_OnAnyListensOthers	(  )
			{
				// given
				var system			= new EventSystem_Any<ScopeA, TestCompA>( _contexts );
				var listener		= Substitute.For<IOnAny<ScopeA,TestCompA>>(  );
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnAny( listener );
				}
				listener.DidNotReceiveWithAnyArgs(  ).OnAny( null, null, null );

				// when
				var entity			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add( new TestCompA(  ) );

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnAny( null, null, null );
			}

			[Test]
			public				void					test_OnAnyRemovedListensOthers(  )
			{
				// given
				var system			= new EventSystem_AnyRemoved<ScopeA, TestCompA>( _contexts );
				var listener		= Substitute.For<IOnAnyRemoved<ScopeA,TestCompA>>(  );
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnAnyRemoved( listener );
				}
				listener.DidNotReceiveWithAnyArgs(  ).OnAnyRemoved( null, null, null );

				// when
				var entity			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add( new TestCompA(  ) );
				entity.Remove<TestCompA>(  );

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnAnyRemoved( null, null, null );
			}

			[Test]
			public				void					test_OnSelfListensSelf(  )
			{
				// given
				var system			= new EventSystem_Self<ScopeA, TestCompA>( _contexts );
				var listener		= Substitute.For<IOnSelf<ScopeA,TestCompA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( null, null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add( new TestCompA(  ) );
					ent.Add_OnSelf( listener );
				}

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnSelf( null, null, null );
			}

			[Test]
			public				void					test_OnSelfNotListensOther(  )
			{
				// given
				var system			= new EventSystem_Self<ScopeA, TestCompA>( _contexts );
				var listener		= Substitute.For<IOnSelf<ScopeA,TestCompA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( null, null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnSelf( listener );
				}
				{
					var entity		= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					entity.Add( new TestCompA(  ) );
				}

				system.Execute(  );

				// then
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( null, null, null );
			}

			[Test]
			public				void					test_OnSelfRemovedListensSelf(  )
			{
				// given
				var system			= new EventSystem_SelfRemoved<ScopeA, TestCompA>( _contexts );
				var listener		= Substitute.For<IOnSelfRemoved<ScopeA,TestCompA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( null, null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add( new TestCompA(  ) );
					ent.Add_OnSelfRemoved( listener );
					ent.Remove<TestCompA>(  );
				}

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnSelfRemoved( null, null, null );
			}

			[Test]
			public				void					test_OnSelfRemovedNotListensOther(  )
			{
				// given
				var system			= new EventSystem_SelfRemoved<ScopeA, TestCompA>( _contexts );
				var listener		= Substitute.For<IOnSelfRemoved<ScopeA,TestCompA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( null, null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnSelfRemoved( listener );
				}
				{
					var entity		= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					entity.Add( new TestCompA(  ) );
				}

				system.Execute(  );

				// then
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( null, null, null );
			}

			[Test]
			public				void					test_OnSelfFlagListensSelfAdded(  )
			{
				// given
				var ent				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				var listener		= Substitute.For<IOnSelf<ScopeA,TestFlagA>>(  );
				ent.Add_OnSelf( listener );

				var system			= new EventSystem_SelfFlag<ScopeA, TestFlagA>( _contexts );

				// when
				ent.Flag<TestFlagA>( false );
				system.Execute(  );
				// then
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( null, null, null );

				// when
				ent.Flag<TestFlagA>( true );
				system.Execute(  );
				// then
				listener.Received(  ).OnSelf( null, ent, _contexts );
			}

			[Test]
			public				void					test_OnSelfFlagListensSelfRemoved(  )
			{
				// given
				var ent				= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				var listener		= Substitute.For<IOnSelf<ScopeA,TestFlagA>>(  );
				ent.Add_OnSelf( listener );
				ent.Flag<TestFlagA>( true );

				var system			= new EventSystem_SelfFlag<ScopeA, TestFlagA>( _contexts );

				// when
				ent.Flag<TestFlagA>( true );
				system.Execute(  );
				// then
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( null, null, null );

				// when
				ent.Flag<TestFlagA>( false );
				system.Execute(  );
				// then
				listener.Received(  ).OnSelf( null, ent, _contexts );
			}
	}
}