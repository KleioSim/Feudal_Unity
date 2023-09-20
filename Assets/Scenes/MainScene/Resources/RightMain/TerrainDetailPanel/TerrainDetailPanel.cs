using Feudal.Scenes.Main;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TerrainDetailPanel : RightMain
{
    public (int x, int y) Position { get; set; }

    public Text title;
    public GameObject workDetailPanel;

    public TraitContainer traitContainer;

    private WorkHood currentWorkHood => workHoods.SingleOrDefault(x => x.isActiveAndEnabled);
    private WorkHood[] workHoods => workDetailPanel.GetComponentsInChildren<WorkHood>(true);

    private LaborWorkDetail laborWork => workDetailPanel.GetComponentsInChildren<LaborWorkDetail>(true).Single();

    void Start()
    {
        var laborWork = workDetailPanel.GetComponentsInChildren<LaborWorkDetail>(true).Single();
        
        laborWork.AddLaborButton.onClick.AddListener(() =>
        {
            showSub.Invoke(typeof(LaborSelector), (obj) =>
            {
                currentWorkHood.SetLabor(obj as string);
            });
        });

        laborWork.RemoveLaborButton.onClick.AddListener(() =>
        {
            ExecUICmd(new CancelTaskCommand(laborWork.taskId));
        });
    }

    public T SetCurrentWorkHood<T>() where T : WorkHood
    {
        workDetailPanel.SetActive(true);

        var current = workHoods.Single(x => x is T) as T;
        current.gameObject.SetActive(true);

        foreach (var workHood in workHoods.Where(x => x != current))
        {
            workHood.gameObject.SetActive(false);
        }

        laborWork.Position = Position;

        return current;
    }
}
