namespace TextAnalisis.TextMetrics
{
    public interface IMetric
    {
        /// <summary>
        /// Выполняет метрику для заданного текста.
        /// </summary>
        /// <param name="text">Текст для анализа по данной метрике.</param>
        void DoMetric(string text);

        /// <summary>
        /// Возвращает текстовое представление метрики (описание и значение метрики).
        /// </summary>
        /// <returns>Строка с описанием и значением метрики.</returns>
        string GetStringMetric();
    }
}
