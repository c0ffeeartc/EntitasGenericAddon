namespace Entitas.Generic
{
    public partial class Entity<TScope>
        : Entitas.Entity
        where TScope : IScope
    {
        public void Add<TComp>(TComp comp) where TComp : TScope, ICompData, ICopyFrom<TComp>,IComponent, new()
        {
            var index = Lookup<TScope, TComp>.Id;
            var component = CreateComponent(index, typeof(TComp));
            ((TComp)component).CopyFrom(comp);
            AddComponent(index, component);
        }

        public void Replace<TComp>(TComp comp) where TComp : TScope, ICompData, ICopyFrom<TComp>, IComponent, new()
        {
            var index = Lookup<TScope, TComp>.Id;
            var component = CreateComponent(index, typeof(TComp));
            ((TComp)component).CopyFrom(comp);
            ReplaceComponent(index, component);
        }

        public void Remove<TComp>()where TComp : TScope, ICompData, IComponent
        {
            RemoveComponent(Lookup<TScope, TComp>.Id);
        }

        public TComp Get<TComp>() where TComp : TScope, IComponent
        {
            return (TComp) GetComponent(Lookup<TScope, TComp>.Id);
        }

        public void Flag<TComp>(bool flag) where TComp : TScope, ICompFlag, IComponent, new()
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

        public bool Is<TComp>() where TComp : TScope, ICompFlag, IComponent
        {
            return HasComponent(Lookup<TScope, TComp>.Id);
        }

        public bool Has<TComp>() where TComp : TScope, IComponent, ICompData
        {
            return HasComponent(Lookup<TScope, TComp>.Id);
        }

        public bool HasIComponent<TComp>() where TComp : TScope, IComponent
        {
            return HasComponent(Lookup<TScope, TComp>.Id);
        }
    }
}
