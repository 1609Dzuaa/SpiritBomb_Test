using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameEnums;

//Bố trí, đặt tên lại các hàm cho clean
//Hiểu rõ về Unity Execution Order
//Cố thử làm mode auto Play
//Quay Video Gameplay

public class UIManager : BaseSingleton<UIManager>
{
    [SerializeField] GameObject _canvasScore;
    [SerializeField] GameObject _canvasGameOver;
    [SerializeField] GameObject _canvasWhiteFrame;
    [SerializeField] float _delayPopUpGOverPanel;
    [SerializeField] float _delayPopUpScoreBoard;
    [SerializeField] GameObject _canvasScoreBoard;
    [SerializeField] GameObject _canvasUIButton;
    [SerializeField] GameObject _canvasUILoseButton;
    [SerializeField] GameObject _canvasFlappyBird;
    [SerializeField] GameObject _canvasTutor;
    [SerializeField] TextMeshProUGUI _txtScore;
    [SerializeField] GameObject _canvasBtnStop;

    int _score = 0;
    bool _isStop;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        if (_txtScore != null)
            _txtScore.alpha = 1f;
        _canvasWhiteFrame.SetActive(false);
        _canvasScoreBoard.SetActive(false);
        _canvasGameOver.SetActive(false);
        _canvasUILoseButton.SetActive(false);
        _canvasScore.SetActive(false);
        _canvasTutor.SetActive(false);
        _canvasBtnStop.SetActive(false);
    }

    private void Start()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnPass, HandleUIScore);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnLose, PopUpGameOver);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnLose, PopDownScore);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.WhiteFrameOnPopUp, PopUpWhiteFrame);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnLose, PopUpScoreBoard);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.UIButtonOnPopUp, PopUpUILoseButton);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.GameOnStart, PopDownCanvasTutor);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnPass, HandleUIScore);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnLose, PopUpGameOver);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnLose, PopDownScore);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.WhiteFrameOnPopUp, PopUpWhiteFrame);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnLose, PopUpScoreBoard);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.UIButtonOnPopUp, PopUpUILoseButton);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.GameOnStart, PopDownCanvasTutor);
    }

    private void Update()
    {
        UpdateTextScore();

    }

    private void UpdateTextScore()
    {
        if (_txtScore)
            _txtScore.text = _score.ToString();
    }

    private void HandleUIScore(object obj)
    {
        _score++;
        //Debug.Log("Passed");
    }

    private void PopUpGameOver(object obj)
    {
        StartCoroutine(DelayPopUpGameOver());
        //Vì Game, Over là 2 obj con của GameOver nên cần phải active nó trước khi sent notify
        _canvasGameOver.SetActive(true);
        //EventsManager.Instance.NotifyObservers(GameEvents.GameOverOnAllowPopUp, null);
    }

    private IEnumerator DelayPopUpGameOver()
    {
        yield return new WaitForSeconds(_delayPopUpGOverPanel);

        EventsManager.Instance.NotifyObservers(GameEvents.GameOverOnAllowPopUp, null);
        EventsManager.Instance.NotifyObservers(GameEvents.GameOverOnPopUp, null);
    }

    private void PopUpWhiteFrame(object obj)
    {
        _canvasWhiteFrame.SetActive(true);
    }

    private void PopUpScoreBoard(object obj)
    {
        StartCoroutine(DelayPopUpScoreBoard());
    }

    private IEnumerator DelayPopUpScoreBoard()
    {
        yield return new WaitForSeconds(_delayPopUpScoreBoard);

        _canvasScoreBoard.SetActive(true);
        EventsManager.Instance.NotifyObservers(GameEvents.ScoreBoardOnPopUp, null);
        EventsManager.Instance.NotifyObservers(GameEvents.ScoreBoardOnUpdateScore, _score); //Cập nhật điểm trên Board
    }

    private void PopDownScore(object obj)
    {
        _canvasScore.SetActive(false);
    }

    private void PopUpUILoseButton(object obj)
    {
        _canvasUILoseButton.SetActive(true);
    }

    public void ButtonStartOnClick()
    {
        PopDownUIMenu();
        SceneManager.LoadScene("MainScene");
        EventsManager.Instance.NotifyObservers(GameEvents.OnPlaySfx, ESfx.Transition);
        _canvasScore.SetActive(true);
        PopUpCanvasTutor();
        PopUpCanvasButtonStop();
    }

    public void ButtonOKOnClick()
    {
        SceneManager.LoadScene("MenuScene");
        EventsManager.Instance.NotifyObservers(GameEvents.OnPlaySfx, ESfx.Transition);
        PopUpUIMenu();
        PopDownUILoseMenu();
        ResetScore();
        PopDownWhiteFrame();
        PopDownCanvasButtonStop();
    }

    public void ButtonStopOnClick()
    {
        Time.timeScale = (_isStop ? 1 : 0);
        _isStop = !_isStop;
    }

    private void PopUpUIMenu()
    {
        _canvasFlappyBird.SetActive(true);
        _canvasUIButton.SetActive(true);
    }

    private void PopDownUIMenu()
    {
        _canvasFlappyBird.SetActive(false);
        _canvasUIButton.SetActive(false);
    }

    private void PopDownUILoseMenu()
    {
        _canvasScoreBoard.SetActive(false);
        _canvasUILoseButton.SetActive(false);
        _canvasGameOver.SetActive(false);
    }

    private void PopDownWhiteFrame()
    {
        _canvasWhiteFrame.SetActive(false);
    }

    private void PopDownCanvasTutor(object obj)
    {
        _canvasTutor.SetActive(false);
    }

    private void PopUpCanvasTutor()
    {
        _canvasTutor.SetActive(true);
    }

    private void PopUpCanvasButtonStop()
    {
        _canvasBtnStop.SetActive(true);
    }

    private void PopDownCanvasButtonStop()
    {
        _canvasBtnStop.SetActive(false);
    }

    private void ResetScore()
    {
        _score = 0;
    }
}
