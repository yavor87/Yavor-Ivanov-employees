using System;

namespace Employees.Core
{
    /// <summary>
    /// Represents a single employee record as obtained from a file.
    /// </summary>
    public class EmployeeRecord
    {
        public EmployeeRecord()
        { }

        public EmployeeRecord(int employeeID, int projectID, DateTime dateFrom, DateTime dateTo)
        {
            EmployeeID = employeeID;
            ProjectID = projectID;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
