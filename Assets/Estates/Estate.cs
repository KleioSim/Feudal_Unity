using Feudal.Interfaces;
using System.Collections.Generic;

namespace Feudal.Estates
{
    public class Estate : IEstate
    {
        private static int estateId;

        public (int x, int y) Position { get; }

        public string Id { get; }

        public EstateType Type { get; }

        public ProductType ProductType { get; }

        public float ProductValue { get; set; }

        public string OwnerId { get; internal set; }

        private static Dictionary<EstateType, (ProductType productType, float productValue)> defs = new Dictionary<EstateType, (ProductType, float)>()
        {
            {
                EstateType.Farm,
                (ProductType.Food, 1.4f)
            },
            {
                EstateType.CopperMine,
                (ProductType.Copper, 1.0f)
            }
        };

        public Estate((int x, int y) position, EstateType estateType)
        {
            Id = estateId++.ToString();

            this.Position = position;
            this.Type = estateType;

            ProductType = defs[Type].productType;
            ProductValue = defs[Type].productValue;
        }
    }
}