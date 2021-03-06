using System;
using System.Diagnostics;
using System.Runtime;
using System.Threading;
using Entitas.Generic;

namespace Tests
{
internal class Program
{
	private static Process currentProcess;

	public static void Main(string[] args)
	{
		Console.WriteLine("Running performance tests...");
		currentProcess = Process.GetCurrentProcess();
		Console.WriteLine($"memory at process start: {MemoryHelper.GetMemoryAllStatsString(currentProcess)}");
    
		Lookup_ScopeManager.RegisterAll();

		Thread.Sleep(1500); // why?

		GCSettings.LatencyMode = GCLatencyMode.LowLatency;
		try
		{
			if (!GC.TryStartNoGCRegion(0))
			{
				Console.WriteLine("Can't start NoGCRegion");
			}
		}
		catch ( Exception ex )
		{
			Console.WriteLine("can't start NoGCRegion. " + ex.Message );
		}

		Console.WriteLine($"memory before tests: {MemoryHelper.GetMemoryAllStatsString(currentProcess)}");

		// // 1 exclude component-data-init time from test
		// run<Entity_AddRemove_CompData_Class>();
		// // 2 include component-data-init into test
		// // (results can be compared with tests 3 4 5 and 11)
		// run<Entity_AddRemove_CompData_Class_AddNewInstance>();
		// // 3 replace component data init by Cache.I.Set
		// // (results can be compared with tests 2 4 5 and 11)
		// run<Entity_AddRemove_CompData_Class_SetUsingCache>();
		// // 4 Create-Apply workflow
		// // (results can be compared with tests 2, 3 5  and 11)
		// run<Entity_AddRemove_CompData_Class_CreateSetApply>();
		// // 3.2 repeat test#3 to double check stats accuracy
		// run<Entity_AddRemove_CompData_Class_SetUsingCache>();
		//
		// // 10 base struct Add test
		// run<Entity_AddRemove_CompData_Struct>();
		// // 11 is like 10 but component initialization is included into test
		// // so results can be compared with tests 2-5
		// run<Entity_AddRemove_CompData_Struct_IncludeInit>();
		run<Entity_AddRemove_CompData_Class_WithGroups>();
		run<Entity_AddRemove_CompData_Struct_WithGroups>();
		run<ReactV_Entity_AddRemove_CompData_Struct_WithGroups>();
		run<Entity_Replace_CompData_Class_WithGroups>();
		run<Entity_Replace_CompData_Struct_WithGroups>();
		run<ReactV_Entity_Replace_CompData_Struct_WithGroups>();

		run<EmptyTest>();

		RunTests_ReactToModifiedEnts( runCount: 100
			, entCount: 10000
			, reactSystemCount: 1
			, modifyEntsPercents: new []{1,25,50,75,100} );

		RunTests_ReactToModifiedEnts( runCount: 100
			, entCount: 10000
			, reactSystemCount: 10
			, modifyEntsPercents: new []{1,25,50,75,100} );

		RunTests_ReactToModifiedEnts( runCount: 100
			, entCount: 10000
			, reactSystemCount: 50
			, modifyEntsPercents: new []{1,25,50,75,100} );

		//
		// run<Entity_Get_CompData_Class>(  );
		// run<Entity_Get_CompData_Struct>(  );
		//
		// run<Entity_Has_CompData_Class>(  );
		// run<Entity_Has_CompData_Struct>(  );
		//
		// run<Entity_Flag_CompFlag>(  );
		// run<Entity_Is_CompFlag>(  );
		//
		// run<Struct_ToString_Implemented>(  );
		// run<Struct_ToString_NotImplemented>(  );
		// run<Struct_ToString_ToGenericTypeString>(  );

		Console.WriteLine($"memory after tests: {MemoryHelper.GetMemoryAllStatsString(currentProcess)}");
    
		try
		{
  			GC.EndNoGCRegion();
		}
		catch (System.InvalidOperationException e)
		{
  			Console.WriteLine("\nMemory validation needs to be improved... " +
				"\nProcess had to clean memory while in NoGC mode" +
				"\nNegative memory diffs indicate that");
			throw;
		}
		catch ( Exception ex )
		{
			Console.WriteLine("Can't start EndNoGCRegion. " + ex.Message );
		}

		// Console.WriteLine("\nPress any key...");
		// Console.Read();
	}

	private static void RunTests_ReactToModifiedEnts(Int32 runCount, Int32 entCount, Int32 reactSystemCount, Int32[] modifyEntsPercents )
	{
		foreach ( var percent in modifyEntsPercents )
		{
			run( ( ) => new ReactToModifiedEnts_Reactive( runCount, entCount, percent, reactSystemCount ) );
			run( ( ) => new ReactToModifiedEnts_Version( runCount, entCount, percent, reactSystemCount ) );
		}
		run<EmptyTest>( );
	}

	static void run<T>() where T : IPerformanceTest, new()
	{
		run<T>(()=>new T());
	}
	
	static void run<T>(Func<T> testFactory) where T : IPerformanceTest
	{
		Thread.Sleep(200);//(500);
		if (typeof(T) == typeof(EmptyTest))
		{
			Console.WriteLine(string.Empty);
			return;
		}

		var test = testFactory.Invoke();
		var testString = test is IToTestString
			? (test as IToTestString).ToTestString()
			: typeof(T).ToString();
		Console.Write( test.Iterations.ToString( "e0" ) + " " + (testString + ": ").PadRight(60));
		// For more reliable results, run before
		PerformanceTestRunner.Run(test);

		Tuple<long,long> msAndMemory = PerformanceTestRunner.Run(testFactory.Invoke());
		Console.Write((msAndMemory.Item1 + " ms").PadRight(10));
		Console.Write(("mem.DIFF " +msAndMemory.Item2 + " bytes").PadRight(30));
		Console.WriteLine("mem.ALL " + MemoryHelper.GetMemoryAllStatsString(currentProcess) + " bytes");
	}
}
}