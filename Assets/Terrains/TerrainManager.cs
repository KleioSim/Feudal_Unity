using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Terrains
{
    public class TerrainManager : IReadOnlyDictionary<(int x, int y), ITerrainItem>
    {
        private Dictionary<(int x, int y), TerrainItem> dict = new Dictionary<(int x, int y), TerrainItem>();
        private IMessageBus messageBus;

        public IEnumerable<(int x, int y)> Keys => ((IReadOnlyDictionary<(int x, int y), TerrainItem>)dict).Keys;

        public IEnumerable<ITerrainItem> Values => ((IReadOnlyDictionary<(int x, int y), TerrainItem>)dict).Values;

        public int Count => ((IReadOnlyCollection<KeyValuePair<(int x, int y), TerrainItem>>)dict).Count;

        public ITerrainItem this[(int x, int y) key] => ((IReadOnlyDictionary<(int x, int y), TerrainItem>)dict)[key];

        public bool ContainsKey((int x, int y) key)
        {
            return ((IReadOnlyDictionary<(int x, int y), TerrainItem>)dict).ContainsKey(key);
        }

        public bool TryGetValue((int x, int y) key, out ITerrainItem value)
        {
            var ret = ((IReadOnlyDictionary<(int x, int y), TerrainItem>)dict).TryGetValue(key, out TerrainItem tempValue);
            value = tempValue;
            return ret;
        }

        IEnumerator<KeyValuePair<(int x, int y), ITerrainItem>> IEnumerable<KeyValuePair<(int x, int y), ITerrainItem>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<(int x, int y), TerrainItem>>)dict).Select(x=>new KeyValuePair<(int x, int y), ITerrainItem>(x.Key, x.Value)).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)dict).GetEnumerator();
        }

        public TerrainManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);

            for(int x=0; x<3; x++)
            {
                for(int y=0; y<3; y++)
                {
                    dict.Add((x, y), new TerrainItem((x, y), x + y % 2 == 0 ? Terrain.Hill : Terrain.Plain));
                }
            }

            dict[(0, 0)].IsDiscovered = true;
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

        [MessageProcess]
        public void OnMessage_TerrainItemDiscoverChanged(Message_TerrainItemDiscoverChanged message)
        {
            dict[message.position].IsDiscovered = message.discoverd;
        }
    }
}