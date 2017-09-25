using System.Linq;

namespace TextAnalisis.TextMetrics
{
    public class LineCountMetric : IMetric
    {
        int lineCount;

        public void DoMetric(string text)
        {
            lineCount += text.Where(c => c == '\n').Count();       
        }

        public string GetStringMetric()
        {
            string result = "Количество строк: ";
            result += (lineCount + 1).ToString();

            return result;
        }
    }
}
