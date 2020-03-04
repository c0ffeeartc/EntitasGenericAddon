using Entitas;
using Entitas.Generic;

//namespace Entitas.Generic
//{
	public partial class Contexts
	{
		private IContext[] _scopedContexts;

		public void AddScopedContexts()
		{
			if ( _scopedContexts != null )
			{
				return;
			}

			_scopedContexts = new IContext[Lookup_ScopeManager.Count];

			for (var i = 0; i < Lookup_ScopeManager.Count; i++)
			{
				_scopedContexts[i] = Lookup_ScopeManager.CreateContext(i,
#if (ENTITAS_FAST_AND_UNSAFE)
				AERCFactories.UnsafeAERCFactory
#else
				AERCFactories.SafeAERCFactory
#endif
					);
			}
		}

		public ScopedContext<TScope> Get<TScope>() where TScope : IScope
		{
			return (ScopedContext<TScope>) _scopedContexts[Lookup<TScope>.Id];
		}
	}
//}
