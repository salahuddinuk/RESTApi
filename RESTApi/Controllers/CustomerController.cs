using Microsoft.AspNetCore.Mvc;
using RESTApi.Helpers;
using RESTApi.Models;
using System.Text.Json;

namespace RESTApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            HandleData wr = new HandleData();

            IEnumerable<Customer> customers = await wr.GetCustomers();
            return Ok(customers);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] List<Customer> cust)
        {
            try
            {
                //List<Customer> customers1 = new List<Customer>();
                bool isidSeq = true;
                int prevId = 0;

                HandleData wr = new HandleData();

                // below codes verfies unique id
                //IEnumerable<Customer> customers = await wr.GetCustomers();
                //foreach (Customer customer in cust)
                //{
                //    if (customer.Id <= prevId)
                //        isidSeq = false;
                //    else if (customers.Any(c => c.Id == customer.Id))
                //        isidSeq = false;
                //    else
                //        prevId = customer.Id;
                //}
                //if(!isidSeq)
                //    throw new ArgumentException("Invalid Id sequence");

                
                await wr.AddCustomer(cust);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
