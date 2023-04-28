using Entitas;
using Entitas.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_EventsFeature2_OnAny
	{
		private				Contexts				_db;

		[SetUp]
		public				void					BeforeEach					(  )
		{
			_db					= new Contexts(  );
			_db.AddScopedContexts(  );
		}

		[TearDown]
		public				void					AfterEach					(  )
		{
			// Important
			OnAny<ScopeA,TestCompA>.I = null;
			OnAny<ScopeA,TestDataA>.I = null;
			OnAny_Removed<ScopeA,TestCompA>.I = null;
			OnAny_Removed<ScopeA,TestDataA>.I = null;
			OnAny_Flag<ScopeA,TestFlagA>.I = null;
		}

		[Test(Description = "OnAny.Invoke is called for each entity in context with class component match")]
		public				void					test_OnAny_InvokeIsCalledForEachEntInContextWithMatchingComp(  )
		{
			// given
			OnAny<ScopeA,TestCompA>.I = Substitute.For<IEventsFeature2_OnAny<ScopeA,TestCompA>>(  );
			var system			= new EventSystem_Any2<ScopeA, TestCompA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Add( new TestCompA(  ) );

			system.Execute(  );

			// then
			OnAny<ScopeA,TestCompA>.I
				.Received(1)
				.Invoke( ent, context );

			OnAny<ScopeA,TestCompA>.I = null;
		}

		[Test(Description = "OnAny.Invoke is not called for entities in context without class component")]
		public				void					test_OnAny_InvokeIsNotCalledForEntsInContextWithoutClassComp(  )
		{
			// given
			OnAny<ScopeA,TestCompA>.I = Substitute.For<IEventsFeature2_OnAny<ScopeA,TestCompA>>(  );
			var system			= new EventSystem_Any2<ScopeA, TestCompA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>();

			system.Execute(  );

			// then
			OnAny<ScopeA,TestCompA>.I
				.DidNotReceiveWithAnyArgs(  )
				.Invoke( null, null );
		}

		[Test(Description= "OnAny listens once Any ent with matching class component")]
		public				void					test_OnAny_ListensOnceAnyEntWithMatchingComp(  )
		{
			// given
			var system			= new EventSystem_Any2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestCompA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			OnAny<ScopeA, TestCompA>.I.Sub( listener.OnAny );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnAny( ent );
		}

		[Test(Description= "OnAny listens once Any ent with matching structComp")]
		public				void					test_OnAny_ListensOnceAnyEntWithMatchingStructComp(  )
		{
			// given
			var system			= new EventSystem_Any2<ScopeA, TestDataA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestDataA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add_( new TestDataA(  ) );
			OnAny<ScopeA, TestDataA>.I.Sub( listener.OnAny );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnAny( ent );
		}

		[Test(Description= "OnAny doesn't listen after OnAny.Unsub")]
		public				void					test_OnAny_NotListensAfter_OnAnyUnsub(  )
		{
			// given
			var system			= new EventSystem_Any2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestCompA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			OnAny<ScopeA, TestCompA>.I.Sub( listener.OnAny );

			// when
			OnAny<ScopeA, TestCompA>.I.Unsub( listener.OnAny );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnAny( null );
		}

		[Test(Description= "OnAny doesn't listen after Events2.I.UnsubAll")]
		public				void					test_OnAny_NotListensAfter_Events2UnsubAll(  )
		{
			// given
			var system			= new EventSystem_Any2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestCompA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			OnAny<ScopeA, TestCompA>.I.Sub( listener.OnAny );

			// when
			Events2.I.UnsubAll(  );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnAny( null );
		}

		[Test(Description= "OnAny listens other ent with matching class component")]
		public				void					test_OnAny_ListensOtherEntWithMatchingClassComp(  )
		{
			// given
			var system			= new EventSystem_Any2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestCompA>>(  );

			// when
			{
				var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
				OnAny<ScopeA, TestCompA>.I.Sub( listener.OnAny );
			}
			var entity			= _db.Get<ScopeA>(  ).CreateEntity(  );
			entity.Add( new TestCompA(  ) );

			system.Execute(  );

			// then
			listener
				.Received(  )
				.OnAny( entity );
		}

		[Test(Description= "OnAny_Removed.Invoke is called for each entity in context with removed class component")]
		public				void					test_OnAnyRemoved_InvokeIsCalledForEachEntityInContextWithRemovedClassComp(  )
		{
			// given
			OnAny_Removed<ScopeA,TestCompA>.I = Substitute.For<IEventsFeature2_OnAny_Removed<ScopeA,TestCompA>>(  );
			var system			= new EventSystem_Any_Removed2<ScopeA, TestCompA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>();

			system.Execute(  );

			// then
			OnAny_Removed<ScopeA,TestCompA>.I
				.Received(1)
				.Invoke( ent, context );
		}

		[Test(Description= "OnAny_Removed.Invoke is not called for entities in context with class component")]
		public				void					test_OnAnyRemoved_InvokeIsNotCalledForEntsInContextWithClassComp(  )
		{
			// given
			OnAny_Removed<ScopeA,TestCompA>.I = Substitute.For<IEventsFeature2_OnAny_Removed<ScopeA,TestCompA>>(  );
			var system			= new EventSystem_Any_Removed2<ScopeA, TestCompA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>();
			ent.Add( new TestCompA(  ) );

			system.Execute(  );

			// then
			OnAny_Removed<ScopeA,TestCompA>.I
				.DidNotReceiveWithAnyArgs(  )
				.Invoke( null, null );
		}

		[Test(Description= "OnAny_Removed listens once Any ent with matching removed class component")]
		public				void					test_OnAnyRemoved_ListensOnceAnyEntWithMatchingRemoveClassComp(  )
		{
			// given
			var system			= new EventSystem_Any_Removed2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestCompA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>(  );
			OnAny_Removed<ScopeA, TestCompA>.I.Sub( listener.OnAny );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnAny( ent );
		}


		[Test(Description= "OnAny_Removed listens once Any ent with matching structComp")]
		public				void					test_OnAnyRemoved_ListensOnceAnyEntWithMatchingStructComp(  )
		{
			// given
			var system			= new EventSystem_Any_Removed2<ScopeA, TestDataA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestDataA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add_( new TestDataA(  ) );
			ent.Remove_<TestDataA>();
			OnAny_Removed<ScopeA, TestDataA>.I.Sub( listener.OnAny );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnAny( ent );
		}

		[Test(Description= "OnAny_Removed doesn't listen after OnAny.Unsub")]
		public				void					test_OnAnyRemoved_NotListensAfterOnAnyUnsub(  )
		{
			// given
			var system			= new EventSystem_Any_Removed2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestCompA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>(  );
			OnAny_Removed<ScopeA, TestCompA>.I.Sub( listener.OnAny );

			// when
			OnAny_Removed<ScopeA, TestCompA>.I.Unsub( listener.OnAny );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnAny( null );
		}

		[Test(Description= "OnAny_Removed doesn't listen after Events2.I.UnsubAll")]
		public				void					test_OnAnyRemoved_NotListenAfterEvents2UnsubAll(  )
		{
			// given
			var system			= new EventSystem_Any_Removed2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestCompA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>(  );
			OnAny_Removed<ScopeA, TestCompA>.I.Sub( listener.OnAny );

			// when
			Events2.I.UnsubAll(  );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnAny( null );
		}

		[Test(Description= "OnAny_Removed listens other ent with matching class component")]
		public				void					test_OnAnyRemoved_ListensOtherEntWithMatchingClassComp(  )
		{
			// given
			var system			= new EventSystem_Any_Removed2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestCompA>>(  );

			// when
			{
				var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
				OnAny_Removed<ScopeA, TestCompA>.I.Sub( listener.OnAny );
			}

			var ent2			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent2.Add( new TestCompA(  ) );
			ent2.Remove<TestCompA>(  );

			system.Execute(  );

			// then
			listener
				.Received(  )
				.OnAny( ent2 );
		}

		[Test(Description= "OnAny_Flag.Invoke is called for each entity in context with Flag class component")]
		public				void					test_OnAnyFlag_InvokeIsCalledForEachEntInContextWithFlagComp(  )
		{
			// given
			OnAny_Flag<ScopeA,TestFlagA>.I = Substitute.For<IEventsFeature2_OnAny_Flag<ScopeA,TestFlagA>>(  );
			var system			= new EventSystem_Any_Flag2<ScopeA, TestFlagA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Flag<TestFlagA>( true );
			ent.Flag<TestFlagA>( false );

			system.Execute(  );

			// then
			OnAny_Flag<ScopeA,TestFlagA>.I
				.Received(1)
				.Invoke( ent, context );
		}

		[Test(Description= "OnAny_Flag.Invoke is not called if value is same as before")]
		public				void					test_OnAnyFlag_InvokeIsNotCalledIfValueIsSame(  )
		{
			// given
			OnAny_Flag<ScopeA,TestFlagA>.I = Substitute.For<IEventsFeature2_OnAny_Flag<ScopeA,TestFlagA>>(  );
			var system			= new EventSystem_Any_Flag2<ScopeA, TestFlagA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Flag<TestFlagA>( false );

			system.Execute(  );

			// then
			OnAny_Flag<ScopeA,TestFlagA>.I
				.DidNotReceiveWithAnyArgs(  )
				.Invoke( null, null );
		}

		[Test(Description= "OnAny_Flag listens once Any ent with matching Flag class component")]
		public				void					test_OnAnyFlag_ListensOnceAnyEntWithMatchingFlagComp(  )
		{
			// given
			var system			= new EventSystem_Any_Flag2<ScopeA, TestFlagA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestFlagA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Flag<TestFlagA>( true );
			ent.Flag<TestFlagA>( false );
			OnAny_Flag<ScopeA, TestFlagA>.I.Sub( listener.OnAny );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnAny( ent );
		}

		[Test(Description= "OnAny_Flag doesn't listen after OnAny.Unsub")]
		public				void					test_OnAnyFlag_NotListensAfterOnAnyUnsub(  )
		{
			// given
			var system			= new EventSystem_Any_Flag2<ScopeA, TestFlagA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestFlagA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Flag<TestFlagA>( true );
			OnAny_Flag<ScopeA, TestFlagA>.I.Sub( listener.OnAny );

			// when
			OnAny_Flag<ScopeA, TestFlagA>.I.Unsub( listener.OnAny );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnAny( null );
		}

		[Test(Description= "OnAny_Flag doesn't listen after Events2.I.UnsubAll")]
		public				void					test_OnAnyFlag_NotListensAfterEvents2UnsubAll(  )
		{
			// given
			var system			= new EventSystem_Any_Flag2<ScopeA, TestFlagA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestFlagA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Flag<TestFlagA>( true );
			OnAny_Flag<ScopeA, TestFlagA>.I.Sub( listener.OnAny );

			// when
			Events2.I.UnsubAll(  );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnAny( null );
		}

		[Test(Description= "OnAny_Flag listens other ent with matching class component")]
		public				void					test_OnAnyFlag_ListensOtherEntWithMatchingClassComp(  )
		{
			// given
			var system			= new EventSystem_Any_Flag2<ScopeA, TestFlagA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnAnySubscriber<ScopeA,TestFlagA>>(  );

			// when
			{
				var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
				OnAny_Flag<ScopeA, TestFlagA>.I.Sub( listener.OnAny );
			}

			var ent2			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent2.Flag<TestFlagA>( true );
			ent2.Flag<TestFlagA>( false );

			system.Execute(  );

			// then
			listener
				.Received(  )
				.OnAny( ent2 );
		}
	}

public interface IEventsFeature2_OnAnySubscriber<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, IEvent_Any<TScope, TComp>, Scope<TScope>
{
	void OnAny(Entity<TScope> ent);
}
}