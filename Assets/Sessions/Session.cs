using Feudal.Interfaces;
using Feudal.MessageBuses;
using Feudal.Terrains;
using System;
using System.Collections;
using System.Collections.Generic;
using Feudal.Tasks;

public class Session
{
    public IEnumerable<ITerrainItem> terrainItems => terrainMgr;
    public IEnumerable<ITask> tasks => taskMgr;

    private IMessageBus messageBus;

    private TerrainManager terrainMgr;
    private TaskManager taskMgr;

    public void ExecUICmd(UICommand uiCmd)
    {
        switch(uiCmd)
        {
            case DiscoverCommand command:
                messageBus.PostMessage(new Message_AddTask(typeof(DiscoverTask), new object[] { command.position }));
                break;
            case NexTurnCommand command:
                messageBus.PostMessage(new Message_NextTurn());
                break;
            case CancelTaskCommand command:
                messageBus.PostMessage(new Message_CancelTask(command.taskId));
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
    }
}
