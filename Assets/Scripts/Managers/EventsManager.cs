using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class EventsManager : BaseSingleton<EventsManager>
{
    private Dictionary<GameEvents, Action<object>> _dictEvents = new();
    private Action<object> PlayerOnCollidedGround;
    private Action<object> PlayerOnCollidedPipe;
    private Action<object> PlayerOnLose;
    private Action<object> PlayerOnPass;
    private Action<object> PipeOnReceiveNewPos;
    private Action<object> OnPlaySfx;
    private Action<object> GameOverOnPopUp;
    private Action<object> ScoreBoardOnPopUp;
    private Action<object> ScoreBoardOnUpdateScore;
    private Action<object> UIButtonOnPopUp;
    private Action<object> ButtonStartOnClick;
    private Action<object> GameOverOnAllowPopUp;
    private Action<object> ParticleOnPopUp;
    private Action<object> NewOnPopUp;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        AddEventsToDictionary();
    }

    private void AddEventsToDictionary()
    {
        HandleAddEventsToDictionary(GameEvents.PlayerOnCollidedGround, PlayerOnCollidedGround);
        HandleAddEventsToDictionary(GameEvents.PlayerOnCollidedPipe, PlayerOnCollidedPipe);
        HandleAddEventsToDictionary(GameEvents.PlayerOnLose, PlayerOnLose);
        HandleAddEventsToDictionary(GameEvents.PlayerOnPass, PlayerOnPass);
        HandleAddEventsToDictionary(GameEvents.PipeOnReceiveNewPos, PipeOnReceiveNewPos);
        HandleAddEventsToDictionary(GameEvents.OnPlaySfx, OnPlaySfx);
        HandleAddEventsToDictionary(GameEvents.GameOverOnPopUp, GameOverOnPopUp);
        HandleAddEventsToDictionary(GameEvents.ScoreBoardOnPopUp, ScoreBoardOnPopUp);
        HandleAddEventsToDictionary(GameEvents.ScoreBoardOnUpdateScore, ScoreBoardOnUpdateScore);
        HandleAddEventsToDictionary(GameEvents.UIButtonOnPopUp, UIButtonOnPopUp);
        HandleAddEventsToDictionary(GameEvents.ButtonStartOnClick, ButtonStartOnClick);
        HandleAddEventsToDictionary(GameEvents.GameOverOnAllowPopUp, GameOverOnAllowPopUp);
        HandleAddEventsToDictionary(GameEvents.ParticleOnPopUp, ParticleOnPopUp);
        HandleAddEventsToDictionary(GameEvents.NewOnPopUp, NewOnPopUp);
    }

    private void HandleAddEventsToDictionary(GameEvents events, Action<object> actions)
    {
        //Vì thứ tự chạy của Awake & OnEnable khá loạn nên phải check kỹ
        if (!_dictEvents.ContainsKey(events))
            _dictEvents.Add(events, actions);
    }

    public void SubcribeToAnEvent(GameEvents eventType, Action<object> function)
    {
        if (!_dictEvents.ContainsKey(eventType))
            _dictEvents.Add(eventType, function);

        _dictEvents[eventType] += function;
        //Debug.Log("Func Registered: " + eventType);
    }

    public void UnSubcribeToAnEvent(GameEvents eventType, Action<object> function)
    {
        _dictEvents[eventType] -= function;
    }

    public void NotifyObservers(GameEvents eventType, object eventArgsType)
    {
        _dictEvents[eventType]?.Invoke(eventArgsType);
    }
}
