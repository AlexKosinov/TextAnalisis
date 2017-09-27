using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextAnalisis;
using TextAnalisis.TextMetrics;
using System.IO;
using TextAnalisis.Core;

namespace TextAnalisisTests
{
    [TestClass]
    public class TextAnaliserTests
    {
        [TestMethod]
        public void ReadFileTest()
        {
            FileLoader file = new FileLoader(new TextFileSettings(@"TestFiles\test1.txt"));

            var expected = "test";

            var actual = file.ReadTextBlock(0, 0x2000);

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void TextLengthDoMetricTest()
        {
            var filePath = @"TestFiles\test1.txt";
            FileLoader file = new FileLoader(new TextFileSettings(filePath));

            IMetric textLength = new TextLengthMetric();            

            var expected = "Количество символов: " + (new FileInfo(filePath).Length).ToString();

            textLength.DoMetric(file.ReadTextBlock(0, 0x2000));

            var actual = textLength.GetStringMetric();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LineCountDoMetricTest()
        {
            var filePath = @"TestFiles\test.txt";
            FileLoader file = new FileLoader(new TextFileSettings(filePath));

            IMetric lineCount = new LineCountMetric();

            var expected = "Количество строк: 4";

            lineCount.DoMetric(file.ReadTextBlock(0, 0x2000));

            var actual = lineCount.GetStringMetric();

            Assert.AreEqual(expected, actual);
        }
    }
}
