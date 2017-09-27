using System.Text.RegularExpressions;

namespace TextAnalisis.TextMetrics
{
    /// <summary>
    /// Метрика для подсчета совпадений шаблона с текстом.
    /// </summary>
    class TextMatchesCountMetric : IMetric
    {
        public string Pattern { get; set; }

        int matchesCount;

        public void DoMetric(string text)
        {
            matchesCount += new Regex(Pattern).Matches(text).Count;
        }

        public string GetStringMetric()
        {
            string result = "Количество совпадений с '" + Pattern + "': ";
            result += matchesCount.ToString();

            return result;
        }
    }
}
