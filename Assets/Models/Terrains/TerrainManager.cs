using Feudal.Interfaces;
using Feudal.MessageBuses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Terrains
{
    public partial class TerrainManager 
    {
        private Dictionary<(int x, int y), TerrainItem> dict = new Dictionary<(int x, int y), TerrainItem>();
        private IMessageBus messageBus;
        private Terrain mapType;

        private Dictionary<Terrain, Dictionary<Terrain, int>> defs = new Dictionary<Terrain, Dictionary<Terrain, int>>()
        {
            {
                Terrain.Hill,
                new Dictionary<Terrain, int>()
                {
                    { Terrain.Hill, 40 },
                    { Terrain.Plain, 20 },
                    { Terrain.Mountion, 20 },
                    { Terrain.Lake, 10 },
                    { Terrain.Marsh, 5 },
                }
            },
            {
                Terrain.Plain,
                new Dictionary<Terrain, int>()
                {
                    { Terrain.Plain, 60 },
                    { Terrain.Hill, 10 },
                    { Terrain.Mountion, 2 },
                    { Terrain.Lake, 20 },
                    { Terrain.Marsh, 8 },
                }
            },
            {
                Terrain.Mountion,
                new Dictionary<Terrain, int>()
                {
                    { Terrain.Mountion, 40 },
                    { Terrain.Plain, 12 },
                    { Terrain.Hill, 37 },
                    { Terrain.Lake, 8 },
                    { Terrain.Marsh, 3 },
                }
            }
        };

        public TerrainManager(IMessageBus messageBus, Terrain mapType, int initSize = 1)
        {
            this.mapType = mapType;
            this.messageBus = messageBus;
            messageBus.Register(this);

            var center = (x: 0, y: 0);

            //var item = new TerrainItem(center, Terrain.Plain);
            //dict.Add(item.Position, item);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var position = (center.x + i, center.y + j);

                    if (position == center)
                    {
                        var item = new TerrainItem(position, Terrain.Plain);
                        dict.Add(item.Position, item);
                    }
                    else
                    {
                        var terrain = GRandom.Get(new (Terrain, int)[] { (Terrain.Plain, 65), (Terrain.Hill, 25), (Terrain.Lake, 10) });

                        var item = new TerrainItem(position, terrain);
                        dict.Add(item.Position, item);
                    }
                }
            }

            for (int i = 2; i <= initSize; i++)
            {
                for (int j = (i * -1) + 1; j < i; j++)
                {
                    var position = (center.x + i, center.y + j);
                    dict.Add(position, new TerrainItem(position, CalcTerrainType(position)));
                }

                for (int j = (i * -1) + 1; j < i; j++)
                {
                    var position = (center.x - i, center.y + j);
                    dict.Add(position, new TerrainItem(position, CalcTerrainType(position)));
                }

                for (int j = (i * -1) + 1; j < i; j++)
                {
                    var position = (center.x + j, center.y + i);
                    dict.Add(position, new TerrainItem(position, CalcTerrainType(position)));
                }

                for (int j = (i * -1) + 1; j < i; j++)
                {
                    var position = (center.x + j, center.y - i);
                    dict.Add(position, new TerrainItem(position, CalcTerrainType(position)));
                }

                var vectPos = (center.x + i, center.y + i);
                dict.Add(vectPos, new TerrainItem(vectPos, CalcTerrainType(vectPos)));

                vectPos = (center.x - i, center.y + i);
                dict.Add(vectPos, new TerrainItem(vectPos, CalcTerrainType(vectPos)));

                vectPos = (center.x + i, center.y - i);
                dict.Add(vectPos, new TerrainItem(vectPos, CalcTerrainType(vectPos)));

                vectPos = (center.x - i, center.y - i);
                dict.Add(vectPos, new TerrainItem(vectPos, CalcTerrainType(vectPos)));
            }

            dict[center].IsDiscovered = true;
            dict[center].resource = TerrainResource.FatSoil;
            dict[(center.x + 1, center.y)].resource = TerrainResource.FatSoil;
            dict[(center.x, center.y + 1)].resource = TerrainResource.CopperLode;

            //dict[center].AddTraits(TerrainTrait.FatSoil);

            //dict[(center.x + 1, center.y)].AddTraits(TerrainTrait.FatSoil);

            //dict[(center.x, center.y + 1)].AddTraits(TerrainTrait.CopperLode);
        }

        [MessageProcess]
        public void OnMessage_AddTerrainItem(Message_AddTerrainItem message)
        {
            if(dict.ContainsKey(message.position))
            {
                return;
            }

            dict.Add(message.position, new TerrainItem(message.position, CalcTerrainType(message.position)));
        }

        [MessageProcess]
        public void OnMessage_TerrainItemDiscoverChanged(Message_TerrainItemDiscoverChanged message)
        {
            dict[message.position].IsDiscovered = message.discoverd;
        }

        private Terrain CalcTerrainType((int x, int y) position)
        {
            var randoms = Enum.GetValues(typeof(Terrain)).OfType<Terrain>().ToDictionary(k => k, _ => 0);

            var nears = new List<TerrainItem>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var tempPos = (position.x + i, position.y + j);

                    if (!dict.TryGetValue(tempPos, out TerrainItem item))
                    {
                        continue;
                    }

                    nears.Add(item);
                }
            }

            foreach(var near in nears)
            {
                if (near.Terrain == mapType)
                {
                    randoms[near.Terrain] += 1;
                }
                else
                {
                    randoms[near.Terrain] += defs[mapType][near.Terrain] / nears.Count() + 1;
                }
            }

            foreach (var key in randoms.Keys.ToArray())
            {
                randoms[key] += defs[mapType][key];
            }

            return GRandom.Get(randoms.Select(x => (x.Key, x.Value)));
        }
    }

    public class GRandom
    {
        public static Random random = new Random();

        internal static Terrain Get(IEnumerable<(Terrain Key, int Value)> group)
        {
            var randomValue = random.Next(0, group.Sum(x => x.Value)+1);

            int inc = 0;
            foreach(var elem in group)
            {
                inc += elem.Value;

                if (randomValue <= inc)
                {
                    return elem.Key;
                }
            }

            throw new Exception();
        }
    }
}