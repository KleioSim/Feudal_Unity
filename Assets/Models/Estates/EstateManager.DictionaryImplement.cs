using Feudal.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Estates
{
    public partial class EstateManager : IReadOnlyDictionary<(int x, int y), IEstate>
    {
        public IEstate this[(int x, int y) key] => ((IReadOnlyDictionary<(int x, int y), Estate>)estates)[key];

        public IEnumerable<(int x, int y)> Keys => ((IReadOnlyDictionary<(int x, int y), Estate>)estates).Keys;

        public IEnumerable<IEstate> Values => ((IReadOnlyDictionary<(int x, int y), Estate>)estates).Values;

        public int Count => ((IReadOnlyCollection<KeyValuePair<(int x, int y), Estate>>)estates).Count;

        public bool ContainsKey((int x, int y) key)
        {
            return ((IReadOnlyDictionary<(int x, int y), Estate>)estates).ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<(int x, int y), IEstate>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<(int x, int y), Estate>>)estates).Select(x => new KeyValuePair<(int x, int y), IEstate>(x.Key, x.Value)).GetEnumerator();
        }

        public bool TryGetValue((int x, int y) key, out IEstate value)
        {
            var ret = ((IReadOnlyDictionary<(int x, int y), Estate>)estates).TryGetValue(key, out Estate estate);
            value = estate;

            return ret;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)estates).GetEnumerator();
        }
    }
}