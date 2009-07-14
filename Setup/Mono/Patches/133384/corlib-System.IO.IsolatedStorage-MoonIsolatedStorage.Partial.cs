#if NET_2_1

#if MOSAPROJECT

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace System.IO.IsolatedStorage
{
	 partial class IsolatedStorage
	{
		static long isolated_storage_get_current_usage (string root)
		{
			throw new System.NotImplementedException();
		}

	}
}

#endif

#endif

