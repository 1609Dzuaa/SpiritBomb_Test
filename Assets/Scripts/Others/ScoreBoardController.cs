using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;

[System.Serializable]
public struct Medal
{
    public EMedal _medalName;
    public Sprite _medalImage;
}

public class ScoreBoardController : MonoBehaviour
{
    //ScoreBoard khi PopUp sẽ hướng lên đến vị trí nhất định

    //Còn UI _curScr, HighestScore
    //_curScr lên dần dần
    //PopUp PlayAgain, Share ?
    //Loop BG, Loop Ground
    //UI Responsive ?

    //Medal:
    //< 10: Không có
    //>= 10: Đồng
    //>= 20: Bạc
    //>= 30: Vàng
    //>= 40: Bạch Kim

    //SB: ScoreBoard
    //CS: CurrentScore

    //Spawn Particle ở vị trí random trong khu vực Medal

    [SerializeField] float _popUpSpeed;
    [SerializeField] float _maxYOffset;
    [SerializeField] float _medalRadius;

    [SerializeField] TextMeshProUGUI _txtScore;
    [SerializeField] TextMeshProUGUI _txtBest;
    [SerializeField] Image _imgMedal;
    [SerializeField, Tooltip("Khoảng thgian mỗi lần tăng 1đ")] float _timeEachIncrease;

    [SerializeField] List<Medal> _listMedals;
    Dictionary<EMedal, Sprite> _dictMedals = new();

    Vector3 _initPos;
    float _maxY;
    bool _allowPopUp; //Cho phép SB Hiện lên
    bool _allowSpawnParticle;
    bool _allowUpdateCurScr; //Cho phép cập nhật CS
    float _entryTime; //Bấm giờ để cập nhật CS
    int _curScr = 0; //Điểm hiện tại
    int _tempCurScr = 0; //Biến tạm điểm hiện tại
    bool _allowPopUpUIButton; //Vì phải phụ thuộc max CS thì UIButton mới PopUp đc

    private void Awake()
    {
        _maxY = transform.position.y + _maxYOffset;
        _initPos = transform.position;
        FillInDictionary();
        //Debug.Log("Subbed");
    }

    private void OnEnable()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.ScoreBoardOnPopUp, AllowPopUp);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.ScoreBoardOnUpdateScore, UpdateScoreBoard);
        _imgMedal.color = new(0f, 0f, 0f, 0f);
    }

    private void OnDisable()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.ScoreBoardOnPopUp, AllowPopUp);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.ScoreBoardOnUpdateScore, UpdateScoreBoard);
        ResetValues();
    }

    private void FillInDictionary()
    {
        for (int i = 0; i < _listMedals.Count; i++)
            if (!_dictMedals.ContainsKey(_listMedals[i]._medalName))
            {
                _dictMedals.Add(_listMedals[i]._medalName, _listMedals[i]._medalImage);
                //Debug.Log("Added: " + _listMedals[i]._medalName);
            }
    }

    // Update is called once per frame
    private void Update()
    {
        HandlePopUp();
        HandleCurrentScore();
    }

    private void HandlePopUp()
    {
        if (_allowPopUp)
        {
            if (_maxY <= transform.position.y)
            {
                if (!_allowUpdateCurScr) AllowUpdateCurrentScore();
                return;
            }

            transform.Translate(new Vector3(0f, _popUpSpeed, 0f) * Time.deltaTime);
            //Debug.Log("here");
        }
    }

    private void AllowPopUp(object obj)
    {
        _allowPopUp = true;
    }

    //ScoreBoard bao gồm 2 phần: Score(Current, Best) và Medal(Br, Sil, ...)
    private void UpdateScoreBoard(object obj)
    {
        _curScr = (int)obj;
        //AllowSpawnParticle();
        int highestScore;
        //Debug.Log("Current: " + _curScr);

        if (PlayerPrefs.HasKey(ESpecialKeys.HighestScore.ToString()))
            highestScore = PlayerPrefs.GetInt(ESpecialKeys.HighestScore.ToString());
        else
        {
            //Chưa có key <=> đây là lân chơi đầu => điểm cao nhất cũng chính là _curScr
            highestScore = _curScr;
            SaveHighestScore(highestScore);
        }

        if (_curScr > highestScore)
        {
            highestScore = _curScr;
            SaveHighestScore(highestScore);
        }

        UpdateScore(_txtBest, highestScore.ToString());
    }

    private void SaveHighestScore(int highest)
    {
        PlayerPrefs.SetInt(ESpecialKeys.HighestScore.ToString(), highest);
        PlayerPrefs.Save();
        EventsManager.Instance.NotifyObservers(GameEvents.NewOnPopUp, null);
    }

    private void UpdateScore(TextMeshProUGUI txt, string scr)
    {
        txt.text = scr;
    }

    private void UpdateMedal()
    {
        //Debug.Log("Cur: " + _curScr);
        _imgMedal.color = new(255f, 255f, 255f, 255f);
        if (_curScr >= 10 && _curScr < 20)
            _imgMedal.sprite = _dictMedals[EMedal.Bronze];
        else if (_curScr >= 20 && _curScr < 30)
            _imgMedal.sprite = _dictMedals[EMedal.Silver];
        else if (_curScr >= 30 && _curScr < 40)
            _imgMedal.sprite = _dictMedals[EMedal.Gold];
        else if (_curScr >= 40)
            _imgMedal.sprite = _dictMedals[EMedal.Platinum];
        else
            _imgMedal.color = new(0f, 0f, 0f, 0f);

        if (_curScr >= 10) EventsManager.Instance.NotifyObservers(GameEvents.ParticleOnPopUp, null);
    }

    private void AllowUpdateCurrentScore()
    {
        _allowUpdateCurScr = true;
        _entryTime = Time.time;
    }

    //Hàm này để xử lý việc tăng dần điểm hiện tại Trên ScoreBoard
    private void HandleCurrentScore()
    {
        if (!_allowUpdateCurScr) return;

        if (_tempCurScr >= _curScr)
        {
            if (!_allowPopUpUIButton)
            {
                _allowPopUpUIButton = true;
                EventsManager.Instance.NotifyObservers(GameEvents.UIButtonOnPopUp, null);
                UpdateMedal(); //max CS r thì hiện Medal
            }
            return;
        }

        if (Time.time - _entryTime >= _timeEachIncrease)
        {
            _entryTime = Time.time;
            _tempCurScr += 1;
        }

        UpdateScore(_txtScore, _tempCurScr.ToString());
        //Debug.Log("Here: " + _tempCurScr + ", " + _curScr);

    }

    private void ResetValues()
    {
        _allowPopUpUIButton = false;
        transform.position = _initPos;
        _tempCurScr = 0;
        _allowUpdateCurScr = false;
        _txtScore.text = 0.ToString();
    }
}
