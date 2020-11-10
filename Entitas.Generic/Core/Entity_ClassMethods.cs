namespace Entitas.Generic
{
    public partial class Entity<TScope>
        : Entitas.Entity
        where TScope : IScope
    {
        public void Add<TComp>(TComp comp) where TComp : class, Scope<TScope>, ICompData, ICopyFrom<TComp>,IComponent, new()
        {
            var index = Lookup<TScope, TComp>.Id;
            var component = CreateComponent(index, typeof(TComp));
            ((TComp)component).CopyFrom(comp);
            AddComponent(index, component);
        }

        public void Replace<TComp>(TComp comp) where TComp : class, Scope<TScope>, ICompData, ICopyFrom<TComp>, IComponent, new()
        {
            var index = Lookup<TScope, TComp>.Id;
            var component = CreateComponent(index, typeof(TComp));
            ((TComp)component).CopyFrom(comp);
            ReplaceComponent(index, component);
        }

        public void Remove<TComp>()where TComp : class, Scope<TScope>, ICompData, IComponent
        {
            RemoveComponent(Lookup<TScope, TComp>.Id);
        }

        public TComp Get<TComp>() where TComp : class, Scope<TScope>, IComponent
        {
            return (TComp) GetComponent(Lookup<TScope, TComp>.Id);
        }

        public TComp Create<TComp>() where TComp : class, Scope<TScope>, ICompData, ICreateApply, IComponent, new()
        {
            var index = Lookup<TScope, TComp>.Id;
            var component = CreateComponent(index, typeof(TComp));
            return (TComp)component;
        }

        public void Apply<TComp>(TComp comp) where TComp : class, Scope<TScope>, ICompData, ICreateApply, IComponent, new()
        {
            var index = Lookup<TScope, TComp>.Id;
            ReplaceComponent(index, comp);
        }

        public TComp Init<TComp>() where TComp : class, Scope<TScope>, ICompData, IComponent, new()
        {
          var index = Lookup<TScope, TComp>.Id;
          var compData = (TComp) CreateComponent(index, typeof(TComp));
          ReplaceComponent(index, compData);
          return compData;
        }

        public void Flag<TComp>(bool flag) where TComp : class, Scope<TScope>, ICompFlag, IComponent, new()
        {
            var index = Lookup<TScope, TComp>.Id;
            var hasComponent = HasComponent(index);

            if (flag)
            {
                if (hasComponent)
                {
                    return;
                }
                var component = CreateComponent(index, typeof(TComp));
                AddComponent( index, component );
            }
            else
            {
                if (!hasComponent)
                {
                    return;
                }
                RemoveComponent(index);
            }
        }

        public bool Is<TComp>() where TComp : class, Scope<TScope>, ICompFlag, IComponent
        {
            return HasComponent(Lookup<TScope, TComp>.Id);
        }

        public bool Has<TComp>() where TComp : class, Scope<TScope>, IComponent, ICompData
        {
            return HasComponent(Lookup<TScope, TComp>.Id);
        }

        public bool HasIComponent<TComp>() where TComp : class, Scope<TScope>, IComponent
        {
            return HasComponent(Lookup<TScope, TComp>.Id);
        }
        
        public void RemoveIfExists<TComp>()where TComp : class, Scope<TScope>, ICompData, IComponent
        {
          if (Has<TComp>()) {
            RemoveComponent(Lookup<TScope, TComp>.Id);
          }
        }
    }
}
