using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class Pipe : GameObjectCollidable
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FireCollisionEvent()
    {
        EventsManager.Instance.NotifyObservers(GameEvents.PlayerOnCollidedPipe, null);
        //Debug.Log("Pipe Collided");
    }
}
