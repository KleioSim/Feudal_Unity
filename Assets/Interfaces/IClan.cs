namespace Feudal.Interfaces
{
    public interface IClan
    {
        public string Id { get; }
        public string Name { get; }

        public int TotalLaborCount { get; }

        public ITask[] tasks { get; }
    }
}