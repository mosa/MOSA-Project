/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

#define ROCKRIDGE

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ClassLib
{
    /// <summary>
    /// Generates ISO9660 images
    /// </summary>
    public class Iso9660Generator
    {
        // http://en.wikipedia.org/wiki/ISO_9660
        // http://users.telenet.be/it3.consultants.bvba/handouts/ISO9960.html
        // ECMA-119 ( a.k.a. ISO-9660 ): http://www.ecma-international.org/publications/standards/Ecma-119.htm
        // System Use Sharing Protocol ( Rock Ridge uses this ): http://www.fordi.org/docs/IEEE%20P1281%20-%20System%20Use%20Sharing%20Protocol.pdf
        // Rock Ridge (system use field): http://www.fordi.org/docs/IEEE%20P1282%20-%20Rock%20Ridge%20Interchange%20Protocol.pdf
        // Joliet (UCS-2): http://bmrc.berkeley.edu/people/chaffee/jolspec.html
        // El Torito spec (bootable): http://www.phoenix.com/NR/rdonlyres/98D3219C-9CC9-4DF5-B496-A286D893E36A/0/specscdrom.pdf
        // El Torito supplement: http://littlesvr.ca/isomaster/eltoritosuppl.php

        // this class's current target is to automatically support Rock Ridge and El Torito. Eventually Joliet will be added I'm sure.
        // there's no real need to make them optional, as the specs are designed to be backwards compatible.

        // the only real optional item is the "pedantic" option. When enabled, I plan to force strict adherance to the ISO9660 file name
        // spec. Specifically:
        // * file names must have ";1" appended to them ( windows violates this )
        // * directory names cannot have "." in them ( some linux distros violate this )
        // * maximum directory depth is 8 ( I can't find any reason why this limitation should exist, and I think one of the specs above relax this anyway )
        // * (I'm thinking something else, too, haven't decided yet...)

        /// <summary>
        /// Constructor
        /// </summary>
        public Iso9660Generator(bool pedantic)
        {
            m_isoRoot = new IsoFolder();
            m_isoRoot.name = ".";
            m_pedantic = pedantic;
        }

        /// <summary>
        /// sets the volume label of the image
        /// </summary>
        /// <param name="volume_label">the requested volume label</param>
        public void SetVolumeLabel(string volume_label)
        {
            // TODO FIXME - fixup label to make sure it's compatible...
            int max_length = 72 - 41 + 1;
           
            m_volumeLabel = volume_label.Replace('.', '_');
            if (m_volumeLabel.Length > max_length)
                m_volumeLabel = m_volumeLabel.Substring(0, max_length);
        }

        /// <summary>
        /// create directory in ISO file
        /// </summary>
        public void MkDir(string path)
        {
            IsoFolder f = m_isoRoot;
            string[] ar = NormalizePath(path).Split('/');
            for (int i = 0; i < ar.Length; i++)
            {
                string key = ar[i].Trim().ToLower();
                if (!f.entries.ContainsKey(key))
                {
                    var subf = new IsoFolder();
                    subf.name = ar[i].Trim();
                    f.entries[key] = subf;
                }
                IsoEntry e = f.entries[key];
                if (e.IsFile)
                {
                    //throw new Exception("cannot create directory \"" + ar[i].Trim() + "\", a file by that name already exists");
                    return; // already exists - silently fail for now
                }
                f = (IsoFolder)e;
            }
        }

        /// <summary>
        /// add a "normal" file to the ISO ( not a boot file )
        /// </summary>
        public void AddFile(string sPath, System.IO.FileInfo fileInfo)
        {
            AddFileEx(NormalizePath(sPath), fileInfo);
        }

        /// <summary>
        /// add a boot file to the ISO
        /// TODO FIXME - add support for boot images other than x86
        /// </summary>
        public void AddBootFile(string sPath, System.IO.FileInfo fileInfo)
        {
            if (m_boot != null)
                throw new Exception("only one boot file can be added to an ISO");
            m_boot = AddFileEx(NormalizePath(sPath), fileInfo);
            m_boot.BootFile = true;
            m_boot.BootInfoTable = m_bootInfoTable;
        }

        /// <summary>
        /// sets the boot load size, see El Torito spec figure 3 offset 6-7
        /// </summary>
        /// <param name="bootLoadSize">the number of 512-byte sectors to load during boot</param>
        public void BootLoadSize(short bootLoadSize)
        {
            m_bootLoadSize = bootLoadSize;
        }

        /// <summary>
        /// enables support for a boot info table within the boot image
        /// </summary>
        /// <param name="bootInfoTable"></param>
        public void SetBootInfoTable(bool bootInfoTable)
        {
            m_bootInfoTable = bootInfoTable;
            if (m_boot != null)
                m_boot.BootInfoTable = bootInfoTable;
        }

        /// <summary>
        /// generate the iso image
        /// </summary>
        public void Generate(string sIsoFileName)
        {
            LogicalBlockSize = 2048;

            m_gen = new Generator(m_pedantic);
            GenerateIso(); // 1st pass to calculate offsets
            m_gen.ResetWithFileStream(System.IO.File.OpenWrite(sIsoFileName));
            GenerateIso(); // 2nd pass to actually write the data
        }

        private class IsoEntry
        {
            public string name;
            public int DataBlock;
            public int DataLength;
            public byte rr_flags;

            public IsoEntry()
            {
                DataBlock = 0;
                DataLength = 0;
                rr_flags = 0;
            }

            public virtual bool IsFile { get { return false; } }

            public virtual bool IsFolder { get { return false; } }
        }

        private class IsoFolder : IsoEntry
        {
            //public System.IO.DirectoryInfo dirInfo;
            public Dictionary<string,IsoEntry> entries;
            public short PathTableEntry;

            public IsoFolder()
            {
                entries = new Dictionary<string,IsoEntry>();
            }

            public void AddEntry(string path, IsoEntry e)
            {
                var tmp = path.IndexOf('/');
                if (tmp >= 0)
                {
                    string subpath = path.Substring(tmp + 1).Trim();
                    path = path.Substring(0, tmp).Trim();
                }
            }

            public override bool IsFolder { get { return true; } }
        }

        private class IsoFile : IsoEntry
        {
            [FlagsAttribute]
            public enum Flags
            {
                BootFile = 1<<0,
                BootInfoTable = 1<<1
            }
            public System.IO.FileInfo fileInfo;
            public Flags flags;

            public IsoFile(System.IO.FileInfo fi)
            {
                fileInfo = fi;
                flags = 0;
            }
            public bool BootFile
            {
                get 
                {
                    return (flags & Flags.BootFile) != 0;
                }
                set
                {
                    if (value)
                        flags |= Flags.BootFile;
                    else
                        flags &= ~Flags.BootFile;
                }
            }
            public bool BootInfoTable
            {
                get
                {
                    return (flags & Flags.BootInfoTable) != 0;
                }
                set
                {
                    if (value)
                        flags |= Flags.BootInfoTable;
                    else
                        flags &= ~Flags.BootInfoTable;
                }
            }

            public override bool IsFile { get { return true; } }
        }

        private IsoFolder m_isoRoot;
        private IsoFile m_boot;
        private bool m_pedantic;
        private string m_volumeLabel;
        private short m_bootLoadSize;
        private bool m_bootInfoTable;

        /// <summary>
        /// take a path and convert back-slashes to forward slashes and remove trailing slash if it exists
        /// </summary>
        private string NormalizePath(string path)
        {
            // first fix-up path separators
            path = path.Replace('\\','/').Trim();
            // check for os-specific leading characters and remove...
            if (path[1] == ':') // dos
                path = path.Substring(2);
            if (path[0] == '/') // dos and unix
                path = path.Substring(1);
            // remove trailing slash if necessary
            if (path[path.Length - 1] == '/')
                path = path.Substring(0, path.Length - 1);
            return path;
        }

        /// <summary>
        /// add a file to the ISO ( common implementation - called by AddFile() and AddBootFile() )
        /// </summary>
        private IsoFile AddFileEx(string path, System.IO.FileInfo fileInfo)
        {
            string key;
            string[] ar = NormalizePath(path).Split('/');
            int i;
            IsoFolder f = m_isoRoot;
            for (i = 0; i < ar.Length-1; i++)
            {
                key = ar[i].Trim().ToLower();
                if (!f.entries.ContainsKey(key))
                {
                    var subf = new IsoFolder();
                    subf.name = ar[i].Trim();
                    f.entries[key] = subf;
                }
                IsoEntry e = f.entries[key];
                if (e.IsFile)
                {
                    throw new Exception("cannot create directory \"" + ar[i].Trim() + "\", a file by that name already exists");
                    //return;
                }
                f = (IsoFolder)e;
            }
            var x = new IsoFile(fileInfo);
            x.name = ar[i].Trim();
            key = ar[i].Trim().ToLower();
            if (f.entries.ContainsKey(key))
            {
                //throw new Exception("file or folder by that name already exists");
                return (IsoFile)f.entries[key]; // just don't add it for now...
            }
            f.entries[key] = x;

            return x;
        }

        private Generator m_gen;
        private int TotalSize;
        private short LogicalBlockSize;
        //const private short VolumeSetSize; ( don't see a need to customize this value at this time )
        private int PathTableSize;
        private int LPathTable;
        private int MPathTable;
        private int BootCatalog;
        private int ContinuationBlock, ContinuationLength;
        private short NextPathTableEntry;
        private int PrimaryVolumeDescriptor;

        /// <summary>
        /// pass all the data to the Generator object, and record offsets as we go
        /// On 1st pass, no data actually gets written. This helps us calculate the correct block #'s
        /// On 2nd pass, we now have the correct block #'s, and we actually write the data
        /// </summary>
        private void GenerateIso()
        {
            // note - a comment followed by numbers in parenthesis is the name of the field and the number in parenthesis is the section number in ECMA-119
            m_gen.DupByte(0,0x8000);

            // BEGIN Volume Descriptors...

            GenPrimaryVolumeDescriptor();

            if (m_boot != null)
            {
                GenBootRecordDescriptor(); // Boot Record MUST be right after PVD ( see El Torito section 2.0 )
            }
            // TODO FIXME - generate any other needed Volume Descriptors here
            GenVolumeDescriptorTerminator();
            
            // END Volume Descriptors

            if (m_boot != null)
            {
                GenBootCatalog(m_boot);
            }

            GenPathTables();

            NextPathTableEntry = 1;
            GenDirectoryTree(m_isoRoot, m_isoRoot, 2);

#if ROCKRIDGE
            GenContinuationBlock();
#endif

            GenFiles(m_isoRoot);

            TotalSize = m_gen.m_idx;
        }

        private void GenPrimaryVolumeDescriptor() // ( 8.4 )
        {
            PrimaryVolumeDescriptor = m_gen.m_idx / LogicalBlockSize;
            var pvd = new FieldValidator(m_gen);
            pvd.Byte(1, 1); // Volume Descriptor Type ( 8.4.1 )
            pvd.AString("CD001", 2, 6); // Standard Identifier ( 8.4.2 )
            pvd.Byte(1, 7); // Volume Descriptor Version ( 8.4.3 )
            pvd.Zero(8, 8); // Unused Field ( 8.4.4 )
            pvd.AString("?", 9, 40); // System Identifier ( 8.4.5 )
            pvd.DString(m_volumeLabel, 41, 72); // Volume Identifier ( 8.4.6 )
            pvd.Zero(73, 80); // Unused Field ( 8.4.7 )
            pvd.IntLSBMSB((TotalSize + LogicalBlockSize - 1) / LogicalBlockSize, 81, 88); // Volume Space Size ( 8.4.8 )
            pvd.Zero(89, 120); // Unused Field ( 8.4.9 )
            pvd.ShortLSBMSB(1, 121, 124); // Volume Set Size ( 8.4.10 )
            pvd.ShortLSBMSB(1, 125, 128); // Volume Sequence Number ( 8.4.11 )
            pvd.ShortLSBMSB(LogicalBlockSize, 129, 132); // Logical Block Size ( 8.4.12 )
            pvd.IntLSBMSB(PathTableSize, 133, 140); // Path Table Size ( 8.4.13 )
            pvd.IntLSB(LPathTable, 141, 144); // L Path Table ( 8.4.14 )
            pvd.IntLSB(0, 145, 148); // Optional L Path Table ( 8.4.15 )
            pvd.IntMSB(MPathTable, 149, 152); // M Path Table ( 8.4.16 )
            pvd.IntMSB(0, 153, 156); // Optional M Path Table ( 8.4.17 )
            pvd.BeginField(157);
            DirectoryRecord(".", m_isoRoot, 1); // Directory Record for Root Directory ( 8.4.18 )
            pvd.EndField(190);
            pvd.DupByte(0x20, 191, 318); // Volume Set Identifier ( 8.4.19 )
            pvd.DupByte(0x20, 319, 446); // Publisher Identifier ( 8.4.20 )
            pvd.DupByte(0x20, 447, 574); // Data Preparer ( 8.4.21 )
            pvd.DupByte(0x20, 575, 702); // Application Identifier ( 8.4.22 )
            pvd.DupByte(0x20, 703, 739); // Copyright File Identifier ( 8.4.23 )
            pvd.DupByte(0x20, 740, 776); // Abstract File Identifier ( 8.4.24 )
            pvd.DupByte(0x20, 777, 813); // Bibliographic File Identifier ( 8.4.25 )
            System.DateTime dNow = System.DateTime.Now;
            pvd.AnsiDateTime(dNow, 814, 830); // Volume Creation Date and Time ( 8.4.26 )
            pvd.AnsiDateTime(dNow, 831, 847); // Volume Modification Date and Time ( 8.4.27 )
            pvd.Zero(848, 864); // Volume Expiration Date and Time ( 8.4.28 )
            pvd.Zero(865, 881); // Volume Effective Date and Time ( 8.4.29 )
            pvd.Byte(1, 882); // File Structure Version ( 8.4.30 )
            pvd.Zero(883, 883); // Reserved ( 8.4.31 )
            pvd.Zero(884, 1395); // Application Use ( 8.4.32 )
            pvd.Zero(1396, 2048); // Reserved for Future Standardization ( 8.4.33 )
        }

        private void GenVolumeDescriptorTerminator() // ISO 9660 - 8.3
        {
            var vdt = new FieldValidator(m_gen);
            vdt.Byte(255, 1); // Volume Descriptor Type ( 8.3.1 )
            vdt.AString("CD001", 2, 6); // Standard Identifier ( 8.4.2 )
            vdt.Byte(1, 7); // Volume Descriptor Version ( 8.4.3 )
            m_gen.FinishBlock();
        }

        private void GenBootRecordDescriptor() // ( ISO 9660 - 8.2, El Torito Figure 7 )
        {
            var br = new FieldValidator(m_gen);
            br.Byte(0, 1); // Volume Descriptor Type ( 8.2.1 ), Boot Record Indicator - must be 0 ( offset 0x00 )
            br.AString("CD001", 2, 6); // Standard Identifier ( 8.2.2 ), ( offset 0x01-0x05 )
            br.Byte(1, 7); // Volume Descriptor Version ( 8.2.3 ), must be 1 for El Torito also ( offset 0x06 )
            br.AString("EL TORITO SPECIFICATION", 8, 39); // Boot System Identifier ( 8.2.4 ), ( offset 0x07-0x26 )
            br.Zero(40, 71); // Boot Identifier ( 8.2.5 ), Unused - must be 0 ( offset 0x27-0x46 )
            br.IntLSB(BootCatalog, 72, 75); // Boot System Use ( 8.2.6 ), Absolute Pointer to first sector of Boot Catalog ( offset 0x47-0x4A )
            m_gen.FinishBlock();
        }

        private void GenBootCatalog(IsoFile f)
        {
            BootCatalog = m_gen.m_idx / LogicalBlockSize;
            // write validation entry first... see El Torito section 2.1
            var ve = new FieldValidator(m_gen);
            ve.Byte(1,1); // Header ID, must be 0x01
            ve.Byte(0, 2); // 0 == x86 - TODO FIXME: 1 == PowerPC, 2 == Mac, see El Torito figure 2
            ve.DupByte(0, 3, 4); // Reserved
            ve.DupByte(0, 5, 0x1C); // Manufacturer/Developer of CD-ROM, see El Torito figure 2
            ve.ShortLSB(0x55AA, 0x1D, 0x1E); // checksum - TODO FIXME - allow custom Manufacturer string - calculate checksum dynamically
            ve.Byte(0x55, 0x1F); // 1st Key Byte
            ve.Byte(0xAA, 0x20); // 2nd Key Byte

            // initial/default entry... see El Torito section 2.2 and figure 3
            var ide = new FieldValidator(m_gen);
            ide.Byte(0x88, 1); // 0x88 = bootable, 0x00 = not bootable

            // TODO FIXME - this is extremely hackish... fix me...
            ide.Byte(0, 2); // no emulation
            /*if ( f.fileInfo.Length <= 1182720 )
                ide.Byte(1, 2); // 1.2 meg diskette
            else if ( f.fileInfo.Length <= 1440 * 1024 )
                ide.Byte(2,2); // 1.44 meg diskette
            else if ( f.fileInfo.Length <= 2880 * 1024 )
                ide.Byte(3,2); // 2.88 meg diskette
            else
                ide.Byte(4,2);*/ // Hard Disk ( drive 80 )

            ide.ShortLSB(0, 3, 4); // Load Segment - 0 == default of 0x7C0
            ide.Byte(0, 5); // System Type, according to El Torito figure 3 this MUST be a copy of the "System Type" from the boot image. In practice this appears to not be the case.
            ide.Byte(0, 6); // Unused, must be 0
            if (m_bootLoadSize == 0)
                m_bootLoadSize = (short)((f.fileInfo.Length - 1) / 0x200 + 1);
            ide.ShortLSB(m_bootLoadSize, 7, 8); // Sector Count
            ide.IntLSB(m_boot.DataBlock, 9, 12); // Logical Block of boot image
            ide.Zero(13, 32); // unused

            m_gen.FinishBlock();
        }

        private void GenPathTables()
        {
            LPathTable = m_gen.m_idx / LogicalBlockSize;
            GenPathTableEx(m_isoRoot, m_isoRoot, true);
            PathTableSize = m_gen.m_idx - LPathTable * LogicalBlockSize;
            m_gen.FinishBlock();

            MPathTable = m_gen.m_idx / LogicalBlockSize;
            GenPathTableEx(m_isoRoot, m_isoRoot, false);
            m_gen.FinishBlock();
        }

        private void GenPathTableEx(IsoFolder parent_folder, IsoFolder this_folder, bool lsb)
        {
            var di = new FieldValidator(m_gen);
            // Path table record ( ECMA-119 section 9.4 )
            byte[] b_di = m_gen.IsoName(this_folder.name, true);
            di.Byte((byte)b_di.Length, 1);
            di.Byte(0, 2); // Extended Attribute Record Length
            if (lsb)
            {
                di.IntLSB(this_folder.DataBlock, 3, 6); // Location of Extent
                di.ShortLSB(parent_folder.PathTableEntry, 7, 8); // Parent Directory Number
            }
            else
            {
                di.IntMSB(this_folder.DataBlock, 3, 6); // Location of Extent
                di.ShortMSB(parent_folder.PathTableEntry, 7, 8); // Parent Directory Number
            }
            di.Bytes(b_di, 9, 8 + b_di.Length); // Directory Identifier
            if ((b_di.Length & 1) != 0)
                di.Byte(0, 9 + b_di.Length); // optional padding if LEN_DI is odd

            foreach (System.Collections.Generic.KeyValuePair<string, IsoEntry> it in this_folder.entries)
            {
                if (it.Value.IsFolder)
                {
                    GenPathTableEx(this_folder, (IsoFolder)it.Value, lsb);
                }
            }
        }

        private void GenDirectoryTree(IsoFolder parent_folder, IsoFolder this_folder, byte root)
        {
            this_folder.PathTableEntry = NextPathTableEntry++;
            this_folder.DataBlock = m_gen.m_idx / LogicalBlockSize;
            DirectoryRecord(".", this_folder, root);
            DirectoryRecord("..", parent_folder, 0);
            foreach (System.Collections.Generic.KeyValuePair<string, IsoEntry> it in this_folder.entries)
            {
                DirectoryRecord(it.Value.name, it.Value, 0);
            }
            this_folder.DataLength = m_gen.m_idx - this_folder.DataBlock * LogicalBlockSize;
            m_gen.FinishBlock();
            foreach (System.Collections.Generic.KeyValuePair<string, IsoEntry> it in this_folder.entries)
            {
                if (it.Value.IsFolder)
                {
                    GenDirectoryTree(this_folder, (IsoFolder)it.Value, 0);
                }
            }
        }

        private void GenContinuationBlock()
        {
            ContinuationBlock = m_gen.m_idx / LogicalBlockSize;
            ContinuationLength = -m_gen.m_idx; // HACK ALERT: we will add the final offset when done, and that will give us the length

            // ER - Extensions Reference - see P1281 section 5.5
            byte[] b_id = m_gen.ascii.GetBytes("RRIP_1991A"); // I can't find documentation anywhere for these values, but they came from an ISO I analyzed.
            byte[] b_des = m_gen.ascii.GetBytes("THE ROCK RIDGE INTERCHANGE PROTOCOL PROVIDES SUPPORT FOR POSIX FILE SYSTEM SEMANTICS");
            byte[] b_src = m_gen.ascii.GetBytes("PLEASE CONTACT DISC PUBLISHER FOR SPECIFICATION SOURCE.  SEE PUBLISHER IDENTIFIER IN PRIMARY VOLUME DESCRIPTOR FOR CONTACT INFORMATION.");
            var er = new FieldValidator(m_gen);
            er.Byte((byte)'E', 1);
            er.Byte((byte)'R', 2);
            System.Diagnostics.Debug.Assert((9 + b_id.Length + b_des.Length + b_src.Length) < 256);
            er.Byte((byte)(8 + b_id.Length + b_des.Length + b_src.Length), 3);
            er.Byte(1, 4); // version
            er.Byte((byte)b_id.Length, 5);
            er.Byte((byte)b_des.Length, 6);
            er.Byte((byte)b_src.Length, 7);
            er.Byte(1, 8); // extension version ( got this value from peeking at another ISO file )
            er.Bytes(b_id, 9, 9 + b_id.Length - 1);
            er.Bytes(b_des, 9 + b_id.Length, 9 + b_id.Length + b_des.Length - 1);
            er.Bytes(b_src, 9 + b_id.Length + b_des.Length, 9 + b_id.Length + b_des.Length + b_src.Length - 1);

            ContinuationLength += m_gen.m_idx;
            m_gen.FinishBlock();
        }

        private void GenFiles(IsoFolder this_folder)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, IsoEntry> it in this_folder.entries)
            {
                if (it.Value.IsFile)
                {
                    GenFile((IsoFile)it.Value);
                }
                else
                {
                    GenFiles((IsoFolder)it.Value);
                }
            }
        }

        /// <summary>
        ///  This function is responsible for generating directory records. However, since more than one directory
        ///  record may be required to get all the information out, it defers the actual directory record generation
        ///  to a sub-function DirectoryRecordEx().
        /// </summary>
        /// <param name="name">The real folder name</param>
        /// <param name="e">object containing info about the folder</param>
        /// <param name="root">1==PVD's root entry, 2==root's "." entry, 0==everything else ( special stuff needs to happen for the root entries )</param>
        private void DirectoryRecord(string name, IsoEntry e, byte root)
        {
            byte[] fi_name = m_gen.IsoName(name, true);
            string cont = DirectoryRecordEx(fi_name, name, e, root, false);
#if ROCKRIDGE
            while (cont.Length > 0)
            {
                cont = DirectoryRecordEx(fi_name, cont, e, 0, true);
            }
#endif
        }

        private string DirectoryRecordEx(byte[] fi_name, string real_name, IsoEntry e, byte root, bool second_pass)
        {
            byte[] /*b_fi,*/ b_su = null;
            /*if (fi_name == ".")
            {
                b_fi = new byte[] { 0 };
                real_name = "";
            }
            else if (fi_name == "..")
            {
                b_fi = new byte[] { 1 };
                real_name = "";
            }
            else
                b_fi = m_gen.ascii.GetBytes(fi_name);*/
            byte LEN_FI = (byte)fi_name.Length;
            byte LEN_DR = (byte)(33 + LEN_FI);
            bool fi_padding = ((LEN_DR & 1) != 0);
            if (fi_padding) LEN_DR++;
            // as much as I HATE to do it, I have to generate this data in both passes for now.
            // I don't yet understand enough about what and how many DR entries have to be made to figure out how to do it "right"
            byte LEN_SU = 0;
#if ROCKRIDGE
            if (root != 1) // don't generate susp on PVD's root entry...
            {
                b_su = Susp(ref real_name, e, root == 2, second_pass, (byte)(255 - LEN_DR));
                if (b_su.Length > 255)
                    throw new NotImplementedException("can't yet handle SUSP > 255 bytes");
                LEN_SU = (byte)b_su.Length;
            }
            else
                real_name = "";
#endif
            LEN_DR += LEN_SU;

            var dr = new FieldValidator(m_gen);
            dr.Byte(LEN_DR, 1); // Length of Directory Record ( 9.1.1 )
            dr.Byte(0, 2); // Extended Attribute Record Length ( 9.1.2 )
            dr.IntLSBMSB(e.DataBlock, 3, 10); // Location of Extent ( 9.1.3 )
#if true
            // in this test - I round the data length up to the next multiple of 2048, didn't help fix my booting problem though...
            dr.IntLSBMSB(((e.DataLength - 1) / 2048 + 1) * 2048, 11, 18); // Data Length ( 9.1.4 )
#else
            dr.IntLSBMSB(e.DataLength, 11, 18); // Data Length ( 9.1.4 )
#endif
            dr.BinaryDateTime(System.DateTime.Now, 19, 25); // Recording Date and Time ( 9.1.5 )
            byte flags = 0;
            if (e.IsFile)
            {
                IsoFile f = (IsoFile)e;
                if ((f.fileInfo.Attributes & System.IO.FileAttributes.Hidden) != 0)
                    flags |= 1; // hidden
            }
            else
            {
                // TODO FIXME - not supporting hidden folders right now
                //IsoFolder f = (IsoFolder)e;
                //if ((f.dirInfo.Attributes & System.IO.DirectoryAttributes.Hidden) != 0)
                //    flags |= 1; // hidden
            }
            if (e.IsFolder)
                flags |= 2; // directory
#if false // I'm disabling this because analysing of a working ISO never sets this bit...
            if (real_name.Length == 0)
                flags |= 128; // final
#endif
            dr.Byte(flags, 26); // flags ( 9.1.6 )
            dr.Byte(0, 27); // File Unit Size ( 9.1.7 )
            dr.Byte(0, 28); // Interleave Gap Size ( 9.1.8 )
            dr.ShortLSBMSB(1, 29, 32); // Volume Sequence Number ( 9.1.9 )
            dr.Byte(LEN_FI, 33); // Length of File Identifier ( 9.1.10 )
            dr.Bytes(fi_name, 34, 33 + LEN_FI);
            if (fi_padding) dr.Zero(34 + LEN_FI, 34 + LEN_FI);
            if (LEN_SU > 0) dr.Bytes(b_su, LEN_DR - LEN_SU + 1, LEN_DR);

            return real_name;
        }

        private byte[] Susp(ref string name, IsoEntry e, bool root, bool second_pass, byte available) // IEEE P1282 SUSP
        {
            var m = new System.IO.MemoryStream();
            bool dots = (name == "." || name == "..");
            if (!second_pass)
            {
                if (root)
                {
                    susp_base_sp(m);
                    susp_base_ce(m);
                }
                if (e.rr_flags != 0)
                    susp_rr_rr(m, e);
            }
            // TODO FIXME - write and generate susp_rr_tf() - timestamps...
            if (dots)
                name = "";
            else if (name.Length > 0)
                name = susp_rr_nm(m, name, available, e);

            if ((m.Length & 1) != 0) m.WriteByte(0); // padding
            byte[] b = m.GetBuffer();
            Array.Resize(ref b, (int)m.Length);
            return b;
        }

        private void susp_base_sp(System.IO.MemoryStream m) // IEEE P1281 SUSP ( 5.3 )
        {
            int start = (int)m.Length;
            m.WriteByte((byte)'S');
            m.WriteByte((byte)'P');
            m.WriteByte(7); // LEN_SP
            m.WriteByte(1); // Version
            m.WriteByte(0xBE); // 1st checksum byte
            m.WriteByte(0xEF); // 2nd checksum byte
            m.WriteByte(0); // LEN_SKP
            System.Diagnostics.Debug.Assert((m.Length - start) == 7);
        }

        private void susp_base_ce(System.IO.MemoryStream m) // IEEE P1282 SUSP ( 5.1 )
        {
            int start = (int)m.Length;
            m.WriteByte((byte)'C');
            m.WriteByte((byte)'E');
            m.WriteByte(28); // LEN_CE
            m.WriteByte(1); // version
            m.Write(Int2LSB(ContinuationBlock), 0, 4);
            m.Write(Int2MSB(ContinuationBlock), 0, 4);
            m.Write(Int2LSB(0), 0, 4); // Continuation Offset
            m.Write(Int2MSB(0), 0, 4); // Continuation Offset
            m.Write(Int2LSB(ContinuationLength), 0, 4);
            m.Write(Int2MSB(ContinuationLength), 0, 4);
            System.Diagnostics.Debug.Assert((m.Length - start) == 28);
        }

        private void susp_rr_rr(System.IO.MemoryStream m, IsoEntry e) // IEEE P1282 Rock Ridge Protocol Identifier ( ? - I can't find this documented, but analysis of iso files reveals it's existence )
        {
            int start = (int)m.Length;
            m.WriteByte((byte)'R');
            m.WriteByte((byte)'R');
            m.WriteByte(5); // LEN_RR
            m.WriteByte(1); // Version
            m.WriteByte(e.rr_flags); // flags
            System.Diagnostics.Debug.Assert((m.Length - start) == 5);
        }

        private string susp_rr_nm(System.IO.MemoryStream m, string name, byte available, IsoEntry e) // IEEE P1282 Alternate Name ( 4.1.4 )
        {
            int start = (int)m.Length;
            if (m.Length > available)
                throw new Exception("buffer overflow - internal design error");
            available -= (byte)m.Length;
            string cont = "";
            byte flags = 0;
            if (name == ".")
                flags |= 2;
            else if (name == "..")
                flags |= 4;
            else if (name.Length > (available - 5))
            {
                flags |= 1; // need another entry...
                cont = name.Substring(available - 5);
                name = name.Substring(0, available - 5);
            }
            e.rr_flags |= 0x08;
            m.WriteByte((byte)'N');
            m.WriteByte((byte)'M');
            m.WriteByte((byte)(name.Length + 5));
            m.WriteByte(1); // version?
            m.WriteByte(flags);
            m.Write(m_gen.ascii.GetBytes(name), 0, name.Length);
            System.Diagnostics.Debug.Assert((m.Length - start) == (name.Length + 5));
            return cont;
        }

        private void GenFile(IsoFile f)
        {
            f.DataBlock = m_gen.m_idx / LogicalBlockSize;
            f.DataLength = (int)f.fileInfo.Length;
            m_gen.WriteFile(f, PrimaryVolumeDescriptor);
            m_gen.FinishBlock();
        }

        /// <summary>
        /// This class is responsible for encoding types of data to the filestream and also keeping track
        /// of our current file position.
        /// </summary>
        private class Generator
        {
            private bool m_pedantic;
            public int m_idx;
            private System.IO.FileStream m_fs;
            public System.Text.ASCIIEncoding ascii;

            public Generator(bool pedantic)
            {
                m_pedantic = pedantic;
                m_idx = 0;
                m_fs = null;
                ascii = new System.Text.ASCIIEncoding();
            }
            public void ResetWithFileStream(System.IO.FileStream fs)
            {
                m_idx = 0;
                m_fs = fs;
            }

            /// <summary>
            /// This function converts a normal name into an iso9660-compatible file name.
            /// TODO FIXME: check the pedantic setting...
            /// </summary>
            /// <param name="name">the normal file name</param>
            /// <param name="folder">if this is a name for a folder</param>
            /// <returns>the iso9660-compatible version of the file name</returns>
            public byte[] IsoName(string name, bool folder)
            {
                if (folder)
                {
                    if (name == ".")
                        return new byte[] { 0 };
                    if (name == "..")
                        return new byte[] { 1 };
#if ROCKRIDGE
                    if (m_pedantic)
                        name = name.Replace('.', '_');
#endif
                }
#if ROCKRIDGE
                name = name.ToUpper();
#endif
                string ext = "";
                int dot = name.LastIndexOf('.');
                if (dot >= 0)
                {
                    ext = name.Substring(dot);
                    name = name.Substring(0, dot);
                }
#if ROCKRIDGE
                name = name.Substring(0, System.Math.Min(name.Length, 8)).Replace('.', '_').Replace(' ', '_');
                ext = ext.Substring(0, System.Math.Min(ext.Length, 4));
#else
                // make sure name isn't too long, 1st pass ( trying to preserve extension )
                int total = name.Length + ext.Length;
                const int max_length = (256 - 34);
                if (total > max_length)
                {
                    int new_length = name.Length - (total - max_length);
                    if ( new_length >= 0 )
                        name = name.Substring ( 0, new_length );
                }
#endif
                name += ext;

#if !ROCKRIDGE
                // make sure name isn't too long, 2nd pass ( if 1st pass failed, then extension is too long - sacrifice it now )
                if (name.Length > max_length)
                    name = name.Substring(0, max_length);
#endif

                return ascii.GetBytes(name);
            }

            public void FinishBlock()
            {
                int extra = ( -m_idx ) % 2048;
                if (extra < 0)
                    extra += 2048;
                DupByte(0, extra);
            }

            public void DupByte(byte b, int count)
            {
                m_idx += count;
                if (m_fs == null) return;
                while (count-- > 0)
                    m_fs.WriteByte(b);
            }

            public void Bytes(byte[] b)
            {
                Bytes(b, 0, b.Length);
            }

            public void Bytes(byte[] b, int offset, int bytes)
            {
                m_idx += bytes;
                if (m_fs == null) return;
                m_fs.Write(b, offset, bytes);
            }

            public void String(string s)
            {
                Bytes(ascii.GetBytes(s));
            }

            /// <summary>
            /// writes a file out to cd. Generate the Boot Info Table if necessary
            /// </summary>
            /// <param name="f">the file to write</param>
            /// <param name="PrimaryVolumeDescriptor">the PVD block, passed in case we need to generate the Boot Info Table</param>
            public void WriteFile(IsoFile f, int PrimaryVolumeDescriptor)
            {
                if (f.fileInfo.Length > 0xffffffff)
                    throw new NotImplementedException(">4G files not implemented");
                int bytes = (int)f.fileInfo.Length;
                if (m_fs == null)
                {
                    m_idx += bytes;
                    return;
                }
                // TODO FIXME - create smaller reusable buffer and read fixed-size chunks at a time...
                byte[] b = new byte[bytes];
                if (bytes != f.fileInfo.OpenRead().Read(b, 0, bytes))
                {
                    throw new Exception("number of bytes read from file != reported length of file: " + f.fileInfo.Name);
                }
                if (f.BootInfoTable)
                {
                    // TODO FIXME - this is TERRIBLE. This should be implemented at a higher level, and
                    // doing it here requires passing the PrimaryVolumeDescriptor to every call of WriteFile()
                    // The reason it is here is because this is the only place where the file actually gets
                    // pulled into memory and I didn't want to modify the boot image on-disk like mkisofs does.
                    Bytes(b, 0, 8);
                    Bytes(Int2LSB(PrimaryVolumeDescriptor));
                    Bytes(Int2LSB(f.DataBlock));
                    Bytes(Int2LSB((int)f.fileInfo.Length));
                    Bytes(Int2LSB(0)); // TODO FIXME - checksum
                    DupByte(0, 40); // reserved
                    Bytes(b, 64, bytes - 64);
                }
                else
                    Bytes(b);
            }
        }

        /// <summary>
        /// This class is used to make sure that we are keeping to the structure offsets as laid out in the specs.
        /// We create a new instance for each structure we output. It passes the data it needs to output to the Generator class
        /// </summary>
        private class FieldValidator
        {
            private Generator m_g;
            private int field_base;
            public FieldValidator(Generator g)
            {
                m_g = g;
                field_base = m_g.m_idx;
            }

            public void BeginField(int expected)
            {
                int actual = m_g.m_idx - field_base + 1;
                if (expected != actual)
                    throw new Exception("offset exception, expected " + expected.ToString() + ", but have " + actual.ToString());
            }

            public void EndField(int expected)
            {
                int actual = m_g.m_idx - field_base;
                if (expected != actual)
                    throw new Exception("offset exception, expected " + expected.ToString() + ", but have " + actual.ToString());
            }

            public void Zero(int start, int end)
            {
                BeginField(start);
                m_g.DupByte(0, end - start + 1);
                EndField(end);
            }

            public void Byte(byte b, int off)
            {
                BeginField(off);
                m_g.DupByte(b,1);
                EndField(off);
            }

            public void Bytes(byte[] b, int start, int end)
            {
                BeginField(start);
                m_g.Bytes(b);
                EndField(end);
            }

            public void DupByte(byte b, int start, int end)
            {
                BeginField(start);
                m_g.DupByte(b, end - start + 1);
                EndField(end);
            }

            public void AString(string s, int start, int end)
            {
                // TODO FIXME - validate contents of string against legal a-string character set
                BeginField(start);
                int need = end - start + 1;
                int have = s.Length;
                if (have > need)
                    s = s.Substring(0, need);
                else
                    s = s.PadRight(need);
                m_g.String(s);

                EndField(end);
            }

            public void DString(string s, int start, int end)
            {
                // TODO FIXME - validate contents of string against legal d-string character set
                BeginField(start);
                int need = end - start + 1;
                int have = s.Length;
                if (have > need)
                    s = s.Substring(0, need);
                else
                    s = s.PadRight(need);
                m_g.String(s);

                EndField(end);
            }

            public void IntLSB(int i, int start, int end)
            {
                BeginField(start);
                m_g.Bytes(Int2LSB(i));
                EndField(end);
            }

            public void IntMSB(int i, int start, int end)
            {
                BeginField(start);
                m_g.Bytes(Int2MSB(i));
                EndField(end);
            }

            public void IntLSBMSB(int i, int start, int end)
            {
                BeginField(start);
                m_g.Bytes(Int2LSB(i));
                m_g.Bytes(Int2MSB(i));
                EndField(end);
            }

            public void ShortLSB(short i, int start, int end)
            {
                BeginField(start);
                m_g.Bytes(Short2LSB(i));
                EndField(end);
            }

            public void ShortMSB(short i, int start, int end)
            {
                BeginField(start);
                m_g.Bytes(Short2MSB(i));
                EndField(end);
            }

            public void ShortLSBMSB(short i, int start, int end)
            {
                BeginField(start);
                m_g.Bytes(Short2LSB(i));
                m_g.Bytes(Short2MSB(i));
                EndField(end);
            }

            public void Digits(int i, int start, int end)
            {
                DString(i.ToString(),start,end);
            }

            public void AnsiDateTime(System.DateTime d, int start, int end) // ( 8.4.26.1 )
            {
                BeginField(start);
                var dt = new FieldValidator(m_g);
                dt.Digits(d.Year, 1, 4);
                dt.Digits(d.Month, 5, 6);
                dt.Digits(d.Day, 7, 8);
                dt.Digits(d.Hour, 9, 10);
                dt.Digits(d.Minute, 11, 12);
                dt.Digits(d.Second, 13, 14);
                dt.Digits(d.Millisecond / 10, 15, 16);
                dt.Digits(0, 17, 17); // TODO FIXME - I don't understand the docs for how to encode the GMT offset...
                EndField(end);
            }

            public void BinaryDateTime(System.DateTime d, int start, int end) // ( 9.1.5 )
            {
                BeginField(start);
                var dt = new FieldValidator(m_g);
                dt.Byte((byte)(d.Year - 1900), 1);
                dt.Byte((byte)d.Month, 2);
                dt.Byte((byte)d.Day, 3);
                dt.Byte((byte)d.Hour, 4);
                dt.Byte((byte)d.Minute, 5);
                dt.Byte((byte)d.Second, 6);
                dt.Byte(0, 7); // TODO FIXME - unsure about how to encode time zone exactly... :(
                EndField(end);
            }

        }

        /// <summary>
        /// converts a short integer value to an LSB byte array
        /// </summary>
        /// <param name="i">the short integer value to convert</param>
        /// <returns>the LSB byte array</returns>
        public static byte[] Short2LSB(short i)
        {
            byte[] b = new byte[2];
            b[0] = (byte)(i & 0xff);
            i >>= 8;
            b[1] = (byte)(i & 0xff);
            return b;
        }

        /// <summary>
        /// converts a short integer value to an MSB byte array
        /// </summary>
        /// <param name="i">the short integer value to convert</param>
        /// <returns>the MSB byte array</returns>
        public static byte[] Short2MSB(short i)
        {
            byte[] b = new byte[2];
            b[1] = (byte)(i & 0xff);
            i >>= 8;
            b[0] = (byte)(i & 0xff);
            return b;
        }

        /// <summary>
        /// converts a 32-bit integer value to an LSB byte array
        /// </summary>
        /// <param name="i">the 32-bit integer value to convert</param>
        /// <returns>the LSB byte array</returns>
        public static byte[] Int2LSB(int i)
        {
            byte[] b = new byte[4];
            b[0] = (byte)(i & 0xff);
            i >>= 8;
            b[1] = (byte)(i & 0xff);
            i >>= 8;
            b[2] = (byte)(i & 0xff);
            i >>= 8;
            b[3] = (byte)(i & 0xff);
            return b;
        }

        /// <summary>
        /// converts a 32-bit integer value to an MSB byte array
        /// </summary>
        /// <param name="i">the 32-bit integer value to convert</param>
        /// <returns>the MSB byte array</returns>
        public static byte[] Int2MSB(int i)
        {
            byte[] b = new byte[4];
            b[3] = (byte)(i & 0xff);
            i >>= 8;
            b[2] = (byte)(i & 0xff);
            i >>= 8;
            b[1] = (byte)(i & 0xff);
            i >>= 8;
            b[0] = (byte)(i & 0xff);
            return b;
        }

    }
}
