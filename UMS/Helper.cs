using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS
{
    public class Helper
    {
        internal static int IntInput(string input)
        {
            while (!int.TryParse(input, out _))
            {
                Console.Write("Invalid input. Please enter a valid number: ");
                input = Console.ReadLine();
            }
            return int.Parse(input);
        }

        internal static double DoubleInput(string input)
        {
            while (!double.TryParse(input, out _))
            {
                Console.Write("Invalid input. Please enter a valid number: ");
                input = Console.ReadLine();
            }
            return double.Parse(input);
        }

        internal static int IdInput(string input)
        {
            while (!int.TryParse(input, out _))
            {
                Console.Write("Invalid input. Please enter a valid ID number: ");
                input = Console.ReadLine();
            }
            return int.Parse(input);
        }

        internal static void DisplayExisting<T>(List<T> entity, string entityName) where T : UMS.Universities.BaseClass
        {
            if (entity.Count == 0)
            {
                Console.WriteLine($"\n**HINT**\nNo {entityName} exist yet.");
                return;
            }

            Console.WriteLine($"\n**HINT**\nExisting {entityName}:");
            foreach (var item in entity)
            {
                Console.WriteLine($"- ID: {item.Id}, Name: {item.Name}");
            }
        }

        internal static int SelectFrom(string input, int[] options)
        {
            int selection;
            while (true)
            {
                while (!int.TryParse(input, out selection))
                {
                    Console.Write("Invalid input. Please select a Valid number from the list of actions: ");
                    input = Console.ReadLine();
                }

                if (options.Contains(selection)) return selection;
                Console.WriteLine($"Please select a number from [{string.Join(", ", options)}]");
                input = Console.ReadLine();
            }
        }

        internal static string GetClassification(double rate)
        {
            if (rate >= 90) return "A";
            if (rate >= 80) return "B";
            if (rate >= 70) return "C";
            if (rate >= 60) return "D";
            return "E";
        }

        internal static int Footer(int navigator)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Press 0 to return to HomePage");
            Console.WriteLine("Press 1 to repeat current action");
            Console.WriteLine("Press 2 to return to Current Category");
            Console.WriteLine("Press 3 to Exit program");

            int repeat = Navigation.NextAction(navigator);
            return repeat;
        }

    }

}
