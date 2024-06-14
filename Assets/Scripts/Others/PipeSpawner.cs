using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class PipeSpawner : BaseGameObject
{
    [SerializeField] float _timeEachSpawn;
    private float _entryTime;
    private bool _canSpawn = true;

    protected override void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.GameOnStart, OnAllowUpdate);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnLose, StopSpawn);
    }

    protected override void Start()
    {
        _entryTime = Time.time;
    }

    protected override void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnLose, StopSpawn);
    }

    protected override void Update()
    {
        HandleSpawnPipe();
    }

    private void HandleSpawnPipe()
    {
        if (!_canSpawn || !_allowUpdate) return;

        if (Time.time - _entryTime >= _timeEachSpawn)
        {
            _entryTime = Time.time;
            GameObject pipe = ObjectPool.Instance.GetObjectInPool(EPoolable.PipeControl);
            pipe.SetActive(true);
            string pipeID = pipe.GetComponent<PipeController>().PipeID;

            PipeControlNewPosInfor PipeControlNewPosInfor = new(pipeID, transform.position);
            EventsManager.Instance.NotifyObservers(GameEvents.PipeOnReceiveNewPos, PipeControlNewPosInfor);
        }
    }

    private void StopSpawn(object obj)
    {
        _canSpawn = false;
    }
}
