using CoreService.model;
using CoreService.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CoreService.Data
{
    public static class DataSeeder
    {
        public static void SeedData(CoreDbContext context)
        {
            if (context.Departments.Count() < 20)
            {
                var departments = new[]
                {
                    new Department { Name = "Engineering", DepartemntCode = "ENG", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Human Resources", DepartemntCode = "HR", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Finance", DepartemntCode = "FIN", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Marketing", DepartemntCode = "MKT", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Sales", DepartemntCode = "SAL", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Customer Support", DepartemntCode = "CS", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Product Management", DepartemntCode = "PM", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Design", DepartemntCode = "DSN", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Quality Assurance", DepartemntCode = "QA", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Legal", DepartemntCode = "LGL", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Operations", DepartemntCode = "OPS", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "IT", DepartemntCode = "IT", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Research & Development", DepartemntCode = "RND", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Administration", DepartemntCode = "ADM", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Security", DepartemntCode = "SEC", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Public Relations", DepartemntCode = "PR", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Business Development", DepartemntCode = "BD", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Data Science", DepartemntCode = "DS", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Training", DepartemntCode = "TRN", CreatedDate = DateTime.UtcNow },
                    new Department { Name = "Logistics", DepartemntCode = "LOG", CreatedDate = DateTime.UtcNow }
                };

                context.Departments.AddRange(departments);
                context.SaveChanges();
            }

            if (context.Employees.Count() < 30)
            {
                var firstNames = new[] { "Ananya", "Rohit", "Priya", "Karan", "Sneha", "Arjun", "Divya", "Rahul", "Neha", "Vikram", "Aisha", "Aditya", "Riya", "Rohan", "Meera", "Varun", "Ishaan", "Kavya", "Siddharth", "Nisha" };
                var lastNames = new[] { "Sharma", "Verma", "Nair", "Mehta", "Iyer", "Rao", "Menon", "Gupta", "Patel", "Singh", "Kumar", "Das", "Joshi", "Bose", "Reddy", "Shah", "Deshmukh", "Chauhan", "Sen", "Bhat" };
                
                var random = new Random(1234); // fixed seed
                var departmentIds = context.Departments.Select(d => d.Id).ToList();

                var employees = new Employee[30];
                for (int i = 0; i < 30; i++)
                {
                    var fName = firstNames[random.Next(firstNames.Length)];
                    var lName = lastNames[random.Next(lastNames.Length)];
                    
                    employees[i] = new Employee
                    {
                        FirstName = fName,
                        LastName = lName,
                        Email = $"{fName.ToLower()}.{lName.ToLower()}{i}@company.com",
                        JoinDate = DateTime.UtcNow.AddDays(-random.Next(1, 1000)),
                        DepartmentId = departmentIds[random.Next(departmentIds.Count)],
                        CreatedUserId = 1
                    };
                }

                context.Employees.AddRange(employees);
                context.SaveChanges();
            }
        }
    }
}
