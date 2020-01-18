using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Manager<GameManager>
{
    #region Settings
    [Header("Game Settings")]
    [SerializeField]
    float gemCreateTime = 0.7f;
    [SerializeField]
    float gemDestroyTime = 0.7f;
    [SerializeField]
    float gemMoveSpeed = 0.7f;
    [Space]
    [SerializeField]
    int pointsForMatch3 = 3;
    [SerializeField]
    int pointsForMatch4 = 5;
    [SerializeField]
    int pointsForMatch5 = 7;

    public float GemCreateTime { get => gemCreateTime; }
    public float GemDestroyTime { get => gemDestroyTime; }
    public float GemMoveSpeed { get => gemMoveSpeed; }
    public int PointsForMatch3 { get => pointsForMatch3; }
    public int PointsForMatch4 { get => pointsForMatch4; }
    public int PointsForMatch5 { get => pointsForMatch5; }
    #endregion

    [Header("UI")]
    [SerializeField]
    GameObject backgroundImage = null;
    [SerializeField]
    GameObject topPanel = null;
    [SerializeField]
    GameObject endPanel = null;
    [SerializeField]
    GameObject pausePanel = null;
    [Space]
    [SerializeField]
    Text timeText = null;
    [SerializeField]
    Text scoreText = null;
    [SerializeField]
    Text comboText = null;
    [SerializeField]
    Text endScoreText = null;

    [Header("Other")]
    [SerializeField]
    Board board = null;

    public int GameTime { get; private set; }
    public float CurrentTime { get; private set; }
    int score;

    bool pause = false;
    public bool IsPaused { get => pause; set {
            pause = value;
            if (value)
            {
                DOTween.PauseAll();
            }
            else
            {
                DOTween.PlayAll();
            }
           
        } }
    public bool IsGameEnd { get; private set; } = false;

    void Start()
    {
        GameTime = StaticClass.GameTime;
        CurrentTime = GameTime;
    }

    private void OnApplicationPause(bool pause)
    {
        IsPaused = pause;
    }

    void Update()
    {
        if(IsPaused)
        {
            backgroundImage.SetActive(true);
            pausePanel.SetActive(true);
        }
        else if(!IsGameEnd)
        {
            backgroundImage.SetActive(false);
            pausePanel.SetActive(false);
            CurrentTime -= Time.deltaTime;

            // Game End
            if (CurrentTime <= 0 && board.IsReady)
            {
                IsGameEnd = true;
                topPanel.SetActive(false);
                backgroundImage.SetActive(true);
                endPanel.SetActive(true);
                endScoreText.text = "Score\n" + score.ToString("D6");

                if (PlayServicesManager.Instance != null)
                {
                    PlayServicesManager.Instance.PostToLeaderboard(score, GameTime);
                }
            }
            else
            {
                timeText.text = "Time: " + FloatToTime(CurrentTime);
                scoreText.text = "Score: " + score.ToString("D6");
                comboText.text = "x " + board.Combo;
            }
        }         
    }

    string FloatToTime(float time)
    {
        time = Mathf.Max(0, time);
        return Mathf.FloorToInt(time/60).ToString("D2") + ":" + Mathf.FloorToInt(time%60).ToString("D2");
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}
