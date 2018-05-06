using Accord;
using Accord.IO;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Filters;
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
        static void Main(string[] args)
        {
            // Create a new reader, opening a given path
            ExcelReader excel = new ExcelReader("Intake Inf Cohort 2017.xlsx");
            ExcelReader excelTest = new ExcelReader("Intake Inf Cohort 2017 - Test Case.xlsx");

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
                codibook.Columns[17].ColumnName,
                codibook.Columns[18].ColumnName,
                codibook.Columns[19].ColumnName,
                codibook.Columns[20].ColumnName,
                codibook.Columns[21].ColumnName,
                codibook.Columns[22].ColumnName,
                codibook.Columns[23].ColumnName,
                codibook.Columns[24].ColumnName,
                codibook.Columns[25].ColumnName,
                codibook.Columns[26].ColumnName,
                codibook.Columns[27].ColumnName,
                codibook.Columns[28].ColumnName,
                codibook.Columns[29].ColumnName,
                codibook.Columns[30].ColumnName,
                codibook.Columns[31].ColumnName,
                codibook.Columns[32].ColumnName,
                codibook.Columns[33].ColumnName,
                codibook.Columns[34].ColumnName,
                codibook.Columns[35].ColumnName,
                codibook.Columns[36].ColumnName
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
                new DecisionVariable(codibook.Columns[17].ColumnName, 2),
                new DecisionVariable(codibook.Columns[18].ColumnName, 2),
                new DecisionVariable(codibook.Columns[19].ColumnName, 2),
                new DecisionVariable(codibook.Columns[20].ColumnName, 2),
                new DecisionVariable(codibook.Columns[21].ColumnName, 2),
                new DecisionVariable(codibook.Columns[22].ColumnName, 2),
                new DecisionVariable(codibook.Columns[23].ColumnName, 2),
                new DecisionVariable(codibook.Columns[24].ColumnName, 2),
                new DecisionVariable(codibook.Columns[25].ColumnName, 2),
                new DecisionVariable(codibook.Columns[26].ColumnName, 2),
                new DecisionVariable(codibook.Columns[27].ColumnName, 2),
                new DecisionVariable(codibook.Columns[28].ColumnName, 2),
                new DecisionVariable(codibook.Columns[29].ColumnName, 2),
                new DecisionVariable(codibook.Columns[30].ColumnName, 2),
                new DecisionVariable(codibook.Columns[31].ColumnName, 2),
                new DecisionVariable(codibook.Columns[32].ColumnName, 2),
                new DecisionVariable(codibook.Columns[33].ColumnName, 2),
                new DecisionVariable(codibook.Columns[34].ColumnName, 2),
                new DecisionVariable(codibook.Columns[35].ColumnName, 2),
                new DecisionVariable(codibook.Columns[36].ColumnName, 2),
            };

            // Learn the training instances!
            Accord.MachineLearning.DecisionTrees.DecisionTree tree = id3.Learn(inputs, outputs);

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
                        { codibook.Columns[17].ColumnName, row.ItemArray[17].ToString() },
                        { codibook.Columns[18].ColumnName, row.ItemArray[18].ToString() },
                        { codibook.Columns[19].ColumnName, row.ItemArray[19].ToString() },
                        { codibook.Columns[20].ColumnName, row.ItemArray[20].ToString() },
                        { codibook.Columns[21].ColumnName, row.ItemArray[21].ToString() },
                        { codibook.Columns[22].ColumnName, row.ItemArray[22].ToString() },
                        { codibook.Columns[23].ColumnName, row.ItemArray[23].ToString() },
                        { codibook.Columns[24].ColumnName, row.ItemArray[24].ToString() },
                        { codibook.Columns[25].ColumnName, row.ItemArray[25].ToString() },
                        { codibook.Columns[26].ColumnName, row.ItemArray[26].ToString() },
                        { codibook.Columns[27].ColumnName, row.ItemArray[27].ToString() },
                        { codibook.Columns[28].ColumnName, row.ItemArray[28].ToString() },
                        { codibook.Columns[29].ColumnName, row.ItemArray[29].ToString() },
                        { codibook.Columns[30].ColumnName, row.ItemArray[30].ToString() },
                        { codibook.Columns[31].ColumnName, row.ItemArray[31].ToString() },
                        { codibook.Columns[32].ColumnName, row.ItemArray[32].ToString() },
                        { codibook.Columns[33].ColumnName, row.ItemArray[33].ToString() },
                        { codibook.Columns[34].ColumnName, row.ItemArray[34].ToString() },
                        { codibook.Columns[35].ColumnName, row.ItemArray[35].ToString() },
                        { codibook.Columns[36].ColumnName, row.ItemArray[36].ToString() },
                    });
                }
                catch (Exception)
                {
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
                    Console.WriteLine("Conclusion " + row.ItemArray[0].ToString() + ": " + answer);
                }
            }

            // Read Key
            Console.ReadKey();
        }
    }
}
