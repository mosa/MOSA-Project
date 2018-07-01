// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Web.Script.Serialization;

public sealed class DynamicJsonConverter : JavaScriptConverter
{
	public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
	{
		if (dictionary == null)
			throw new ArgumentNullException(nameof(dictionary));

		return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
	}

	public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) => throw new NotImplementedException();

	public override IEnumerable<Type> SupportedTypes
	{
		get { return new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) })); }
	}

	#region Nested type: DynamicJsonObject

	private sealed class DynamicJsonObject : DynamicObject
	{
		private readonly IDictionary<string, object> _dictionary;

		public DynamicJsonObject(IDictionary<string, object> dictionary)
		{
			_dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (!_dictionary.TryGetValue(binder.Name, out result))
			{
				// return null to avoid exception.  caller can check for null this way...
				result = null;
				return true;
			}

			result = WrapResultObject(result);
			return true;
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			if (indexes.Length == 1 && indexes[0] != null)
			{
				if (!_dictionary.TryGetValue(indexes[0].ToString(), out result))
				{
					// return null to avoid exception.  caller can check for null this way...
					result = null;
					return true;
				}

				result = WrapResultObject(result);
				return true;
			}

			return base.TryGetIndex(binder, indexes, out result);
		}

		private static object WrapResultObject(object result)
		{
			if (result is IDictionary<string, object> dictionary)
				return new DynamicJsonObject(dictionary);

			var arrayList = result as ArrayList;
			if (arrayList?.Count > 0)
			{
				return arrayList[0] is IDictionary<string, object>
					? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)))
					: new List<object>(arrayList.Cast<object>());
			}

			return result;
		}
	}

	#endregion Nested type: DynamicJsonObject
}
