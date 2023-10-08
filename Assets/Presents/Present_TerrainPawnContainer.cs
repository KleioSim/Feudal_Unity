using Feudal.Interfaces;
using Feudal.Tasks;
using System.Linq;
using UnityEngine.UI;

namespace Feudal.Presents
{
    class Present_TerrainPawnContainer : Present<TerrainPawnContainer>
    {
        public override void Refresh(TerrainPawnContainer view)
        {
            view.SetTraitItems(session.terrainItems.Values
                .Where(x =>IsNeedPawn(x))
                .Select(x => x.Position)
                .ToArray());
        }

        private bool IsNeedPawn(ITerrainItem x)
        {
            return (x.IsDiscovered && x.resource != null)
                                || session.tasks.Any(task => x.Position == task.Position);
        }
    }

    class Present_TerrainPawn : Present<TerrainPawn>
    {
        public override void Refresh(TerrainPawn view)
        {
            var position = (view.Position.x, view.Position.y);

            var terrain = session.terrainItems[position];

            if (!terrain.IsDiscovered)
            {
                view.SetResource(null);
            }
            else
            {
                view.SetResource(terrain.resource?.ToString());
            }

            if(session.estates.TryGetValue(position, out IEstate estate))
            {
                view.workHood.SetProduct((estate.ProductType.ToString(), estate.ProductValue));
            }

            var task = session.tasks.SingleOrDefault(x => x.Position == position);
            if (task == null)
            {
                view.workHood.SetLabor(null);
            }
            else
            {

                var clanLabor = session.clans.SingleOrDefault(x => x.Id == task.ClanId);
                view.workHood.SetLabor(clanLabor.Name);

                switch (task)
                {
                    case DiscoverTask discoverTask:
                        view.workHood.SetProduct(("探索", discoverTask.Percent));
                        break;
                    case EstateBuildTask estateBuildTask:
                        view.workHood.SetProduct(($"建设{estateBuildTask.estateType}", estateBuildTask.Percent));
                        break;
                    case EstateWorkTask:
                        break;
                    default:
                        throw new System.Exception();
                }
            }
        }
    }
}