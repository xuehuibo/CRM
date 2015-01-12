using System.Collections.Generic;
using System.Web.Http;
using CRM.Models;

namespace CRM.Controllers
{
    public class CustomerApiController : ApiController
    {
        // GET api/customerapi
        public IEnumerable<CCustomer> Get()
        {
            return new CCustomer[] {};
        }

        // GET api/customerapi/5
        public CCustomer Get(int id)
        {
            return null;
        }

        // POST api/customerapi
        public void Post(CCustomer value)
        {
        }

        // PUT api/customerapi/5
        public void Put(int id, CCustomer value)
        {
        }

        // DELETE api/customerapi/5
        public void Delete(int id)
        {
        }
    }
}
