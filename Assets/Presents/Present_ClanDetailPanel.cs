using Feudal.Tasks;
using System.Linq;

namespace Feudal.Presents
{

    class Present_ClanDetailPanel : Present<ClanDetailPanel>
    {
        public override void Refresh(ClanDetailPanel view)
        {
            view.SetEstateItems(session.estates.Values.Where(x=>x.OwnerId == view.ClanId).Select(x => x.Id).ToArray());
        }
    }

    class Present_EstateItemInClanDetail : Present<EstateItemInClanDetail>
    {
        public override void Refresh(EstateItemInClanDetail view)
        {
            var estate = session.estates.Values.Single(x => x.Id == view.EstateId);
            view.Position = estate.Position;

            view.title.text = estate.Type.ToString();
            view.outputType.text = estate.ProductType.ToString();
            view.outputValue.text = estate.ProductValue.ToString();

            var task = session.tasks.OfType<EstateWorkTask>().SingleOrDefault(x => x.estateId == estate.Id);
            if (task == null)
            {
                view.worker.text = "--";
                view.outputDisableMask.SetActive(true);
                return;
            }

            view.outputDisableMask.SetActive(false);

            var clan = session.clans.SingleOrDefault(x => x.Id == task.ClanId);
            view.worker.text = clan.Name;
        }
    }

    class Present_TotalEstateView : Present<TotalEstateView>
    {
        public override void Refresh(TotalEstateView view)
        {
            var clanId = view.GetComponentInParent<ClanDetailPanel>().ClanId;

            var estates = session.estates.Values.Where(x => x.OwnerId == clanId);
            view.count.text = estates.Count().ToString();
        }
    }
}