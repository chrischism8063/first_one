using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Program_3_Chism
{
    class Sudoku_Tester
    {
        int[,] puzzle;
        private int ROW_LENGTH = 9;
        //private int COLUMN_LENGTH = 9;
        String[, ,] testResults;        

        public Sudoku_Tester()
        {
            puzzle = new int[9, 9];
            testResults = new string[3, 9, 4];
        }//END constructor

        public void LoadSolution(String filename)
        {
            String[] substrings;

            try
            {
                StreamReader sr = new StreamReader(filename);

                using (sr)
                {
                    string line;
                    for (int a = 0; a < ROW_LENGTH; a++)
                    {
                        line = sr.ReadLine();
                        char delimiter = ',';
                        substrings = line.Split(delimiter);
                        int b = 0;
                        foreach (String subs in substrings)
                        {
                            puzzle[a, b] = int.Parse(substrings[b]);
                            b++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}\n", e.Message);
            }
        }//END method loadSolution

        //Used to display the board once the solution has been loaded
        public void showBoard()
        {
            Console.WriteLine("LOADED SOLUTION FROM FILE");
            Console.WriteLine("-------------------");
            for (int a = 0; a < 9; a++)
            {
                Console.Write("|");
                for (int b = 0; b < 9; b++)
                {
                    Console.Write("{0}", puzzle[a, b]);
                    Console.Write("|");
                }
                Console.Write("\n");
            }
            Console.WriteLine("-------------------");
        }//END method showBoard

        //Check all rows to see if sum adds up to 45
        //Good is TRUE
        public bool ScanRows()
        {
            bool rowSumGood = true;
            string badRow;

            for (int row = 0; row < 9; row++)
            {
                for (int b = 0; b < 9; b++)
                {
                    for (int column = b + 1; column < 9; column++)
                    {
                        if (puzzle[row, b].Equals(puzzle[row, column]))
                        {
                            badRow = row.ToString();
                            testResults[0, row, 0] = "false";
                            testResults[0, row, 1] = "ROW:  " + badRow;
                            rowSumGood = false;
                            break;
                        }
                    }
                }
            }
            return rowSumGood;
        }//END method scanRows

        //Check all columns to see if sum adds up to 45
        //Good is TRUE
        public bool ScanColumns()
        {
            bool colSumGood = true;
            string badCol = null;

            for (int column = 0; column < 9; column++)
            {
                for (int b = 0; b < 9; b++)
                {
                    for (int row = b + 1; row < 9; row++)
                    {
                        if (puzzle[b, column].Equals(puzzle[row, column]))
                        {
                            badCol = column.ToString();
                            testResults[1, column, 0] = "false";
                            testResults[1, column, 1] = "COLUMN:  " + badCol;
                            colSumGood = false;
                            break;
                        }
                    }
                }
            }
            return colSumGood;
        }//END method ScanColumns

        //Checks all blocks to see if there are repeats
        //Good is TRUE = NO REPEATS PRESENT
        public bool ScanBlocks()
        {
            int nextRow = 0;
            int actualRow = 0;
            int nextColumn = 0;
            int actualColumn = 0;
            int blockWith = 3;
            int blockHeight = 3;
            int blockNum = 0;
            bool blocksGood = true;

            List<int> blockList = new List<int>();

            //organize parameters that are within each block such as block one is row#s 0 to 2, column#s 0 to 2: creates a list then use another method to determine repeats
            for (int blockRowTransition = 0; blockRowTransition < 3; blockRowTransition++)
            {
                for (int blockColTransition = 0; blockColTransition < 3; blockColTransition++)
                {
                    for (int row = 0; row < 3; row++)
                    {
                        for (int column = 0; column < 3; column++)
                        {
                            if (blockList.Contains(puzzle[nextRow, nextColumn]))
                            {
                                //record bad location
                                blocksGood = false;
                                testResults[2, blockNum, 0] = "false";
                                //indicate index# of block +1
                                testResults[2, blockNum, 1] = "BLOCK: " + blockNum.ToString();
                                //indicate row#
                                testResults[2, blockNum, 2] = "ROW: " + Convert.ToString(actualRow);
                                //indicate column#
                                testResults[2, blockNum, 3] = "COLUMN: " + Convert.ToString(actualColumn);
                            }
                            blockList.Add(puzzle[nextRow, nextColumn]);
                            nextColumn++;
                            actualColumn++;
                        }//END loop to transition columns of one: 3 TIMES
                        actualRow++;
                        actualColumn -= 3;
                        nextColumn -= 3;
                        nextRow++;
                    }//END loop to transition rows of one: 3 TIMES
                    nextColumn += blockWith;
                    actualColumn += 3;
                    actualRow -= 3;
                    nextRow -= blockHeight;
                    blockList.Clear();
                    blockNum++;
                }//END loop to transition blocks by sets of three columns: 2 TIMES
                actualColumn -= 9;
                actualRow += 3;
                nextColumn = 0;
                nextRow += blockHeight;
            }//END loop to transition blocks by sets of three rows: 2 TIMES
            //change array values for results array
            return blocksGood;
        }//END method ScanBlocks

        public void DisplayResults()
        {
            Console.WriteLine("INVALID");
            for (int a = 0; a < 3; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    if (!Convert.ToBoolean(testResults[a, b, 0]))
                    {
                        if (testResults[a, b, 2] != null)
                        {
                            Console.WriteLine("{0}\t{1}\t{2}", testResults[a, b, 1], testResults[a, b, 2], testResults[a, b, 3]);
                        }
                        else if (testResults[a, b, 1] != null)
                            Console.WriteLine("{0}", testResults[a, b, 1]);
                    }//END check if bool is false
                }//END loop b
            }//END loop a
        }//END method DisplayResults

        //Checks ScanRows, ScanColumns, and ScanBlocks for return of TRUE; TRUE will mean solution is correct
        public bool CheckPuzzle()
        {
            bool isCorrect = true;
            bool row = ScanRows();
            bool col = ScanColumns();
            bool block = ScanBlocks();

            //Console.WriteLine("{0}\t{1}\t{2}", row, col, block);

            if (!(row) || !(col) || !(block))
            {
                isCorrect = false;
                DisplayResults();
            }
            else
                Console.WriteLine("VALID");

            return isCorrect;
        }//END method PUzzleCorrect()
    }//END class SudokuTester
}//END namespace
