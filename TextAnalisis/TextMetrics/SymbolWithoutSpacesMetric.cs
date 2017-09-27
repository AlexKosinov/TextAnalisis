using System.Linq;

namespace TextAnalisis.TextMetrics
{
    /// <summary>
    /// Метрика для подсчета количества не пустых символов в тексте.
    /// </summary>
    public class SymbolWithoutSpacesMetric : IMetric
    {
        int count;

        public void DoMetric(string text)
        {
            count += text.Where(c => c != ' ' && c != '\r' && c != '\n' && c != '\t' && c != '\v' && c != '\f').Count();
        }

        public string GetStringMetric()
        {
            string result = "Количество символов без пробелов: ";
            result += count.ToString();

            return result;
        }
    }
}
