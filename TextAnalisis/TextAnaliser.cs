using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAnalisis.TextMetrics;

namespace TextAnalisis
{
    public class TextAnaliser
    {
        string filePath;
        string text;

        long fileSize;

        //MemoryMappedFile mmf;

        /*       public string Text
               {
                   get
                   {
                       return text;
                   }
               }*/

        public TextAnaliser(string filePath)
        {
            this.filePath = filePath;

            FileInfo fi = new FileInfo(filePath);
            fileSize = fi.Length;

            //mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, "Text");
        }

        public string ReadFile(long offset, long length)
        {
            if (length > fileSize)
                length = fileSize;

            // Create the memory-mapped file.
            using (var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, "Text"))
            {
                using (var accessor = mmf.CreateViewAccessor(offset, length))
                {
                    var readOut = new byte[length];
                    accessor.ReadArray(0, readOut, 0, readOut.Length);
                    return Encoding.Default.GetString(readOut);
                }
            }
        }

        public string GetMetric(IMetric metric)
        {
            string result = String.Empty;

            long blockSize = 0x10000;
            long offset = 0, length = fileSize < blockSize ? fileSize : blockSize;

            while ((offset + length) <= fileSize)
            {
                metric.DoMetric(ReadFile(offset, length));
                offset += length;
            }

            result = metric.GetStringMetric();

            return result;
        }

        public string NextPage()
        {


            return text;
        }
    }
}
