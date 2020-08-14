using System;
using System.Threading;
using Entitas.Generic;

namespace Tests
{
internal class Program
{
	public static void Main(string[] args)
	{
		Console.WriteLine("Running performance tests...");
		Lookup_ScopeManager.RegisterAll();

		Thread.Sleep(1500); // why?

		run<Entity_AddRemove_CompData_Class>();
		run<Entity_AddRemove_CompData_Struct>();
		run<Entity_AddRemove_CompData_Class_WithGroups>();
		run<Entity_AddRemove_CompData_Struct_WithGroups>();
		run<Entity_Replace_CompData_Class_WithGroups>();
		run<Entity_Replace_CompData_Struct_WithGroups>();

		run<Entity_Get_CompData_Class>(  );
		run<Entity_Get_CompData_Struct>(  );

		run<Entity_Has_CompData_Class>(  );
		run<Entity_Has_CompData_Struct>(  );

		run<Entity_Flag_CompFlag>(  );
		run<Entity_Is_CompFlag>(  );

		// Console.WriteLine("\nPress any key...");
		// Console.Read();
	}

	//Running performance tests...

	static void run<T>() where T : IPerformanceTest, new()
	{
		Thread.Sleep(100);
		if (typeof(T) == typeof(EmptyTest))
		{
			Console.WriteLine(string.Empty);
			return;
		}
		var test = new T();
		Console.Write( test.Iterations.ToString( "e0" ) + " " + (typeof(T) + ": ").PadRight(60));
		// For more reliable results, run before
		PerformanceTestRunner.Run(test);
		var ms = PerformanceTestRunner.Run(new T());
		Console.WriteLine(ms + " ms");
	}
}
}