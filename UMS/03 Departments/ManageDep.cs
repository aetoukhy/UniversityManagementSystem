using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UMS.Colleges;
using UMS.Subjects;
using UMS.Universities;

namespace UMS.Departments
{
    public class ManageDep
    {
        private static int _navigator = 3;

        public static int idCounter = 0;
        public static void CreateDepartment()
        {
            idCounter++;
            int depId = idCounter;

            Console.WriteLine("Enter Department Name: ");
            string name = Console.ReadLine();

            if (Data.Departments.Any(dep => dep.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("A Department with that name already exists.");
                if (Helper.Footer(_navigator) == 1) CreateDepartment();
                return;
            }

            Department department = new Department() { Id = depId, Name = name };
            Data.Departments.Add(department);
            Console.WriteLine($"\"{department.Name}\" Department created successfully with ID: {department.Id}.");

            if (Helper.Footer(_navigator) == 1) CreateDepartment();
        }

        public static Department CreateDepartment(string name)
        {
            idCounter++;
            int depId = idCounter;

            Department dep = new Department() { Id = depId, Name = name };
            Data.Departments.Add(dep);

            return dep;
        }

        public static Department RetrieveDep(int depId)
        {
            return Data.Departments.FirstOrDefault(dep => dep.Id == depId);
        }

        public static void PrintDep(Department dep)
        {
            if (dep != null)
            {
                Console.WriteLine($"ID: {dep.Id}");
                Console.WriteLine($"Name: {dep.Name}");
                Console.WriteLine($"College: {(dep.DepCollege != null ? dep.DepCollege.Name : "---")}");
                foreach (var sub in dep.DepSubjects)
                {
                    Console.WriteLine($"- {sub.Name}");
                }
            }
            else
            {
                Console.WriteLine("Department not found.");
            }
            Console.WriteLine("-----------------------------");
        }

        public static void PrintAllDepsData()
        {
            foreach (var dep in Data.Departments)
            {
                PrintDep(dep);
            }

            if (Helper.Footer(_navigator) == 1) PrintAllDepsData();
        }

        public static void UpdateDep()
        {
            Helper.DisplayExisting(Data.Departments, "Departments");
            Console.WriteLine($"Enter Department ID to be update: ");
            int depId = Helper.IdInput(Console.ReadLine());
            Department department = RetrieveDep(depId);

            if (department != null)
            {
                Console.WriteLine("Select action number from the list below:");
                Console.WriteLine("1. Update Department name");
                Console.WriteLine("2. Update Department College");

                int[] options1 = { 1, 2 };
                int i = Helper.SelectFrom(Console.ReadLine(), options1);

                switch (i)
                {
                    case 1:
                        Console.WriteLine("Enter new name: ");

                        string new_name = Console.ReadLine();
                        department.Name = new_name;
                        break;

                    case 2:
                        College oldCollege = department.DepCollege;
                        if (oldCollege != null)
                        {
                            Helper.DisplayExisting(Data.Colleges, "Colleges");
                            Console.WriteLine($"Enter ID of new College to be assigned to {department.Name} Department: ");
                            int newCollId = Helper.IdInput(Console.ReadLine());
                            College newCollege = ManageCollege.RetrieveCollege(newCollId);

                            if (newCollege != null)
                            {
                                oldCollege.CollDeps.Remove(department);
                                department.DepCollege = newCollege;
                                newCollege.CollDeps.Add(department);
                            }
                            else
                            {
                                Console.WriteLine("New College not found.");
                                Console.WriteLine($"{department.Name} Department is still assigned to {oldCollege.Name} College.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Department was not initially assigned to any college.");
                            Console.WriteLine($"Assign a college to first {department.Name} to be able to update it later.");
                        }

                        break;
                }

            }
            else Console.WriteLine("Department not found.");

            if (Helper.Footer(_navigator) == 1) UpdateDep();
        }

        public static void DeleteDep()
        {
            Helper.DisplayExisting(Data.Departments, "Departments");
            Console.WriteLine($"Enter Department ID to delete: ");
            int depId = Helper.IdInput(Console.ReadLine());
            Department department = RetrieveDep(depId);

            if (department != null)
            {
                Console.WriteLine($"Are you sure to delete \"{department.Name}\" Department?");
                Console.WriteLine($"1. Delete Permenantly");
                Console.WriteLine($"2. Cancel");

                int[] options1 = { 1, 2 };
                int i = Helper.SelectFrom(Console.ReadLine(), options1);

                if (i == 1)
                {
                    foreach (var college in Data.Colleges)
                    {
                        if (college.CollDeps.Contains(department))
                        {
                            college.CollDeps.Remove(department);
                        }
                    }

                    foreach (var subject in Data.Subjects)
                    {
                        if (subject.SubjectDep == department)
                        {
                            subject.SubjectDep = null;
                        }
                    }

                    Data.Departments.Remove(department);
                    Console.WriteLine("Department deleted successfully.");
                }
                else if (i == 2)
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Department not found.");
            }

            if (Helper.Footer(_navigator) == 1) DeleteDep();
        }

        public static void AssignCollegeToDep()
        {
            Helper.DisplayExisting(Data.Departments, "Departments");
            Console.WriteLine("Enter Department ID to assign to a College: ");
            int depId = Helper.IdInput(Console.ReadLine());
            Department dep = RetrieveDep(depId);
            if (dep != null)
            {
                Helper.DisplayExisting(Data.Colleges, "Colleges");
                Console.WriteLine($"Enter College ID to be assigned to {dep.Name} Department: ");
                int collId = Helper.IdInput(Console.ReadLine());
                College college = ManageCollege.RetrieveCollege(collId);
                if (college != null)
                {
                    if (college.CollDeps.Contains(dep))
                        Console.WriteLine($"{dep.Name} Department already exists in {college.Name} College.");
                    else
                    {
                        dep.DepCollege?.CollDeps.Remove(dep);
                        dep.DepCollege = college;
                        college.CollDeps.Add(dep);
                        Console.WriteLine($"{dep.Name} Department is now assigned to {college.Name} College.");
                    }
                }
                else
                {
                    Console.WriteLine("College not found.");
                }
            }
            else
            {
                Console.WriteLine("Department not found.");
            }

            if (Helper.Footer(_navigator) == 1) AssignCollegeToDep();
        }

        public static void AssignSubjectsToDep()
        {
            Helper.DisplayExisting(Data.Departments, "Departments");
            Console.WriteLine("Enter Department ID to assign subjects: ");
            int depId = Helper.IdInput(Console.ReadLine());
            Department dep = RetrieveDep(depId);
            if (dep != null)
            {
                Console.WriteLine("Specify number of subjects to be assigned: ");
                int numOfSubjects = Helper.IntInput(Console.ReadLine());
                if (ManageSubject.idCounter > 0)
                {
                    if (numOfSubjects > ManageSubject.idCounter)
                    {
                        Console.WriteLine($"Number exceeds existing subjects.\nLimit is set to: {ManageSubject.idCounter}");
                        numOfSubjects = ManageSubject.idCounter;
                        if (numOfSubjects == 0)
                        {
                            Console.Write("Operation skipped.");
                            if (Helper.Footer(1) == 1) AssignSubjectsToDep();
                            return;
                        }

                    }
                    Console.WriteLine("Enter Subjects IDs:");
                    Helper.DisplayExisting(Data.Subjects, "Subjects");
                    for (int i = 0; i < numOfSubjects; i++)
                    {
                        int subId = Helper.IntInput(Console.ReadLine());
                        Subject subject = ManageSubject.RetrieveSubject(subId);

                        if (subject != null)
                        {
                            if (!dep.DepSubjects.Contains(subject))
                            {
                                dep.DepSubjects.Add(subject);
                            }
                            if (subject.SubjectDep != dep)
                            {
                                subject.SubjectDep = dep;
                            }
                        }
                        else Console.WriteLine("Subject not found");
                    }
                }
                else
                {
                    Console.WriteLine("No Subjects created yet!");
                    Console.WriteLine("Create Subjects first then assign them on Departments.");
                }
            }
            else Console.WriteLine($"Department not found");

            if (Helper.Footer(_navigator) == 1) AssignSubjectsToDep();
        }

    }
}
