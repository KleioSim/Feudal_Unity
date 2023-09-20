using Feudal.Interfaces;
using Feudal.Scenes.Main;

namespace Feudal.Presents
{
    class Present_TerrainDetailPanel : Present<TerrainDetailPanel>
    {
        public override void Refresh(TerrainDetailPanel view)
        {
            var terrainItem = session.terrainItems[view.Position];

            view.title.text = terrainItem.Terrain.ToString();

            view.workDetailPanel.SetActive(false);

            if (!terrainItem.IsDiscovered)
            {
                var workHood = view.SetCurrentWorkHood<DisoverWorkHood>();
                workHood.Position = view.Position;
                return;
            }

            if (session.estates.TryGetValue(view.Position, out IEstate estate))
            {
                var workHood = view.SetCurrentWorkHood<EstateWorkHood>();
                workHood.Position = view.Position;
                workHood.estateId = estate.Id;
                return;
            }
        }
    }
}