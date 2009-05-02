#if MOSAPROJECT

using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using Mono.Xml;

namespace Mono.Security.Cryptography
{
#if INSIDE_CORLIB
	internal
#else
	public 
#endif
	partial class KeyPairPersistence
	{
		internal static bool _CanSecure (string root)
		{
			throw new System.NotImplementedException();
		}
		internal static bool _ProtectUser (string path)
		{
			throw new System.NotImplementedException();
		}
		internal static bool _ProtectMachine (string path)
		{
			throw new System.NotImplementedException();
		}
		internal static bool _IsUserProtected (string path)
		{
			throw new System.NotImplementedException();
		}
		internal static bool _IsMachineProtected (string path)
		{
			throw new System.NotImplementedException();
		}

	}
}

#endif
