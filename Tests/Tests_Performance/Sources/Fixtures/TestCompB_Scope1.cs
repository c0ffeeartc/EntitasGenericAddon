using System;
using Entitas;
using Entitas.Generic;

public sealed class TestCompB_Scope1 : IComponent
	, Scope<TestScope1>
	, ICompData
	, ICopyFrom<TestCompB_Scope1>
{
	public Int32 data;
	public void CopyFrom( TestCompB_Scope1 other)
	{
		data = other.data;
	}

	public TestCompB_Scope1( Int32 data )
	{
		this.data = data;
	}

	public TestCompB_Scope1( )
	{
	}
}

 public struct TestCompBStruct_Scope1 : IComponent
		, Scope<TestScope1>
		, ICompData
{
	public Int32 data;

	public TestCompBStruct_Scope1( Int32 data )
	{
		this.data = data;
	}
}