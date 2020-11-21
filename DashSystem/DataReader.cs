using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DashSystem
{
    public sealed class ProductCsvData : ICsvData
    {
        public uint Id { get; private set; }
        
        public string Name { get; private set; }
        
        public decimal Price { get; private set; }
        
        public bool Active { get; private set; }

        public void ReadLine(char separator, string csvLine)
        {
            string[] fields = csvLine.Split(separator);

            Id = uint.Parse(fields[0]);
            Name = fields[1];
            Price = decimal.Parse(fields[2]);
            Active = bool.Parse(fields[3]);
        }
    }

    public interface ICsvData
    {
        void ReadLine(char separator, string csvLine);
    }
    
    public sealed class DataReader<T> where T : ICsvData, new()
    {
        private const string DataDirectoryName = "Data";

        private readonly char separator;

        public DataReader(char separator) => this.separator = separator;
        
        public IEnumerable<T> ReadFile(string fileName)
        {
            string dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));

            if (dir == null)
            {
                throw new DirectoryNotFoundException("Could not get project root directory!");
            }
            
            string path = Path.Combine(dir, DataDirectoryName, fileName);

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