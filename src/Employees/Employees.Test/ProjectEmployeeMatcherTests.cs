using Employees.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace Employees.Test
{
    public class ProjectEmployeeMatcherTests
    {
        [Fact]
        public void FindsCorrectMatch_SingleMatchInProduct()
        {
            // Arrange
            EmployeeRecord[] employeeRecord = new EmployeeRecord[]
            {
                new EmployeeRecord(1, 1, new DateTime(2012, 1, 1), new DateTime(2014, 6, 1)),
                new EmployeeRecord(2, 1, new DateTime(2013, 1, 1), new DateTime(2015, 6, 1)),
                new EmployeeRecord(3, 1, new DateTime(2015, 7, 1), new DateTime(2018, 6, 1)),
                new EmployeeRecord(1, 2, new DateTime(2010, 1, 1), new DateTime(2012, 6, 1)),
                new EmployeeRecord(2, 2, new DateTime(2013, 1, 1), new DateTime(2013, 6, 1)),
            };
            int daysWorkedTogether = (new DateTime(2014, 6, 1) - new DateTime(2013, 1, 1)).Days;

            ProjectEmployeeMatcher matcher = new ProjectEmployeeMatcher();
            IEnumerable<ProjectEmployeeMatch> matches;

            // Act
            matches = matcher.FindMatches(employeeRecord);

            // Assert
            Assert.NotNull(matches);
            Assert.Collection(matches, c =>
            {
                Assert.Equal(1, c.EmployeeID1);
                Assert.Equal(2, c.EmployeeID2);
                Assert.Equal(1, c.ProjectID);
                Assert.Equal(daysWorkedTogether, c.DaysWorked);
            });
        }

        [Fact]
        public void FindsCorrectMatch_DoubleMatchInSingleProductSamePair()
        {
            // Arrange
            EmployeeRecord[] employeeRecord = new EmployeeRecord[]
            {
                new EmployeeRecord(1, 1, new DateTime(2012, 1, 1), new DateTime(2014, 6, 1)),
                new EmployeeRecord(2, 1, new DateTime(2013, 1, 1), new DateTime(2015, 6, 1)),
                new EmployeeRecord(3, 1, new DateTime(2010, 7, 1), new DateTime(2011, 6, 1)),
                new EmployeeRecord(1, 2, new DateTime(2010, 1, 1), new DateTime(2012, 6, 1)),
                new EmployeeRecord(2, 2, new DateTime(2013, 1, 1), new DateTime(2013, 6, 1)),
                new EmployeeRecord(1, 1, new DateTime(2016, 1, 1), new DateTime(2018, 2, 1)),
                new EmployeeRecord(2, 1, new DateTime(2017, 1, 1), new DateTime(2018, 8, 1)),
            };
            int daysWorkedTogether1 = (new DateTime(2014, 6, 1) - new DateTime(2013, 1, 1)).Days;
            int daysWorkedTogether2 = (new DateTime(2018, 2, 1) - new DateTime(2017, 1, 1)).Days;

            ProjectEmployeeMatcher matcher = new ProjectEmployeeMatcher();
            IEnumerable<ProjectEmployeeMatch> matches;

            // Act
            matches = matcher.FindMatches(employeeRecord);

            // Assert
            Assert.NotNull(matches);
            Assert.Collection(matches, c =>
            {
                Assert.Equal(1, c.EmployeeID1);
                Assert.Equal(2, c.EmployeeID2);
                Assert.Equal(1, c.ProjectID);
                Assert.Equal(daysWorkedTogether1 + daysWorkedTogether2, c.DaysWorked);
            });
        }

        [Fact]
        public void FindsCorrectMatch_DoubleMatchInTwoProducts()
        {
            // Arrange
            EmployeeRecord[] employeeRecord = new EmployeeRecord[]
            {
                new EmployeeRecord(1, 1, new DateTime(2012, 1, 1), new DateTime(2014, 6, 1)),
                new EmployeeRecord(2, 1, new DateTime(2013, 1, 1), new DateTime(2015, 6, 1)),
                new EmployeeRecord(3, 1, new DateTime(2015, 7, 1), new DateTime(2018, 6, 1)),
                new EmployeeRecord(1, 2, new DateTime(2010, 1, 1), new DateTime(2012, 6, 1)),
                new EmployeeRecord(2, 2, new DateTime(2013, 1, 1), new DateTime(2013, 6, 1)),
                new EmployeeRecord(3, 2, new DateTime(2011, 1, 1), new DateTime(2011, 6, 1)),
            };
            int daysWorkedTogether1 = (new DateTime(2014, 6, 1) - new DateTime(2013, 1, 1)).Days;
            int daysWorkedTogether2 = (new DateTime(2011, 6, 1) - new DateTime(2011, 1, 1)).Days;

            ProjectEmployeeMatcher matcher = new ProjectEmployeeMatcher();
            IEnumerable<ProjectEmployeeMatch> matches;

            // Act
            matches = matcher.FindMatches(employeeRecord);

            // Assert
            Assert.NotNull(matches);
            Assert.Collection(matches, c =>
            {
                Assert.Equal(1, c.EmployeeID1);
                Assert.Equal(2, c.EmployeeID2);
                Assert.Equal(1, c.ProjectID);
                Assert.Equal(daysWorkedTogether1, c.DaysWorked);
            }, c =>
            {
                Assert.Equal(1, c.EmployeeID1);
                Assert.Equal(3, c.EmployeeID2);
                Assert.Equal(2, c.ProjectID);
                Assert.Equal(daysWorkedTogether2, c.DaysWorked);
            });
        }

        [Fact]
        public void FindsNoMatch()
        {
            // Arrange
            EmployeeRecord[] employeeRecord = new EmployeeRecord[]
            {
                new EmployeeRecord(1, 1, new DateTime(2012, 1, 1), new DateTime(2014, 6, 1)),
                new EmployeeRecord(3, 1, new DateTime(2015, 7, 1), new DateTime(2018, 6, 1)),
                new EmployeeRecord(1, 2, new DateTime(2010, 1, 1), new DateTime(2012, 6, 1)),
                new EmployeeRecord(2, 2, new DateTime(2013, 1, 1), new DateTime(2013, 6, 1)),
            };
            int daysWorkedTogether = (new DateTime(2014, 6, 1) - new DateTime(2013, 1, 1)).Days;

            ProjectEmployeeMatcher matcher = new ProjectEmployeeMatcher();
            IEnumerable<ProjectEmployeeMatch> matches;

            // Act
            matches = matcher.FindMatches(employeeRecord);

            // Assert
            Assert.NotNull(matches);
            Assert.Empty(matches);
        }
    }
}
