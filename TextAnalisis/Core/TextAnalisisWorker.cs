using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TextAnalisis.TextMetrics;

namespace TextAnalisis.Core
{
    public class TextAnalisisWorker
    {
        FileLoader file;
        IFileSettings fileSettings;
        List<IMetric> metrics;
        string metricResults;      

        /// <summary>
        /// Возникает при начале анализа текста.
        /// </summary>
        public event Action<object> OnBeginAnalis;

        /// <summary>
        /// Возникает при окончании анализа текста.
        /// </summary>
        public event Action<object> OnCompleted;

        bool isActive;

        #region Properties

        /// <summary>
        /// Получает текстовое представление метрик
        /// </summary>
        public string MetricResults
        {
            get
            {
                return metricResults;
            }
        }

        /// <summary>
        /// Получает или задает параметры файла. А также открывает данный файл.
        /// </summary>
        public IFileSettings Settings
        {
            get
            {
                return fileSettings;
            }
            set
            {
                fileSettings = value;
                file = new FileLoader(value);
            }
        }

        /// <summary>
        /// Получает или задает метрики необходимые для анализа.
        /// </summary>
        public List<IMetric> Metrics
        {
            get
            {
                return metrics;
            }
            set
            {
                metrics = value;
            }
        }

        /// <summary>
        /// Возращает текущий открытый файл.
        /// </summary>
        public FileLoader File
        {
            get
            {
                return file;
            }
        }

        #endregion

        /// <summary>
        /// Начинает анализ текста.
        /// </summary>
        public void Start()
        {
            isActive = true;
            Worker();
        }

        /// <summary>
        /// Прекращает анализ текста и закрывает файл.
        /// </summary>
        public void Abort()
        {
            isActive = false;
            file?.Dispose();
            file = null;
        }

        /// <summary>
        /// Выполняет анализ текста по заданным метрикам.
        /// </summary>
        private async void Worker()
        {
            OnBeginAnalis?.Invoke(this);

            if (!isActive)
            {
                OnCompleted?.Invoke(this);
                return;
            }

            metricResults = String.Empty;
            try
            {
                if (metrics?.Count > 0)
                {
                    for (int i = 0; i < metrics.Count; i++)
                    {
                        metricResults += await GetMetric(metrics[i]) + "\n";
                    }
                }
                else
                {
                    metricResults = "Метрики не заданы.";
                }
            }
            catch { }

            OnCompleted?.Invoke(this);
            isActive = false;
        }

        /// <summary>
        /// Получает значение метрики для текущего файла.
        /// </summary>
        /// <param name="metric">Метрика текста.</param>
        /// <returns>Текствое представление метрики.</returns>
        private async Task<string> GetMetric(IMetric metric)
        {
            string result = String.Empty;
            
            long blockSize = 0x20000;
            long offset = 0, length = file.fileSize < blockSize ? file.fileSize : blockSize;
            var fileSize = file.fileSize;

            await Task.Run(() =>
            {
                while ((offset + length) <= fileSize)
                {
                    if (!isActive)
                        break;

                    metric.DoMetric(file.ReadTextBlock(offset, length));
                    offset += length;
                }
            });
            result = metric.GetStringMetric();

            return result;
        }
    }
}
