using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using NHibernate.Linq;

using AddPerson.Mappings;
using AddPerson.Models;

namespace AddPerson
{
    class Program
    {
        static void Main(string[] args)
        {


            for (; ; )
            {

                Console.Write("Enter name of person to add: ");
                string personName = Console.ReadLine();

                if (personName.Trim().Length == 0)
                {
                    Console.WriteLine("Blank not allowed. Retry");
                    continue;
                }

                using (var session = NHMapping.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        // var px = session.Get<Person>(1);
                        //var px = session.Query<Person>().Single(x => x.PersonId == 2);
                        //px.PersonName = personName;
                        //session.Save(px);

                        // session.Save(new Person { PersonName = personName, CountryBorned = session.Load<Country>(1) });
                        

                        // session.Delete(session.Load<Person>(3));


                        session.Delete(new Person { PersonId = 4 });

                        tx.Commit();
                    }
                }
            }
            

            Console.ReadLine();
        }

        private static void TestCache()
        {
            for (int i = 0; i < 10; i++)
            {
                using (var session = NHMapping.GetSessionFactory().OpenSession())
                {

                    var peopleList =
                        (from p in session.Query<Person>()
                              // .Cacheable()
                             .Fetch(x => x.CountryBorned)
                         orderby p.CountryBorned.CountryId
                         select new { PersonName = p.PersonName, BornedAt = p.CountryBorned.CountryName }).ToList();


                    foreach (var x in peopleList)
                    {
                        Console.WriteLine("{0} {1}", x.PersonName, x.BornedAt);
                    }


                    // With Cache: 28 seconds
                    // Without Cache: 41 seconds
                }
            }
        }
    }
}
