using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Estates
{
    public class EstateManager : IReadOnlyDictionary<(int x, int y), IEstate>
    {
        private Dictionary<(int x, int y), Estate> estates = new Dictionary<(int x, int y), Estate>();
        private IMessageBus messageBus;

        public EstateManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);

            var estate = new Estate((0, 0), EstateType.Farm);
            estates.Add(estate.Position, estate);
        }

        [MessageProcess]
        public void OnMessage_AddEstate(Message_AddEstate message)
        {
            var estate = new Estate(message.position, message.estateType);
            estates.Add(estate.Position, estate);
        }

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