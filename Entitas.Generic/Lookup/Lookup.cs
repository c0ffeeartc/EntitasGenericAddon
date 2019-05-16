namespace Entitas.Generic
{
    public class Lookup<TScope, TComponent> where TScope : IScope
    {
        public static int Id = -1;
    }
    public class Lookup<TScope> where TScope : IScope
    {
        public static int Id = -1;
    }
}