using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecordParser
{
    public class RecordSet : HashSet<Record>
    {
        /// <summary>
        /// Create a RecordSet by collecting distinct records from given input files
        /// </summary>
        /// <param name="inputFiles">Array of input text files</param> 
        public static RecordSet CreateRecordSet(string[] inputFiles)
        {
            var recordSet = new RecordSet();
            foreach (var file in inputFiles)
            {
                try
                {
                    recordSet.UnionWith(GetRecordsFromFile(file));
                }
                catch (FileNotFoundException e)
                {
                    // if an input file does not exist, log the exception and continue
                    Console.WriteLine("Exception: {0} {1}", e.Message, e.FileName);
                }
            }
            return recordSet;
        }

        /// <summary>
        /// Create a RecordSet by collecting distinct records from a given input file
        /// </summary>
        /// <param name="filepath">input file path</param> 
        /// <exception cref="FileNotFoundException">Thrown when input file was not found</exception>
        private static RecordSet GetRecordsFromFile(string filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException("Input file was not found", filepath);

            var recordSet = new RecordSet();

            string line;
            using (var file = new StreamReader(filepath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        recordSet.Add(CreateRecord(line));
                    }
                    catch (ArgumentException e)
                    {
                        // if a line is invalid, log the exception and continue
                        Console.WriteLine("Exception: {0} in input file {1}.", e.Message, filepath);
                    }
                }
            }
            return recordSet;
        }

        private static IEnumerable<Record> GetRecordsFromFile1(string filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException("Input file was not found", filepath);

            var recordSet = new RecordSet();

            string line;
            using (var file = new StreamReader(filepath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    Record record = null;
                    try
                    {
                        record = CreateRecord(line);
                    }
                    catch (ArgumentException e)
                    {
                        // if a line is invalid, log the exception and continue
                        Console.WriteLine("Exception: {0} in input file {1}.", e.Message, filepath);
                        continue;
                    }
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Parse a record from a text file line and create an instance of Record class
        /// </summary>
        /// <param name="parameterString">A string of parameters delimited by commas, pipes or whitespaces</param> 
        /// <exception cref="ArgumentException">Thrown when parameter string is NULL or contains invalid data</exception>
        public static Record CreateRecord(string parameterString)
        {
            if (string.IsNullOrWhiteSpace(parameterString))
                throw new ArgumentNullException("parameterString");

            var parameters = parameterString.Split(new string[] { ",", "|", " " }, StringSplitOptions.RemoveEmptyEntries);

            // parameter string should have exactly 5 significant parameters in the following order: 
            // LastName, FirstName, Gender, FavoriteColor, DateOfBirth

            if (parameters.Length != 5)
                throw new ArgumentException("Parameter string parsing exception: Expected: 5 parameters, Actual: " + parameters.Length + " parameters.");

            var gender = Record.Genders.Invalid;
            if (parameters[2].Equals("Female", StringComparison.InvariantCultureIgnoreCase) || parameters[2].Equals("F", StringComparison.InvariantCultureIgnoreCase))
                gender = Record.Genders.Female;
            else if (parameters[2].Equals("Male", StringComparison.InvariantCultureIgnoreCase) || parameters[2].Equals("M", StringComparison.InvariantCultureIgnoreCase))
                gender = Record.Genders.Male;
            else
                throw new ArgumentException("Invalid Gender parameter value: " + parameters[2]);

            DateTime birthdate;
            if (!DateTime.TryParse(parameters[4], out birthdate))
                throw new ArgumentException("Invalid Date of Birth parameter value: " + parameters[4]);

            return new Record(
                parameters[0], // last name
                parameters[1], // first name
                gender,
                parameters[3], // favorite color: assumed color validation is not required
                birthdate
            );
        }

        public IEnumerable<Record> SortByGenderAndThenByLastName() => this.OrderBy(r => r.Gender).ThenBy(r => r.LastName);

        public IEnumerable<Record> SortByDateOfBirth() => this.OrderBy(r => r.DateOfBirth);

        public IEnumerable<Record> SortByLastNameDescending() => this.OrderByDescending(r => r.LastName);

        public IEnumerable<Record> SortByLastNameThenByFirstName() => this.OrderBy(r => r.LastName).ThenBy(r => r.FirstName);

        public IEnumerable<Record> SortByColor() 
        {
            //this.ToList<Record>().Sort(new ComparerByColorThenByLastName());
            SortedSet<Record> set = new SortedSet<Record>(this, new ComparerByColorThenByLastName());
            return set;
        }

        class ComparerByLastName : IComparer<Record>
        {
            public int Compare(Record x, Record y)
            {
                return x.LastName.CompareTo(y.LastName);
            }
        }

        class ComparerByColorThenByLastName : IComparer<Record>
        {
            public int Compare(Record x, Record y)
            {
                var result = x.FavoriteColor.CompareTo(y.FavoriteColor);
                if(result == 0)
                    return x.LastName.CompareTo(y.LastName);
                return result;
            }
        }


    }
}

