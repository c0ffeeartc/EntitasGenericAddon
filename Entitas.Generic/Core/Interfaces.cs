namespace Entitas.Generic
{
    public interface IScope { }
    public interface Scope<TScope> where TScope : IScope { }
	public interface ICompData { }
	public interface ICompFlag { }
	public interface IUnique { }
	public interface ICreateApply { }
	public interface ICopyFrom<T>
	{
		void CopyFrom(T other);
	}
}