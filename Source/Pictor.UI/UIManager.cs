using System;

namespace Pictor.UI
{
	public static class UIManager
	{
		private static System.Reflection.Assembly theme = null;

		public static ButtonWidget CreateButton(double x, double y, string lable)
		{
			if (null != theme)
			{
				object holder = Activator.CreateInstance(theme.GetType("Pictor.Theme"));
				object result = theme.GetType("Pictor.Theme").InvokeMember("CreateButton", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, holder, new object[] { x, y, lable });
				return result as ButtonWidget;
				//return (ButtonWidget)theme.GetModule("Pictor.Theme").GetMethod("CreateButton", new Type[] { typeof(double), typeof(double), typeof(string) }).Invoke(null, new object[] { x, y, lable });
			}
			else
				return new ButtonWidget(x, y, lable);
		}

		public static ButtonWidget CreateButton(double x, double y, string lable,
			double textHeight, double textPadding, double borderWidth, double borderRadius)
		{
			if (null != theme)
			{
				object holder = Activator.CreateInstance(theme.GetType("Pictor.Theme"));
				object result = theme.GetType("Pictor.Theme").InvokeMember("CreateButton", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, holder, new object[] { x, y, lable, textHeight, textPadding, borderWidth, borderRadius });
				return result as ButtonWidget;
				//return (ButtonWidget)theme.GetModule("Pictor.Theme").GetMethod("CreateButton", new Type[] { typeof(double), typeof(double), typeof(string) }).Invoke(null, new object[] { x, y, lable });
			}
			else
				return new ButtonWidget(x, y, lable, textHeight, textPadding, borderWidth, borderRadius);
		}

		public static void LoadTheme(string name)
		{
			string cwd = System.AppDomain.CurrentDomain.BaseDirectory;
			string file = cwd + "Pictor.Theme." + name + ".dll";
			theme = System.Reflection.Assembly.LoadFile(file);
		}
	}
}
