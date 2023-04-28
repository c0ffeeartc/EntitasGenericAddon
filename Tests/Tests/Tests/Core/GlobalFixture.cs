using Entitas.Generic;
using NUnit.Framework;

namespace Tests;

[SetUpFixture]
public class GlobalFixture
{
	[OneTimeSetUp]
	public				void					GlobalBeforeAll()
	{
		Lookup_ScopeManager.RegisterAll();
	}
}