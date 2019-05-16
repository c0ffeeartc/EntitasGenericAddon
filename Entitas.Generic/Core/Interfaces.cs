namespace Entitas.Generic
{
    public interface IScope { }
	public interface ICompData { }
	public interface ICompFlag { }
	public interface IUnique { }
	public interface ICopyFrom<T>
	{
		void CopyFrom(T other);
	}
}