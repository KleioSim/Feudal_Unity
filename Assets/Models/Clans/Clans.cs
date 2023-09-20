using Feudal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Clans
{
    public class Clan : IClan
    {
        internal static Func<string, ITask[]> funcQueryTasks;
        internal static Func<string, IEstate[]> funcQueryEstates;

        private static int clanId;

        public string Id { get; }

        public string Name { get; }

        public int TotalLaborCount { get; }

        public ITask[] tasks
        {
            get
            {
                return funcQueryTasks(Id);
            }
        }

        public IEstate[] estates
        {
            get
            {
                return funcQueryEstates(Id);
            }
        }

        public int PopCount { get; set; }

        internal Dictionary<ProductType, ProductData> productMgr;
        public IReadOnlyDictionary<ProductType, IProductData> ProductMgr { get; }

        public Clan()
        {
            Id = clanId++.ToString();
            Name = Id;

            TotalLaborCount = 3;

            PopCount = 1000;

            productMgr = Enum.GetValues(typeof(ProductType)).OfType<ProductType>().ToDictionary(k => k, v=> new ProductData());
            ProductMgr = productMgr.ToDictionary(k => k.Key, v => v.Value as IProductData);

            productMgr[ProductType.Food].Current = 100;
        }
    }

    public class ProductData : IProductData
    {
        public decimal Current { get; set; }
        public decimal Surplus => EstateWorkOuputs.Values.Sum();

        internal Dictionary<string, decimal> estateWorkOuputs = new Dictionary<string, decimal>();
        public IReadOnlyDictionary<string, decimal> EstateWorkOuputs => estateWorkOuputs;

        public void Settle()
        {
            Current += Surplus;
        }
    }
}