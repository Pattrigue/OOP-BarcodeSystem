namespace DashSystem.CsvDataAccess
{
    public sealed class UserCsvData : ICsvData
    {
        public uint Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public decimal Balance { get; private set; }
        public string Email { get; private set; }

        public void ReadLine(char separator, string csvLine)
        {
            string[] fields = csvLine.Split(separator);

            Id = uint.Parse(fields[0]);
            FirstName = fields[1];
            LastName = fields[2];
            Username = fields[3];
            Balance = decimal.Parse(fields[4]);
            Email = fields[5];
        }
    }
}