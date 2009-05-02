#if MOSAPROJECT

using System.IO;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Runtime.InteropServices;

namespace System
{
	public partial class Environment
	{
		public static int ExitCode
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}
#if NET_1_1
		static
#endif
		public bool HasShutdownStarted
		{
			get
			{
				throw new System.NotImplementedException();
			}
		}
		public static string MachineName {
			get
			{
				throw new System.NotImplementedException();
			}
		}
		public static string NewLine {
			get
			{
				throw new System.NotImplementedException();
			}
		}
		internal static PlatformID Platform {
			get
			{
				throw new System.NotImplementedException();
			}
		}
		internal static string GetOSVersionString ()
		{
			throw new System.NotImplementedException();
		}
		public static int TickCount {
			get
			{
				throw new System.NotImplementedException();
			}
		}
		public static string UserName {
			get
			{
				throw new System.NotImplementedException();
			}
		}
		public static void Exit (int exitCode)
		{
			throw new System.NotImplementedException();
		}
		public static string[] GetCommandLineArgs ()
		{
			throw new System.NotImplementedException();
		}
		internal static string internalGetEnvironmentVariable (string variable)
		{
			throw new System.NotImplementedException();
		}
		private static string GetWindowsFolderPath (int folder)
		{
			throw new System.NotImplementedException();
		}
		private static void internalBroadcastSettingChange ()
		{
			throw new System.NotImplementedException();
		}
		internal static void InternalSetEnvironmentVariable (string variable, string value)
		{
			throw new System.NotImplementedException();
		}
		public static int ProcessorCount {
			get
			{
				throw new System.NotImplementedException();
			}
		}
		internal static string internalGetGacPath ()
		{
			throw new System.NotImplementedException();
		}
		private static string [] GetLogicalDrivesInternal ()
		{
			throw new System.NotImplementedException();
		}
		private static string [] GetEnvironmentVariableNames ()
		{
			throw new System.NotImplementedException();
		}
		internal static string GetMachineConfigPath ()
		{
			throw new System.NotImplementedException();
		}
		internal static string internalGetHome ()
		{
			throw new System.NotImplementedException();
		}

	}
}

#endif
