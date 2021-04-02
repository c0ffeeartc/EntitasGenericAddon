using System.Collections.Generic;

namespace Entitas.Generic
{
public sealed class Events2 : IEvents2
{
	public static			IEvents2				I						= new Events2();
	private					List<IUnsubAll>			_unsubAllList			= new List<IUnsubAll>();

	public					void					Add						( IUnsubAll unsubAll )
	{
		_unsubAllList.Add( unsubAll );
	}

	public					void					UnsubAll				(  )
	{
		for ( int i = 0; i < _unsubAllList.Count; i++ )
		{
			_unsubAllList[i].UnsubAll();
		}
	}
}

public interface IEvents2: IUnsubAll
{
	void					Add						( IUnsubAll unsubAll );
}

public interface IUnsubAll
{
	void					UnsubAll				(  );
}
}