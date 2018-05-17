using Accord;
using Accord.IO;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Filters;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Program
    {
        static Codification Codification { get; set; }

        static void Main(string[] args)
        {
            // Create a new reader, opening a given path
            ExcelReader excel = new ExcelReader("Intake Inf Cohort 2017 - Training Set.xlsx");
            ExcelReader excelTest = new ExcelReader("Intake Inf Cohort 2017 - Test Set.xlsx");

            // Afterwards, we can query the file for all
            // worksheets within the specified workbook:
            string[] sheets = excel.GetWorksheetList();
            string[] sheetsTest = excelTest.GetWorksheetList();

            // Finally, we can request an specific sheet:
            DataTable data = excel.GetWorksheet(sheets[0]);
            DataTable dataTest = excelTest.GetWorksheet(sheets[0]);

            // Loop through each column in data
            foreach (DataColumn column in data.Columns)
            {
                // Replace empty with underscore
                column.ColumnName = column.ColumnName.Replace(" ", "_");
            }

            // Create a new codification codebook to 
            // convert strings into integer symbols
            Codification codibook = new Codification(data);

            // Set codibook
            Codification = codibook;

            // Translate our training data into integer symbols using our codebook:
            DataTable symbols = codibook.Apply(data);
            int[][] inputs = symbols.ToJagged<int>(
                codibook.Columns[5].ColumnName,
                codibook.Columns[7].ColumnName,
                codibook.Columns[8].ColumnName,
                codibook.Columns[9].ColumnName,
                codibook.Columns[12].ColumnName,
                codibook.Columns[13].ColumnName,
                codibook.Columns[14].ColumnName,
                codibook.Columns[15].ColumnName,
                codibook.Columns[16].ColumnName,
                codibook.Columns[20].ColumnName,
                codibook.Columns[29].ColumnName,
                codibook.Columns[30].ColumnName,
                codibook.Columns[34].ColumnName
            );
            int[] outputs = symbols.ToMatrix<int>(codibook.Columns[6].ColumnName).GetColumn(0);

            // Create a teacher ID3 algorithm
            var id3 = new ID3Learning()
            {
                new DecisionVariable(codibook.Columns[5].ColumnName, 2),
                new DecisionVariable(codibook.Columns[7].ColumnName, codibook.Columns[7].NumberOfSymbols),
                new DecisionVariable(codibook.Columns[8].ColumnName, codibook.Columns[8].NumberOfSymbols),
                new DecisionVariable(codibook.Columns[9].ColumnName, 3),
                new DecisionVariable(codibook.Columns[12].ColumnName, 10),
                new DecisionVariable(codibook.Columns[13].ColumnName, 10),
                new DecisionVariable(codibook.Columns[14].ColumnName, 10),
                new DecisionVariable(codibook.Columns[15].ColumnName, 10),
                new DecisionVariable(codibook.Columns[16].ColumnName, 2),
                new DecisionVariable(codibook.Columns[20].ColumnName, 2),
                new DecisionVariable(codibook.Columns[29].ColumnName, 2),
                new DecisionVariable(codibook.Columns[30].ColumnName, 2),
                new DecisionVariable(codibook.Columns[34].ColumnName, 2),
            };

            // Learn the training instances!
            Accord.MachineLearning.DecisionTrees.DecisionTree tree = id3.Learn(inputs, outputs);

            // Create a console table for display
            ConsoleTable table = new ConsoleTable("Studentnumber", "Advice", "Conclusion");

            // Loop through each row in data
            foreach (DataRow row in dataTest.Rows)
            {

                // The tree can now be queried for new examples through 
                // its decide method. For example, we can create a query
                int[] query = null;

                try
                {
                    query = codibook.Transform(new[,]
                    {
                        { codibook.Columns[5].ColumnName, row.ItemArray[5].ToString() },
                        { codibook.Columns[7].ColumnName, row.ItemArray[7].ToString() },
                        { codibook.Columns[8].ColumnName, row.ItemArray[8].ToString() },
                        { codibook.Columns[9].ColumnName, row.ItemArray[9].ToString() },
                        { codibook.Columns[12].ColumnName, row.ItemArray[12].ToString() },
                        { codibook.Columns[13].ColumnName, row.ItemArray[13].ToString() },
                        { codibook.Columns[14].ColumnName, row.ItemArray[14].ToString() },
                        { codibook.Columns[15].ColumnName, row.ItemArray[15].ToString() },
                        { codibook.Columns[16].ColumnName, row.ItemArray[16].ToString() },
                        { codibook.Columns[20].ColumnName, row.ItemArray[20].ToString() },
                        { codibook.Columns[29].ColumnName, row.ItemArray[29].ToString() },
                        { codibook.Columns[30].ColumnName, row.ItemArray[30].ToString() },
                        { codibook.Columns[34].ColumnName, row.ItemArray[34].ToString() },
                    });
                }
                catch (Exception)
                {
                    // Show the result of skipped students 
                    var studentnumber = row.ItemArray[0].ToString();
                    var advice = row.ItemArray[6].ToString();
                    var conclusion = "(Twijfel)";
                    table.AddRow(studentnumber, advice, conclusion);

                    continue;
                }

                // And then predict the label using
                int predicted = tree.Decide(query);

                // Any predictions off are ignored for consistency
                if (predicted != -1)
                {
                    // We can translate it back to strings using
                    string answer = codibook.Revert("advies", predicted);

                    // Show the result in the output
                    var studentnumber = row.ItemArray[0].ToString();
                    var advice = row.ItemArray[6].ToString();
                    var conclusion = answer;
                    table.AddRow(studentnumber, advice, conclusion);
                }
                else
                {
                    // Show the result of skipped students  
                    var studentnumber = row.ItemArray[0].ToString();
                    var advice = row.ItemArray[6].ToString();
                    var conclusion = "(Twijfel)";
                    table.AddRow(studentnumber, advice, conclusion);
                }
            }

            // Write the table in console
            table.Write();

            // Read Key
            Console.ReadKey();
        }
    }
}
