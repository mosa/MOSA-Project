// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Workspace.Experiment.Debug
{
	public class YamlQuery2
	{
		private object yamlDic;
		private string key;
		private object current;

		public YamlQuery2(object yamlDic)
		{
			this.yamlDic = yamlDic;
		}

		public YamlQuery2 On(string key)
		{
			this.key = key;
			this.current = query<object>(this.current ?? this.yamlDic, this.key, null);
			return this;
		}

		public YamlQuery2 Get(string prop)
		{
			if (this.current == null)
				throw new InvalidOperationException();

			this.current = query<object>(this.current, null, prop, this.key);
			return this;
		}

		public List<T> ToList<T>()
		{
			if (this.current == null)
				throw new InvalidOperationException();

			return (this.current as List<object>).Cast<T>().ToList();
		}

		private IEnumerable<T> query<T>(object _dic, string key, string prop, string fromKey = null)
		{
			var result = new List<T>();
			if (_dic == null)
				return result;
			if (typeof(IDictionary<object, object>).IsAssignableFrom(_dic.GetType()))
			{
				var dic = (IDictionary<object, object>)_dic;
				var d = dic.Cast<KeyValuePair<object, object>>();

				foreach (var dd in d)
				{
					if (dd.Key as string == key)
					{
						if (prop == null)
						{
							result.Add((T)dd.Value);
						}
						else
						{
							result.AddRange(query<T>(dd.Value, key, prop, dd.Key as string));
						}
					}
					else if (fromKey == key && dd.Key as string == prop)
					{
						result.Add((T)dd.Value);
					}
					else
					{
						result.AddRange(query<T>(dd.Value, key, prop, dd.Key as string));
					}
				}
			}
			else if (typeof(IEnumerable<object>).IsAssignableFrom(_dic.GetType()))
			{
				var t = (IEnumerable<object>)_dic;
				foreach (var tt in t)
				{
					result.AddRange(query<T>(tt, key, prop, key));
				}
			}
			return result;
		}
	}
}
