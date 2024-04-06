using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using RESTApi.Models;
using System.Text.Json;
using System.Xml;

namespace RESTApi.Helpers
{

    public class HandleData
    {

        public async Task AddCustomer(List<Customer> customers)
        {

            List<Customer> customerList = new List<Customer>();
            List<Customer> newCustomers = new List<Customer>();

            string fileName = "Customers.json";
            string filePath = @MapPath("JsonData");

            string combinePath = filePath + "\\" + fileName;
            bool exists = Directory.Exists(filePath);
            if (!exists)
                Directory.CreateDirectory(filePath);

            bool fileExists = File.Exists(combinePath);

            
            if (!fileExists)
            {
                File.Create(combinePath).Close();
                var jsonData = JsonSerializer.Serialize(customers);
                System.IO.File.WriteAllText(combinePath, jsonData);
            }
            else
            {
                var jsonData = System.IO.File.ReadAllText(combinePath);
                customerList = JsonSerializer.Deserialize<List<Customer>>(jsonData)
                                  ?? new List<Customer>();

                int index = 0;
                int insertAt = 0;

                customers = await orderedCustomers(customers); // sort list by last name then first name if last name is same

                foreach (Customer nCust in customers)
                {
                    // only add if new customer last and first name
                    if (!customerList.Any(c => c.LastName == nCust.LastName && c.FirstName == nCust.FirstName))
                    {

                        foreach (Customer cust in customerList)
                        {
                            int c = string.Compare(nCust.LastName, cust.LastName);
                            index = customerList.IndexOf(cust);


                            if (c == 0)
                            {
                                int cc = string.Compare(nCust.FirstName, cust.FirstName);
                                index = customerList.IndexOf(cust);

                                if (cc > 0)
                                    index++;
                                else if (cc < 0)
                                    index--;
                            }
                            else if (c > 0)
                                index++;
                            else if (c < 0)
                                index--;
                        }

                        customerList.Insert((index < 0) ? 0 : index, nCust);
                    }

                }

                System.IO.File.WriteAllText(combinePath, JsonSerializer.Serialize(customerList));
            }

        }
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            string fileName = "Customers.json";
            string filePath = @MapPath("JsonData");

            string combinePath = filePath + "\\" + fileName;

            bool fileExists = File.Exists(combinePath);

            if (!fileExists)
            {
                return Enumerable.Empty<Customer>();
            }

            var jsonData = System.IO.File.ReadAllText(combinePath);
            // De-serialize to object or create new list
            var customers = JsonSerializer.Deserialize<List<Customer>>(jsonData)
                                  ?? new List<Customer>();

            return customers;
        }

        public string MapPath(string fileName)
        {
            return Path.Combine(
                (string)AppDomain.CurrentDomain.GetData("ContentRootPath"),
                fileName);
        }
        public async Task<List<Customer>> orderedCustomers(List<Customer> customers)
        {
            for (int index = 0; index < (customers.Count - 1); index++)
            {
                int c = string.Compare(customers[index].LastName, customers[index + 1].LastName);
                if (c > 0) //if first number is greater then second then swap
                {
                    //swap

                    var temp = customers[index];
                    customers[index] = customers[index + 1];
                    customers[index + 1] = temp;
                    //swap = true;
                }
                else if (c == 0)
                {
                    int cc = string.Compare(customers[index].FirstName, customers[index + 1].FirstName);

                    if (cc > 0)
                    {
                        var temp = customers[index];
                        customers[index] = customers[index + 1];
                        customers[index + 1] = temp;
                    }
                }
            }

            return customers;
        }
    }
}
