using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class TriggerZone : GameObjectCollidable
{
    private bool _activated;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        _activated = false;
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
        if (_activated) return;

        EventsManager.Instance.NotifyObservers(GameEvents.PlayerOnPass, null);
        EventsManager.Instance.NotifyObservers(GameEvents.OnPlaySfx, ESfx.Passed);
        //Debug.Log("Fired");
        _activated = true;
    }

    protected override void HandleCollision() { }
}
