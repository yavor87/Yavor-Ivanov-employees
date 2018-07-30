using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Employees.Core
{
    /// <summary>
    /// Reads Employees from a stream.
    /// </summary>
    public class EmployeeReader
    {
        public char Separator { get; set; }

        public CultureInfo DateTimeCulture { get; set; }

        public IEnumerable<EmployeeRecord> EnumerateRecords(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string line;
            string[] tokens;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                tokens = line.Split(Separator);
                yield return ParseEmployeeRecord(tokens);
            }
        }

        public EmployeeRecord ParseEmployeeRecord(string[] tokens)
        {
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
