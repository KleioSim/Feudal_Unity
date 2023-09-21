using System.Linq;

namespace Feudal.Presents
{
    class Present_EstateStaticsPanel : Present<EstateStaticsPanel>
    {
        public override void Refresh(EstateStaticsPanel view)
        {
            view.SetEstateItems(session.playerClan.estates.Select(x=>x.Id).ToArray());
        }
    }
}