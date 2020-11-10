// ReSharper disable once UnusedMember.Global
// ReSharper disable once CheckNamespace
public class Struct_ToString_Implemented : IPerformanceTest
{
	private const			int						n						= 1000000;
	private		TestCompStruct_ToString_Implemented	_testComp;
	public					int						Iterations				=> n;

	public					void					Before					(  )
	{
		_testComp			= new TestCompStruct_ToString_Implemented( "test" );
	}

	public					void					Run						(  )
	{
		for ( var i = 0; i < n; i++ )
		{
			_testComp.ToString(  );
		}
	}
}