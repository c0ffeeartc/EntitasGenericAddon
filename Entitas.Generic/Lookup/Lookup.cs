using System;
using System.Collections.Generic;

namespace Entitas.Generic
{
public class Lookup<TScope, TComponent>
		where TScope : IScope
		where TComponent : Scope<TScope>
{
	public static			int						Id						= -1;
}

public class Lookup<TScope> where TScope : IScope
{
	public static			int						Id						= -1;
	public static			int						CompCount;
	public static			List<Type>				CompTypes				= new List<Type>(  );
	public static			List<String>			CompNamesFull			= new List<String>(  );
	public static			List<String>			CompNamesPretty			= new List<String>(  );
	public static			Dictionary<Type,Int32>	CompTypeToI				= new Dictionary<Type,Int32>(  );
	public static			bool					IsDefined				( Type compType ) => CompTypeToI.ContainsKey( compType );
	private static			Type[]					_compTypesArray;
	private static			String[]				_compNamesFullArray;
	private static			String[]				_compNamesPrettyArray;
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
	public static			String[]				CompNamesFullArray
	{
		get
		{
			if ( _compNamesFullArray == null
				|| _compNamesFullArray.Length != CompNamesFull.Count )
			{
				_compNamesFullArray		= CompNamesFull.ToArray(  );
			}
			return _compNamesFullArray;
		}
	}
	public static			String[]				CompNamesPrettyArray
	{
		get
		{
			if ( _compNamesPrettyArray == null
				|| _compNamesPrettyArray.Length != CompNamesPretty.Count )
			{
				_compNamesPrettyArray		= CompNamesPretty.ToArray(  );
			}
			return _compNamesPrettyArray;
		}
	}
}

public class Scopes
{
	public static			List<Func<IContext>>	CreateContext			= new List<Func<IContext>>(  );
	public static			int						Count;
	public static			List<Type>				ScopedContextTypes		= new List<Type>();
	public static			List<Type>				IScopeTypes				= new List<Type>();
	public static			List<Type>				CompScopeTypes			= new List<Type>();
}
}