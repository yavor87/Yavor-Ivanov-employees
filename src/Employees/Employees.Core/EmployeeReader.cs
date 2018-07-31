using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Employees.Core
{
    /// <summary>
    /// Reads Employees from a stream.
    /// </summary>
    public class EmployeeReader
    {
        public EmployeeReader()
        {
            this.Separator = ',';
        }

        public char Separator { get; set; }

        public CultureInfo DateTimeCulture { get; set; }

        public async Task<IReadOnlyCollection<EmployeeRecord>> ReadRecordsAsync(Stream stream)
        {
            List<EmployeeRecord> records = new List<EmployeeRecord>();
            StreamReader reader = new StreamReader(stream);
            string line;
            while (!reader.EndOfStream)
            {
                line = await reader.ReadLineAsync();
                records.Add(ParseEmployeeRecord(line));
            }
            return records;
        }

        public EmployeeRecord ParseEmployeeRecord(string line)
        {
            string[] tokens = line.Split(this.Separator);
            if (tokens.Length != 4)
                throw new InvalidDataException("Employee record not formatted correctly");

            EmployeeRecord employee = new EmployeeRecord();
            employee.EmployeeID = int.Parse(tokens[0]);
            employee.ProjectID = int.Parse(tokens[1]);
            employee.DateFrom = this.ParseDate(tokens[2]);
            employee.DateTo = !StringComparer.InvariantCultureIgnoreCase.Equals(tokens[3].Trim(), "NULL") ?
                this.ParseDate(tokens[3]) : DateTime.Today;
            return employee;
        }

        private DateTime ParseDate(string dateString)
        {
            DateTime result;
            if (DateTimeCulture != null)
                return DateTime.Parse(dateString, DateTimeCulture);

            if (DateTime.TryParse(dateString, out result))
            {
                return result;
            }
            return DateTime.Parse(dateString, CultureInfo.InvariantCulture);
        }
    }
}
