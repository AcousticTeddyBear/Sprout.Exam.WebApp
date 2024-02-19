using Sprout.Exam.DataAccess.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Sprout.Exam.DataAccess
{
    public class DataSeeder
    {
        private readonly SproutExamDbContext dbContext;

        public DataSeeder(SproutExamDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            if (dbContext.Employees.Any())
            {
                return;
            }

            string fileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "seed_data.json");
            string jsonString = File.ReadAllText(fileName);
            var clients = JsonSerializer.Deserialize<List<EmployeeEntity>>(jsonString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            dbContext.AddRange(clients);
            dbContext.SaveChanges();
        }
    }
}
