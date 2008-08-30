/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Mosa.ObjectFiles.Elf32.Format;
using Mosa.ObjectFiles.Elf32.Format.Sections;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;

namespace Mosa.ObjectFiles.Elf32
{
    public class Elf32ObjectFileBuilder : ObjectFileBuilderBase
    {
        Semaphore _assemblySync = new Semaphore(1, 1);
        Semaphore _methodSync = new Semaphore(1, 1);
        Elf32MachineKind _machineKind;
        string _currentFilename;
        Elf32File _currentFile;

        static Elf32ObjectFileBuilder()
        {
        }

        public Elf32ObjectFileBuilder(Elf32MachineKind machineType)
        {
            _machineKind = machineType;
        }

        public override string Name { get { return "Elf32 Object File Writer"; } }

        public override void OnAssemblyCompileBegin(AssemblyCompiler compiler)
        {
            _assemblySync.WaitOne();
            _currentFilename = Path.ChangeExtension(compiler.Assembly.Name, ".o");
            _currentFile = new Elf32File(_machineKind);
            _currentFile.SymbolTable.OnCreateSymbol += new CreateSymbolHandler(SymbolTable_OnCreateSymbol);
        }

        public override void OnAssemblyCompileEnd(AssemblyCompiler compiler)
        {
            ApplyLinks();
            using (Stream outputStream = File.Open(_currentFilename, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(outputStream))
            {
                _currentFile.Write(writer);
            }
            _assemblySync.Release();
        }

        void SymbolTable_OnCreateSymbol(Elf32Symbol symbol)
        {
            if (symbol.Tag is RuntimeMethod)
            {
                RuntimeMethod method = (RuntimeMethod)symbol.Tag;
                symbol.Name = method.ToString();
                symbol.Type = Elf32SymbolType.STT_FUNC;
                symbol.Bind = Elf32SymbolBinding.STB_GLOBAL;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void OnMethodCompileBegin(MethodCompilerBase compiler)
        {
            _methodSync.WaitOne();
            Elf32Symbol sym = _currentFile.SymbolTable.GetSymbol(compiler.Method);
            _currentFile.Code.BeginSymbol(sym);
        }

        public override void OnMethodCompileEnd(MethodCompilerBase compiler)
        {
            MemoryStream codeStream = compiler.RequestCodeStream() as MemoryStream;
            if (null == codeStream)
            {
                throw new NotSupportedException("Elf32ObjectFileBuilder requires a MemoryStream for native code");
            }
            Elf32Symbol sym = _currentFile.SymbolTable.GetSymbol(compiler.Method);
            _currentFile.Code.EndSymbol(
                sym,
                codeStream.GetBuffer(),
                0,
                (int)codeStream.Length
            );
            _methodSync.Release();
        }

        List<LinkRequest> linkRequests = new List<LinkRequest>();

        public override long Link(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, RuntimeMember target)
        {
            LinkRequest link = new LinkRequest(
                linkType,
                method,
                methodOffset,
                methodRelativeBase,
                target
            );

            long value;
            if (TryResolveLink(link, out value))
            {
                return value;
            }
            else
            {
                linkRequests.Add(link);
                return 0;
            }
        }
        private void ApplyLinks()
        {
            for (int i = 0; i < linkRequests.Count; i++)
            {
                LinkRequest link = linkRequests[i];
                long value;
                if (TryResolveLink(link, out value))
                {
                    ApplyPatch(link, value);
                    linkRequests.RemoveAt(i);
                    i--;
                }
            }
            Elf32RelocationSection codeRelocs = _currentFile.CodeRelocations;
            if (linkRequests.Count == 0)
            {
                _currentFile.Sections.Remove(codeRelocs);
            }
            else
            {
                foreach (LinkRequest link in linkRequests)
                {
                    Elf32Symbol methodSym = _currentFile.SymbolTable.GetSymbol(link.Method);
                    Elf32Symbol targetSym = _currentFile.SymbolTable.GetSymbol(link.Target);
                    codeRelocs.Add(
                        link.LinkType,
                        methodSym.Offset + link.MethodOffset,
                        link.MethodOffset - link.MethodRelativeBase,
                        targetSym
                    );
                }
            }
        }
        private bool TryResolveLink(LinkRequest link, out long value)
        {
            Elf32Symbol methodSym = _currentFile.SymbolTable.GetSymbol(link.Method);
            Elf32Symbol targetSym = _currentFile.SymbolTable.GetSymbol(link.Target);

            if (methodSym.IsDefined && targetSym.IsDefined)
            {
                switch (link.LinkType & LinkType.KindMask)
                {
                    case LinkType.RelativeOffset:
                        value = targetSym.Offset - (methodSym.Offset + link.MethodRelativeBase);
                        break;
                    case LinkType.AbsoluteAddress:
                        value = targetSym.Offset;
                        break;
                    default: throw new NotSupportedException();
                }
                switch (link.LinkType & LinkType.SizeMask)
                {
                    case LinkType.I1: value = (sbyte)value; return true;
                    case LinkType.I2: value = (short)value; return true;
                    case LinkType.I4: value = (int)value; return true;
                    case LinkType.I8: return true;
                    default: throw new NotSupportedException();
                }
            }
            else
            {
                value = 0;
                return false;
            }
        }
        private void ApplyPatch(LinkRequest link, long value)
        {
            Elf32Symbol methodSym = _currentFile.SymbolTable.GetSymbol(link.Method);

            switch (link.LinkType & LinkType.SizeMask)
            {
                case LinkType.I1:
                    methodSym.Section.ApplyPatch(
                        methodSym.Offset + link.MethodOffset,
                        (sbyte)value
                    );
                    break;
                case LinkType.I2:
                    methodSym.Section.ApplyPatch(
                        methodSym.Offset + link.MethodOffset,
                        (short)value
                    );
                    break;
                case LinkType.I4:
                    methodSym.Section.ApplyPatch(
                        methodSym.Offset + link.MethodOffset,
                        (int)value
                    );
                    break;
                case LinkType.I8:
                    methodSym.Section.ApplyPatch(
                        methodSym.Offset + link.MethodOffset,
                        (long)value
                    );
                    break;
                default: throw new NotSupportedException();
            }
        }
    }
}
