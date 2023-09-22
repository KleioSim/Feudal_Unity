using System.Linq;

namespace Feudal.Presents
{
    class Present_Pawn_TerrainTraitContainer : Present<TerrainTraitContainer>
    {
        public override void Refresh(TerrainTraitContainer view)
        {
            view.SetTraitItems(session.terrainItems.Values
                .Where(x=>x.IsDiscovered && x.Traits.Any())
                .Select(x=>x.Position)
                .ToArray());
        }
    }

    class Present_Pawn_TerrainTraitItem : Present<TerrainTraitItem>
    {
        public override void Refresh(TerrainTraitItem view)
        {
            view.title.text = session.terrainItems[view.Position].Traits.First().ToString();
        }
    }
}