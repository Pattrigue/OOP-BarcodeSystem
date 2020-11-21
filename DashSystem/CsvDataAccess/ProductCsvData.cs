namespace DashSystem.CsvDataAccess
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
            Name = RemoveHtmlTagsAndQuotationMarksFromString(fields[1]);
            Price = decimal.Parse(fields[2]);
            Active = fields[3] != "0";
        }

        private static string RemoveHtmlTagsAndQuotationMarksFromString(string str)
        {
            int startIndex = -1;
            int length = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '>' && startIndex == -1)
                {
                    startIndex = i + 1;
                }

                if (str[i] == '<' && startIndex != -1)
                {
                    length = i - startIndex;
                }
            }

            if (startIndex != -1 && length != 0)
            {
                str = str.Substring(startIndex, length);
            }

            str = str.Replace("\"", string.Empty);

            return str;
        }
    }
}