// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Tool.GDBDebugger.GDB
{
	public class MemoryCache
	{
		public const int BlockSize = 0x800;

		public Connector Connector { get; private set; }

		private readonly Dictionary<ulong, byte[]> buffer = new Dictionary<ulong, byte[]>();
		private readonly HashSet<ulong> requested = new HashSet<ulong>();
		private readonly HashSet<ulong> received = new HashSet<ulong>();
		private readonly List<Request> requests = new List<Request>();
		private readonly object sync = new object();

		private class Request
		{
			public ulong Address { get; private set; }
			public uint Size { get; private set; }
			public OnMemoryRead OnMemoryRead { get; private set; }

			public List<ulong> Blocks { get; } = new List<ulong>();

			public Request(ulong address, uint size, OnMemoryRead onMemoryRead)
			{
				Address = address;
				Size = size;
				OnMemoryRead = onMemoryRead;
			}
		}

		public MemoryCache(Connector connector)
		{
			Connector = connector;
		}

		public void Clear()
		{
			lock (sync)
			{
				buffer.Clear();
				requests.Clear();
				requested.Clear();
				received.Clear();
			}
		}

		public void ReadMemory(ulong address, uint size, OnMemoryRead onMemoryRead)
		{
			var request = new Request(address, size, onMemoryRead);

			var start = Alignment.AlignDown(address, BlockSize);
			var end = Alignment.AlignUp(address + (ulong)size, BlockSize);

			var queries = new List<ulong>();

			lock (sync)
			{
				requests.Add(request);

				for (var i = start; i < end; i = i + BlockSize)
				{
					if (!requested.Contains(i))
					{
						requested.Add(i);

						//Connector.ReadMemory(i, BlockSize, OnMemoryRead);
						queries.Add(i);
					}

					if (!received.Contains(i))
					{
						request.Blocks.Add(i);
					}
				}
			}

			foreach (var q in queries)
			{
				Connector.ReadMemory(q, BlockSize, OnMemoryRead);
			}

			Process();
		}

		private void OnMemoryRead(ulong address, byte[] bytes)
		{
			lock (sync)
			{
				if (!requested.Contains(address))
					return;

				if (received.Contains(address))
					return;

				received.Add(address);
				buffer.Add(address, bytes);

				foreach (var request in requests)
				{
					lock (request)
					{
						request.Blocks.Remove(address);
					}
				}
			}

			Process();
		}

		private void Process()
		{
			lock (sync)
			{
				foreach (var request in requests.ToArray())
				{
					Process(request);
				}
			}
		}

		private void Process(Request request)
		{
			lock (sync)
			{
				if (request.Blocks.Count != 0)
					return;

				requests.Remove(request);

				var data = new byte[request.Size];

				// blocks start/end
				ulong start = Alignment.AlignDown(request.Address, BlockSize);
				ulong end = Alignment.AlignUp(request.Address + (ulong)request.Size, BlockSize);

				ulong requestStart = request.Address;
				ulong requestEnd = request.Address + request.Size;

				lock (sync)
				{
					for (ulong blockStart = start; blockStart < end; blockStart += BlockSize)
					{
						ulong blockEnd = blockStart + BlockSize;

						int blockOffset = 0;
						int dataOffset = 0;

						ulong overlapStart = Math.Max(requestStart, blockStart);
						ulong overlapEnd = Math.Min(requestEnd, blockEnd);
						int len = (int)(overlapEnd - overlapStart);

						if (requestStart > blockStart)
						{
							blockOffset = (int)(requestStart - blockStart);
						}

						if (blockStart > requestStart)
						{
							dataOffset = (int)(blockStart - requestStart);
						}

						var block = buffer[blockStart];

						try
						{
							Array.Copy(block, blockOffset, data, dataOffset, len);
						}
						catch (Exception e)
						{
							Debug.WriteLine(e.ToString());
						}
					}
				}

				request.OnMemoryRead(request.Address, data);
			}
		}
	}
}
