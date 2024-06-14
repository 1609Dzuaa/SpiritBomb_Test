using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;

public class NewController : MonoBehaviour
{
    [SerializeField] Image _newImg;

    private void OnEnable()
    {
        _newImg.color = new(0f, 0f, 0f, 0f);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.NewOnPopUp, AllowPopUpNewImage);
    }

    private void OnDisable()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.NewOnPopUp, AllowPopUpNewImage);
    }

    private void AllowPopUpNewImage(object obj)
    {
        _newImg.color = new(255f, 255f, 255f, 255f);
    }
}
