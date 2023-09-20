using Feudal.Scenes.Main;
using Feudal.Tasks;
using System.Linq;

namespace Feudal.Presents
{
    class Present_EstateWorkHood : Present<EstateWorkHood>
    {
        public override void Refresh(EstateWorkHood view)
        {
            var estate = session.estates[view.Position];

            view.title.text = estate.Type.ToString();
            view.productType.text = estate.ProductType.ToString();
            view.productValue.text = estate.ProductValue.ToString();

            var task = session.tasks.OfType<EstateWorkTask>().SingleOrDefault(x => x.estateId == estate.Id);
            view.disableMask.SetActive(task == null);
        }
    }
}