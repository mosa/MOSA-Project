// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.Compiler.Framework
{
	public class MethodScanner
	{
		public bool IsEnabled { get; set; }

		private Compiler Compiler { get; }
		private HashSet<MosaType> allocatedTypes = new HashSet<MosaType>();
		private HashSet<MosaMethod> invokedMethods = new HashSet<MosaMethod>();

		private object _lock = new object();

		public MethodScanner(Compiler compiler)
		{
			Compiler = compiler;
			IsEnabled = compiler.CompilerOptions.EnableMethodScanner;

			Initialize();
		}

		public void TypeAllocated(MosaType type)
		{
			if (!IsEnabled)
				return;

			lock (allocatedTypes)
			{
				if (allocatedTypes.Contains(type))
					return;

				allocatedTypes.Add(type);

				Debug.WriteLine("New Type Allocated: " + type.FullName);

				// find all invoked methods for this type
				lock (_lock)
				{
					foreach (var method in type.Methods)
					{
						if (invokedMethods.Contains(method))
						{
							ScheduleMethod(method);
						}
					}

					foreach (var property in type.Properties)
					{
						if (property.GetterMethod != null && invokedMethods.Contains(property.GetterMethod))
						{
							ScheduleMethod(property.GetterMethod);
						}

						if (property.SetterMethod != null && invokedMethods.Contains(property.SetterMethod))
						{
							ScheduleMethod(property.SetterMethod);
						}
					}
				}
			}
		}

		public void MethodInvoked(MosaMethod method)
		{
			if (!IsEnabled)
				return;

			lock (_lock)
			{
				if (invokedMethods.Contains(method))
					return;

				invokedMethods.Add(method);

				if (method.IsStatic)
				{
					ScheduleMethod(method);
				}
			}
		}

		private void ScheduleMethod(MosaMethod method)
		{
			Debug.WriteLine("Scanner Scheduling: " + method.ToString());
			Compiler.CompilationScheduler.Schedule(method);
		}

		public void Initialize()
		{
			var entryPoint = Compiler.TypeSystem.EntryPoint;

			if (entryPoint != null)
			{
				invokedMethods.Add(entryPoint);
				ScheduleMethod(entryPoint);
			}

			//foreach (var type in Compiler.TypeSystem.AllTypes)
			//{
			//	foreach (var method in type.Methods)
			//	{
			//		// TODO
			//	}
			//}
		}
	}
}
