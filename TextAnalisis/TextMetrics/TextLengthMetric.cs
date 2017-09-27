namespace TextAnalisis.TextMetrics
{
    /// <summary>
    /// Метрика для подсчета длины текста.
    /// </summary>
    public class TextLengthMetric : IMetric
    {
        int symbolCount;

        public void DoMetric(string text)
        {
            symbolCount += text.Length;
        }

        public string GetStringMetric()
        {
            string result = "Количество символов: ";
            result += symbolCount.ToString();

            return result;
        }
    }
}
