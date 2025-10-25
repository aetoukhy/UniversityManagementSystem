using System;
using System.Collections.Generic;
using System.Linq;
using UMS.Departments;

namespace UMS.Subjects
{
    public static class ManageSubject
    {
        private static int _navigator = 4;
        public static int idCounter = 0;

        public static void CreateSubject()
        {
            idCounter++;
            int subId = idCounter;

            Console.WriteLine("Enter Subject Name: ");
            string name = Console.ReadLine();

            if (Data.Subjects.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Subject name already exists.");
                if (Helper.Footer(_navigator) == 1) CreateSubject();
                return;
            }

            Console.WriteLine("Enter Subject Full Mark: ");
            double fullMark = Helper.DoubleInput(Console.ReadLine());

            Subject subject = new Subject() { Id = subId, Name = name, FullMark = fullMark };
            Data.Subjects.Add(subject);
            Console.WriteLine($"Subject \"{subject.Name}\" created successfully with ID: {subject.Id}.");

            if (Helper.Footer(_navigator) == 1) CreateSubject();
        }

        public static Subject RetrieveSubject(int id) =>
            Data.Subjects.FirstOrDefault(s => s.Id == id);

        public static void UpdateSub()
        {
            Console.Write("Enter Subject ID: ");
            int subId = Helper.IdInput(Console.ReadLine());
            Subject subject = RetrieveSubject(subId);

            if (subject != null)
            {
                Console.WriteLine("Select action number from the list below:");
                Console.WriteLine("1. Update Subject Name");
                Console.WriteLine("2. Update Subject Department");
                Console.WriteLine("3. Update Subject Full Mark");

                int[] options = { 1, 2, 3 };
                int choice = Helper.SelectFrom(Console.ReadLine(), options);

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter new name: ");
                        string newName = Console.ReadLine();
                        subject.Name = newName;
                        Console.WriteLine("Subject name updated successfully.");
                        break;

                    case 2:
                        Department oldDep = subject.SubjectDep;
                        if (oldDep != null)
                        {
                            Helper.DisplayExisting(Data.Departments, "Departments");
                            Console.WriteLine($"Enter ID of new Department to be assigned to {subject.Name}: ");
                            int newDepId = Helper.IdInput(Console.ReadLine());
                            Department newDep = ManageDep.RetrieveDep(newDepId);

                            if (newDep != null)
                            {
                                oldDep.DepSubjects.Remove(subject);
                                subject.SubjectDep = newDep;
                                if (!newDep.DepSubjects.Contains(subject))
                                    newDep.DepSubjects.Add(subject);

                                Console.WriteLine($"{subject.Name} reassigned to {newDep.Name} Department successfully.");
                            }
                            else
                            {
                                Console.WriteLine("New Department not found.");
                                Console.WriteLine($"{subject.Name} is still assigned to {oldDep.Name} Department.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Subject was not initially assigned to any Department.");
                            Console.WriteLine($"Assign a Department first to {subject.Name} to be able to update it later.");
                        }
                        break;

                    case 3:
                        Console.Write("Enter new Full Mark: ");
                        double newMark = Helper.DoubleInput(Console.ReadLine());
                        subject.FullMark = newMark;
                        Console.WriteLine("Full Mark updated successfully.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Subject not found.");
            }

            if (Helper.Footer(_navigator) == 1) UpdateSub();
        }

        public static void PrintSubject(Subject sub)
        {
            if (sub != null)
            {
                Console.WriteLine($"ID: {sub.Id}");
                Console.WriteLine($"Name: {sub.Name}");
                Console.WriteLine($"Full Mark: {sub.FullMark}");
                Console.WriteLine($"Department: {(sub.SubjectDep != null ? sub.SubjectDep.Name : "Unassigned")}");
            }
            else Console.WriteLine("Subject not found.");
            Console.WriteLine("-----------------------------");
        }

        public static void PrintAllSubjects()
        {
            foreach (var s in Data.Subjects) PrintSubject(s);
            if (Helper.Footer(_navigator) == 1) PrintAllSubjects();
        }

        public static void AssignDepartmentToSubject()
        {
            Helper.DisplayExisting(Data.Subjects, "Subjects");
            Console.Write("Enter Subject ID: ");
            int id = Helper.IdInput(Console.ReadLine());
            Subject sub = RetrieveSubject(id);

            if (sub == null) { Console.WriteLine("Subject not found."); return; }

            Helper.DisplayExisting(Data.Departments, "Departments");
            Console.Write("Enter Department ID: ");
            int depId = Helper.IdInput(Console.ReadLine());
            Department dep = ManageDep.RetrieveDep(depId);

            if (dep == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            sub.SubjectDep?.DepSubjects.Remove(sub);

            sub.SubjectDep = dep;
            if (!dep.DepSubjects.Contains(sub)) dep.DepSubjects.Add(sub);

            Console.WriteLine($"{sub.Name} assigned to {dep.Name} Department.");
            if (Helper.Footer(_navigator) == 1) AssignDepartmentToSubject();
        }

        public static void DeleteSubject()
        {
            Helper.DisplayExisting(Data.Subjects, "Subjects");
            Console.Write("Enter Subject ID: ");
            int id = Helper.IdInput(Console.ReadLine());
            Subject sub = RetrieveSubject(id);

            if (sub == null) { Console.WriteLine("Subject not found."); return; }

            Console.WriteLine($"Are you sure to delete {sub.Name}?");
            Console.WriteLine("1. Delete Permanently\n2. Cancel");
            int[] options = { 1, 2 };
            int choice = Helper.SelectFrom(Console.ReadLine(), options);

            if (choice == 1)
            {
                sub.SubjectDep?.DepSubjects.Remove(sub);
                Data.Subjects.Remove(sub);
                Console.WriteLine("Subject deleted successfully.");
            }
            else Console.WriteLine("Deletion cancelled.");

            if (Helper.Footer(_navigator) == 1) DeleteSubject();
        }
    }
}
