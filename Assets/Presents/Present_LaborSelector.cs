using System.Linq;

namespace Feudal.Presents
{
    public class Present_LaborSelector : Present<LaborSelector>
    {
        public override void Refresh(LaborSelector view)
        {
            view.SetLaborItems(session.clans.Select(x => x.Id).ToArray());
        }
    }
}