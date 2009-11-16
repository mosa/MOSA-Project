using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.StageVisualizer
{
    public class Output
    {
        public String[] Lines;

        public class Section
        {
            public string Method;
            public string Stage;
            public int Start;
            public int End;
            public int Block;
            public string Label;
        }

        public List<Section> Sections = new List<Section>();

        public Output(string[] lines)
        {
            Lines = lines;
            Parse();
        }

        public Output(string file)
        {
            Lines = System.IO.File.ReadAllLines(file);
            Parse();
        }

        protected void NewSection(string method, string stage, int block, string label, int start, int end)
        {
            if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(label) || start < 0)
                return;

            Section section = new Section();
            section.Method = method;
            section.Stage = stage;
            section.Block = block;
            section.Start = start;
            section.End = end;
            section.Label = label;
            Sections.Add(section);
        }

        public void Parse()
        {
            string method = string.Empty;
            string stage = string.Empty;
            int block = 0;
            string label = string.Empty;
            int start = -1;
            int end = 0;

            string method1 = "IR representation of method ";
            string method2 = " after stage ";
            string block1 = "Block #";
            string block2 = " - label ";

            Sections.Clear();

            for (int i = 0; i < Lines.Length; i++)
            {
                string line = Lines[i];
                if (line.StartsWith(method1))
                {
                    end = i - 1;
                    NewSection(method, stage, block, label, start, end);
                    start = i; // +1;
                    int mid = line.IndexOf(method2);

                    method = line.Substring(method1.Length, mid - method1.Length);
                    stage = line.Substring(mid + method2.Length);

                    block = -1;
                    label = string.Empty;
                }
                else
                    if (line.StartsWith(block1))
                    {
                        end = i - 1;
                        NewSection(method, stage, block, label, start, end);
                        start = i;

                        int mid = line.IndexOf(block2);

                        string blk = line.Substring(block1.Length, mid - block1.Length);
                        block = Convert.ToInt32(blk);

                        label = line.Substring(mid + block2.Length);
                    }
            }
        }

        public List<string> GetMethods()
        {
            Dictionary<string, int> list1 = new Dictionary<string, int>();

            foreach (Section section in Sections)
                if (!list1.ContainsKey(section.Method))
                    list1.Add(section.Method, 0);

            List<string> list = new List<string>();

            foreach (string entry in list1.Keys)
                list.Add(entry);

            return list;
        }

        public List<string> GetStages(string method)
        {
            Dictionary<string, int> list1 = new Dictionary<string, int>();

            foreach (Section section in Sections)
                if (section.Method == method)
                    if (!list1.ContainsKey(section.Stage))
                        list1.Add(section.Stage, 0);

            List<string> list = new List<string>();

            foreach (string entry in list1.Keys)
                list.Add(entry);

            return list;
        }

        public List<string> GetLabels(string method, string stage)
        {
            Dictionary<string, int> list1 = new Dictionary<string, int>();

            foreach (Section section in Sections)
                if (section.Method == method && (section.Stage == stage || string.IsNullOrEmpty(stage)))
                    if (!list1.ContainsKey(section.Label))
                        list1.Add(section.Label, 0);

            List<string> list = new List<string>();

            foreach (string entry in list1.Keys)
                list.Add(entry);

            return list;
        }

        public List<string> GetBlocks(string method, string stage)
        {
            List<string> blocks = new List<string>();

            foreach (Section section in Sections)
                if (section.Method == method && section.Stage == stage)
                    blocks.Add(section.Block.ToString());

            return blocks;
        }

        public List<string> GetText(string method, string stage, string label)
        {
            List<string> lines = new List<string>();

            foreach (Section section in Sections)
                if (section.Method == method
                    && (section.Stage == stage || string.IsNullOrEmpty(stage))
                    && (section.Label == label || string.IsNullOrEmpty(label)))
                {
                    for (int i = section.Start; i <= section.End; i++)
                        lines.Add(Lines[i]);
                }

            return lines;
        }
    }
}
