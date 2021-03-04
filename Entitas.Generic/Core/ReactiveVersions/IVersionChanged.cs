namespace Entitas.Generic
{
public enum VersionChangeId
{
	Undefined = 0,
	Add = 1,
	Replace = 1,
	Remove = 1,
}

public interface IVersionChanged
{
	int VersionChanged {get;set;}
	VersionChangeId VersionChangedId {get;set;}
}

public static class Version
{
	public static int Global = 1;
}

public interface IVersionLast
{
	int VersionLast {get;set;}
}
}