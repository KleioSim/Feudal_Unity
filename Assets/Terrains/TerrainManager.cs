using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections;
using System.Collections.Generic;

namespace Feudal.Terrains
{
    public class TerrainManager : IEnumerable<ITerrainItem>
    {
        private Dictionary<(int x, int y), TerrainItem> dict = new Dictionary<(int x, int y), TerrainItem>();
        private IMessageBus messageBus;

        public IEnumerator<ITerrainItem> GetEnumerator()
        {
            return ((IEnumerable<TerrainItem>)dict.Values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dict.Values).GetEnumerator();
        }

        public TerrainManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);

            dict.Add((0, 0), new TerrainItem((0, 0), Terrain.Hill));
            dict.Add((0, 1), new TerrainItem((0, 1), Terrain.Hill));
            dict.Add((1, 0), new TerrainItem((1, 0), Terrain.Hill));
        }

        [MessageProcess]
        public void OnMessage_AddTerrainItem(Message_AddTerrainItem message)
        {
            if(dict.ContainsKey(message.position))
            {
                return;
            }

            dict.Add(message.position, new TerrainItem(message.position, message.terrainType));
        }
    }
}