using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddPerson.Models
{
    public class Country
    {
        public virtual int CountryId { get; set; }
        public virtual string CountryName { get; set; }

        public virtual IList<Person> NaturalBorns { get; set; }
    }
}