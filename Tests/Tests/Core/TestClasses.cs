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
			, IEvent_AnyRemoved<ScopeA, TestCompA>, IEvent_Self<ScopeA, TestCompA>, IEvent_SelfRemoved<ScopeA, TestCompA>
	{
		public				Single					Value;

		public				TestCompA				Set						( Single value = 0 )
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

	internal sealed class TestCompB : IComponent, ICompData, ICopyFrom<TestCompB>, ScopeB
	{
		public				Single					Value;

		public				TestCompB				Set						( Single value = 0 )
		{
			Value					= value;
			return this;
		}

		public				void					CopyFrom				( TestCompB other )
		{
			Value					= other.Value;
		}
	}

	internal sealed class TestFlagA : IComponent, ICompFlag, Scope<ScopeA>
	{ }
	internal sealed class TestFlagB : IComponent, ICompFlag, Scope<ScopeB>
	{ }
	internal sealed class TestFlagAUnique : IComponent, ICompFlag, Scope<ScopeA>, IUnique
	{ }

}