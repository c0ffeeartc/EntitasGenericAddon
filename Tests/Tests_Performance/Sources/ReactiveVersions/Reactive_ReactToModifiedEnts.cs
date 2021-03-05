using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Generic;
using Tests;
using Ent = Entitas.Generic.Entity<TestScope1>;

public sealed class Reactive_ReactToModifiedEnts : IPerformanceTest, IToTestString
{
	public Reactive_ReactToModifiedEnts ( Int32 iterations, Int32 entsCount, Int32 entsModifyPercent )
	{
		_entsCount					= entsCount;
		_entsModifyPercent			= entsModifyPercent;
		_iterations					= iterations;
	}

	private readonly		Int32					_entsCount;
	private readonly		Int32					_entsModifyPercent;
	private readonly		Int32					_iterations;
	private	ReplaceCompInPercentOfEntitiesSystem	_systemModifyEnts;
	private				ReactToModifiedEnts_System	_systemReactToModifiedEnts;
	public					int						Iterations				=> _iterations;

	public					void					Before					(  )
	{
		var contexts				= new Contexts(  );
		contexts.AddScopedContexts(  );

		var _context				= contexts.Get<TestScope1>(  );

		for ( var i = 0; i < _entsCount; i++ )
		{
			var ent					= _context.CreateEntity(  );
			ent.Add_(new TestCompAStruct_Scope1());
		}

		// create after entities to not react to their creation
		_systemModifyEnts			= new ReplaceCompInPercentOfEntitiesSystem(_context, _entsModifyPercent);
		_systemReactToModifiedEnts = new ReactToModifiedEnts_System(_context);
	}

	public					void					Run						(  )
	{
		for ( int i = 0; i < _iterations; i++ )
		{
			_systemModifyEnts.Execute();
			_systemReactToModifiedEnts.Execute();
		}
	}

	public String ToTestString( )
	{
		return $"{GetType()}"
			//+ $":i{_iterations.ToString()}"
			+ $":{_entsCount.ToString("e0")}"
			+ $"*{_entsModifyPercent.ToString()}%";
	}
}

public sealed class Version_ReactToModifiedEnts : IPerformanceTest, IToTestString
{
	public Version_ReactToModifiedEnts ( Int32 iterations, Int32 entsCount, Int32 entsModifyPercent )
	{
		_entsCount					= entsCount;
		_entsModifyPercent			= entsModifyPercent;
		_iterations					= iterations;
	}

	private readonly		Int32					_entsCount;
	private readonly		Int32					_entsModifyPercent;
	private readonly		Int32					_iterations;
	private	Version_ReplaceCompInPercentOfEntitiesSystem	_systemModifyEnts;
	private		Version_ReactToModifiedEnts_System	_systemReactToModifiedEnts;
	public					int						Iterations				=> _iterations;

	public					void					Before					(  )
	{
		var contexts				= new Contexts(  );
		contexts.AddScopedContexts(  );

		var _context				= contexts.Get<TestScope1>(  );

		for ( var i = 0; i < _entsCount; i++ )
		{
			var ent					= _context.CreateEntity(  );
			ent.AddV_(new TestCompAStruct_Scope1());
		}

		// create after entities to not react to their creation
		_systemModifyEnts			= new Version_ReplaceCompInPercentOfEntitiesSystem(_context, _entsModifyPercent);
		_systemReactToModifiedEnts = new Version_ReactToModifiedEnts_System(_context);
	}

	public					void					Run						(  )
	{
		for ( int i = 0; i < _iterations; i++ )
		{
			_systemModifyEnts.Execute();
			_systemReactToModifiedEnts.Execute();
		}
	}

	public String ToTestString( )
	{
		return $"{GetType()}"
			//+ $":i{_iterations.ToString()}"
			+ $":{_entsCount.ToString("e0")}"
			+ $"*{_entsModifyPercent.ToString()}%";
	}
}

internal class ReplaceCompInPercentOfEntitiesSystem : IExecuteSystem
{
	public ReplaceCompInPercentOfEntitiesSystem(ScopedContext<TestScope1> context, Single percentOfEntsToModify)
	{
		_group = context.GetGroup(Matcher<TestScope1,TestCompAStruct_Scope1>.I);
		Single percentClamped = Math.Min(Math.Max(percentOfEntsToModify,0f), 100f);
		_percentOfEntsToModify = percentClamped;
	}

	private readonly IGroup<Ent> _group;
	private readonly List<Ent> _entsBuff = new List<Ent>();
	private readonly Single _percentOfEntsToModify;
	private readonly TestCompAStruct_Scope1 _comp = new TestCompAStruct_Scope1();

	public void Execute()
	{
		_group.GetEntities(_entsBuff);
		Single entsCountToModify = _entsBuff.Count / 100f * _percentOfEntsToModify;
		for ( var i = 0; i < _entsBuff.Count && i < entsCountToModify; i ++ )
		{
			var ent = _entsBuff[i];
			ent.Replace_( _comp );
		}
	}
}

internal class ReactToModifiedEnts_System : ReactiveSystem<Ent>
{
	public ReactToModifiedEnts_System(IContext<Ent> context ) : base( context )
	{
	}

	protected override ICollector<Ent> GetTrigger(IContext<Ent> context ) { return context.CreateCollector(
		Matcher<TestScope1,TestCompAStruct_Scope1>.I ); }

	protected override Boolean Filter(Ent entity )
		=> entity.Has_<TestCompAStruct_Scope1>();

	protected override void Execute(List<Ent> entities )
	{
		for ( var i = 0; i < entities.Count; i++ )
		{
			var ent = entities[i];
			var comp = ent.Get_<TestCompAStruct_Scope1>();
			comp.data++;
		}
	}
}

internal class Version_ReplaceCompInPercentOfEntitiesSystem : IExecuteSystem
{
	public Version_ReplaceCompInPercentOfEntitiesSystem(ScopedContext<TestScope1> context, Single percentOfEntsToModify)
	{
		_group = context.GetGroup(Matcher<TestScope1,TestCompAStruct_Scope1>.I);
		Single percentClamped = Math.Min(Math.Max(percentOfEntsToModify,0f), 100f);
		_percentOfEntsToModify = percentClamped;
	}

	private readonly IGroup<Ent> _group;
	private readonly List<Ent> _entsBuff = new List<Ent>();
	private readonly Single _percentOfEntsToModify;
	private readonly TestCompAStruct_Scope1 _comp = new TestCompAStruct_Scope1();

	public void Execute()
	{
		_group.GetEntities(_entsBuff);
		Single entsCountToModify = _entsBuff.Count / 100f * _percentOfEntsToModify;
		for ( var i = 0; i < _entsBuff.Count && i < entsCountToModify; i ++ )
		{
			var ent = _entsBuff[i];
			ent.ReplaceV_( _comp );
		}
	}
}

internal class Version_ReactToModifiedEnts_System : ExecuteSystem
{
	public Version_ReactToModifiedEnts_System(IContext<Ent> context)
	{
		_group = context.GetGroup(Matcher<TestScope1,TestCompAStruct_Scope1>.I);
	}

	private readonly IGroup<Ent> _group;
	private readonly List<Ent> _entsBuff = new List<Ent>();

	public override void Exec( )
	{
		_group.GetEntities(_entsBuff);
		for ( var i = 0; i < _entsBuff.Count; i++ )
		{
			var ent = _entsBuff[i];
			if ( !ent.HasChanged_<TestCompAStruct_Scope1>( VersionLast ) )
			{
				continue;
			}

			var comp = ent.Get_<TestCompAStruct_Scope1>();
			comp.data++;
		}
	}
}
