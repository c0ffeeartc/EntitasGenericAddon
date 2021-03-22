using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Threading;

namespace PerformanceTests
{
public class MemoryHelper
{
	public static Process CurProcess;

	public static string GetMemoryAllStatsString(Process process)
	{
	  return String.Join(", ", GetMemoryStats(process));
	}

	public static List<long> GetMemoryStats(Process process)
	{
		return new List<long>
		{
			GC.GetTotalMemory(false),
			process.PeakVirtualMemorySize64,
			process.PeakPagedMemorySize64,
			process.VirtualMemorySize64,
			process.PrivateMemorySize64,
			process.PagedMemorySize64,
			process.NonpagedSystemMemorySize64,
			process.PagedSystemMemorySize64,
		};
	}

	public static void MemoryBegin( )
	{
		CurProcess = Process.GetCurrentProcess();
		Console.WriteLine($"Memory at process start: {GetMemoryAllStatsString(CurProcess)}");

		Thread.Sleep(1500);  // too much?

		Console.WriteLine($"Memory before tests: {GetMemoryAllStatsString(CurProcess)}");

		GCSettings.LatencyMode = GCLatencyMode.LowLatency;

		try
		{
			if ( !GC.TryStartNoGCRegion( 0 ) )
			{
				Console.WriteLine( "Can't start NoGCRegion" );
			}
		}
		catch ( Exception ex )
		{
			Console.WriteLine( "Can't start NoGCRegion. " + ex.Message );
		}
		finally
		{
			Console.WriteLine( "" );
		}
	}

	public static void MemoryEnd( )
	{
		Console.WriteLine( $"\nMemory after tests: {GetMemoryAllStatsString( CurProcess )}" );

		try
		{
			GC.EndNoGCRegion( );
		}
		catch ( System.InvalidOperationException e )
		{
			Console.WriteLine( "\nMemory validation needs to be improved... " +
				"\nProcess had to clean memory while in NoGC mode" +
				"\nNegative memory diffs indicate that" );
			throw;
		}
		catch ( Exception ex )
		{
			Console.WriteLine( "Can't start EndNoGCRegion. " + ex.Message );
		}
	}
  }
}
