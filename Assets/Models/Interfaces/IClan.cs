using System;
using System.Collections.Generic;

namespace Feudal.Interfaces
{
    public interface IClan
    {
        public string Id { get; }
        public string Name { get; }
        public ClanType ClanType { get; }

        public int PopCount { get; }
        public int TotalLaborCount { get; }

        public IReadOnlyDictionary<ProductType, IProductData> ProductMgr { get; }

        public ITask[] tasks { get; }
        public IEstate[] estates { get; }
    }

    public interface IProductData
    {
        public decimal Current { get; }
        public decimal Surplus { get; }

        public IReadOnlyDictionary<string, decimal> EstateWorkOuputs { get; }
    }

    public enum ClanType
    {
        [CountryClan]
        Guo,

        [CountryClan]
        Ye,

        Rong
    }
    
    public class CountryClanAttribute : Attribute
    {

    }

    public enum EstateType
    {
        Farm,
        CopperMine
    }

    public enum ProductType
    {
        Food,
        Copper
    }

    public interface IEstate
    {
        public (int x, int y) Position { get; }

        public string Id { get; }
        public EstateType Type { get; }

        public ProductType ProductType { get; }
        public decimal ProductValue { get; }

        public string OwnerId { get; }
    }
}