using AccountManagementPortal.Repositories;

namespace AccountManagementPortal.Data;

public class Customers
{ 
    public List<CustomerRepository> CustomerList { get; set; } = new List<CustomerRepository>
        {
            new ()
            {
                CustomerId = "John123",
                FullName = "John Doe",
                DateOfBirth = new DateTime(1980, 5, 20),
                ContactNumber = "9875655555",
                Email = "john.doe@example.com",
                Address = "456 Elm St, Cityville",
                InstitutionId = "IDIB0001",
                AccountNumber = "987654321098"
            },
            new ()
            {
                CustomerId = "Jane123",
                FullName = "Jane Smith",
                DateOfBirth = new DateTime(1990, 11, 10),
                ContactNumber = "444-444-4444",
                Email = "jane.smith@example.com",
                Address = "123 Oak St, Townsville",
                InstitutionId = "IDIB0001",
                AccountNumber = "123456789012"
            }
        };
}