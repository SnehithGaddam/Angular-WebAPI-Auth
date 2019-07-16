using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AngularWebApiAuthExample.WebApis.Controllers
{
    //[Authorize]
    [RoutePrefix("api/People")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PeopleController : ApiController
    {
        private List<Person> people;

        public PeopleController()
        {
            people = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 35
                },
                new Person
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName = "Jane",
                    Age = 19
                },
                new Person
                {
                    Id = 3,
                    FirstName = "Longs",
                    LastName = "Peak",
                    Age = 6425
                }
            };
        }

        [HttpGet]
        [Route("List")]
        [Authorize(Roles = "Admin, User")]
        public List<Person> List()
        {
            return people;
        }

        /// <summary>
        /// You could return IHttpActionResult also
        /// When using IHttpActionResult you would: return Ok(person) Or return NotFound();
        /// The reason I don't use that is that the documentation auto produced will not know
        /// the data type returned. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get/{id:int}")]
        [Authorize(Roles = "User")]
        public Person Get(int id)
        {
            var person = people.FirstOrDefault(x => x.Id == id);

            if (person == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return person;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
