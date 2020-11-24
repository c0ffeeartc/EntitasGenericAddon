using System;

public static class TypeExtensions
{
	public static			string					ToGenericTypeString		( this Type t )
	{
		if (!t.IsGenericType)
		{
			return t.Name;
		}
		var typeName 				= t.Name;
		var curTypeName				= typeName.Substring(0, typeName.IndexOf('`'));

		var genericArgs				= !t.IsGenericTypeDefinition
			? t.GetGenericArguments()
			: Type.EmptyTypes;

		var genericArgsStr			= string.Empty;
		for (var i = 0; i < genericArgs.Length; i++)
		{
			if (i > 0)
			{
				genericArgsStr		+= ",";
			}
			genericArgsStr			+= genericArgs[i].ToGenericTypeString();
		}

		return $"{curTypeName}<{genericArgsStr}>";
	}
}