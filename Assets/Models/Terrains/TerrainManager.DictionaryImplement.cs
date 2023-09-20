using Feudal.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Terrains
{
    public partial class TerrainManager : IReadOnlyDictionary<(int x, int y), ITerrainItem>
    {

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
            return ((IEnumerable<KeyValuePair<(int x, int y), TerrainItem>>)dict).Select(x => new KeyValuePair<(int x, int y), ITerrainItem>(x.Key, x.Value)).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)dict).GetEnumerator();
        }

    }
}