using Feudal.Scenes.Main;

namespace Feudal.Presents
{
    class Present_MainScene : Present<MainScene>
    {
        public override void Refresh(MainScene view)
        {
            view.PlayerClanId = session.playerClan.Id;
        }
    }
}