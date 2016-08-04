using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WorkerSalary
{
    class NewWorkerFromFile
    {
        public static List<Worker> GetNamesAndSalaries_FromTheFile_ToNewWorker(string path)
        {
            List<Worker> list = new List<Worker>();
            using (StreamReader reader = File.OpenText(path))
            {
                int index = 0;
                while (!reader.EndOfStream)
                {
                    string[] ArrayNameOrSalary = reader.ReadLine().Split(' ');
                    string name, salary;
                    decimal decSalary = 0;

                    name = ArrayNameOrSalary[0];
                    salary = ArrayNameOrSalary[1];

                    SplitLine_NameAndSalary(ArrayNameOrSalary, ref name, ref salary);

                    CheckFormat(ref decSalary, ref salary);

                    AddTheWorker(ref  index, list, name, decSalary);

                    index++;

                }

            }
            return list;
        }

        private static void AddTheWorker(ref int index, List<Worker> list, string name, decimal decSalary)
        {
            if (decSalary < 500)
            {
                list.Add(new SalaryForHour());
            }
            else
                list.Add(new FixedSalary());

            decSalary = list[index].AverageSalary(decSalary);
            list[index].Name = name;
            list[index].AverageMonthSalary = decSalary;
            list[index].Id = list[index].GetHashCode();
        }

        public static void OutputInformation(List<Worker> list, string sortStatus)
        {
            Console.WriteLine(sortStatus);
            foreach (Worker a in list)
                Console.WriteLine(a);  
        }
        

        private static void SplitLine_NameAndSalary(string[] ArrayNameOrSalary, ref string name, ref string salary)
        {
            for (int i = 0; i < ArrayNameOrSalary.Count(); i++)
                if (i % 2 == 0)
                    name = ArrayNameOrSalary[i];

                else
                    salary = ArrayNameOrSalary[i];
        }

        static void CheckFormat(ref decimal decSalary, ref string salary)
        {
            try
            {
                decSalary = Convert.ToDecimal(salary);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("{0}", ex.Message);
            }
        }
    }


    abstract class Worker : IComparable<Worker>
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        int id;
        public int Id
        {
        get {return id; }
        set {id = value; }
        }
        public decimal AverageMonthSalary;
        public abstract decimal AverageSalary(decimal salary);

        public static bool operator <(Worker a, Worker b)
        {
            return a < b;
        }
        public static bool operator >(Worker a, Worker b)
        {
            return a > b;
        }
        public static bool operator ==(Worker a, Worker b)
        {
            return a == b;
        }

        public static bool operator !=(Worker a, Worker b)
        {
            return a != b;
        }

        public override string ToString()
        {

            return String.Format("Id:{1}      AverageSalary:{0}    Name:{2}", this.AverageMonthSalary, this.Id, this.name);
        }



        public int CompareTo(Worker obj)
        {
            if (obj.AverageMonthSalary < this.AverageMonthSalary) return -1;
            if (obj.AverageMonthSalary > this.AverageMonthSalary) return 1;
            if (obj.AverageMonthSalary == this.AverageMonthSalary)
            {
                if (obj.id < this.id) return 1;
                if (obj.id > this.id) return -1;
                if (obj.id == this.id) return 0;
            }
            return 0;
        }
    }

    class FixedSalary : Worker
    {
        public override decimal AverageSalary(decimal salary)
        {
            return salary;
        }
    }
    class SalaryForHour : Worker
    {
        public override decimal AverageSalary(decimal salary)
        {
            return salary *= 8 * 20.8M;
        }
    }


    class Programm
    {
        void ShowAverageSalary_ForMonth(List<Worker> list)
        {
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            List<Worker> EndList = NewWorkerFromFile.GetNamesAndSalaries_FromTheFile_ToNewWorker(@"D:/checking.txt");
            NewWorkerFromFile.OutputInformation(EndList, "Before");
            EndList.Sort();
            NewWorkerFromFile.OutputInformation(EndList, "After");
            Console.ReadLine();

        }
    }
}