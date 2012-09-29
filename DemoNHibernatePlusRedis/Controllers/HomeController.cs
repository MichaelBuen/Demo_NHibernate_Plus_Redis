using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NHibernate.Linq;

using DemoNHibernatePlusRedis.Mappings;
using DemoNHibernatePlusRedis.Models;

namespace DemoNHibernatePlusRedis.Controllers
{
    public class HomeController : Controller
    {
        //  
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        public ViewResult AddPerson()
        {
            return View();
        }


        [HttpPost]
        public string AddPerson(Person p)
        {
            using (var session = NHMapping.GetSessionFactory().OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {

                    p.CountryBorned = session.Load<Country>(1);
                    session.Save(p);

                    tx.Commit();
                }
            }

            return p.PersonName;

        }


        class PersonInfo 
        { 
            public string PersonName { get; set; }
            public string BornedAt { get; set; }
        };

        //static IList<PersonInfo> _peopleList = null;

        //public HomeController()
        //{
        //    if (_peopleList != null) return;
        //    using (var session = NHMapping.GetSessionFactory().OpenSession())
        //    {
        //        _peopleList =
        //            (from p in session.Query<Person>().Cacheable().Fetch(x => x.CountryBorned)
        //             orderby p.CountryBorned.CountryId
        //             select new PersonInfo { PersonName = p.PersonName, BornedAt = p.CountryBorned.CountryName }).ToList();
        //    }
        //}

        
        public JsonResult People()
        {
            using (var session = NHMapping.GetSessionFactory().OpenSession())
            {

                var peopleList =
                    (from p in session.Query<Person>()
                         .Cacheable()
                         .Fetch(x => x.CountryBorned)
                     orderby p.CountryBorned.CountryId, p.PersonId 
                     select new { PersonName = p.PersonName, BornedAt = p.CountryBorned.CountryName }).ToList();
                    
                    

                // With caching: 23 seconds
                // Without caching: 38 seconds

                return Json(peopleList, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
    