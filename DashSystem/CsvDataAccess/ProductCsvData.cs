using DashSystem.Products;

namespace DashSystem.CsvDataAccess
{
    public sealed class ProductCsvData : ICsvData
    {
        private uint id;

        private string name;

        private decimal price;

        private bool active;

        public void ReadLine(char separator, string csvLine)
        {
            string[] fields = csvLine.Split(separator);

            id = uint.Parse(fields[0]);
            name = RemoveHtmlTagsAndQuotationMarksFromString(fields[1]);
            price = decimal.Parse(fields[2]);
            active = fields[3] != "0";
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

        public static explicit operator Product(ProductCsvData productCsvData)
        {
            return new Product(productCsvData.id, productCsvData.name, productCsvData.price, productCsvData.active, false);
        }
    }
}