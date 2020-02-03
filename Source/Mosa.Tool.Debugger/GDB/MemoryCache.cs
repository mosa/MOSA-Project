// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Mosa.Tool.Debugger.GDB
{
	public class MemoryCache
	{
		public const int BlockSize = 0x800;

		public Connector Connector { get; }

		private readonly Dictionary<ulong, byte[]> buffer = new Dictionary<ulong, byte[]>();
		private readonly HashSet<ulong> requested = new HashSet<ulong>();
		private readonly HashSet<ulong> received = new HashSet<ulong>();
		private readonly List<Request> requests = new List<Request>();
		private readonly object sync = new object();

		private class Request
		{
			public ulong Address { get; }
			public uint Size { get; }
			public OnMemoryRead OnMemoryRead { get; }

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
			ThreadPool.QueueUserWorkItem(state => { ReadMemoryInternal(address, size, onMemoryRead); });
		}

		private void ReadMemoryInternal(ulong address, uint size, OnMemoryRead onMemoryRead)
		{
			var request = new Request(address, size, onMemoryRead);

			var start = Alignment.AlignDown(address, BlockSize);
			var end = Alignment.AlignUp(address + size, BlockSize);

			var queries = new List<ulong>();

			lock (sync)
			{
				requests.Add(request);

				for (var i = start; i < end; i += BlockSize)
				{
					if (!requested.Contains(i))
					{
						requested.Add(i);

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

				// blocks start/end
				ulong start = Alignment.AlignDown(request.Address, BlockSize);
				ulong end = Alignment.AlignUp(request.Address + request.Size, BlockSize);

				ulong requestStart = request.Address;
				ulong requestEnd = request.Address + request.Size;

				var data = new byte[request.Size];

				for (ulong blockStart = start; blockStart < end; blockStart += BlockSize)
				{
					ulong blockEnd = blockStart + BlockSize;

					int blockOffset = 0;
					int dataOffset = 0;

					ulong overlapStart = Math.Max(requestStart, blockStart);
					ulong overlapEnd = Math.Min(requestEnd, blockEnd);
					int len = (int)(overlapEnd - overlapStart);

					if (len <= 0)
						continue;

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
						System.Diagnostics.Debug.WriteLine(e.ToString());
					}
				}

				ThreadPool.QueueUserWorkItem(state => { request.OnMemoryRead(request.Address, data); });
			}
		}
	}
}
