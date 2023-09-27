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
                .Where(x => 
                    (x.IsDiscovered && x.resource != null)
                    || session.tasks.Any(task => x.Position == task.Position))
                .Select(x => x.Position)
                .ToArray());
        }
    }

    class Present_TerrainPawn : Present<TerrainPawn>
    {
        public override void Refresh(TerrainPawn view)
        {
            var terrain = session.terrainItems[view.Position];

            if (terrain.resource == null)
            {
                view.resource.SetActive(false);
            }
            else
            {
                view.resource.GetComponentInChildren<Text>().text = terrain.resource.ToString();
                view.resource.SetActive(terrain.IsDiscovered);
            }

            view.workHood.gameObject.SetActive(false);

            if(session.estates.TryGetValue(view.Position, out IEstate estate))
            {
                view.workHood.gameObject.SetActive(true);

                view.workHood.productType.text = estate.ProductType.ToString();
                view.workHood.productValue.text = estate.ProductValue.ToString();
            }

            view.workHood.clanLabor.SetActive(false);
            view.workHood.productMask.SetActive(true);
            var task = session.tasks.SingleOrDefault(x => x.Position == view.Position);
            if(task != null)
            {
                view.workHood.clanLabor.SetActive(true);
                view.workHood.gameObject.SetActive(true);

                view.workHood.productMask.SetActive(false);

                var clanLabor = session.clans.SingleOrDefault(x => x.Id == task.ClanId);
                view.workHood.clanLabor.GetComponentInChildren<Text>().text = clanLabor.Name;

                switch(task)
                {
                    case DiscoverTask discoverTask:
                        {
                            view.workHood.productType.text = "探索";
                            view.workHood.productValue.text = discoverTask.Percent.ToString();
                        }
                        break;
                    case EstateBuildTask estateBuildTask:
                        {
                            view.workHood.productType.text = $"建设{estateBuildTask.estateType}";
                            view.workHood.productValue.text = estateBuildTask.Percent.ToString();
                        }
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