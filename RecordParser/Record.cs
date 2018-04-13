using System;

namespace RecordParser
{
    public class Record : IEquatable<Record>
    {
        public enum Genders { Invalid = -1, Female, Male };

        private static int RecordID = 1;

        public int Id { get; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public Genders Gender { get; private set; }
        public string FavoriteColor { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        public Record(string lastName, string firstName, Genders gender, string favoriteColor, DateTime dateOfBirth)
        {
            Id = RecordID++;
            LastName = lastName;
            FirstName = firstName;
            Gender = gender;
            FavoriteColor = favoriteColor; 
            DateOfBirth = dateOfBirth;
        }
             
        public override string ToString()
        {
            var gender = Gender == Genders.Female ? "Female" : "Male";
            var birthdate = string.Format("{0:M/d/yyyy}", DateOfBirth);
            return $"{Id}, {LastName}, {FirstName}, {gender}, {FavoriteColor}, {birthdate}";
        }

        // The below methods are necessary to prevent duplicates in RecordSet

        public bool Equals(Record other)
        {
            // consider records equal if LastName, FirstName, Gender and DateOfBirth are equal
            // Favorite Color is not considered
            return LastName.Equals(other.LastName, StringComparison.InvariantCultureIgnoreCase)
                && FirstName.Equals(other.FirstName, StringComparison.InvariantCultureIgnoreCase)
                && Gender == other.Gender
                && DateOfBirth.Equals(other.DateOfBirth);
        }

        public override bool Equals(object obj) => this.Equals(obj as Record);

        // build a unique hashcode based on 4 fields
        public override int GetHashCode() =>
            LastName.ToUpper().GetHashCode() ^ FirstName.ToUpper().GetHashCode() ^ Gender.GetHashCode() ^ DateOfBirth.GetHashCode();
    }
}
