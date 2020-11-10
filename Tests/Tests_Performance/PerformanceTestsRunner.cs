using System;
using System.Diagnostics;
using Tests;

public static class PerformanceTestRunner
{
	static readonly Stopwatch _stopwatch;

	static PerformanceTestRunner() {
		_stopwatch = new Stopwatch();
	}

	public static Tuple<long,long> Run(IPerformanceTest test) {
		test.Before();
		_stopwatch.Reset();
    var currentProcess = Process.GetCurrentProcess();
    var bytesBefore = MemoryHelper.GetMemoryStats(currentProcess);
    _stopwatch.Start();
		test.Run();
		_stopwatch.Stop();
    var bytesAfter = MemoryHelper.GetMemoryStats(currentProcess);
    // only GC memory shows some differences...
    long memoryDiff = bytesAfter[0] - bytesBefore[0];
    return new Tuple<long, long>(_stopwatch.ElapsedMilliseconds, memoryDiff);
	}
}
