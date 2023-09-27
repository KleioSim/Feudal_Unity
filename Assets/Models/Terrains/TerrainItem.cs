using Feudal.Interfaces;
using System.Collections.Generic;

namespace Feudal.Terrains
{
    class TerrainItem : ITerrainItem
    {
        private readonly (int x, int y) position;
        public (int x, int y) Position => position;

        private Terrain terrain;
        public Terrain Terrain => terrain;

        public bool IsDiscovered { get; internal set; }

        //private List<TerrainTrait> traits = new List<TerrainTrait>();
        //public IEnumerable<TerrainTrait> Traits => traits;

        public TerrainResource? resource { get; internal set; }

        public TerrainItem((int, int) position, Terrain terrain)
        {
            this.position = position;
            this.terrain = terrain;
        }

        //public void AddTraits(params TerrainTrait[] traits)
        //{
        //    foreach(var trait in traits)
        //    {
        //        if(this.traits.Contains(trait))
        //        {
        //            continue;
        //        }

        //        this.traits.Add(trait);
        //    }
        //}
    }
}