using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework
{
    public static class Code
    {
        public const string ObjectClassDefinition = @"
            namespace System
            {
                public class Object
                {
                    public Object()
                    {
                    }

                    public virtual int GetHashCode()
                    {
                        return 0;
                    }

                    public virtual string ToString()
                    {
                        return null;
                    }

                    public virtual bool Equals(object obj)
                    {
                        return true;
                    }

                    public virtual void Finalize()
                    {
                    }
                }
            }
        ";
    }
}
