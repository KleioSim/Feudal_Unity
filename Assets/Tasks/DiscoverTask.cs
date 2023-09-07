using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections.Generic;

namespace Feudal.Tasks
{
    public class DiscoverTask : Task
    {
        public DiscoverTask(string clanId, object[] parameters) : base(clanId)
        {
            Position = (((int x, int y))parameters[0]);
        }

        internal override IEnumerable<Message> OnFinished()
        {
            var messages = new List<Message>();

            messages.Add(new Message_TerrainItemDiscoverChanged(Position, true));
            for (int i = Position.x - 1; i <= Position.x + 1; i++)
            {
                for (int j = Position.y - 1; j <= Position.y + 1; j++)
                {
                    var pos = (i, j);
                    messages.Add(new Message_AddTerrainItem(pos, Terrain.Hill));
                }
            }

            return messages;
        }
    }
}