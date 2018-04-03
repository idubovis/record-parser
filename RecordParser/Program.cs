﻿using System;
using System.Collections.Generic;

namespace RecordParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Record Parser Console Application.\nDeveloped by Ilya Dubovis. April 2018.");

            var recordSet = RecordSet.CreateRecordSet(new string[] 
            {
                @"../../App_Data/records1.txt",
                @"../../App_Data/records2.txt",
                @"../../App_Data/records3.txt"
            });

            OutputCollection("(0) Original distinct collection of records", recordSet);

            OutputCollection("(1) Records sorted by Gender, then by Last Name, acending", recordSet.SortByGenderAndThenByLastName());
            OutputCollection("(2) Records sorted by Date of Birth, ascending", recordSet.SortByDateOfBirth());
            OutputCollection("(3) Records sorted by Last Name, descending", recordSet.SortByLastNameDescending());

            Console.ReadKey();
        }

        private static void OutputCollection(string title, IEnumerable<Record> collection)
        {
            Console.WriteLine("\n{0}:", title);
            foreach (var record in collection)
                Console.WriteLine(record);
        }
    }
}