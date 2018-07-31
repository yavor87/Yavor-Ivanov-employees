using Employees.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Employees.Test
{
    public class EmployeeReaderTests
    {
        [Fact]
        public async void ReadsEmployees_SingleDateFormat()
        {
            // Arrange
            EmployeeReader target = new EmployeeReader();
            target.Separator = ',';
            string fileContents =
                "143, 12, 2013-11-01, 2014-01-05" + Environment.NewLine +
                "218, 10, 2012-05-16, NULL" + Environment.NewLine +
                "143, 10, 2009-01-01, 2011-04-27";
            Stream data = new MemoryStream(Encoding.Default.GetBytes(fileContents));
            IReadOnlyCollection<EmployeeRecord> records;

            // Act
            records = await target.ReadRecordsAsync(data);

            data.Close();

            // Assert
            Assert.NotNull(records);
            Assert.Collection(records, c =>
            {
                Assert.Equal(143, c.EmployeeID);
                Assert.Equal(12, c.ProjectID);
                Assert.Equal(new DateTime(2013, 11, 01), c.DateFrom);
                Assert.Equal(new DateTime(2014, 01, 05), c.DateTo);
            }, c =>
            {
                Assert.Equal(218, c.EmployeeID);
                Assert.Equal(10, c.ProjectID);
                Assert.Equal(new DateTime(2012, 05, 16), c.DateFrom);
                Assert.Equal(DateTime.Today, c.DateTo);
            }, c =>
            {
                Assert.Equal(143, c.EmployeeID);
                Assert.Equal(10, c.ProjectID);
                Assert.Equal(new DateTime(2009, 01, 01), c.DateFrom);
                Assert.Equal(new DateTime(2011, 04, 27), c.DateTo);
            });
        }

        [Theory]
        [InlineData("dd/MM/yyyy")]
        [InlineData("dd-MM-yyyy")]
        [InlineData("d-M-yyyy")]
        public async void ReadsEmployees_DateFormat(string formatString)
        {
            // Arrange
            EmployeeReader target = new EmployeeReader();
            target.Separator = ',';
            string fileContents =
                $"143, 12, {new DateTime(2013, 11, 01).ToString(formatString, CultureInfo.InvariantCulture)}" +
                    $", {new DateTime(2014, 01, 05).ToString(formatString, CultureInfo.InvariantCulture)}" + Environment.NewLine +
                $"218, 10, {new DateTime(2012, 05, 16).ToString(formatString, CultureInfo.InvariantCulture)}, NULL" + Environment.NewLine +
                $"143, 10, {new DateTime(2009, 01, 01).ToString(formatString, CultureInfo.InvariantCulture)}," +
                    $" {new DateTime(2011, 04, 27).ToString(formatString, CultureInfo.InvariantCulture)}";
            Stream data = new MemoryStream(Encoding.Default.GetBytes(fileContents));
            IReadOnlyCollection<EmployeeRecord> records;

            // Act
            records = await target.ReadRecordsAsync(data);

            data.Close();

            // Assert
            Assert.NotNull(records);
            Assert.Collection(records, c =>
            {
                Assert.Equal(143, c.EmployeeID);
                Assert.Equal(12, c.ProjectID);
                Assert.Equal(new DateTime(2013, 11, 01), c.DateFrom);
                Assert.Equal(new DateTime(2014, 01, 05), c.DateTo);
            }, c =>
            {
                Assert.Equal(218, c.EmployeeID);
                Assert.Equal(10, c.ProjectID);
                Assert.Equal(new DateTime(2012, 05, 16), c.DateFrom);
                Assert.Equal(DateTime.Today, c.DateTo);
            }, c =>
            {
                Assert.Equal(143, c.EmployeeID);
                Assert.Equal(10, c.ProjectID);
                Assert.Equal(new DateTime(2009, 01, 01), c.DateFrom);
                Assert.Equal(new DateTime(2011, 04, 27), c.DateTo);
            });
        }

        [Fact]
        public async void ReadsEmployees_SpecifiedDateCulture()
        {
            // Arrange
            EmployeeReader target = new EmployeeReader();
            target.DateTimeCulture = new CultureInfo("en-US");
            target.Separator = ',';
            string fileContents =
                $"143, 12, {new DateTime(2013, 11, 01).ToString(target.DateTimeCulture)}" +
                    $", {new DateTime(2014, 01, 05).ToString(target.DateTimeCulture)}" + Environment.NewLine +
                $"218, 10, {new DateTime(2012, 05, 16).ToString(target.DateTimeCulture)}, NULL" + Environment.NewLine +
                $"143, 10, {new DateTime(2009, 01, 01).ToString(target.DateTimeCulture)}," +
                    $" {new DateTime(2011, 04, 27).ToString(target.DateTimeCulture)}";
            Stream data = new MemoryStream(Encoding.Default.GetBytes(fileContents));
            IReadOnlyCollection<EmployeeRecord> records;

            // Act
            records = await target.ReadRecordsAsync(data);

            data.Close();

            // Assert
            Assert.NotNull(records);
            Assert.Collection(records, c =>
            {
                Assert.Equal(143, c.EmployeeID);
                Assert.Equal(12, c.ProjectID);
                Assert.Equal(new DateTime(2013, 11, 01), c.DateFrom);
                Assert.Equal(new DateTime(2014, 01, 05), c.DateTo);
            }, c =>
            {
                Assert.Equal(218, c.EmployeeID);
                Assert.Equal(10, c.ProjectID);
                Assert.Equal(new DateTime(2012, 05, 16), c.DateFrom);
                Assert.Equal(DateTime.Today, c.DateTo);
            }, c =>
            {
                Assert.Equal(143, c.EmployeeID);
                Assert.Equal(10, c.ProjectID);
                Assert.Equal(new DateTime(2009, 01, 01), c.DateFrom);
                Assert.Equal(new DateTime(2011, 04, 27), c.DateTo);
            });
        }

        [Fact]
        public void WrongEmployeeID_ThrowsException()
        {
            // Arrange
            EmployeeReader target = new EmployeeReader();
            target.Separator = ',';
            string line = "14t, 12, 2013-11-01, 2014-01-05";

            // Act
            Assert.Throws<FormatException>(() => target.ParseEmployeeRecord(line));
        }

        [Fact]
        public void WrongProjectID_ThrowsException()
        {
            // Arrange
            EmployeeReader target = new EmployeeReader();
            target.Separator = ',';
            string line = "143, I2, 2013-11-01, 2014-01-05";

            // Act
            Assert.Throws<FormatException>(() => target.ParseEmployeeRecord(line));
        }

        [Fact]
        public void UnknownDateFormat_ThrowsException()
        {
            // Arrange
            EmployeeReader target = new EmployeeReader();
            target.Separator = ',';
            string line = "218, 10, 15-16-2012, NULL";

            // Act
            Assert.Throws<FormatException>(() => target.ParseEmployeeRecord(line));
        }

        [Fact]
        public void NotUniformColumns_ThrowsException()
        {
            // Arrange
            EmployeeReader target = new EmployeeReader();
            target.Separator = ',';
            string fileContents =
                "143, 11, 2013-11-01, 2014-01-05" + Environment.NewLine +
                "218, 10, 15-16-2012" + Environment.NewLine +
                "143, 10, 2009-01-01, 2011-04-27";
            Stream data = new MemoryStream(Encoding.Default.GetBytes(fileContents));

            // Act
            Assert.ThrowsAsync<InvalidDataException>(async () => await target.ReadRecordsAsync(data));

            data.Close();
        }
    }
}
