using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextAnalisis;
using TextAnalisis.TextMetrics;
using System.IO;

namespace TextAnalisisTests
{
    [TestClass]
    public class TextAnaliserTests
    {
        [TestMethod]
        public void ReadFileTest()
        {
            TextAnaliser ctrl = new TextAnaliser(@"TestFiles\test1.txt");

            var expected = "test";

            var actual = ctrl.ReadFile(0, 0x2000);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadFileBigFileTest()
        {
            TextAnaliser ctrl = new TextAnaliser(@"e:\ExtremelyLargeImage.data");

            //var expected = "test";

            var actual = ctrl.ReadFile(133217728, 128);

            //Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TextLengthDoMetricTest()
        {
            IMetric textLength = new TextLengthMetric();
            var filePath = @"TestFiles\test1.txt";
            TextAnaliser ctrl = new TextAnaliser(filePath);

            var expected = "Количество символов: " + (new FileInfo(filePath).Length).ToString();

            var actual = ctrl.GetMetric(textLength);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LineCountDoMetricTest()
        {
            IMetric textLength = new LineCountMetric();
            var filePath = @"TestFiles\test.txt";
            TextAnaliser ctrl = new TextAnaliser(filePath);

            var expected = "Количество строк: 4";

            var actual = ctrl.GetMetric(textLength);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TextLengthDoMetricBigFileTest()
        {
            IMetric textLength = new TextLengthMetric();
            var filePath = @"e:\ExtremelyLargeImage.data";
            TextAnaliser ctrl = new TextAnaliser(filePath);

            var expected = "Количество символов: " + (new FileInfo(filePath).Length).ToString();

            var actual = ctrl.GetMetric(textLength);

            Assert.AreEqual(expected, actual);
        }
    }
}
