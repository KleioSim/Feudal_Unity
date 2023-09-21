using Feudal.Tasks;
using System.Linq;

namespace Feudal.Presents
{
    class Present_EstateStaticItem : Present<EstateStaticsItem>
    {
        public override void Refresh(EstateStaticsItem view)
        {
            var estate = session.estates.Values.Single(x => x.Id == view.EstateId);
            view.Position = estate.Position;

            view.title.text = estate.Type.ToString();
            view.outputType.text = estate.ProductType.ToString();
            view.outputValue.text = estate.ProductValue.ToString();

            var task = session.tasks.OfType<EstateWorkTask>().SingleOrDefault(x => x.estateId == estate.Id);
            if(task == null)
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
}