using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
    [System.NonSerialized]
	public MapManager mapManager;
	public uint level = 1;
	public bool cameraFree = true;
    private bool reset;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject oraclePrefab;
    public GameObject exitPrefab;

    public float speed = 5.0f;
	[System.NonSerialized]
    public Player player;
	[System.NonSerialized]
    public List<Enemy> enemyList = new List<Enemy>();
	[System.NonSerialized]
    public int turnLeft;
    private int movingUnitDoneCount;

	[System.NonSerialized]
	public ItemManager itemManager;

	[System.NonSerialized]
    public UserInterface userInterface;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		DontDestroyOnLoad(gameObject);
        GetComponent<MapManager>().initOnce();
        init();
	}

    public void enemySkipMove()
    {
        Enemy enemyRef = null;
        float distance = float.MaxValue;
        float distanceCur;

        foreach (Enemy enemy in enemyList)
        {
            distanceCur = enemy.distanceToTarget();
            if (distanceCur < distance)
            {
                distance = distanceCur;
                enemyRef = enemy;
            }
        }
        if (enemyRef != null)
            enemyRef.skipMoveCount = 4;
    }

    private void init()
    {
		// itemManager
		itemManager = GetComponent<ItemManager>();
		itemManager.removeNotLootedItems();

        // map
        mapManager = GetComponent<MapManager>();
        mapManager.MapSetup(level);

        // reset UnitFactory
        UnitFactory.reset();

        // player
        player = UnitFactory.createPlayer();

        // Camera
		instance.cameraFree = false;

        Camera camera = Camera.main;
        camera.transform.position = player.gameObject.transform.position - new Vector3(0, 0, 30);

		instance.cameraFree = true;

        // enemy
        for (int i = 0; i < mapManager.getEnemyCount(); i ++)
        {
            Cart enemySpawn = instance.mapManager.spawnEnemy();
            Enemy enemy = UnitFactory.createEnemy(enemySpawn);
            enemy.setTarget(player);
        }

		// spawn items
		mapManager.spawnItems();

        // UserInterface
        userInterface = GetComponent<UserInterface>();
        userInterface.init();

        movingUnitDoneCount = enemyList.Count + 1; // + 1 for player
        turnLeft = 0;
        reset = false;
    }

    public void movingUnitIsDone()
    {
        movingUnitDoneCount++;
    }

	private void Update()
	{
        //Debug.Log("MovingUnitList.Count " + movingUnitList.Count + " movingUnitDoneCount " + movingUnitDoneCount);
        if (movingUnitDoneCount != enemyList.Count + 1)
            return;

        // You better wait for all move() Coroutines to be done before you reset the game
        if (Input.GetKeyDown("space") || reset)
        {
            init();
            return;
        }

        if (turnLeft <= 0)
            return;
        turnLeft--;

        movingUnitDoneCount = 0;
        player.attemptMove();

        enemyList = new List<Enemy>(UnitFactory.enemyList);
        foreach (Enemy enemy in enemyList)
            enemy.attemptMove();
	}

    public void win()
    {
        Debug.Log("You Win!");
        reset = true;
        level++;
    }

    public void gameOver()
    {
        Debug.Log("Game Over");
        reset = true;
        level = 1;
    }
}
