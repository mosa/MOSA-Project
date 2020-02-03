// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Dynamic;
using System.Linq;
using System.Text.Json;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class DynamicJsonObject : DynamicObject
	{
		private readonly JsonElement _element;

		public DynamicJsonObject(JsonElement element)
		{
			_element = element;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (!_element.TryGetProperty(binder.Name, out JsonElement value))
			{
				// return null to avoid exception.  caller can check for null this way...
				result = null;
				return true;
			}

			result = WrapResultObject(value);
			return true;
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			if (indexes.Length == 1 && indexes[0] != null)
			{
				if (!_element.TryGetProperty(indexes[0].ToString(), out JsonElement value))
				{
					// return null to avoid exception.  caller can check for null this way...
					result = null;
					return true;
				}

				result = WrapResultObject(value);
				return true;
			}

			return base.TryGetIndex(binder, indexes, out result);
		}

		private static object WrapResultObject(JsonElement value)
		{
			switch (value.ValueKind)
			{
				case JsonValueKind.Array:
					return value.EnumerateArray()
						.Select(x => WrapResultObject(x))
						.ToList();

				case JsonValueKind.Object:
					return new DynamicJsonObject(value);

				case JsonValueKind.False:
				case JsonValueKind.True:
					return value.GetBoolean();

				case JsonValueKind.String:
					return value.GetString();

				case JsonValueKind.Number:
					return value.GetInt64();

				case JsonValueKind.Null:
				case JsonValueKind.Undefined:
					return null;

				default:
					return value;
			}
		}
	}
}
