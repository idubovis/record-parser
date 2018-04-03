using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace RecordParser.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParameterStringWithInvalidDelimiter()
        {
            var recordString = "Doe; John; Male; Blue; 2/10/90";
            var record = RecordSet.CreateRecord(recordString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInsufficientParameterString()
        {
            var recordString = "Doe, John, Male";
            var record = RecordSet.CreateRecord(recordString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParameterStringInWrongOrder()
        {
            var recordString = "Doe | John | Male| 2/10/90 | Blue";
            var record = RecordSet.CreateRecord(recordString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidGenderParameter()
        {
            var recordString = "Doe John Man Blue 2/10/90";
            var record = RecordSet.CreateRecord(recordString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidDateOfBirthParameter()
        {
            var recordString = "Doe John Male Blue 22/10/90";
            var record = RecordSet.CreateRecord(recordString);
        }

        [TestMethod]
        public void TestRecordComparison()
        {
            var record = new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10));

            var inputString1 = "Doe, John, Male, Blue, 02/10/90";   // comma-delimited representation of the above record
            var inputString2 = "Doe John Male Blue 2/10/1990";      // space-delimited representation of the above record
            var inputString3 = "Doe | John | M | Blue | 2/10/90";   // pipe-delimited representation of the above record

            var inputString4 = "Doe, John, Male, Blue, 12/10/2005";// different DOB
            var inputString5 = "Doe Joseph Male Blue 2/10/90";     // different First Name

            Assert.AreEqual(record, RecordSet.CreateRecord(inputString1));
            Assert.AreEqual(record, RecordSet.CreateRecord(inputString2));
            Assert.AreEqual(record, RecordSet.CreateRecord(inputString3));

            Assert.AreNotEqual(record, RecordSet.CreateRecord(inputString4));
            Assert.AreNotEqual(record, RecordSet.CreateRecord(inputString5));
        }

        [TestMethod]
        public void TestRecordSetSorting()
        {
            var records = new RecordSet {
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10)),
                new Record("Brown", "Amy", Record.Genders.Female, "Orange", new DateTime(1995, 3, 13)), // duplicate => should be eliminated by RecordSet
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1999, 6, 20)),    // not a duplicate: different DOB 
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31))
            };

            var recordsSortedByGenderAndThenByLastName = new RecordSet {
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1999, 6, 20)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10)),
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10))
            };

            var recordsSortedByBirthDate = new RecordSet {
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1999, 6, 20)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10))
            };

            var recordsSortedByLastNameDescending = new RecordSet {
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1999, 6, 20)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31))
            };

            var recordsSortedByLastNameAndThenByFirstName = new RecordSet {
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1999, 6, 20)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10)),
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
            };

            Assert.IsTrue(recordsSortedByGenderAndThenByLastName
                            .SequenceEqual(records.SortByGenderAndThenByLastName()));
            Assert.IsTrue(recordsSortedByBirthDate
                            .SequenceEqual(records.SortByDateOfBirth()));
            Assert.IsTrue(recordsSortedByLastNameDescending
                            .SequenceEqual(records.SortByLastNameDescending()));
            Assert.IsTrue(recordsSortedByLastNameAndThenByFirstName
                            .SequenceEqual(records.SortByLastNameThenByFirstName()));
        }
    }
}
