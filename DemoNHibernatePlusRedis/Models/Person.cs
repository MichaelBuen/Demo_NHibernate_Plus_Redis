using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoNHibernatePlusRedis.Models
{
    public class Person
    {
        public virtual int PersonId { get; set; }
        public virtual string PersonName { get; set; }

        public virtual Country CountryBorned { get; set; }
    }

}   