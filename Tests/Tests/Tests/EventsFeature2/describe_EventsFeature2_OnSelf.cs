using Entitas;
using Entitas.Generic;
using NSpec;
using NSubstitute;

namespace Tests
{
public class describe_EventsFeature2_OnSelf : nspec
{
	private				Contexts				_db;

	private				void					test_OnSelf					(  )
	{
		Lookup_ScopeManager.RegisterAll(  );

		before					= ()=>
		{
			_db					= new Contexts(  );
			_db.AddScopedContexts(  );
		};

		after					= ()=>
		{
			// Important
			OnSelf<ScopeA,TestCompA>.I = null;
			OnSelf<ScopeA,TestDataA>.I = null;
		};

		it["OnSelf.Invoke is called for each entity in context with class component match"] = ()=>
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
		};

		it["OnSelf.Invoke is not called for entities in context without class component"] = ()=>
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
		};

		it["OnSelf listens once self ent with matching class component"] = ()=>
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
		};

		it["OnSelf listens once self ent with matching structComp"] = ()=>
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
		};

		it["OnSelf doesn't listen after OnSelf.Unsub"] = ()=>
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
		};

		it["OnSelf doesn't listen after Events2.I.UnsubAll"] = ()=>
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
		};

		it["OnSelf doesn't listen other ent with matching class component"] = ()=>
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
		};
	}

	private				void					test_OnSelf_Removed			(  )
	{
		Lookup_ScopeManager.RegisterAll(  );

		before					= ()=>
		{
			_db					= new Contexts(  );
			_db.AddScopedContexts(  );
		};

		after					= ()=>
		{
			// Important
			OnSelf<ScopeA,TestCompA>.I = null;
			OnSelf<ScopeA,TestDataA>.I = null;
			OnSelf_Removed<ScopeA,TestCompA>.I = null;
			OnSelf_Removed<ScopeA,TestDataA>.I = null;
		};

		it["OnSelf_Removed.Invoke is called for each entity in context with removed class component"] = ()=>
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
		};

		it["OnSelf_Removed.Invoke is not called for entities in context with class component"] = ()=>
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
		};

		it["OnSelf_Removed listens once self ent with matching removed class component"] = ()=>
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
		};

		it["OnSelf_Removed listens once self ent with matching structComp"] = ()=>
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
		};

		it["OnSelf_Removed doesn't listen after OnSelf.Unsub"] = ()=>
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
		};

		it["OnSelf_Removed doesn't listen after Events2.I.UnsubAll"] = ()=>
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
		};

		it["OnSelf_Removed doesn't listen other ent with matching class component"] = ()=>
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
		};
	}

	private				void					test_OnSelf_Flag			(  )
	{
		Lookup_ScopeManager.RegisterAll(  );

		before					= ()=>
		{
			_db					= new Contexts(  );
			_db.AddScopedContexts(  );
		};

		after					= ()=>
		{
			// Important
			OnSelf_Flag<ScopeA,TestFlagA>.I = null;
		};

		it["OnSelf_Flag.Invoke is called for each entity in context with Flag class component"] = ()=>
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
		};

		it["OnSelf_Flag.Invoke is not called if value is same as before"] = ()=>
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
		};

		it["OnSelf_Flag listens once self ent with matching Flag class component"] = ()=>
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
		};

		it["OnSelf_Flag doesn't listen after OnSelf.Unsub"] = ()=>
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
		};

		it["OnSelf_Flag doesn't listen after Events2.I.UnsubAll"] = ()=>
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
		};

		it["OnSelf_Flag doesn't listen other ent with matching class component"] = ()=>
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
		};
	}
}

public interface IEventsFeature2_OnSelfSubscriber<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, IEvent_Self<TScope, TComp>, Scope<TScope>
{
	void OnSelf(Entity<TScope> ent);
}
}