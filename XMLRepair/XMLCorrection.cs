using BenchmarkDotNet.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XMLRepair
{
    public class XMLCorrection
    {
        private string filename;

        public void SetFile(string file)
        {
            filename = file;
        }

        bool IsBroken(string content)
        {
            try
            {
                XElement.Parse(content);
            }
            catch (System.Xml.XmlException)
            {
                return true;
            }
            return false;
        }

        bool IsBroken(string content, ref int line, ref int position)
        {
            try
            {
                XElement.Parse(content);
            }
            catch (System.Xml.XmlException ex)
            {
                line = ex.LineNumber;
                position = ex.LinePosition;
                return true;
            }
            return false;
        }

        [GlobalSetup]
        public void Setup()
        {
            filename = "test.xml";
        }

        [Benchmark]
        public string RepairLinq()
        {
            var content = File.ReadAllText(filename);
            if (!IsBroken(content))
                return content;
            string correct = new string(content.Where(c => XmlConvert.IsXmlChar(c)).ToArray());

            return correct;
        }

        [Benchmark]
        public string RepairExactPosition()
        {
            var content = File.ReadAllText(filename);
            int line = 0;
            int position = 0;
            string correct = content;
            var lines = File.ReadLines(filename).ToList();
            while (IsBroken(correct, ref line, ref position))
            {
                lines[line - 1] = lines.ElementAt(line - 1).Remove(position - 1);
                correct = string.Join("", lines.ToArray());
            }
            return correct;
        }

        [Benchmark]
        public string RepairAppendBlocks()
        {
            var content = File.ReadAllText(filename);
            if (!IsBroken(content))
                return content;

            StringBuilder builder = new StringBuilder();
            int offset = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (!XmlConvert.IsXmlChar(content[i]))
                {
                    builder.Append(content.Substring(offset, i - offset));
                    offset = i + 1;
                }
            }
            if (offset < content.Length)
                builder.Append(content.Substring(offset, content.Length - offset));
            
            return builder.ToString();
        }

        [Benchmark]
        public string RepairAppendChars()
        {
            var content = File.ReadAllText(filename);
            if (!IsBroken(content))
                return content;

            StringBuilder builder = new StringBuilder();
            foreach (var x in content)
            {
                if (XmlConvert.IsXmlChar(x))
                    builder.Append(x);
            }

            return builder.ToString();
        }
    }
}
