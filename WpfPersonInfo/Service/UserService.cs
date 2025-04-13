using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WpfPersonInfo.Model;

namespace WpfPersonInfo.Service
{
    public class UserService
    {
        private static readonly string _dataFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "users.json");

        public async Task<ObservableCollection<Person>> LoadUsersAsync()
        {
            if (!File.Exists(_dataFilePath))
            {
                var users = GenerateUsers();
                await SaveUsersAsync(new ObservableCollection<Person>(users));
                return new ObservableCollection<Person>(users);
            }

            try
            {
                await using var fileStream = File.OpenRead(_dataFilePath);
                var users = await JsonSerializer.DeserializeAsync<List<Person>>(fileStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new ObservableCollection<Person>(users ?? GenerateUsers());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                return new ObservableCollection<Person>(GenerateUsers());
            }
        }

        public async Task SaveUsersAsync(ObservableCollection<Person> users)
        {
            try
            {
                await using var fileStream = File.Create(_dataFilePath);
                await JsonSerializer.SerializeAsync(fileStream, users.ToList(),
                    new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
                throw;
            }
        }

        private List<Person> GenerateUsers()
        {
            var rand = new Random();
            var users = new List<Person>();

            var firstNames = new[] { "John", "Ann", "Alex", "Maria", "Mark", "Emma", "David", "Iryna", "Valentyna" };
            var lastNames = new[] { "Smith", "Johnson", "Shevchenko", "Brown", "Miller", "Lee", "Ponomarenko", "Ivanenko" };
            var domains = new[] { "gmail.com", "ukma.edu.ua", "outlook.com", "example.com" };

            for (int i = 0; i < 50; i++)
            {
                var firstName = firstNames[rand.Next(firstNames.Length)];
                var lastName = lastNames[rand.Next(lastNames.Length)];
                var email = $"{firstName.ToLower()}.{lastName.ToLower()}{rand.Next(100, 9999)}@{domains[rand.Next(domains.Length)]}";

                int year = rand.Next(1891, 2024);
                int month = rand.Next(1, 13);
                int day = rand.Next(1, DateTime.DaysInMonth(year, month) + 1);
                DateTime birthDate = new DateTime(year, month, day);

                try
                {
                    var person = new Person(firstName, lastName, email, birthDate);
                    users.Add(person);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating user: {ex.Message}");
                }
            }

            return users;
        }
    }
}