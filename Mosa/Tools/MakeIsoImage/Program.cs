/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.MakeIsoImage
{
    class Program
    {
        static void Main(string[] args)
        {
            //var f = new System.IO.FileInfo("C:\\cvs\\r3utils\\ParseIso\\SharpOS.iso");
            //f.Attributes

            // TODO FIXME - support remappings something like -map boot/boot.bin=c:/muos/build/debug/bin/iso9660_boot.bin
#if false
            var test = new Mosa.ClassLib.Iso9660Generator(false);
            test.AddFile("Long File Name.txt",new System.IO.FileInfo("C:\\cvs\\mosa\\Mosa\\Tools\\MakeIsoImage\\bin\\Debug\\Long File Name.txt"));
            test.Generate("Iso9660Generator.iso");
            return;
#endif

            int i;
            var iso = new Mosa.ClassLib.Iso9660Generator(false);
            for (i = 0; i < args.Length; i++)
            {
                if ( args[i].Trim()[0] != '-' )
                    break;
                switch (args[i].Trim())
                {
                    case "-boot":
                        string name = args[++i];
                        iso.AddBootFile(name, new System.IO.FileInfo(name));
                        break;
                    case "-boot-load-size":
                        short boot_load_size;
                        if ( short.TryParse(args[++i], out boot_load_size) )
                            iso.BootLoadSize ( boot_load_size );
                        break;
                    case "-boot-info-table":
                        iso.SetBootInfoTable(true);
                        break;
                    case "-label":
                        i++;
                        string s = args[i];
                        iso.SetVolumeLabel(s);
                        break;
                    default:
                        break;
                }
            }
            // at this point, args[i] should be our iso image name
            if (i >= args.Length)
            {
                Console.Write("Missing iso file name");
                return;
            }
            string iso_file_name = args[i++];
            // now args[i] is root folder
            if (i >= args.Length)
            {
                Console.Write("Missing root folder");
                return;
            }
            while (i < args.Length)
            {
                AddDirectoryTree(iso,args[i++],"");
            }
            iso.Generate(iso_file_name);
        }
        static private void AddDirectoryTree ( Mosa.ClassLib.Iso9660Generator iso, string root, string virtual_prepend )
        {
            int i;
            var dirinfo = new System.IO.DirectoryInfo(root.Replace('/','\\'));
            System.IO.FileInfo[] files = dirinfo.GetFiles();
            for (i = 0; i < files.Length; i++)
            {
                iso.AddFile(virtual_prepend + files[i].Name, files[i]);
            }
            System.IO.DirectoryInfo[] dirs = dirinfo.GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                iso.MkDir(virtual_prepend + dirs[i].Name);
                AddDirectoryTree(iso, root + '/' + dirs[i].Name, virtual_prepend + dirs[i].Name + '/');
            }
        }
    }
}
