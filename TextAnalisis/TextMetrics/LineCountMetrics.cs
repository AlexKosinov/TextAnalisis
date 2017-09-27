using System.Linq;

namespace TextAnalisis.TextMetrics
{
    /// <summary>
    /// Метрика для подсчета количества строк в тексте.
    /// </summary>
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
