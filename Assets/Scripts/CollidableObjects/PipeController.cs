using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public struct PipeControlNewPosInfor
{
    public string ID;
    public Vector3 Position;

    public PipeControlNewPosInfor(string id, Vector3 pos)
    {
        ID = id;
        Position = pos;
    }
}

public class PipeController : BaseGameObject
{
    [SerializeField] float _xVelo;
    [SerializeField] float _existTime;
    [SerializeField] int _minOffset;
    [SerializeField] int _maxOffset;

    private bool _canMove = true;
    private float _entryTime;
    private string _pipeID;

    public string PipeID { get => _pipeID; }

    protected override void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.GameOnStart, OnAllowUpdate);
    }

    private void OnEnable()
    {
        _entryTime = Time.time;
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnLose, StopMove);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PipeOnReceiveNewPos, ReceiveNewPosition);
    }

    private void OnDisable()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnLose, StopMove);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PipeOnReceiveNewPos, ReceiveNewPosition);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        _pipeID = "Pipe " + Guid.NewGuid().ToString();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!_allowUpdate) return;

        Move();
        CheckIfOutOfLifeTime();
    }

    private void Move()
    {
        if (_canMove) transform.position -= new Vector3(_xVelo, 0f, 0f) * Time.deltaTime;
    }

    private void CheckIfOutOfLifeTime()
    {
        if (!_canMove) return;

        if (Time.time - _entryTime >= _existTime)
            gameObject.SetActive(false);
    }

    private void StopMove(object obj)
    {
        //Player lose thì stop hết các pipe
        _canMove = false;
    }

    private void ReceiveNewPosition(object  obj)
    {
        PipeControlNewPosInfor info = (PipeControlNewPosInfor)obj;

        if (_pipeID != info.ID) return;

        //Trả PipeController về vị trí spawn
        transform.position = info.Position;

        float newPosY = UnityEngine.Random.Range(_minOffset, _maxOffset + 1) / 10f;
        Debug.Log("NewPosY: " + newPosY);

        transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
    }
}
