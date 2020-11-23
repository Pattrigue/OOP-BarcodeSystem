using BarcodeSystem.Users;

namespace BarcodeSystem.CsvDataAccess
{
    public sealed class UserCsvData : ICsvData
    {
        private uint id;
        
        private string firstName;
        private string lastName;
        private string username;

        private decimal balance;
        
        private string email;

        public void ReadLine(char separator, string csvLine)
        {
            string[] fields = csvLine.Split(separator);

            id = uint.Parse(fields[0]);
            firstName = fields[1];
            lastName = fields[2];
            username = fields[3];
            balance = decimal.Parse(fields[4]);
            email = fields[5];
        }

        public static explicit operator User(UserCsvData userCsvData)
        {
            return new User(userCsvData.id, userCsvData.firstName, userCsvData.lastName, userCsvData.username, userCsvData.email, userCsvData.balance);
        }
    }
}