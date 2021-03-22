public interface IPerformanceTest {

    int Iterations { get; }
    void Before();
    void Run();
}
