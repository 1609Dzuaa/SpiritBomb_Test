using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;

public class UIGameObject : MonoBehaviour
{
    [SerializeField, Tooltip("Giá trị alpha mỗi lần chỉnh sửa")] float _alphaModify;
    [SerializeField] float _timeEachModify;
    [SerializeField] Image _imageUI;
    [SerializeField, Tooltip("Tick vào chọn nếu obj này tăng dần alpha")] bool _isIncreasing;

    private float _entryTime;
    private float _alpha = 0f;
    private bool _isAllowToIncrease;

    private void OnEnable()
    {
        _imageUI.color = new(_imageUI.color.r, _imageUI.color.g, _imageUI.color.b, (_isIncreasing) ? 0f : 1f);
        _alpha = (_isIncreasing) ? 0f : 1f;
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.GameOverOnPopUp, ReceiveNotify);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.WhiteFrameOnPopUp, WhiteFrameReceiveNotify);
    }

    private void OnDisable()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.GameOverOnPopUp, ReceiveNotify);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.WhiteFrameOnPopUp, WhiteFrameReceiveNotify);
        _isAllowToIncrease = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isIncreasing)
            HandleIncreaseAlpha();
        else 
            HandleDecreaseAlpha();
    }

    private void ReceiveNotify(object obj)
    {
        _isAllowToIncrease = true;
        _entryTime = Time.time;
        //Debug.Log("Called");
    }

    private void WhiteFrameReceiveNotify(object obj)
    {
        _isAllowToIncrease = true;
        _entryTime = Time.time;
        //Debug.Log("Called");
    }

    private void HandleIncreaseAlpha()
    {
        if (!_isAllowToIncrease) return;
        if (_alpha >= 1.0f) return;

        if (Time.time - _entryTime >= _timeEachModify)
        {
            _entryTime = Time.time;
            _alpha += _alphaModify;
            _imageUI.color = new(_imageUI.color.r, _imageUI.color.g, _imageUI.color.b, _alpha);
            //Debug.Log("Alpha: " + _alpha);
        }
    }

    private void HandleDecreaseAlpha()
    {
        if (!_isAllowToIncrease) 
            return;
        if (_alpha <= 0.0f) 
            return;

        if (Time.time - _entryTime >= _timeEachModify)
        {
            _entryTime = Time.time;
            _alpha -= _alphaModify;
            _imageUI.color = new(_imageUI.color.r, _imageUI.color.g, _imageUI.color.b, _alpha);
            //Debug.Log("Alpha: " + _alpha);
        }
    }
}
