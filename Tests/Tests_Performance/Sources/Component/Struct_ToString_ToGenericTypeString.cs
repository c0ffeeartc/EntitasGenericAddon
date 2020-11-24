// ReSharper disable once UnusedMember.Global
// ReSharper disable once CheckNamespace
public class Struct_ToString_ToGenericTypeString : IPerformanceTest
{
	private const			int						n						= 100000;
	private		TestCompStruct_ToString_ToGenericTypeString	_testComp;
	public					int						Iterations				=> n;

	public					void					Before					(  )
	{
		_testComp			= new TestCompStruct_ToString_ToGenericTypeString( "test" );
	}

	public					void					Run						(  )
	{
		for ( var i = 0; i < n; i++ )
		{
			_testComp.ToString(  );
		}
	}
}