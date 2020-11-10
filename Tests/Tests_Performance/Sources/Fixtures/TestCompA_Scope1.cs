﻿using System;
 using Entitas;
 using Entitas.Generic;

public sealed class TestCompA_Scope1 : IComponent
		, Scope<TestScope1>
		, ICompData
    , ICreateApply
		, ICopyFrom<TestCompA_Scope1>
{
	public Int32 data;
	public void CopyFrom( TestCompA_Scope1 other)
	{
		data = other.data;
	}

  public TestCompA_Scope1 Set(Int32 value)  // optional, allows using Cache<T>.I.Set(). Not needed for struct components
  {
    data = value;
    return this;
  }

	public TestCompA_Scope1( Int32 data )
	{
		this.data = data;
	}

	public TestCompA_Scope1( )
	{
	}
}

public struct TestCompAStruct_Scope1 : IComponent
		, Scope<TestScope1>
		, ICompData
{
	public Int32 data;

	public TestCompAStruct_Scope1( Int32 data )
	{
		this.data = data;
	}
}

public sealed class TestCompAFlag_Scope1 : IComponent
		, Scope<TestScope1>
		, ICompFlag
{
}