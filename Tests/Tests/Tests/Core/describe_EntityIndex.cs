using System;
using Entitas;
using Entitas.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class describe_EntityIndex
	{
		private					Contexts				_contexts = null!;

		[SetUp]
		public					void					BeforeEach()
		{
			_contexts				= new Contexts(  );
			_contexts.AddScopedContexts(  );
		}

		[Test]
		public					void					test_EntityIndex_GetAllEntsBy_0EntsByNonMatchingIndex(  )
		{
			// given
			var indexKey			= "indexKey";
			var contextA			= _contexts.Get<ScopeA>(  );
			contextA.AddEntityIndex(
				indexKey
				, contextA.GetGroup( Matcher<ScopeA,TestCompA>.I )
				, ( e,  c ) => ((TestCompA )c).Value );

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 1 ) );
			}

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 2 ) );
			}

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 2 ) );
			}

			// when
			var ents = contextA.GetAllEntsBy<ScopeA, TestCompA, Int32>(indexKey, 100);

			// then
			ents.Count.Should(  ).Be( 0 );
		}

		[Test]
		public					void					test_EntityIndex_GetAllEntsBy_CorrectAmountOfEntsByMatchingIndex(  )
		{
			// given
			var indexKey			= "indexKey";
			var contextA			= _contexts.Get<ScopeA>(  );
			contextA.AddEntityIndex(
				indexKey
				, contextA.GetGroup( Matcher<ScopeA,TestCompA>.I )
				, ( e,  c ) => ((TestCompA )c).Value );

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 1 ) );
			}

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 2 ) );
			}

			{
				var entity			= contextA.CreateEntity(  );
				entity.Add( new TestCompA(  ).Set( 2 ) );
			}

			{
				// when
				var ents = contextA.GetAllEntsBy<ScopeA, TestCompA, Int32>(indexKey, 1);

				// then
				ents.Count.Should(  ).Be( 1 );
			}

			{
				// when
				var ents = contextA.GetAllEntsBy<ScopeA, TestCompA, Int32>(indexKey, 2);

				// then
				ents.Count.Should(  ).Be( 2 );
			}
		}

		[Test]
		public					void					test_PrimaryEntityIndex_GetSingleEntBy_NullByNonMatchingIndex(  )
		{
			// given
			var indexKey			= "indexKey";
			var indexKey2			= "indexKey2";
			var contextB			= _contexts.Get<ScopeB>(  );
			contextB.AddPrimaryEntityIndex(
				indexKey
				, contextB.GetGroup( Matcher<ScopeB,TestCompB>.I )
				, ( e,  c ) => ((TestCompB )c).Value );

			contextB.AddPrimaryEntityIndex(
				indexKey2
				, contextB.GetGroup( Matcher<ScopeB,TestCompB2>.I )
				, ( e,  c ) => ((TestCompB2)c).Value );

			var entity			= contextB.CreateEntity(  );
			entity.Add( new TestCompB(  ).Set( 1 ) );

			// when
			var ent = contextB.GetSingleEntBy<ScopeB, TestCompB, Int32>(indexKey2, 1);

			// then
			ent.Should().BeNull();
		}

		[Test]
		public					void					test_PrimaryEntityIndex_GetSingleEntBy_CorrectByMatchingIndex(  )
		{
			// given
			var indexKey			= "indexKey";
			var contextB			= _contexts.Get<ScopeB>(  );
			contextB.AddPrimaryEntityIndex(
				indexKey
				, contextB.GetGroup( Matcher<ScopeB,TestCompB>.I )
				, ( e,  c ) => ((TestCompB )c).Value );

			var entity			= contextB.CreateEntity(  );
			entity.Add( new TestCompB(  ).Set( 1 ) );

			// when
			var ent = contextB.GetSingleEntBy<ScopeB, TestCompB, Int32>(indexKey, 1);

			// then
			ent.Should().BeSameAs( entity );
		}

		[Test]
		public					void					test_PrimaryEntityIndex_AddSameValueTwiceThrows(  )
		{
			// given
			var indexKey			= "indexKey";
			var contextB			= _contexts.Get<ScopeB>(  );
			contextB.AddPrimaryEntityIndex(
				indexKey
				, contextB.GetGroup( Matcher<ScopeB,TestCompB>.I )
				, ( e,  c ) => ((TestCompB )c).Value );

			{
				var entity			= contextB.CreateEntity(  );
				entity.Add( new TestCompB(  ).Set( 1 ) );
			}

			// when
			Action act = (  )=>
				{
					var entity			= contextB.CreateEntity(  );
					entity.Add( new TestCompB(  ).Set( 1 ) );
				};

			// then
			act.Should().Throw<EntityIndexException>(  );
		}
	}
}
