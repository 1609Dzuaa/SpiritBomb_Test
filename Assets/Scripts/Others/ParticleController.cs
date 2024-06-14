using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleController : MonoBehaviour
{
    [SerializeField] float _minX, _maxX, _minY, _maxY;
    [SerializeField] Image _particleImg;

    private void OnEnable()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.GameEvents.ParticleOnPopUp, AllowParticle);
        _particleImg.color = new(0f, 0f, 0f, 0f);
    }

    private void OnDisable()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.GameEvents.ParticleOnPopUp, AllowParticle);
    }

    //Vứt ở cuối frame trong Animation Event
    private void ReceiveNewPosition()
    {
        float newPosX = Random.Range(_minX, _maxX);
        float newPosY = Random.Range(_minY, _maxY);

        transform.localPosition = new Vector3(newPosX, newPosY, transform.position.z);
    }

    private void AllowParticle(object obj)
    {
        _particleImg.color = new(255f, 255f, 255f, 255f);
    }
}
