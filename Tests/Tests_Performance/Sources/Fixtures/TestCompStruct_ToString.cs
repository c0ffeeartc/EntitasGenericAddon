using System;
using Entitas;
using Entitas.Generic;

public struct TestCompStruct_ToString_Implemented
		: IComponent
		, ICompData
		, Scope<TestScope1>
{
	public TestCompStruct_ToString_Implemented( String s = "test" )
	{
		Str = "test";
	}

	public String Str;

	public override String ToString( )
	{
		return Str;
	}
}

public struct TestCompStruct_ToString_NotImplemented
		: IComponent
		, ICompData
		, Scope<TestScope1>
{
	public TestCompStruct_ToString_NotImplemented( String s = "test" )
	{
		Str = "test";
	}

	public String Str;
}

public struct TestCompStruct_ToString_ToGenericTypeString
		: IComponent
		, ICompData
		, Scope<TestScope1>
{
	public TestCompStruct_ToString_ToGenericTypeString( String s = "test" )
	{
		Str = "test";
	}

	public String Str;

	public override String ToString( )
	{
		return typeof( TestCompStruct_ToString_ToGenericTypeString ).ToGenericTypeString(  );
	}
}
