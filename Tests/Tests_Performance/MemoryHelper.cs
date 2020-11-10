using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tests {
  public class MemoryHelper {
    public static List<long> GetMemoryStats(Process process) {
      return new List<long> {
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

    public static string GetMemoryAllStatsString(Process process) {
      return String.Join(", ", GetMemoryStats(process));
    }
  }
}