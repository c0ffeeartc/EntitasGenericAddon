using System;
using Entitas;
using Entitas.Generic;

namespace Tests
{
	internal interface STestA : IScope {}
	internal interface STestB : IScope {}

	internal sealed class TestCompA : IComponent, ICompData, ICopyFrom<TestCompA>, STestA
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

	internal sealed class TestCompAUnique : IComponent, ICompData, ICopyFrom<TestCompAUnique>, STestA, IUnique
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

	internal sealed class TestCompB : IComponent, ICompData, ICopyFrom<TestCompB>, STestB
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

	internal sealed class TestFlagA : IComponent, ICompFlag, STestA
	{ }
	internal sealed class TestFlagB : IComponent, ICompFlag, STestB
	{ }
	internal sealed class TestFlagAUnique : IComponent, ICompFlag, STestA, IUnique
	{ }

}