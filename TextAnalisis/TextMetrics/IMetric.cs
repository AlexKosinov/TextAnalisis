namespace TextAnalisis.TextMetrics
{
    public interface IMetric
    {
        // Получить метрику
        void DoMetric(string text);

        string GetStringMetric();
    }
}
