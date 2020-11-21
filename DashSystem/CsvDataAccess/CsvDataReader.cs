using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DashSystem.CsvDataAccess
{
    public sealed class CsvDataReader<T> where T : ICsvData, new()
    {
        private readonly char separator;

        public CsvDataReader(char separator) => this.separator = separator;
        
        public IEnumerable<T> ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found: ", path);
            }

            return File.ReadAllLines(path)
                .Skip(1)
                .Select(csvLine =>
                {
                    T csvData = new T();
                    csvData.ReadLine(separator, csvLine);

                    return csvData;
                });
        }
    }
}