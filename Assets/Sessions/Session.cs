using Feudal.Clans;
using Feudal.Estates;
using Feudal.Interfaces;
using Feudal.MessageBuses;
using Feudal.Tasks;
using Feudal.Terrains;
using System;
using System.Collections.Generic;

public class Session
{
    public IReadOnlyDictionary<(int x, int y), ITerrainItem> terrainItems => terrainMgr;
    public IReadOnlyDictionary<(int x, int y), IEstate> estates => estateMgr;

    public IEnumerable<ITask> tasks => taskMgr;
    public IEnumerable<IClan> clans => clanMgr;

    private IMessageBus messageBus;

    private TerrainManager terrainMgr;
    private TaskManager taskMgr;
    private ClanManager clanMgr;
    private EstateManager estateMgr;

    public void ExecUICmd(UICommand uiCmd)
    {
        switch(uiCmd)
        {
            case UpdateViewCommand:
                break;
            case DiscoverCommand command:
                messageBus.PostMessage(new Message_AddTask(typeof(DiscoverTask), command.clanId, new object[] { command.position }));
                break;
            case NexTurnCommand:
                messageBus.PostMessage(new Message_NextTurn());
                break;
            case CancelTaskCommand command:
                messageBus.PostMessage(new Message_CancelTask(command.taskId));
                break;
            case EstateStartWorkCommand command:
                messageBus.PostMessage(new Message_AddTask(typeof(EstateWorkTask), command.clanId, new object[] { command.position, command.estateId }));
                break;
            default:
                throw new Exception();
        }
    }

    public Session()
    {
        messageBus = new MessageBus();

        terrainMgr = new TerrainManager(messageBus, Terrain.Hill);
        taskMgr = new TaskManager(messageBus);
        clanMgr = new ClanManager(messageBus);
        estateMgr = new EstateManager(messageBus);
    }
}