using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public class Lookup<TScope, TComponent> where TScope : IScope
{
	public static			int						Id						= -1;
}

public class Lookup<TScope> where TScope : IScope
{
	public static			int						Id						= -1;
	public static			int						CompCount;
	public static			List<Type>				CompTypes				= new List<Type>(  );
	public static			List<String>			CompNames				= new List<String>(  );
	private static			Type[]					_compTypesArray;
	private static			String[]				_compNamesArray;
	public static			Type[]					CompTypesArray
	{
		get
		{
			if ( _compTypesArray == null
				|| _compTypesArray.Length != CompTypes.Count )
			{
				_compTypesArray		= CompTypes.ToArray(  );
			}
			return _compTypesArray;
		}
	}
	public static			String[]				CompNamesArray
	{
		get
		{
			if ( _compNamesArray == null
				|| _compNamesArray.Length != CompNames.Count )
			{
				_compNamesArray		= CompNames.ToArray(  );
			}
			return _compNamesArray;
		}
	}
}

public class ScopeCount
{
	public static			int						Value;
}
}