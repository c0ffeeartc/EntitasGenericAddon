using Entitas.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class describe_Lookup
{
	private					Contexts				_contexts;

	[SetUp]
	public					void					BeforeEach()
	{
		_contexts			= new Contexts(  );
		_contexts.AddScopedContexts(  );
	}

	[Test]
	public					void					test_Lookup_IsDefined	(  )
	{
		Lookup<ScopeA>.IsDefined(typeof(TestCompA)).Should().BeTrue();
		Lookup<ScopeA>.IsDefined(typeof(TestCompB)).Should().BeFalse();
	}

	[Test]
	public					void					test_Lookup_CompTypeToI	(  )
	{
		(Lookup<ScopeA>.CompTypeToI[typeof(TestCompA)] == Lookup<ScopeA,TestCompA>.Id).Should(  ).BeTrue(  );
		(Lookup<ScopeA>.CompTypeToI[typeof(TestDataA)] == Lookup<ScopeA,TestDataA>.Id).Should(  ).BeTrue(  );
		(Lookup<ScopeA>.CompTypeToI[typeof(TestCompA)] != Lookup<ScopeA,TestDataA>.Id).Should(  ).BeTrue(  );
		(Lookup<ScopeA>.CompTypeToI[typeof(TestDataA)] != Lookup<ScopeA,TestCompA>.Id).Should(  ).BeTrue(  );
	}
}
