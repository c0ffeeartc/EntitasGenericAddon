using System;
using Entitas;
using Entitas.Generic;

public struct TestCompAStructV_Scope1 : IComponent
		, Scope<TestScope1>
		, ICompData
		, IVersionChanged
{
	public TestCompAStructV_Scope1( Int32 data )
	{
		this.data = data;
		VersionChanged = 0;
		VersionChangedId = VersionChangeId.Undefined;
	}

	public Int32 data;

	public Int32 VersionChanged { get; set; }
	public VersionChangeId VersionChangedId { get; set; }
}

public struct TestCompBStructV_Scope1 : IComponent
		, Scope<TestScope1>
		, ICompData
		, IVersionChanged
{
	public TestCompBStructV_Scope1( Int32 data )
	{
		this.data = data;
		VersionChanged = 0;
		VersionChangedId = VersionChangeId.Undefined;
	}

	public Int32 data;

	public Int32 VersionChanged { get; set; }
	public VersionChangeId VersionChangedId { get; set; }
}