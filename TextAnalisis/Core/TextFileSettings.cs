using System.IO;

namespace TextAnalisis.Core
{
    /// <summary>
    /// Представляет собой параметры текстового файла.
    /// </summary>
    public class TextFileSettings : IFileSettings
    {        
        public string filePath { get; }

        public long fileSize { get; }

        /// <summary>
        /// Инициализирует новый экемпляр класса, представляющего параметры текстового файла.
        /// </summary>
        /// <param name="filePath">Полное имя файла.</param>
        public TextFileSettings(string filePath)
        {
            this.filePath = filePath;

            FileInfo fi = new FileInfo(filePath);
            fileSize = fi.Length;
        }
    }
}
