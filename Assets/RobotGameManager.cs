using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGameManager : MonoBehaviour
{
    [SerializeField] GameObject robot;
    [SerializeField] GameObject[] objectsToSpawnInWarehouse;
    [SerializeField] int[] numObjectsToSpawn;

    [SerializeField] GameObject duck;
    [SerializeField] Transform[] blueTeamDuckSpawns, redTeamDuckSpawns;
    [SerializeField] Transform blueSpawn, redSpawn;
    [SerializeField] Transform blueItemSpawn, redItemSpawn;
    public Transform redCarouselDuckSpawn, blueCarouselDuckSpawn;

    [SerializeField] Transform objectHolder;
    List<GameObject> spawnedItems;

    [SerializeField] float itemOffset;

    GameTimer timer;

    public Action gameStart;

    public ScoreKeeper scoreKeeper;

    bool gameStarted;

    static RobotGameManager _rg;

    public static RobotGameManager rg
    {
        get
        {
            return _rg;
        }
    }

    private void Awake()
    {
        _rg = this;

        if (!scoreKeeper)
        {
            scoreKeeper = GameObject.Find("ScoreKeeper").GetComponent<ScoreKeeper>();
        }

    }

    void Start()
    {
        spawnedItems = new List<GameObject>();
        timer = FindObjectOfType<GameTimer>();
         SpawnRobots();
    }

    void SpawnRobots()
    {
        Instantiate(robot, blueSpawn.position, blueSpawn.rotation);
    }

    void SpawnDucks()
    {
        int spawnIndex = UnityEngine.Random.Range(0, 3);

        SpawnItem(duck, blueTeamDuckSpawns[spawnIndex].position);
        SpawnItem(duck, redCarouselDuckSpawn.position);
        SpawnItem(duck, redTeamDuckSpawns[spawnIndex].position );
        SpawnItem(duck, blueCarouselDuckSpawn.position);
    }

    public void SpawnNewDuck(Transform carousel)
    {
        SpawnItem(duck, carousel.position);
    }

    void SpawnWarehouseItems()
    {
        for (int i = 0; i < objectsToSpawnInWarehouse.Length; i++)
        {
            for (int j = 0; j < numObjectsToSpawn[i]; j++)
            {
                Vector3 offset = new Vector3(UnityEngine.Random.Range(-itemOffset, itemOffset), 0, UnityEngine.Random.Range(-itemOffset, itemOffset));
                SpawnItem(objectsToSpawnInWarehouse[i], blueItemSpawn.position + offset);
            }
        }

        for (int i = 0; i < objectsToSpawnInWarehouse.Length; i++)
        {
            for (int j = 0; j < numObjectsToSpawn[i]; j++)
            {
                Vector3 offset = new Vector3(UnityEngine.Random.Range(-itemOffset, itemOffset), 0, UnityEngine.Random.Range(-itemOffset, itemOffset));
                SpawnItem(objectsToSpawnInWarehouse[i], redItemSpawn.position + offset);
            }
        }
    }

    public void StartGame()
    {
        if (gameStarted) return;

        if(gameStart != null)gameStart();

        SpawnDucks();
        SpawnWarehouseItems();
        GameObject.FindGameObjectWithTag("JointHub").BroadcastMessage("Reset");
        GameObject.FindGameObjectWithTag("RedHub").BroadcastMessage("Reset");
        GameObject.FindGameObjectWithTag("BlueHub").BroadcastMessage("Reset");
        GameObject[] g = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject G in g) { G.BroadcastMessage("Reset"); }

        timer.StartGame();
        scoreKeeper.resetScore();
        gameStarted = true;
    }

    public void EndGame()
    {
        gameStarted = false;
        foreach (GameObject obj in spawnedItems)
        {
            Destroy(obj);
        }
    }

    void SpawnItem(GameObject objToSpawn,Vector3 pos)
    {
        spawnedItems.Add(Instantiate(objToSpawn, pos, objToSpawn.transform.rotation, objectHolder));
    }
}
