namespace TextAnalisis.Core
{
    public interface IFileSettings
    {
        /// <summary>
        /// Возвращает полное имя файла.
        /// </summary>
        string filePath { get; }

        /// <summary>
        /// Возвращает размер файла.
        /// </summary>
        long fileSize { get; }
    }
}
