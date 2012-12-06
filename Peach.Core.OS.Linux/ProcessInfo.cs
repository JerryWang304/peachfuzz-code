using System;
using System.IO;
using System.Diagnostics;

namespace Peach.Core
{
	[PlatformImpl(Platform.OS.Linux)]
	public class ProcessInfoImpl : IProcessInfo
	{
		private static string StatPath = "/proc/{0}/stat";

		private enum Fields : int
		{
			State = 0,
			UserTime = 11,
			KernelTime = 12,
			Max = 13,
		}

		private static string[] ReadProc(int pid)
		{
			string stat;

			try
			{
				stat = File.ReadAllText(string.Format(StatPath, pid));
			}
			catch (Exception ex)
			{
				return null;
			}

			int start = stat.IndexOf('(');
			int end = stat.LastIndexOf(')');

			if (stat.Length < 2 || start < 0 || end < start)
				return null;

			string before = stat.Substring(0, start);
			string middle = stat.Substring(start + 1, end - start - 1);
			string after = stat.Substring(end + 1);

			string[] strPid = before.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (strPid.Length != 1 || strPid[0] != pid.ToString())
				return null;

			string[] parts = after.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length < (int)Fields.Max)
				return null;

			return parts;
		}

		public ProcessInfo Snapshot(Process p)
		{
			var parts = ReadProc(p.Id);
			if (parts == null)
				throw new InvalidOperationException();

			ProcessInfo pi = new ProcessInfo();

			pi.Id = p.Id;
			pi.ProcessName = p.ProcessName;
			pi.Responding = parts[(int)Fields.State] != "Z";

			pi.UserProcessorTime = TimeSpan.FromTicks(long.Parse(parts[(int)Fields.UserTime]));
			pi.PrivilegedProcessorTime = TimeSpan.FromTicks(long.Parse(parts[(int)Fields.KernelTime]));
			pi.TotalProcessorTime = pi.UserProcessorTime + pi.PrivilegedProcessorTime;

			pi.PrivateMemorySize64 = p.PrivateMemorySize64;         // /proc/[pid]/status VmData
			pi.VirtualMemorySize64 = p.VirtualMemorySize64;         // /proc/[pid]/status VmSize
			pi.PeakVirtualMemorySize64 = p.PeakVirtualMemorySize64; // /proc/[pid]/status VmPeak
			pi.WorkingSet64 = p.WorkingSet64;                       // /proc/[pid]/status VmRSS
			pi.PeakWorkingSet64 = p.PeakWorkingSet64;               // /proc/[pid]/status VmHWM

			return pi;
		}
	}

}