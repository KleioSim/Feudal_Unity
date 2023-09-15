//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Feudal.Scenes.Main
//{
//    public class TerrainWorkDetail : MonoBehaviour
//    {
//        public GameObject workHoodContent;

//        public GameObject laborPanel;
//        public Text laborTitle;

//        public (int x, int y) Position { get; set; }
//        public WorkHood CurrentWorkHood => workHoodContent.GetComponentInChildren<WorkHood>();

//        internal T SetCurrentWorkHood<T>() where T : WorkHood
//        {
//            this.gameObject.SetActive(true);

//            var workHoods = workHoodContent.GetComponentsInChildren<WorkHood>(true);

//            var currentWorkHood = workHoods.Single(x => x is T) as T;
//            currentWorkHood.gameObject.SetActive(true);

//            foreach (var workHood in workHoods.Where(x => x != currentWorkHood))
//            {
//                workHood.gameObject.SetActive(false);
//            }

//            return currentWorkHood;
//        }
//    }
//}
