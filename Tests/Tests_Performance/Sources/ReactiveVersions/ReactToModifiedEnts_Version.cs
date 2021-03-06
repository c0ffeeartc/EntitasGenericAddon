using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Generic;
using Tests;
using Ent = Entitas.Generic.Entity<TestScope1>;


public sealed class ReactToModifiedEnts_Version : IPerformanceTest, IToTestString
{
	public					ReactToModifiedEnts_Version	( Int32 iterations, Int32 entsCount, Int32 entsModifyPercent, Int32 reactSystemsCount ) 
	{
		_iterations					= iterations;
		_entsCount					= entsCount;
		_entsModifyPercent			= entsModifyPercent;
		_reactSystemsCount			= reactSystemsCount;
	}

	private readonly		Int32					_iterations;
	private readonly		Int32					_entsCount;
	private readonly		Int32					_entsModifyPercent;
	private readonly		Int32					_reactSystemsCount;
	private	ReplaceCompInPercentOfEntitiesSystem_Version	_systemModifyEnts;
	private List<ReactToModifiedEnts_System_Version> _systemReactToModifiedEnts	= new List<ReactToModifiedEnts_System_Version>();
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
		_systemModifyEnts			= new ReplaceCompInPercentOfEntitiesSystem_Version(_context, _entsModifyPercent);
		for( var i = 0; i < _reactSystemsCount; i++ )
		{
			_systemReactToModifiedEnts.Add( new ReactToModifiedEnts_System_Version( _context ) );
		}
	}

	public					void					Run						(  )
	{
		for ( int i = 0; i < _iterations; i++ )
		{
			_systemModifyEnts.Execute();
			for( var j = 0; j < _reactSystemsCount; j++ )
			{
				_systemReactToModifiedEnts[j].Execute(  );
			}
		}
	}

	public String ToTestString( )
	{
		return $"{GetType()}"
			//+ $":i{_iterations.ToString()}"
			+ $"(r:{_reactSystemsCount.ToString()}"
			+ $",{_entsCount.ToString("e0")}"
			+ $"*{_entsModifyPercent.ToString()}%)"
			;
	}
}

internal class ReplaceCompInPercentOfEntitiesSystem_Version : IExecuteSystem
{
	public ReplaceCompInPercentOfEntitiesSystem_Version(ScopedContext<TestScope1> context, Single percentOfEntsToModify)
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

internal class ReactToModifiedEnts_System_Version : ExecuteSystem
{
	public ReactToModifiedEnts_System_Version(IContext<Ent> context)
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
