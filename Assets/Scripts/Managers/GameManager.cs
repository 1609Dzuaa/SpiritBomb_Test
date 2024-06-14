using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    //Dùng nếu kh muốn lưu data giữa các phiên chơi

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
