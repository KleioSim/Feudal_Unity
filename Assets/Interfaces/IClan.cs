namespace Feudal.Interfaces
{
    public interface IClan
    {
        public string Id { get; }
        public string Name { get; }

        public int TotalLaborCount { get; }

        public ITask[] tasks { get; }
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
        public float ProductValue { get; }
    }
}