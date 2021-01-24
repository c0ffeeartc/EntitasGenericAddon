using Entitas;
using Entitas.Generic;
using NSpec;
using NSubstitute;

namespace Tests
{
public class describe_EventsFeature2 : nspec
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
				.Invoke(context, ent.creationIndex, ent);

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
				.Invoke( null, 0, null );
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
}

public interface IEventsFeature2_OnSelfSubscriber<TScope,TComp>
		where TScope : IScope
		where TComp : IComponent, ICompData, IEvent_Self<TScope, TComp>, Scope<TScope>
{
	void OnSelf(Entity<TScope> ent);
}
}