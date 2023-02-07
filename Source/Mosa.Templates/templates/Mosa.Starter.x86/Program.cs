// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;

namespace Mosa.Starter.x86
{
    public static class Program
    {
        public static void Setup()
        {
            Screen.ResetColor();
            Screen.Clear();
            Screen.WriteLine("Hello World!");
        }

        public static void Loop()
        { }
    }
}
