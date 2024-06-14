using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class ScrollGround : MonoBehaviour
{
    [SerializeField] float _scrollSpeed;
    [SerializeField] float _initX;
    [SerializeField] float _minX;

    bool _canMove = true;

    private void Start()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnLose, StopMove);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnLose, StopMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canMove) return;

        transform.Translate(new Vector3(-_scrollSpeed, 0f, 0f) * Time.deltaTime);

        if (transform.position.x < _minX)
            transform.position = new Vector3(_initX, transform.position.y, transform.position.z);
    }

    private void StopMove(object obj)
    {
        _canMove = false;
    }
}
