using System;
using Entitas;
using Entitas.Generic;

namespace Tests
{
	internal interface ScopeA : IScope {}
	internal interface ScopeB : IScope {}

	internal sealed class TestCompA : IComponent, ICompData, ICopyFrom<TestCompA>, Scope<ScopeA>
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