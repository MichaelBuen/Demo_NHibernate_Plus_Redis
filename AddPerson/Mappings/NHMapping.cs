using System;
using System.Collections.Generic;
using System.Linq;


using NHibernate;


using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

using AddPerson.Models;

namespace AddPerson.Mappings
{
    public static class NHMapping
    {
        private static ISessionFactory _isf = null;
        public static ISessionFactory GetSessionFactory()
        {            
            if (_isf != null) return _isf;

            var cfg = new StoreConfiguration();

            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(
                    "Server=localhost; Database=Test; Trusted_Connection=true; MultipleActiveResultSets=true")
                    )

                .Cache(c => c.ProviderClass(typeof(NHibernate.Caches.Redis.RedisProvider).AssemblyQualifiedName)
                .UseQueryCache().UseSecondLevelCache())
                  
                .Mappings(m =>
                m.AutoMappings
                    .Add(AutoMap.AssemblyOf<Person>(cfg)
                    .Conventions.Add<ReferenceConvention>()
                    .Conventions.Add<HasManyConvention>()
                    .Override<Country>(x => x.HasMany(y => y.NaturalBorns).KeyColumn("CountryBorned_CountryId"))
                    )
                )
                .BuildSessionFactory();


            _isf = sessionFactory;

            return _isf;
        }
    }


    class StoreConfiguration : DefaultAutomappingConfiguration
    {
        readonly IList<Type> _objectsToMap = new List<Type>()
        {
            // whitelisted objects to map
            typeof(Person), typeof(Country)
        };
        public override bool IsId(FluentNHibernate.Member member)
        {
            // return base.IsId(member);
            return member.Name == member.DeclaringType.Name + "Id";
        }
        public override bool ShouldMap(Type type) { return _objectsToMap.Any(x => x == type); }


    }

    class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(
                instance.Name + "_" + instance.Property.PropertyType.Name + "Id");
        }
    }

    class HasManyConvention : IHasManyConvention
    {

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Inverse();
            instance.Cascade.AllDeleteOrphan();
        }
    }

}