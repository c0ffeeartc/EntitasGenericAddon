using System;
using Entitas;
using Entitas.Generic;

namespace Tests
{
	public interface ScopeA : IScope {}
	public interface ScopeB : IScope {}

	public sealed class TestCompA
			: IComponent
			, ICompData
			, ICopyFrom<TestCompA>
			, Scope<ScopeA>
			, IEvent_Any<ScopeA, TestCompA>
			, IEvent_AnyRemoved<ScopeA, TestCompA>
			, IEvent_Self<ScopeA, TestCompA>
			, IEvent_SelfRemoved<ScopeA, TestCompA>
			, IGetAllEntsByIndex<Int32>
	{
		public				Int32					Value;

		public				TestCompA				Set						( Int32 value = 0 )
		{
			Value					= value;
			return this;
		}

		public				void					CopyFrom				( TestCompA other )
		{
			Value					= other.Value;
		}
	}

	public struct TestDataA
			: IComponent
			, ICompData
			, Scope<ScopeA>
			, IEvent_Any<ScopeA, TestDataA>
			, IEvent_AnyRemoved<ScopeA, TestDataA>
			, IEvent_Self<ScopeA, TestDataA>
			, IEvent_SelfRemoved<ScopeA, TestDataA>
	{
		public				Single					Value;

		public TestDataA( Single value)
		{
			Value = value;
		}
	}

	internal struct TestDataAUnique : IComponent, ICompData, Scope<ScopeA>, IUnique
	{
		public				Single					Value;

		public TestDataAUnique( Single value)
		{
			Value = value;
		}
	}

	internal sealed class TestCompAUnique : IComponent, ICompData, ICopyFrom<TestCompAUnique>, Scope<ScopeA>, IUnique
	{
		public				Single					Value;

		public				TestCompAUnique				Set						( Single value = 0 )
		{
			Value					= value;
			return this;
		}

		public				void					CopyFrom				( TestCompAUnique other )
		{
			Value					= other.Value;
		}
	}

	internal sealed class TestCompB
			: IComponent
			, ICompData
			, ICopyFrom<TestCompB>
			, Scope<ScopeB>
			, IGetSingleEntByIndex<Int32>
	{
		public				Int32					Value;

		public				TestCompB				Set						( Int32 value = 0 )
		{
			Value					= value;
			return this;
		}

		public				void					CopyFrom				( TestCompB other )
		{
			Value					= other.Value;
		}
	}

	public sealed class TestComp_CreateApply_A
			: IComponent
			, ICompData
			, ICreateApply
			, Scope<ScopeA>
	{
		public				Single					Value;
	}

	public sealed class TestFlagA : IComponent
			, ICompFlag
			, Scope<ScopeA>
			, IEvent_Self<ScopeA,TestFlagA>
			, IEvent_Any<ScopeA,TestFlagA>
	{ }
	internal sealed class TestFlagB : IComponent, ICompFlag, Scope<ScopeB>
	{ }
	internal sealed class TestFlagAUnique : IComponent, ICompFlag, Scope<ScopeA>, IUnique
	{ }

}