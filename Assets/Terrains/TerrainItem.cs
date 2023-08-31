using Feudal.Interfaces;

namespace Feudal.Terrains
{
    class TerrainItem : ITerrainItem
    {
        private readonly (int x, int y) position;
        public (int x, int y) Position => position;

        private Terrain terrain;
        public Terrain Terrain => terrain;

        public bool IsDiscovered { get; set; } = true;

        public TerrainItem((int, int) position, Terrain terrain)
        {
            this.position = position;
            this.terrain = terrain;
        }
    }
}