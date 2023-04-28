using Entitas;
using Entitas.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_EventsFeature2_OnSelf
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
			OnSelf<ScopeA,TestCompA>.I = null;
			OnSelf<ScopeA,TestDataA>.I = null;
			OnSelf_Removed<ScopeA,TestCompA>.I = null;
			OnSelf_Removed<ScopeA,TestDataA>.I = null;
			OnSelf_Flag<ScopeA,TestFlagA>.I = null;
		}

		[Test(Description="OnSelf.Invoke is called for each entity in context with class component match")]
		public				void					test_OnSelf_InvokeIsCalledForEachEntInContextWithMatchingComp(  )
		{
			// given
			OnSelf<ScopeA,TestCompA>.I = Substitute.For<IEventsFeature2_OnSelf<ScopeA,TestCompA>>(  );
			var system			= new EventSystem_Self2<ScopeA, TestCompA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Add( new TestCompA(  ) );

			system.Execute(  );

			// then
			OnSelf<ScopeA,TestCompA>.I
				.Received(1)
				.Invoke( ent.creationIndex, ent, context );

			OnSelf<ScopeA,TestCompA>.I = null;
		}

		[Test(Description = "OnSelf.Invoke is not called for entities in context without class component")]
		public				void					test_OnSelf_InvokeIsNotCalledForEntsInContextWithoutMatchingComp(  )
		{
			// given
			OnSelf<ScopeA,TestCompA>.I = Substitute.For<IEventsFeature2_OnSelf<ScopeA,TestCompA>>(  );
			var system			= new EventSystem_Self2<ScopeA, TestCompA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>();

			system.Execute(  );

			// then
			OnSelf<ScopeA,TestCompA>.I
				.DidNotReceiveWithAnyArgs(  )
				.Invoke( 0, null, null );
		}

		[Test(Description = "OnSelf listens once self ent with matching class component")]
		public				void					test_OnSelf_ListensOnceSelfEntWithMatchingComp(  )
		{
			// given
			var system			= new EventSystem_Self2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestCompA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			OnSelf<ScopeA, TestCompA>.I.Sub( ent.creationIndex, listener.OnSelf );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnSelf( ent );
		}

		[Test(Description = "OnSelf listens once self ent with matching structComp")]
		public				void					test_OnSelf_ListensOnceSelfEntWithMatchingStructComp(  )
		{
			// given
			var system			= new EventSystem_Self2<ScopeA, TestDataA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestDataA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add_( new TestDataA(  ) );
			OnSelf<ScopeA, TestDataA>.I.Sub( ent.creationIndex, listener.OnSelf );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnSelf( ent );
		}

		[Test(Description = "OnSelf doesn't listen after OnSelf.Unsub")]
		public				void					test_OnSelf_NotListensAfterOnSelfUnsub(  )
		{
			// given
			var system			= new EventSystem_Self2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestCompA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			OnSelf<ScopeA, TestCompA>.I.Sub( ent.creationIndex, listener.OnSelf );

			// when
			OnSelf<ScopeA, TestCompA>.I.Unsub( ent.creationIndex, listener.OnSelf );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}

		[Test(Description = "OnSelf doesn't listen after Events2.I.UnsubAll")]
		public				void					test_OnSelf_NotListensAfterEvents2UnsubAll(  )
		{
			// given
			var system			= new EventSystem_Self2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestCompA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			OnSelf<ScopeA, TestCompA>.I.Sub( ent.creationIndex, listener.OnSelf );

			// when
			Events2.I.UnsubAll(  );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}

		[Test(Description = "OnSelf doesn't listen other ent with matching class component")]
		public				void					test_OnSelf_NotListensOtherEntWithMatchingComp(  )
		{
			// given
			var system			= new EventSystem_Self2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestCompA>>(  );

			// when
			{
				var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
				OnSelf<ScopeA, TestCompA>.I.Sub( ent.creationIndex, listener.OnSelf );
			}
			{
				var entity		= _db.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add( new TestCompA(  ) );
			}

			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}

		[Test(Description = "OnSelf_Removed.Invoke is called for each entity in context with removed class component")]
		public				void					test_OnSelfRemoved_InvokeIsCalledForEachEntInContextWithRemovedComp(  )
		{
			// given
			OnSelf_Removed<ScopeA,TestCompA>.I = Substitute.For<IEventsFeature2_OnSelf_Removed<ScopeA,TestCompA>>(  );
			var system			= new EventSystem_Self_Removed2<ScopeA, TestCompA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>();

			system.Execute(  );

			// then
			OnSelf_Removed<ScopeA,TestCompA>.I
				.Received(1)
				.Invoke( ent.creationIndex, ent, context );
		}

		[Test(Description = "OnSelf_Removed.Invoke is not called for entities in context with class component")]
		public				void					test_OnSelfRemoved_InvokeIsNotCalledForEntsInContextWithRemovedComp(  )
		{
			// given
			OnSelf_Removed<ScopeA,TestCompA>.I = Substitute.For<IEventsFeature2_OnSelf_Removed<ScopeA,TestCompA>>(  );
			var system			= new EventSystem_Self_Removed2<ScopeA, TestCompA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>();
			ent.Add( new TestCompA(  ) );

			system.Execute(  );

			// then
			OnSelf_Removed<ScopeA,TestCompA>.I
				.DidNotReceiveWithAnyArgs(  )
				.Invoke( 0, null, null );
		}

		[Test(Description = "OnSelf_Removed listens once self ent with matching removed class component")]
		public				void					test_OnSelfRemoved_ListensOnceSelfEntWithMatchingRemovedComp(  )
		{
			// given
			var system			= new EventSystem_Self_Removed2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestCompA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>(  );
			OnSelf_Removed<ScopeA, TestCompA>.I.Sub( ent.creationIndex, listener.OnSelf );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnSelf( ent );
		}

		[Test(Description = "OnSelf_Removed listens once self ent with matching structComp")]
		public				void					test_OnSelfRemoved_ListensOnceSelfEntWithMatchingStructComp(  )
		{
			// given
			var system			= new EventSystem_Self_Removed2<ScopeA, TestDataA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestDataA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add_( new TestDataA(  ) );
			ent.Remove_<TestDataA>();
			OnSelf_Removed<ScopeA, TestDataA>.I.Sub( ent.creationIndex, listener.OnSelf );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnSelf( ent );
		}

		[Test(Description = "OnSelf_Removed doesn't listen after OnSelf.Unsub")]
		public				void					test_OnSelfRemoved_NotListensAfterOnSelfUnsub(  )
		{
			// given
			var system			= new EventSystem_Self_Removed2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestCompA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>(  );
			OnSelf_Removed<ScopeA, TestCompA>.I.Sub( ent.creationIndex, listener.OnSelf );

			// when
			OnSelf_Removed<ScopeA, TestCompA>.I.Unsub( ent.creationIndex, listener.OnSelf );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}

		[Test(Description = "OnSelf_Removed doesn't listen after Events2.I.UnsubAll")]
		public				void					test_OnSelfRemoved_NotListensAfterEvents2UnsubAll(  )
		{
			// given
			var system			= new EventSystem_Self_Removed2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestCompA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Add( new TestCompA(  ) );
			ent.Remove<TestCompA>(  );
			OnSelf_Removed<ScopeA, TestCompA>.I.Sub( ent.creationIndex, listener.OnSelf );

			// when
			Events2.I.UnsubAll(  );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}

		[Test(Description = "OnSelf_Removed doesn't listen other ent with matching class component")]
		public				void					test_OnSelfRemoved_NotListensOtherEntWithMatchingComp(  )
		{
			// given
			var system			= new EventSystem_Self_Removed2<ScopeA, TestCompA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestCompA>>(  );

			// when
			{
				var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
				OnSelf_Removed<ScopeA, TestCompA>.I.Sub( ent.creationIndex, listener.OnSelf );
			}
			{
				var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
				ent.Add( new TestCompA(  ) );
				ent.Remove<TestCompA>(  );
			}

			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}

		[Test(Description = "OnSelf_Flag.Invoke is called for each entity in context with Flag class component")]
		public				void					test_OnSelfFlag_InvokeIsCalledForEachEntInContextWithFlagComp(  )
		{
			// given
			OnSelf_Flag<ScopeA,TestFlagA>.I = Substitute.For<IEventsFeature2_OnSelf_Flag<ScopeA,TestFlagA>>(  );
			var system			= new EventSystem_Self_Flag2<ScopeA, TestFlagA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Flag<TestFlagA>( true );
			ent.Flag<TestFlagA>( false );

			system.Execute(  );

			// then
			OnSelf_Flag<ScopeA,TestFlagA>.I
				.Received(1)
				.Invoke( ent.creationIndex, ent, context );
		}

		[Test(Description = "OnSelf_Flag.Invoke is not called if value is same as before")]
		public				void					test_OnSelfFlag_InvokeIsNotCalledIfValueIsSame(  )
		{
			// given
			OnSelf_Flag<ScopeA,TestFlagA>.I = Substitute.For<IEventsFeature2_OnSelf_Flag<ScopeA,TestFlagA>>(  );
			var system			= new EventSystem_Self_Flag2<ScopeA, TestFlagA>(_db);

			// when
			var context			= _db.Get<ScopeA>(  );
			var ent				= context.CreateEntity(  );
			ent.Flag<TestFlagA>( false );

			system.Execute(  );

			// then
			OnSelf_Flag<ScopeA,TestFlagA>.I
				.DidNotReceiveWithAnyArgs(  )
				.Invoke( 0, null, null );
		}

		[Test(Description = "OnSelf_Flag listens once self ent with matching Flag class component")]
		public				void					test_OnSelfFlag_ListensOnceSelfEntWithMatchingFlagComp(  )
		{
			// given
			var system			= new EventSystem_Self_Flag2<ScopeA, TestFlagA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestFlagA>>(  );

			// when
			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Flag<TestFlagA>( true );
			ent.Flag<TestFlagA>( false );
			OnSelf_Flag<ScopeA, TestFlagA>.I.Sub( ent.creationIndex, listener.OnSelf );

			system.Execute(  );
			system.Execute(  );

			// then
			listener
				.Received( 1 )
				.OnSelf( ent );
		}

		[Test(Description = "OnSelf_Flag doesn't listen after OnSelf.Unsub")]
		public				void					test_OnSelfFlag_NotListensAfterOnSelfUnsub(  )
		{
			// given
			var system			= new EventSystem_Self_Flag2<ScopeA, TestFlagA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestFlagA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Flag<TestFlagA>( true );
			OnSelf_Flag<ScopeA, TestFlagA>.I.Sub( ent.creationIndex, listener.OnSelf );

			// when
			OnSelf_Flag<ScopeA, TestFlagA>.I.Unsub( ent.creationIndex, listener.OnSelf );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}

		[Test(Description = "OnSelf_Flag doesn't listen after Events2.I.UnsubAll")]
		public				void					test_OnSelfFlag_NotListensAfterEvents2UnsubAll(  )
		{
			// given
			var system			= new EventSystem_Self_Flag2<ScopeA, TestFlagA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestFlagA>>(  );

			var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
			ent.Flag<TestFlagA>( true );
			OnSelf_Flag<ScopeA, TestFlagA>.I.Sub( ent.creationIndex, listener.OnSelf );

			// when
			Events2.I.UnsubAll(  );
			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}

		[Test(Description = "OnSelf_Flag doesn't listen other ent with matching class component")]
		public				void					test_OnSelfFlag_NotListensOtherEntWithMatchingComp(  )
		{
			// given
			var system			= new EventSystem_Self_Flag2<ScopeA, TestFlagA>(_db);
			var listener		= Substitute.For<IEventsFeature2_OnSelfSubscriber<ScopeA,TestFlagA>>(  );

			// when
			{
				var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
				OnSelf_Flag<ScopeA, TestFlagA>.I.Sub( ent.creationIndex, listener.OnSelf );
			}
			{
				var ent			= _db.Get<ScopeA>(  ).CreateEntity(  );
				ent.Flag<TestFlagA>( true );
				ent.Flag<TestFlagA>( false );
			}

			system.Execute(  );

			// then
			listener
				.DidNotReceiveWithAnyArgs(  )
				.OnSelf( null );
		}
	}

public interface IEventsFeature2_OnSelfSubscriber<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, IEvent_Self<TScope, TComp>, Scope<TScope>
{
	void OnSelf(Entity<TScope> ent);
}
}