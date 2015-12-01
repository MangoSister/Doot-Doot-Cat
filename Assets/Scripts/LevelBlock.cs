using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class LevelBlock : TranslationScroller
{
    public List<EnemyDog> enemyDogs;
    public List<AeroPlane> enemyPlanes;
    public List<PowerUp> powerUps; 
    public List<Bird> birds;
    public List<Ufo> ufos;
    public List<GameObject> miscs;

    public GameObject background;
    public GameObject ground;
    public GameObject side;

    public Texture2D bgTex;
    public Texture2D groundTex;
    public Texture2D sideTex;

    private void Start()
    {
        background.GetComponent<MeshRenderer>().material.mainTexture = bgTex;
        if (groundTex != null)
            ground.GetComponent<MeshRenderer>().material.mainTexture = groundTex;
        else ground.GetComponent<MeshRenderer>().enabled = false;
        if (sideTex != null)
            side.GetComponent<MeshRenderer>().material.mainTexture = sideTex;
        else side.GetComponent<MeshRenderer>().enabled = false;

        InitScroller();
    }

    private void Update()
    {
        UpdateScroller();
    }

    private void OnDestroy()
    {
        foreach (var dog in enemyDogs)
            Destroy(dog);
        enemyDogs.Clear();

        foreach (var plane in enemyPlanes)
            Destroy(plane);
        enemyPlanes.Clear();

        foreach (var power in powerUps)
            Destroy(power);
        powerUps.Clear();

        foreach (var misc in miscs)
            Destroy(misc);
        miscs.Clear();

        foreach (var ufo in ufos)
            Destroy(ufo);
        ufos.Clear();

        foreach (var bird in birds)
            Destroy(bird);
        birds.Clear();
    }
}
