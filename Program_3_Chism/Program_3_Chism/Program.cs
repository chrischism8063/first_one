using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program_3_Chism
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "C:\\sudoku-bad-4.txt";


            Console.WriteLine("***************SUDOKU*TESTER*PROGRAM**************************************");
            Sudoku_Tester testingClass = new Sudoku_Tester();
            testingClass.LoadSolution(filename);
            testingClass.showBoard();
            testingClass.CheckPuzzle();
            Console.ReadLine();
        }//END method Main
    }//END class Program
}//END namespace
