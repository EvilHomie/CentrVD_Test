using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FixedEmployeesViewerPlugin
{
    [Author(Name = "Danila Pavlenko")]
    public class CommandHandler : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            logger.Info("Starting Viewer");
            logger.Info("Type q or quit to exit");
            logger.Info("Available commands: list, add, del");

            var employeesList = args.Cast<EmployeesDTO>().ToList();

            string command = "";

            while (!command.ToLower().Contains("quit")
                && !command.ToLower().Contains("q"))
            {
                Console.Write("> ");
                command = Console.ReadLine();

                switch (command)
                {
                    case "list":
                        ShowListCommand(employeesList);
                        break;
                    case "add":
                        AddCommand(employeesList);
                        break;
                    case "del":
                        DelCommand(employeesList);
                        break;
                }

                Console.WriteLine("");
            }

            return employeesList.Cast<DataTransferObject>();
        }

        void ShowListCommand(List<EmployeesDTO> employeesList)
        {
            if (employeesList.Count == 0)
            {
                Console.Write("List is empty");
                return;
            }

            int index = 0;
            foreach (var employee in employeesList)
            {
                Console.WriteLine($"{index} Name: {employee.Name} | Phone: {employee.Phone}");
                ++index;
            }
        }

        void AddCommand(List<EmployeesDTO> employeesList)
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();
            Console.WriteLine($"{name} added to employees");

            EmployeesDTO newEmployee = new EmployeesDTO()
            {
                Name = name,
                //Phone = phone                //если DTO имеет сеттер в свойсте Phone
            };
            newEmployee.AddPhone(phone); //используется в первоначальной версии (когда поля телефонов не заполнялись)

            employeesList.Add(newEmployee);
        }

        void DelCommand(List<EmployeesDTO> employeesList)
        {
            if (employeesList.Count == 0)
            {
                Console.Write("Nothing to delete");
                return;
            }

            Console.Write("Index of employee to delete: ");
            int indexToDelete;
            if (!Int32.TryParse(Console.ReadLine(), out indexToDelete))
            {
                logger.Error("Not an index or not an int value!");
            }
            else
            {
                if (indexToDelete >= 0 && indexToDelete < employeesList.Count())
                {
                    employeesList.RemoveAt(indexToDelete);
                }
            }
        }
    }
}
