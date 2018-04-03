# record-parser

RecordParser solution is created in Visual Studio 2017. Target framework: .NET Framework 4.6.1.

RecordParser solution contains 4 projects:
1. RecordParser - console application that parses and collects distinct records from 3 text files, and outputs them in 3 ordered views:
- sorted by gender (females before males), then by last name, ascending;
- sorted by birth date, ascending;
- sorted by last name, descending.

2. RecordParser.Tests - unit tests for RecordParser application.

3. RecordParser.WebApp - RESTful web service that uses entities from RecordParser application to parse and sort data, and provides API with the following endpoints:
- GET /records - returns all records in the original order;
- GET /records/gender - returns records sorted by gender (the same as the correspondent sorting from the console app);
- GET /records/birthdate - returns records sorted by birthdate (the same as the correspondent sorting from the console app);
- GET /records/name - returns records sorted by name (sorted by last name, then by first name, ascending);
- POST /records - posts a single data line in any of the 3 formats supported by RecordParser.

  All endpoints return JSON-formatted result.

4. RecordParser.WebApp.Tests - unit tests for RecordParser.WebApp application.

Assumptions:
- The delimiters (commas, pipes and spaces) do not appear anywhere in the data values themselves;
- FavoriteColor value does not require validation;
- Persistent datastore is not required for the web service.

The repository also contains packages referenced by the web application and test applications.
