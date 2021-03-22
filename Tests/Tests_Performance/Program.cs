using System.Threading;
using Entitas.Generic;
using PerformanceTests;
using R = PerformanceTests.PerformanceTestRunner;

namespace Tests
{
internal class Program
{
	public static void Main(string[] args)
	{
		Lookup_ScopeManager.RegisterAll();

		Thread.Sleep(500); // 1500

		MemoryHelper.MemoryBegin();

		R.Log("CompData");
		// 1 exclude component-data-init time from test
		R.Run<Entity_AddRemove_CompData_Class>();
		// 2 include component-data-init into test
		// (results can be compared with tests 3 4 5 and 11)
		R.Run<Entity_AddRemove_CompData_Class_AddNewInstance>();
		// 3 replace component data init by Cache.I.Set
		// (results can be compared with tests 2 4 5 and 11)
		R.Run<Entity_AddRemove_CompData_Class_SetUsingCache>();
		// 4 Create-Apply workflow
		// (results can be compared with tests 2, 3 5  and 11)
		R.Run<Entity_AddRemove_CompData_Class_CreateSetApply>();
		// 3.2 repeat test#3 to double check stats accuracy
		R.Run<Entity_AddRemove_CompData_Class_SetUsingCache>();
    
		// 10 base struct Add test
		R.Run<Entity_AddRemove_CompData_Struct>();
		// 11 is like 10 but component initialization is included into test
		// so results can be compared with tests 2-5
		R.Run<Entity_AddRemove_CompData_Struct_IncludeInit>();
		R.Run<Entity_AddRemove_CompData_Class_WithGroups>();
		R.Run<Entity_AddRemove_CompData_Struct_WithGroups>();
		R.Run<Entity_Replace_CompData_Class_WithGroups>();
		R.Run<Entity_Replace_CompData_Struct_WithGroups>();

		R.Log("\nCompData.Get");
		R.Run<Entity_Get_CompData_Class>(  );
		R.Run<Entity_Get_CompData_Struct>(  );

		R.Log("\nCompData.Has");
		R.Run<Entity_Has_CompData_Class>(  );
		R.Run<Entity_Has_CompData_Struct>(  );

		R.Log("\nFlag");
		R.Run<Entity_Flag_CompFlag>(  );
		R.Run<Entity_Is_CompFlag>(  );

		R.Log("\nMisc");
		R.Run<Struct_ToString_Implemented>(  );
		R.Run<Struct_ToString_NotImplemented>(  );
		R.Run<Struct_ToString_ToGenericTypeString>(  );

		MemoryHelper.MemoryEnd(  );
	}
}
}