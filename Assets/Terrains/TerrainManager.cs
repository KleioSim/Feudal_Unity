using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections;
using System.Collections.Generic;

namespace Feudal.Terrains
{
    public class TerrainManager : IReadOnlyDictionary<(int x, int y), ITerrainItem>
    {
        private Dictionary<(int x, int y), ITerrainItem> dict = new Dictionary<(int x, int y), ITerrainItem>();
        private IMessageBus messageBus;

        public IEnumerable<(int x, int y)> Keys => ((IReadOnlyDictionary<(int x, int y), ITerrainItem>)dict).Keys;

        public IEnumerable<ITerrainItem> Values => ((IReadOnlyDictionary<(int x, int y), ITerrainItem>)dict).Values;

        public int Count => ((IReadOnlyCollection<KeyValuePair<(int x, int y), ITerrainItem>>)dict).Count;

        public ITerrainItem this[(int x, int y) key] => ((IReadOnlyDictionary<(int x, int y), ITerrainItem>)dict)[key];

        public bool ContainsKey((int x, int y) key)
        {
            return ((IReadOnlyDictionary<(int x, int y), ITerrainItem>)dict).ContainsKey(key);
        }

        public bool TryGetValue((int x, int y) key, out ITerrainItem value)
        {
            return ((IReadOnlyDictionary<(int x, int y), ITerrainItem>)dict).TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<(int x, int y), ITerrainItem>> IEnumerable<KeyValuePair<(int x, int y), ITerrainItem>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<(int x, int y), ITerrainItem>>)dict).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)dict).GetEnumerator();
        }

        public TerrainManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);

            dict.Add((0, 0), new TerrainItem((0, 0), Terrain.Plain));
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