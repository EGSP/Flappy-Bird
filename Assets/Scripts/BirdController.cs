using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Очки считать по интервалу времени, т.к. он равен времени проходу столбцов

public class BirdController : MonoBehaviour
{
    [SerializeField] private Vector3 StartPosition; // Начальная позиция птицы
    [SerializeField] private float JumpForce = 1; // Сила прыжка
    [SerializeField] private float RotationSpeed = 1; // Скорость поворота
    [SerializeField] private bool IsAlive = true; 
    


    private Rigidbody2D rig;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private Sprite Idle;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager.OnPlayStartedEvent += OnPlayStart;
        GameManager.gameManager.OnFlyStartedEvent += OnFlyStart;
        GameManager.gameManager.OnPlayStopedEvent += OnPlayStop;

        rig = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnPlayStart()
    {
        transform.position = StartPosition;
        rig.velocity *= 0;

        IsAlive = true;

        rig.isKinematic = true;
        anim.enabled = true;
    }

    void OnFlyStart()
    {
        GameManager.gameManager.OnUpdateEvent += OnUpdate;
        rig.isKinematic = false;
    }

    // Update is called once per frame
    void OnUpdate()
    {
        if (IsAlive == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Jump();
        }
        else
        {
            print("fall");
            Fall();
        }
        
        // Вращение птицы
        var dot = Vector3.Dot(Vector3.up, rig.velocity.normalized);
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(0,0,-23), Quaternion.Euler(0, 0, 33), (dot+1)/2*RotationSpeed);

    }

    void OnPlayStop()
    {
        GameManager.gameManager.OnUpdateEvent -= OnUpdate;
    }

    public void Jump()
    {
        SoundController.soundController.PlayBirdFly();
        rig.velocity = Vector3.zero;
        rig.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    // Падение вниз
    public void Fall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position-Vector3.up,1);
        
        // Если птица окончательно упала, то заканчиваем игру
        if(hit.collider != null)
        {
            GameManager.gameManager.StopPlay();
        }
    }

    // Столкновение с препятствием
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && IsAlive)
        {
            Death();
        }
    }

    // Прохождение препятствия и начисление очков
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gate")
        {
            
            GameManager.gameManager.AddPoint();
            SoundController.soundController.PlayGetPoint();
        }
    }

    // Птица начинает падать вниз - конец полёта
    void Death()
    {
        SoundController.soundController.PlayBirdDie();

        IsAlive = false;
        GameManager.gameManager.StopFly();

        anim.enabled = false;
        sprite.sprite = Idle;
    }


}
