using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Dynamo
{
    public class DynamicCodeManager
    {
        #region internal constants
        Dictionary<string, string> _conditionSnippet = new Dictionary<string, string>();
        Dictionary<string, string> _methodSnippet = new Dictionary<string, string>();
        string CodeStart = "using System;\r\nusing System.Collections.Generic;\r\n//using System.Linq;\r\nusing System.Text;\r\nusing System.Data;\r\nusing System.Reflection;\r\nusing System.CodeDom.Compiler;\r\nusing Microsoft.CSharp;\r\nusing System.Drawing;\r\nnamespace Dynamo\r\n{\r\n  public class Dynamic \r\n  {\r\n";
        string DynamicConditionPrefix = "__dm_";
        string ConditionTemplate = "  public bool {0}{1}(params object[] p) {{ return {2}; }}\r\n";
        string MethodTemplate = "   public dynamic {0}(params object[] p) {{\r\n{1}\r\n    }}\r\n";
        string CodeEnd = "  }\r\n}";
        List<string> _references = new List<string>("System.dll,System.Core.dll,System.Data.dll,System.Xml.dll,System.Windows.Forms.dll,System.Drawing.dll,Microsoft.CSharp.dll".Split(new char[] { ',' }));
        Assembly _assembly = null;
        #endregion

        public Assembly Assembly { get { return _assembly; } }

        #region manage snippets
        public void Clear()
        {
            _methodSnippet.Clear();
            _conditionSnippet.Clear();
            _assembly = null;
        }
        public void Clear(string name)
        {
            if (_conditionSnippet.ContainsKey(name))
            {
                _assembly = null;
                _conditionSnippet.Remove(name);
            }
            else if (_methodSnippet.ContainsKey(name))
            {
                _assembly = null;
                _methodSnippet.Remove(name);
            }
        }

        public void AddCondition(string conditionName, string booleanExpression)
        {
            if (_conditionSnippet.ContainsKey(conditionName))
                throw new InvalidOperationException(string.Format("There is already a condition called '{0}'", conditionName));
            StringBuilder src = new StringBuilder(CodeStart);
            src.AppendFormat(ConditionTemplate, DynamicConditionPrefix, conditionName, booleanExpression);
            src.Append(CodeEnd);
            Compile(src.ToString()); //if the condition is invalid an exception will occur here
            _conditionSnippet[conditionName] = booleanExpression;
            _assembly = null;
        }

        public void AddMethod(string methodName, string methodSource)
        {
            if (_methodSnippet.ContainsKey(methodName))
                throw new InvalidOperationException(string.Format("There is already a method called '{0}'", methodName));
            if (methodName.StartsWith(DynamicConditionPrefix))
                throw new InvalidOperationException(string.Format("'{0}' is not a valid method name because the '{1}' prefix is reserved for internal use with conditions", methodName, DynamicConditionPrefix));
            StringBuilder src = new StringBuilder(CodeStart);
            src.AppendFormat(MethodTemplate, methodName, methodSource);
            src.Append(CodeEnd);
            Trace.TraceError("SOURCE\r\n{0}", src);
            Compile(src.ToString()); //if the condition is invalid an exception will occur here
            _methodSnippet[methodName] = methodSource;
            _assembly = null;
        }
        #endregion

        #region use snippets
        public dynamic getObj()
        {
            if (_assembly == null)
            {
                Compile();
            }
            return _assembly.CreateInstance("Dynamo.Dynamic");
        }
        public Assembly getAssembly()
        {
            if (_assembly == null)
            {
                Compile();
            }
            return _assembly;
        }

        #endregion

        #region support routines
        public string ProduceConditionName(Guid conditionId)
        {
            StringBuilder cn = new StringBuilder();
            foreach (char c in conditionId.ToString().ToCharArray()) if (char.IsLetterOrDigit(c)) cn.Append(c);
            string conditionName = cn.ToString();
            return string.Format("_dm_{0}", cn);
        }
        private void Compile()
        {
            if (_assembly == null)
            {
                StringBuilder src = new StringBuilder(CodeStart);
                foreach (KeyValuePair<string, string> kvp in _conditionSnippet)
                    src.AppendFormat(ConditionTemplate, DynamicConditionPrefix, kvp.Key, kvp.Value);
                foreach (KeyValuePair<string, string> kvp in _methodSnippet)
                    src.AppendFormat(MethodTemplate, kvp.Key, kvp.Value);
                src.Append(CodeEnd);
                Trace.TraceError("SOURCE\r\n{0}", src);
                _assembly = Compile(src.ToString());
            }
        }
        private Assembly Compile(string sourceCode)
        {
            CompilerParameters cp = new CompilerParameters();
            /*foreach (var v in _references.ToArray())
            {
                cp.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\" + v);
            }*/
            cp.ReferencedAssemblies.AddRange(_references.ToArray());
            var m = Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName;
            cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            cp.CompilerOptions = "/target:library /optimize";
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            CompilerResults cr = (new CSharpCodeProvider(new Dictionary<String, String> { { "CompilerVersion", "v4.0" } })).CompileAssemblyFromSource(cp, sourceCode);
            if (cr.Errors.Count > 0) throw new Exception(); // compilerexception cr.Errors
            return cr.CompiledAssembly;
        }
        #endregion

        public bool HasItem(string methodName)
        {
            return _conditionSnippet.ContainsKey(methodName) || _methodSnippet.ContainsKey(methodName);
        }
    }
}