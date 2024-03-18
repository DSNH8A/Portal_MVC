using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        ///// <summary>
        ///// <c>GetUser</c> gets the user associated with the specified ID
        ///// <para>
        ///// <see cref="GenericType{T}"/>
        ///// </para>
        ///// <list type="bullet">
        ///// <item><description><para><em>The unique user ID.</em></para></description></item>
        ///// </list>
        ///// <list type="number">
        ///// <item><description><para><em>The unique username</em></para></description></item>
        ///// </list>
        ///// <returns><strong>An employee object with the specified ID</strong></returns>
        ///// </summary>
        /// <summary>
        /// Gets the list of all Employees.
        /// </summary>
        /// <returns>The list of Employees.</returns>
        // GET: api/Employee
        //[HttpGet]
        //public IEnumerable<Employee> Get()
        //{
        //    return GetEmployees();
        //}

        /// <summary>
        /// Gets the Employee associated with the given id.
        /// </summary>
        /// <returns>The employee object.</returns>
        // GET: api/Employee/5
        [HttpGet("{id}", Name = "Get")]
        public Employee Get(int id)
        {
            return GetEmployees().Find(e => e.Id == id);
        }
        /// <summary>
        /// Creates an Employee.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Employee
        ///     {        
        ///       "firstName": "Mike",
        ///       "lastName": "Andrew",
        ///       "emailId": "Mike.Andrew@gmail.com"        
        ///     }
        /// </remarks>
        /// <param name="employee"></param>  
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>      
        // POST: api/Employee
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public Employee Post([FromBody] Employee employee)
        {
            // Logic to create new Employee
            return new Employee();
        }
        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Employee employee)
        {
            // Logic to update an Employee
        }
        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private List<Employee> GetEmployees()
        {
            return new List<Employee>()
            {
                new Employee()
                {
                    Id = 1,
                    FirstName= "John",
                    LastName = "Smith",
                    EmailId ="John.Smith@gmail.com"
                },
                new Employee()
                {
                    Id = 2,
                    FirstName= "Jane",
                    LastName = "Doe",
                    EmailId ="Jane.Doe@gmail.com"
                }
            };
        }
    }
}
