using NUnit.Framework;
using System;
using System.Xml.Linq;

namespace XMLRepair
{
    public class XMLCorrectionTest
    {
        XMLCorrection correct = new XMLCorrection();

        [TestCase("test.xml")]
        public void TestRepairLinq(string file)
        {
            correct.SetFile(file);
            Assert.DoesNotThrow(() => XElement.Parse(correct.RepairLinq()));
        }

        [TestCase("test.xml")]
        public void TestRepairExactPosition(string file)
        {
            correct.SetFile(file);
            Assert.DoesNotThrow(() => XElement.Parse(correct.RepairExactPosition()));
        }

        [TestCase("test.xml")]
        public void TestRepairAppendChars(string file)
        {
            correct.SetFile(file);
            Assert.DoesNotThrow(() => XElement.Parse(correct.RepairAppendChars()));
        }

        [TestCase("test.xml")]
        public void TestRepairAppendBlocks(string file)
        {
            correct.SetFile(file);
            Assert.DoesNotThrow(() => XElement.Parse(correct.RepairAppendBlocks()));
        }
    }
}