using Feudal.Interfaces;
using Feudal.MessageBuses;
using Feudal.Terrains;
using System;
using System.Collections;
using System.Collections.Generic;
using Feudal.Tasks;
using Feudal.Clans;

public class Session
{
    public IReadOnlyDictionary<(int x, int y), ITerrainItem> terrainItems => terrainMgr;
    public IEnumerable<ITask> tasks => taskMgr;
    public IEnumerable<IClan> clans => clanMgr;

    private IMessageBus messageBus;

    private TerrainManager terrainMgr;
    private TaskManager taskMgr;
    private ClanManager clanMgr;

    public void ExecUICmd(UICommand uiCmd)
    {
        switch(uiCmd)
        {
            case DiscoverCommand command:
                messageBus.PostMessage(new Message_AddTask(typeof(DiscoverTask), new object[] { command.position }));
                break;
            case NexTurnCommand:
                messageBus.PostMessage(new Message_NextTurn());
                break;
            case CancelTaskCommand command:
                messageBus.PostMessage(new Message_CancelTask(command.taskId));
                break;
            case UpdateViewCommand:
                break;
            default:
                throw new Exception();
        }
    }

    public Session()
    {
        messageBus = new MessageBus();

        terrainMgr = new TerrainManager(messageBus);
        taskMgr = new TaskManager(messageBus);
        clanMgr = new ClanManager(messageBus);
    }
}