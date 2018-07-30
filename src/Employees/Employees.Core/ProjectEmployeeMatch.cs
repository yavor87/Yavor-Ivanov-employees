namespace Employees.Core
{
    /// <summary>
    /// Represents how many days have <see cref="EmployeeID1"/> worked together with <see cref="EmployeeID2"/>
    /// on project <see cref="ProjectID"/>.
    /// </summary>
    public class ProjectEmployeeMatch
    {
        public ProjectEmployeeMatch(int projectID, int employeeID1, int employeeID2, int daysWorked)
        {
            ProjectID = projectID;
            EmployeeID1 = employeeID1;
            EmployeeID2 = employeeID2;
            DaysWorked = daysWorked;
        }

        public int EmployeeID1 { get; set; }
        public int EmployeeID2 { get; set; }
        public int ProjectID { get; set; }
        public int DaysWorked { get; set; }
    }
}
