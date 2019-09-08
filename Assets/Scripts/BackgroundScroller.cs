using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    
    public SpriteRenderer Mountains;
    public SpriteRenderer Forest;
    public SpriteRenderer Grass;
    [Space]
    public float MountainsFactor;
    public float ForestFactor;
    public float GrassFactor;


    private void Start()
    {
        GameManager.gameManager.OnUpdateEvent += OnUpdate;
        GameManager.gameManager.OnPlayStartedEvent += OnPlayStart;
        GameManager.gameManager.OnFlyStopedEvent += OnFlyStop;
    }

    void OnPlayStart()
    {
        GameManager.gameManager.OnUpdateEvent -= OnUpdate;
        GameManager.gameManager.OnUpdateEvent += OnUpdate;

    }

    private void OnUpdate()
    {
        var ofs = Mountains.material.mainTextureOffset;
        ofs += new Vector2(MountainsFactor * Time.deltaTime,0);
        Mountains.material.mainTextureOffset = ofs;

        ofs = Forest.material.mainTextureOffset;
        ofs += new Vector2(ForestFactor * Time.deltaTime, 0);
        Forest.material.mainTextureOffset = ofs;

        ofs = Grass.material.mainTextureOffset;
        ofs += new Vector2(GrassFactor * Time.deltaTime, 0);
        Grass.material.mainTextureOffset = ofs;

    }


    void OnFlyStop()
    {
        GameManager.gameManager.OnUpdateEvent -= OnUpdate;
    }
}
