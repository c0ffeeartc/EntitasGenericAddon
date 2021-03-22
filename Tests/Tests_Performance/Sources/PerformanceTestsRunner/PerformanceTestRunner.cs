using System;
using System.Diagnostics;
using System.Threading;
using Tests;

namespace PerformanceTests
{
public static class PerformanceTestRunner
{
	static readonly Stopwatch _stopwatch;

	static PerformanceTestRunner()
	{
		_stopwatch = new Stopwatch();
	}

	public static Tuple<long,long> Run(IPerformanceTest test)
	{
		test.Before();
		_stopwatch.Reset();
		var bytesBefore = MemoryHelper.GetMemoryStats(MemoryHelper.CurProcess);
		_stopwatch.Start();
		test.Run();
		_stopwatch.Stop();
		var bytesAfter = MemoryHelper.GetMemoryStats(MemoryHelper.CurProcess);
		// only GC memory shows some differences...
		long memoryDiff = bytesAfter[0] - bytesBefore[0];
		return new Tuple<long, long>(_stopwatch.ElapsedMilliseconds, memoryDiff);
	}

	public static void Log(string message = null)
	{
		Console.WriteLine(message ?? String.Empty);
	}

	public static void Run<T>()
			where T : class, IPerformanceTest, new()
	{
		Run(()=>new T());
	}

	public static void Run<T>(Func<T> testFactory)
			where T : class, IPerformanceTest
	{
		Thread.Sleep(200);//(500);

		var test = testFactory.Invoke();
		var testString = test is IToTestString t
			? t.ToTestString()
			: test.ToTestStringDefault();

		Console.Write( test.Iterations.ToString( "e0" ) + " " + (testString + ": ").PadRight(60));
		Run(test); // For more reliable results, run before

		Tuple<long,long> msAndMemory = Run(testFactory.Invoke());

		Console.Write((msAndMemory.Item1 + " ms").PadRight(10));
		Console.Write(("mem.DIFF " +msAndMemory.Item2 + " bytes").PadRight(30));
		Console.WriteLine("mem.ALL " + MemoryHelper.GetMemoryAllStatsString(MemoryHelper.CurProcess) + " bytes");
	}

	public static string ToTestStringDefault (this IPerformanceTest self)
	{
		return self.GetType().ToString();
	}
}
}
