using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

[System.Serializable]
public struct Sounds
{
    public ESfx _sfxName;
    public AudioClip _clip;
}

public class SoundsManager : BaseSingleton<SoundsManager>
{
    [SerializeField] List<Sounds> _listSounds = new();
    [SerializeField] float _delayDieSfx;
    private AudioSource _source;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.OnPlaySfx, PlaySfx);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.OnPlaySfx, PlaySfx);
    }

    private void PlaySfx(object obj)
    {
        foreach (var sound in _listSounds)
        {
            if ((ESfx)obj == sound._sfxName)
            {
                if(sound._sfxName == ESfx.Die)
                {
                    StartCoroutine(PlayDeadSfx(sound._clip));
                    return;
                }
                _source.clip = sound._clip;
                _source.PlayOneShot(_source.clip, 1.0f);
                //Debug.Log("Played: " + sound._sfxName);
            }
        }
    }

    private IEnumerator PlayDeadSfx(AudioClip clip)
    {
        yield return new WaitForSeconds(_delayDieSfx);

        _source.clip = clip;
        _source.PlayOneShot(_source.clip, 0.5f);
    }
}
