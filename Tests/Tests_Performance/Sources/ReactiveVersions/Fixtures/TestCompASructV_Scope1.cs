using System;
using Entitas;
using Entitas.Generic;

public struct TestCompAStructV_Scope1 : IComponent
		, Scope<TestScope1>
		, ICompData
{
	public TestCompAStructV_Scope1( Int32 data )
	{
		Data = data;
	}

	public Int32 Data;
}

public struct TestCompBStructV_Scope1 : IComponent
		, Scope<TestScope1>
		, ICompData
{
	public TestCompBStructV_Scope1( Int32 data )
	{
		Data = data;
	}

	public Int32 Data;
}