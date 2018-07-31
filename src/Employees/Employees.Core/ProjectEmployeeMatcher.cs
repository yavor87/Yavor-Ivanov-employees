using System;
using System.Collections.Generic;
using System.Linq;

namespace Employees.Core
{
    /// <summary>
    /// Matches employees based on how many total days they have worked on a project.
    /// </summary>
    public class ProjectEmployeeMatcher
    {
        public IEnumerable<ProjectEmployeeMatch> FindMatches(IReadOnlyCollection<EmployeeRecord> employees)
        {
            Dictionary<Tuple<int, int>, ProjectEmployeeMatch> matches = new Dictionary<Tuple<int, int>, ProjectEmployeeMatch>();
            Comparer<EmployeeRecord> comparer = Comparer<EmployeeRecord>.Create((x, y) => x.EmployeeID.CompareTo(y.EmployeeID));

            var employeesByProjects = employees.GroupBy(empl => empl.ProjectID);
            foreach (var employeesByProject in employeesByProjects)
            {
                int projectID = employeesByProject.Key;
                var employeeCombinations = GetKCombinations(employeesByProject, comparer);
                foreach (var employeeCombination in employeeCombinations)
                {
                    DateTime startA = employeeCombination.Item1.DateFrom;
                    DateTime startB = employeeCombination.Item2.DateFrom;
                    DateTime endA = employeeCombination.Item1.DateTo;
                    DateTime endB = employeeCombination.Item2.DateTo;

                    if ((endB < startA) || (startB > endA))
                    {
                        continue;
                    }

                    int daysDiff = Math.Min(
                        Math.Min((endA - startA).Days, (endA - startB).Days),
                        Math.Min((endB - startB).Days, (endB - startA).Days));

                    ProjectEmployeeMatch match;
                    var key = new Tuple<int, int>(employeeCombination.Item1.EmployeeID, employeeCombination.Item2.EmployeeID);
                    if (matches.TryGetValue(key, out match))
                    {
                        match.DaysWorked += daysDiff;
                    }
                    else
                    {
                        match = new ProjectEmployeeMatch(projectID, employeeCombination.Item1.EmployeeID,
                            employeeCombination.Item2.EmployeeID, daysDiff);
                        matches.Add(key, match);
                    }
                }
            }

            return matches.Values;
        }

        static IEnumerable<Tuple<T, T>> GetKCombinations<T>(IEnumerable<T> list, IComparer<T> comparer)
        {
            return list.SelectMany(t => list.Where(o => comparer.Compare(o, t) > 0),
                (t1, t2) => Tuple.Create(t1, t2));
        }
    }
}
