using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] private float GizmoCentreRadius = 1;
    [Space]
    [SerializeField] private float Range = 1; // Дальность от центра
    [SerializeField] private float HeightRange = 1; 

    [SerializeField] private float Speed = 0.03f; // Скорость смещения стен
    [SerializeField] private float Interval = 1; // Промежуток времени между стенами interval>0

    [SerializeField] private GameObject wallPrefab = null;
    private Queue<Transform> Walls = new Queue<Transform>(); // Пул стен, неактивированные
    private List<Transform> ActiveWalls = new List<Transform>(); // Активированные стены

    private Vector3 RangePos; // Заранее просчитаный вектор позиции по Range
    private Vector3 HeightRangePos;
    private float interval;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager.OnPlayStartedEvent += OnPlayStart;
        GameManager.gameManager.OnFlyStartedEvent += OnFlyStart;
        GameManager.gameManager.OnFlyStopedEvent += OnFlyStop;


        int count = Mathf.RoundToInt(((Range*2)/Speed)/(Interval*60))+1; // Количество столбцов
        print(count);
        Walls = new Queue<Transform>(count);
        ActiveWalls = new List<Transform>(count);
        
        for(int i = 0;i<count; i++) // Создаём клоны префаба стены
        {
            var newWall = Instantiate(wallPrefab).transform;
            newWall.gameObject.SetActive(false);
            Walls.Enqueue(newWall);
        }

        RangePos = transform.position + new Vector3(Range, 0, 0);
        HeightRangePos = transform.position + new Vector3(0, HeightRange, 0);
       

        interval = Interval;
    }

    private void OnPlayStart()
    {
        interval = Interval;
        
        // Убираем все стены в очередь
        for (int i = 0; i < ActiveWalls.Count; i++)
        {
            var wall = ActiveWalls[i];

            wall.gameObject.SetActive(false);
            wall.position = new Vector3(RangePos.x, 0, 0);
            //ActiveWalls.Remove(wall);
            Walls.Enqueue(wall);
        }

        ActiveWalls.Clear();

    }

    void OnFlyStart()
    {
        GameManager.gameManager.OnUpdateEvent += OnUpdate;
    }

    // Update is called once per frame
    void OnUpdate()
    {
        interval -= Time.deltaTime;
        if (interval < 0)
        {
            interval = Interval;

            float rand = Random.Range(0f,1000f);

            var wall = Walls.Dequeue();

            wall.gameObject.SetActive(true);
            
            wall.position = new Vector3(RangePos.x, HeightRangePos.y - HeightRange * 2 * (rand/1000),0);

            ActiveWalls.Add(wall); // Перемещаем стену в активный список
        }

        for(int i = 0; i < ActiveWalls.Count; i++)
        {
            var wall = ActiveWalls[i];

            if ((wall.transform.position - RangePos).magnitude > Range * 2)
            {
                wall.gameObject.SetActive(false);
                ActiveWalls.Remove(wall);
                Walls.Enqueue(wall);
            }
            else
            {
                wall.transform.position -= new Vector3(Speed, 0, 0);
            }
        }
    }

    void OnFlyStop()
    {
        GameManager.gameManager.OnUpdateEvent -= OnUpdate;
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(transform.position, GizmoCentreRadius);

        Gizmos.color = Color.green;

        // Range
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(Range, 0, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(-Range, 0, 0));

        Gizmos.DrawSphere(transform.position + new Vector3(Range, 0, 0), GizmoCentreRadius*0.5f);
        Gizmos.DrawSphere(transform.position + new Vector3(-Range, 0, 0), GizmoCentreRadius*0.5f);

        // Height
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, HeightRange, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -HeightRange, 0));

        Gizmos.DrawSphere(transform.position + new Vector3(0, HeightRange, 0), GizmoCentreRadius * 0.5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0, -HeightRange, 0), GizmoCentreRadius * 0.5f);
    }
}
