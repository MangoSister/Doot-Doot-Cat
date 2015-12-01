using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : GenericSingleton<LevelController>
{
    public LevelBlock levelBlockPrefab;
    public EnemyDog enemyDogPrefab;
    public AeroPlane planePrefab;
    public PowerUp powerUpPrefab;
    public Bird birdPrefab;
    public Ufo ufoPrefab;
    public List<GameObject> miscPrefabs;
    public GameObject confettiPrefab;

    public ShittyCat character;

    private LevelBlock bottomBlock;
    private LevelBlock topBlock;

    public float normalScrollSpeed;
    public float centeringScrollSpeed;
    public float currScrollSpeed;

    public List<Texture2D> backgroundTex;
    public Texture2D groundTex;
    public List<Texture2D> sideTex;
    public Rect screenBoundary;

    private bool pause;
    public bool Pause {
        get { return pause; }
        set
        {
            if (!pause && value)
            {
                bottomBlock.enabled = false;
                topBlock.enabled = false;
                character.EnableCtrl(false);
                character.enabled = false;
            }
            else if(pause && !value)
            {
                bottomBlock.enabled = true;
                topBlock.enabled = true;
                character.EnableCtrl(true);
                character.enabled = true;
            }

            pause = value;
        }
    }

    private LevelParser levelParser;
    private LevelData levelData;
    public int currBlock { get; private set; }
    public int totalBlock {
        get { return levelData.blocks.Count; }
    }

    private bool centering;

    private void Start()
    {
        //Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
        levelParser = GetComponent<LevelParser>();
        levelData = levelParser.LoadLevelFromFile();
        currBlock = 0;

        currScrollSpeed = normalScrollSpeed;

        bottomBlock = GenLoadedLevelBlock(currBlock, new Vector2(0f, 0f), currScrollSpeed, 0.5f, 20f);
        topBlock = GenLoadedLevelBlock(currBlock + 1, new Vector2(0f, 10f), currScrollSpeed, 0f, 20f);
        currBlock++;
        //bottomBlock = CreateLevelBlock(new Vector2(0f, 0f), currScrollSpeed, 0.5f, 20f,
        //    new List<Vector2>() { new Vector2(3.5f, 0) },
        //    new List<Vector2>() { new Vector2(-9f, 3) },
        //    new List<Vector2>() { new Vector2(3.5f, -2f), new Vector2(-3.5f, -2f) },
        //                                backgroundTex[0]);

        //topBlock = CreateLevelBlock(new Vector2(0f, 10f), currScrollSpeed, 0f, 20f,
        //    new List<Vector2>() { new Vector2(3.5f, 0) },
        //    new List<Vector2>() { new Vector2(-9f, 3) },
        //    new List<Vector2>() { new Vector2(3.5f, -2f), new Vector2(-3.5f, -2f) },
        //                                backgroundTex[0]);

        
        character.boundary = screenBoundary;
        character.PeakTouched += OnCenteringBegin;
        character.CenteringEnd += OnCenteringEnd;
        //OnPhaseChange

        centering = false;
        StartCoroutine(CheckLevelEndCoroutine());
    }

    private void OnCenteringBegin()
    {
        currScrollSpeed = centeringScrollSpeed;
        topBlock.scrollSpeed = currScrollSpeed;
        bottomBlock.scrollSpeed = currScrollSpeed;
        centering = true;
    }

    private void OnCenteringEnd()
    {
        currScrollSpeed = normalScrollSpeed;
        topBlock.scrollSpeed = currScrollSpeed;
        bottomBlock.scrollSpeed = currScrollSpeed;
        centering = false;
    }

    private void OnBlockEnd(Scroller scroller)
    {
        if (!(scroller is LevelBlock))
            throw new UnityException("cannot cast scroller to levelblock");
        LevelBlock block = scroller as LevelBlock;
        Destroy(block.gameObject);

        currBlock++;
        if (currBlock < levelData.blocks.Count)
        {
            bottomBlock = topBlock;

            Debug.Log("generate new block!");
            //topBlock = CreateLevelBlock(new Vector2(0f, 10f), currScrollSpeed, 0f, 20f,
            //    new List<Vector2>() { new Vector2(3.5f, 0) },
            //    new List<Vector2>() { new Vector2(-9f, 3) },
            //    new List<Vector2>() { new Vector2(3.5f, -2f), new Vector2(-3.5f, -2f) },
            //    backgroundTex[0]);
            topBlock = GenLoadedLevelBlock(currBlock, new Vector2(0f, bottomBlock.transform.position.y + 9.9f), currScrollSpeed, 0f, 20f);
        }        
    }

    private LevelBlock GenLoadedLevelBlock
        (int index, Vector2 pos, float scrollSpeed, float startOffset, float tileSize)
    {
        LevelBlock block = Instantiate(levelBlockPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity) as LevelBlock;
        block.type = Scroller.ScrollType.Vertical;
        block.scrollSpeed = scrollSpeed;
        block.startOffset = startOffset;
        block.tileSize = tileSize;

        BlockData blockData = levelData.blocks[index];
        block.bgTex = backgroundTex[blockData.bgIndex];
        block.groundTex = blockData.ground ? groundTex : null;
        block.sideTex = sideTex[blockData.sideIndex];

        foreach (var dogParam in blockData.dogs)
        {
            EnemyDog dog = Instantiate(enemyDogPrefab, new Vector3(pos.x + dogParam._posX, pos.y + dogParam._posY, 0f), Quaternion.identity) as EnemyDog;
            dog.wanderRange = dogParam._wanderRange;
            dog.wanderFreq = dogParam._wanderFreq;
            dog.wanderOffset = dogParam._wanderOffset;
            dog.fallingAcc = dogParam._fallingAcc;
            dog.maxFallingSpeed = dogParam._maxFallingSpeed;
            dog.transform.parent = block.transform;
            block.enemyDogs.Add(dog);
        }

        foreach (var planeParam in blockData.planeParams)
        {
            AeroPlane pl = Instantiate(planePrefab, new Vector3(pos.x + planeParam._posX, pos.y + planeParam._posY, 0f), Quaternion.identity) as AeroPlane;
            pl.transform.parent = block.transform;
            pl.direction = planeParam._direction;
            pl.flyDist = planeParam._flyDist;
            pl.flySpeed = planeParam._flySpeed;
            pl.waitTime = planeParam._waitTime;
            pl.transform.parent = block.transform;
            block.enemyPlanes.Add(pl);
        }

        foreach (var powerUpParam in blockData.powerUpParams)
        {
            PowerUp pu = Instantiate(powerUpPrefab, new Vector3(pos.x + powerUpParam._posX, pos.y + powerUpParam._posY, 0f), Quaternion.identity) as PowerUp;
            pu.transform.parent = block.transform;
        }

        foreach (var birdParam in blockData.birdParams)
        {
            Bird bird = Instantiate(birdPrefab, new Vector3(pos.x + birdParam._posX, pos.y + birdParam._posY, 0f), Quaternion.identity) as Bird;
            bird.transform.parent = block.transform;
            bird.direction = birdParam._direction;
            bird.flyDist = birdParam._flyDist;
            bird.flySpeed = birdParam._flySpeed;
            bird.waitTime = birdParam._waitTime;
            bird.transform.parent = block.transform;
            block.birds.Add(bird);
        }

        foreach (var ufoParam in blockData.ufoParams)
        {
            Ufo ufo = Instantiate(ufoPrefab, new Vector3(pos.x + ufoParam._posX, pos.y + ufoParam._posY, 0f), Quaternion.identity) as Ufo;
            ufo.transform.parent = block.transform;
            ufo.direction = ufoParam._direction;
            ufo.flyDist = ufoParam._flyDist;
            ufo.flySpeed = ufoParam._flySpeed;
            ufo.waitTime = ufoParam._waitTime;
            ufo.circularMoveRadius = ufoParam._circularMoveRadius;
            ufo.circularMoveFreq = ufoParam._circularMoveFreq;
            ufo.transform.parent = block.transform;
            block.ufos.Add(ufo);
        }

        foreach (var miscParam in blockData.miscParams)
        {
            var miscPrefab = miscPrefabs[miscParam._type];
            GameObject misc = Instantiate(miscPrefab, new Vector3(pos.x + miscParam._posX, pos.y + miscParam._posY, 0f), Quaternion.identity) as GameObject;
            misc.transform.localScale = Vector3.one * miscParam._size;
            misc.transform.parent = block.transform;
            block.miscs.Add(misc);
        }

        block.PeriodOver += OnBlockEnd;
        return block;
    }

    private IEnumerator CheckLevelEndCoroutine()
    {
        while(currBlock < levelData.blocks.Count - 1 || centering)
            yield return null;
        float currCharacterY = character.transform.position.y;
        float topDist = screenBoundary.yMax - currCharacterY;
        float currBlockY = topBlock.transform.position.y;
        float animTime = currBlockY / currScrollSpeed;
        
        character.ActivateDrone(0.5f * topDist / animTime);
        Instantiate(confettiPrefab, Vector3.up * 10f, Quaternion.identity);
        yield return new WaitForSeconds(animTime);
        currScrollSpeed = 0f;
        topBlock.scrollSpeed = currScrollSpeed;
        bottomBlock.scrollSpeed = currScrollSpeed;

        yield return new WaitForSeconds(3f);
        SceneManager.Instance.TransitScene(SceneManager.SceneType.Credit);
    }
}
