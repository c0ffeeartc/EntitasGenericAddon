using Entitas.Generic;
using NSpec;
using NSubstitute;

namespace Tests
{
	public class describe_Event_StructMethods : nspec
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
				var system			= new EventSystem_Any_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnAny<ScopeA,TestDataA>>(  );
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnAny( listener );
				}
				listener.DidNotReceiveWithAnyArgs(  ).OnAny( default(TestDataA), null, null );

				// when
				var entity			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add_( new TestDataA(  ) );

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnAny( default(TestDataA), null, null );
			};

			it["OnAnyRemoved listens other"] = (  ) =>
			{
				// given
				var system			= new EventSystem_AnyRemoved_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnAnyRemoved<ScopeA,TestDataA>>(  );
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_OnAnyRemoved( listener );
				}
				listener.DidNotReceiveWithAnyArgs(  ).OnAnyRemoved( default(TestDataA), null, null );

				// when
				var entity			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
				entity.Add_( new TestDataA(  ) );
				entity.Remove_<TestDataA>(  );

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnAnyRemoved( default(TestDataA), null, null );
			};

			it["OnSelf listens self"] = ()=>
			{
				// given
				var system			= new EventSystem_Self_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnSelf<ScopeA,TestDataA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( default(TestDataA), null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_( new TestDataA(  ) );
					ent.Add_OnSelf( listener );
				}

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnSelf( default(TestDataA), null, null );
			};

			it["OnSelf doesn't listen other"] = ()=>
			{
				// given
				var system			= new EventSystem_Self_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnSelf<ScopeA,TestDataA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( default(TestDataA), null, null );

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
				listener.DidNotReceiveWithAnyArgs(  ).OnSelf( default(TestDataA), null, null );
			};

			it["OnSelfRemoved listens self"] = ()=>
			{
				// given
				var system			= new EventSystem_SelfRemoved_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnSelfRemoved<ScopeA,TestDataA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( default(TestDataA), null, null );

				// when
				{
					var ent			= _contexts.Get<ScopeA>(  ).CreateEntity(  );
					ent.Add_( new TestDataA(  ) );
					ent.Add_OnSelfRemoved( listener );
					ent.Remove_<TestDataA>(  );
				}

				system.Execute(  );

				// then
				listener.ReceivedWithAnyArgs(  ).OnSelfRemoved( default(TestDataA), null, null );
			};

			it["OnSelfRemoved doesn't listen other"] = ()=>
			{
				// given
				var system			= new EventSystem_SelfRemoved_<ScopeA, TestDataA>( _contexts );
				var listener		= Substitute.For<IOnSelfRemoved<ScopeA,TestDataA>>(  );
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( default(TestDataA), null, null );

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
				listener.DidNotReceiveWithAnyArgs(  ).OnSelfRemoved( default(TestDataA), null, null );
			};
		}
	}
}