namespace DashSystem.Models.CsvDataAccess
{
    public interface ICsvData
    {
        void ReadLine(char separator, string csvLine);
    }
}