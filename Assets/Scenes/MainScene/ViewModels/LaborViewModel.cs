#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    class LaborViewModel : ViewModel
    {
        public readonly string clanId;

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private int idleCount;
        public int IdleCount
        {
            get => idleCount;
            set
            {
                SetProperty(ref idleCount, value);

                IsEnable = IdleCount != 0;
            }
        }

        private int totalCount;
        public int TotalCount
        {
            get => totalCount;
            set => SetProperty(ref totalCount, value);
        }

        private bool isEnable;
        public bool IsEnable
        {
            get => isEnable;
            private set => SetProperty(ref isEnable, value);
        }

        public LaborViewModel(string clanId)
        {
            this.clanId = clanId;
        }
    }
}