using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace TextAnalisis.Core
{
    /// <summary>
    /// Предоставляет возможность считывания большого файла по блокам.
    /// </summary>
    public class FileLoader : IDisposable
    {
        string filePath;

        public readonly long fileSize;

        MemoryMappedFile mmf;

        bool disposed = false;

        /// <summary>
        /// Инизиализирует новый экземпляр класса FileLoader и создает из файла на диске размещенный в памяти файл.
        /// </summary>
        /// <param name="fileSettings">Параметры файла.</param>
        public FileLoader(IFileSettings fileSettings)
        {
            filePath = fileSettings.filePath;
            fileSize = fileSettings.fileSize;

            // Создание сопоставленного в памяти файла.
            mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, "Text");
        }

        /// <summary>
        /// Считывает блок текста, который имеет смещение и размер, из файла сопоставленного в памяти.
        /// Если размер файла меньше размера блока, считывается весь файл.
        /// </summary>
        /// <param name="offset">Байт с которого должен начаться блок</param>
        /// <param name="length">Размер блока</param>
        /// <returns>Возвращает считанный блок с заданным смещением и размером</returns>
        public string ReadTextBlock(long offset, long length)
        {
            if (length > fileSize)
                length = fileSize;

            // Создание представления сопоставленного в памяти файла.
            using (var accessor = mmf.CreateViewAccessor(offset, length))
            {
                var readOut = new byte[length];
                accessor.ReadArray(0, readOut, 0, readOut.Length);
                return Encoding.Default.GetString(readOut);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                mmf.Dispose();
            }

            disposed = true;
        }
    }
}
