using Newtonsoft.Json;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LoadEmployeePlugin
{
    [Author(Name = "Danila Pavlenko")]
    internal class LoadEmployee : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> input)
        {
            logger.Info("Loading employees");

            var employeesList = input.Cast<EmployeesDTO>().ToList();

            var tempList = JsonConvert.DeserializeObject<Dummyjson>(Properties.Resources.dummyjson);

            foreach (var item in tempList.users)
            {
                EmployeesDTO newEmployee = new EmployeesDTO()
                {
                    Name = $"{item.firstName} {item.lastName}"
                };
                newEmployee.AddPhone(item.phone);
                employeesList.Add(newEmployee);

                /* Версия если исправить DTO, добавив сеттер к свойству Phone. При использовании удалить или закоментировать строки 25-30
                employeesList.Add(new EmployeesDTO()
                {
                    Name = $"{item.firstName} {item.lastName}",
                    Phone = item.phone
                });
                */

            }

            logger.Info($"Loaded {employeesList.Count()} employees");
            return employeesList.Cast<DataTransferObject>();
        }


        // классы для десирилизации списка размещенного в Resources взятого с https://dummyjson.com/users
        // помещены внутрь класса т.к. должны использоваться только в нем
        class Dummyjson
        {
            public List<User> users { get; set; }
        }

        class User
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string phone { get; set; }
        }
    }
}
