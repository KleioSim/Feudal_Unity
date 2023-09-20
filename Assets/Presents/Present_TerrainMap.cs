using Feudal.Scenes.Main;
using System.Linq;
using UnityEngine;
using static KleioSim.Tilemaps.TilemapObservable;

namespace Feudal.Presents
{
    class Present_TerrainMap : Present<TerrainMap>
    {
        public override void Refresh(TerrainMap view)
        {
            var dataItemDict = view.terrainItems.ToDictionary(item => (item.Position.x, item.Position.y), item => item);

            var needRemoveKeys = dataItemDict.Keys.Except(session.terrainItems.Keys).ToArray();
            var needAddKeys = session.terrainItems.Keys.Except(dataItemDict.Keys).ToArray();

            foreach (var key in needRemoveKeys)
            {
                view.terrainItems.Remove(dataItemDict[key]);
            }

            foreach (var key in needAddKeys)
            {
                view.terrainItems.Add(new DataItem()
                {
                    Position = new Vector3Int(key.x, key.y),
                    TileKey = session.terrainItems[key].GetTerrainDataType()
                });
            }

            foreach (var item in view.terrainItems)
            {
                RefreshData_DataItem(item);
            }
        }

        public void RefreshData_DataItem(DataItem item)
        {
            var terrain = session.terrainItems[(item.Position.x, item.Position.y)];
            item.TileKey = terrain.GetTerrainDataType();
        }
    }
}