using System;
using System.Collections.Generic;
using System.Linq;
using UMS.Departments;
using UMS.Subjects;

namespace UMS.Students
{
    public static class ManageStudent
    {
        private static int _navigator = 5;
        public static int idCounter = 0;

        public static void CreateStudent()
        {
            idCounter++;
            int id = idCounter;

            Console.WriteLine("Enter Student Name: ");
            string name = Console.ReadLine();

            Helper.DisplayExisting(Data.Departments, "Departments");
            Console.Write("Enter Department ID: ");
            int depId = Helper.IdInput(Console.ReadLine());
            Department dep = ManageDep.RetrieveDep(depId);

            Student std = new Student() { Id = id, Name = name, StdDep = dep };
            Data.Students.Add(std);

            if (dep != null && !dep.DepStds.Contains(std))
                dep.DepStds.Add(std);
            else Console.WriteLine("Department not found");

                Console.WriteLine($"Student \"{name}\" created successfully with ID: {id}.");

            if (Helper.Footer(_navigator) == 1) CreateStudent();
        }

        public static Student RetrieveStd(int id) =>
            Data.Students.FirstOrDefault(s => s.Id == id);

        public static void PrintStudent(Student s)
        {
            if (s == null) { Console.WriteLine("Student not found."); return; }

            Console.WriteLine($"ID: {s.Id}");
            Console.WriteLine($"Name: {s.Name}");
            Console.WriteLine($"Department: {(s.StdDep != null ? s.StdDep.Name : "Unassigned")}");
            Console.WriteLine("Subjects and Marks:");

            if (s.StdMarks.Count == 0)
                Console.WriteLine("No marks assigned.");
            else
                foreach (var kv in s.StdMarks)
                    Console.WriteLine($"- {kv.Key.Name}: {kv.Value}");

            Console.WriteLine($"Average: {AvgMark(s.Id):F2}");
            Console.WriteLine($"Status: {(StdPassed(s.Id) ? "Passed" : "Failed")}");
            Console.WriteLine("-----------------------------");
        }

        public static void PrintAllStdsData()
        {
            foreach (var s in Data.Students)
            {
                PrintStudent(s);
            }
            Helper.Footer(_navigator);
        }


        public static double AvgMark(int id)
        {
            var s = RetrieveStd(id);
            if (s?.StdMarks.Count > 0)
                return s.StdMarks.Values.Average();
            return 0;
        }

        public static bool StdPassed(int id)
        {
            var s = RetrieveStd(id);
            if (s == null || s.StdMarks.Count == 0) return false;

            double total = s.StdMarks.Values.Sum();
            double full = s.StdMarks.Keys.Sum(sub => sub.FullMark);
            double percent = (total / full) * 100;

            return percent >= 50;
        }

        public static void AssignDepartmentToStudent()
        {
            Helper.DisplayExisting(Data.Students, "Students");
            Console.Write("Enter Student ID: ");
            int id = Helper.IdInput(Console.ReadLine());
            Student s = RetrieveStd(id);

            if (s == null) { Console.WriteLine("Student not found."); return; }

            Helper.DisplayExisting(Data.Departments, "Departments");
            Console.Write("Enter Department ID: ");
            int depId = Helper.IdInput(Console.ReadLine());
            Department dep = ManageDep.RetrieveDep(depId);

            if (dep == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            s.StdDep?.DepStds.Remove(s);

            s.StdDep = dep;
            if (!dep.DepStds.Contains(s)) dep.DepStds.Add(s);

            Console.WriteLine($"{s.Name} assigned to {dep.Name} Department.");
            if (Helper.Footer(_navigator) == 1) AssignDepartmentToStudent();
        }

        public static void AssignSubjectsToStudent()
        {
            Helper.DisplayExisting(Data.Students, "Students");
            Console.Write("Enter Student ID: ");
            int id = Helper.IdInput(Console.ReadLine());
            Student s = RetrieveStd(id);

            if (s == null) { Console.WriteLine("Student not found."); return; }

            Helper.DisplayExisting(Data.Subjects, "Subjects");
            Console.Write("How many subjects to assign? ");
            int num = Helper.IntInput(Console.ReadLine());

            for (int i = 0; i < num; i++)
            {
                Console.Write($"Enter Subject ID #{i + 1}: ");
                int subId = Helper.IdInput(Console.ReadLine());
                Subject sub = ManageSubject.RetrieveSubject(subId);

                if (sub == null) { Console.WriteLine("Subject not found."); continue; }

                Console.Write($"Enter mark for {sub.Name} (Max {sub.FullMark}): ");
                double mark;
                while (true)
                {
                    mark = Helper.DoubleInput(Console.ReadLine());
                    if (mark <= sub.FullMark) break;
                    Console.WriteLine("Mark exceeds FullMark. Try again.");
                }

                s.StdMarks[sub] = mark;
            }

            if (Helper.Footer(_navigator) == 1) AssignSubjectsToStudent();
        }

        public static void DeleteStudent()
        {
            Helper.DisplayExisting(Data.Students, "Students");
            Console.Write("Enter Student ID: ");
            int id = Helper.IdInput(Console.ReadLine());
            Student std = RetrieveStd(id);

            if (std == null) { Console.WriteLine("Student not found."); return; }

            Console.WriteLine($"Delete {std.Name}?\n1. Yes\n2. No");
            int[] opts = { 1, 2 };
            int choice = Helper.SelectFrom(Console.ReadLine(), opts);

            if (choice == 1)
            {
                std.StdDep?.DepStds.Remove(std);
                Data.Students.Remove(std);
                Console.WriteLine("Student deleted successfully.");
            }
            else Console.WriteLine("Cancelled.");

            if (Helper.Footer(_navigator) == 1) DeleteStudent();
        }
    }
}
