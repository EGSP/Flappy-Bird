using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public delegate void EventDelegate();
    public event EventDelegate OnPlayStartedEvent = delegate { };
    public event EventDelegate OnPlayStopedEvent = delegate { };

    public event EventDelegate OnFlyStartedEvent = delegate { };
    public event EventDelegate OnFlyStopedEvent = delegate { };


    public event EventDelegate OnUpdateEvent = delegate { };

    [SerializeField] private Canvas MainMenu;
    [SerializeField] private Canvas GetReady;
    [SerializeField] private Canvas GameOver;
    [SerializeField] private Canvas Points;

    [SerializeField] private TextMeshProUGUI BestP = null;
    [SerializeField] private TextMeshProUGUI CurP = null;
    [SerializeField] private TextMeshProUGUI GameP = null;

    private int currentPoints = 0;
    private int bestPoints = 0;

    public void Awake()
    {
        gameManager = this;

        MainMenu.gameObject.SetActive(true);
        GetReady.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        bestPoints = PlayerPrefs.GetInt("Best");
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateEvent?.Invoke();
    }

    // Play
    public void StartPlay()
    {
        OnPlayStartedEvent?.Invoke();

        GetReady.gameObject.SetActive(true);
        GameOver.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);

        ClearPoints();
    }

    public void StopPlay()
    {
        OnPlayStopedEvent?.Invoke();

        GetReady.gameObject.SetActive(false);
        Points.gameObject.SetActive(false);

        BestP.text = ComparePoints().ToString();
        GameOver.gameObject.SetActive(true);
    }

    // Fly
    public void StartFly()
    {
        OnFlyStartedEvent?.Invoke();

        GetReady.gameObject.SetActive(false);
        Points.gameObject.SetActive(true);
    }

    public void StopFly()
    {
        OnFlyStopedEvent?.Invoke();
    }


    // POINTS
    public void AddPoint()
    {
        currentPoints++;
        CurP.text = currentPoints.ToString();
        GameP.text = currentPoints.ToString();
    }

    public void ClearPoints()
    {
        currentPoints = 0;
        CurP.text = currentPoints.ToString();
        GameP.text = currentPoints.ToString();
    }

    public int GetCurrentPoints()
    {
        return currentPoints;
    }

    public int GetBestPoints()
    {
        return bestPoints;
    }

    // Изменяет текущий рекорд при условии, что набрано больше очков, и возвращает значение рекорда
    public int ComparePoints()
    {
        if(currentPoints> bestPoints)
        {
            bestPoints = currentPoints;

            PlayerPrefs.SetInt("Best", bestPoints);
            PlayerPrefs.Save();
        }

        return bestPoints;
    }

}
