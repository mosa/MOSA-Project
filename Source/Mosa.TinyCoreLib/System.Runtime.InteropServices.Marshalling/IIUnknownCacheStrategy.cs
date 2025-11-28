namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public interface IIUnknownCacheStrategy
{
	public readonly struct TableInfo
	{
		private readonly object _dummy;

		private readonly int _dummyPrimitive;

		public RuntimeTypeHandle ManagedType
		{
			get
			{
				throw null;
			}
			init
			{
			}
		}

		public unsafe void** Table
		{
			get
			{
				throw null;
			}
			init
			{
			}
		}

		public unsafe void* ThisPtr
		{
			get
			{
				throw null;
			}
			init
			{
			}
		}
	}

	void Clear(IIUnknownStrategy unknownStrategy);

	unsafe TableInfo ConstructTableInfo(RuntimeTypeHandle handle, IIUnknownDerivedDetails interfaceDetails, void* ptr);

	bool TryGetTableInfo(RuntimeTypeHandle handle, out TableInfo info);

	bool TrySetTableInfo(RuntimeTypeHandle handle, TableInfo info);
}
