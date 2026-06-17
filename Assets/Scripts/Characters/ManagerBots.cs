using Netologia.Quest.Characters;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ManagerBots : MonoBehaviour
{
    private List<BaseMovement> _bots = new List<BaseMovement>();

    private void Start()
    {
        _bots.AddRange(GetComponentsInChildren<BaseMovement>());
    }

    private void Update()
    {
        foreach (var bot in _bots)
        {
            bot.ManualUpdate();
        }
    }

    public List<BaseMovement> GetBots() => _bots;
    public void AddBotOnCollection(BaseMovement bot) => _bots.Add(bot);
    public void AddBotsOnCollection(BaseMovement[] bots) => _bots.AddRange(bots);
    public void AddBotsOnCollection(List<BaseMovement> bots) => _bots.AddRange(bots);

    public static MovementPointData RandomIdlePointGenerate(WorkerMovement bot, ManagerObjects managerObjects)
    {
        List<MovementPointData> idlePoints = new List<MovementPointData>();
        var objects = managerObjects.GetCollectionIdlePoints;
        foreach (var idlePoint in managerObjects.GetCollectionIdlePoints)
        {
            bool isDistanceUse = Vector3.Distance(bot.transform.position, idlePoint.ActionPointPosition) <= bot.RadiusSerchIdle;
            if (isDistanceUse && idlePoint.CharacterCanUse(bot))
            {
                idlePoints.Add(idlePoint);
            }
        }

        if (idlePoints.Count > 0)
        {
            int indexRandom = UnityEngine.Random.Range(0, idlePoints.Count - 1);
            idlePoints[indexRandom].SetCharacterUse(bot);
            return idlePoints[indexRandom];
        }
        else
            return null;
    }
}
