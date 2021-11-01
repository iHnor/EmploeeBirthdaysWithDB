using System;
using System.Collections.Generic;


namespace EmployeeBirthdays
{
    public class EmployeesSorter
    {
        private DateTime currentDate = DateTime.Today;
        private Dictionary<int, List<Employee>> employeesByMonth = new Dictionary<int, List<Employee>>();

        public EmployeesSorter(List<Employee> employees)
        {

            foreach (var emp in employees)
            {
                if (!this.employeesByMonth.ContainsKey(emp.getDateBirth().Month))
                {
                    this.employeesByMonth.Add(emp.getDateBirth().Month, new List<Employee>());
                }
                this.employeesByMonth[emp.getDateBirth().Month].Add(emp);
            }
        }

        public List<Employee> GetSortEmployees(int key)
        {
            List<Employee> res = new List<Employee>(employeesByMonth[key]);
            res.Sort((x, y) => x.getDateBirth().Day.CompareTo(y.getDateBirth().Day));
            return res;
        }

        public bool isElemInDictionary(int key)
        {
            return employeesByMonth.ContainsKey(key);
        }
    }
}