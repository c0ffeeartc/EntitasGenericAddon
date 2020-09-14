using Entitas.Generic;
using NSpec;
using NSubstitute;

namespace Tests
{
	public class describe_Event_ClassMethods : nspec
	{
		private				Contexts				_contexts;

		private				void					test_CoreMethods		(  )
		{
			Lookup_ScopeManager.RegisterAll(  );

			before					= ()=>
			{
				_contexts			= new Contexts(  );
				_contexts.AddScopedContexts(  );
			};

			it["OnAny listens other"] = ()=>
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
			};

			it["OnAnyRemoved listens other"] = (  ) =>
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
			};

			it["OnSelf listens self"] = ()=>
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
			};

			it["OnSelf doesn't listen other"] = ()=>
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
			};

			it["OnSelfRemoved listens self"] = ()=>
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
			};

			it["OnSelfRemoved doesn't listen other"] = ()=>
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
			};

			it["OnSelfFlag listens self Added"] = ()=>
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
			};

			it["OnSelfFlag listens self Removed"] = ()=>
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
			};
		}
	}
}