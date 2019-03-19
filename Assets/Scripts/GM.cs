using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

#if UNITY_EDITOR
using UnityEditor.Analytics;
#endif


//As the game relies on this its a good idea to make this script's
//Execution order make it run before other scripts
//Check Project Settings - Execution order 

public class GM : MonoBehaviour {

    enum State {
        NewGame
        ,PlayGame
        ,NewLevel
        ,NewPlayer
        ,GameOver
    }

    State mState= State.NewGame;

    public int PlayerScore = 0; //Score

    int PlayerLevel = 0;  //Level Played
    int PlayerLife = 0;

    public  int BulletsFired = 0; //Accuracy
    public  int BulletsHit = 0;

    float TimeInLevel = 0;
    float TimeInGame = 0;

    const  int DefaultStartRockCount = 5;     //Built in defaults
    const  int DefaultStartLives = 3;
    const  float DefaultStartFireRate = 0.5f;


    public int StartRockCount { get; private set; } //We will get these from the cloud
    public int StartLives { get; private set; } //We will get these from the cloud
    public float StartFireRate { get; private set; } //We will get these from the cloud

    public  int  BigRockDamage { get; private set; }
    public int  MediumRockDamage { get; private set; }
    public int SmallRockDamage { get; private set; }

    public int BigRockScore { get; private set; }
    public int MediumRockScore { get; private set; }
    public int SmallRockScore { get; private set; }


    public int Lives { get; private set; } //Lives

    public int LevelStartRockCount;


    public  GameObject PlayerShipPrefab; //Link to player ship prefab

    public  GameObject[] RockPrefab; //Link to Rock prefabs

    public  GameObject BasicBulletPrefab; //Link to Bullet prefab

    public GameObject HomingBulletPrefab; //Link to Bullet prefab

    public GameObject SplittingBulletPrefab; //Link to Bullet prefab

    public GameObject LifePrefab;

    GameObject mPlayer; //Keep link to player

    public static GM singleton { get; private set; }   //Expose public getter

    Coroutine NextLevelDaemon;

    private void Awake() { //Runs as soon as object is instantiated, before Start()
        if (singleton == null) { //first time we are started this is null
            singleton = this; //Now set it to ourself
            DontDestroyOnLoad(gameObject); //Make sure GO is not unloaded 
            Debug.Assert(PlayerShipPrefab != null, "Please assign PlayerShip in IDE");
            Debug.Assert(RockPrefab != null, "Please assign Rock in IDE");

            RemoteSettings.Updated += GetCloudSettings; //Add our remote settings handler, this will update if settings change

            
            #if UNITY_EDITOR
            AnalyticsSettings.testMode = true; //Makes it happen without caching  for testing
#endif

        } else if (singleton != this) { //If we are run again then make sure we are the same object
            Destroy(gameObject); //If not destroy the imposter (duplicate) to make sure there is only one
        }
    }

    void GetCloudSettings() {
        StartRockCount = RemoteSettings.GetInt("StartRockCount", DefaultStartRockCount); //Get setting from cloud
        StartLives = RemoteSettings.GetInt("StartLives", DefaultStartLives);
        StartFireRate = RemoteSettings.GetFloat("StartFireRate", DefaultStartFireRate);


        BigRockDamage = 30; //We could also get these fro the cloud
        MediumRockDamage = 20;
        SmallRockDamage = 10;

        BigRockScore = 50;
        MediumRockScore = 100;
        SmallRockScore = 120;

        Debug.LogFormat("{0} Settings used", RemoteSettings.WasLastUpdatedFromServer() ? "Cloud" : "Cached"); //Did we update from cloud or not
    }

    //Helper function to generate a valid screen position
    public static Vector2 RandomPosition() {
        float tHeight = Camera.main.orthographicSize; //Work out screen bounds
        float tWidth = tHeight * Camera.main.aspect;

        return new Vector2(Random.Range(-tWidth, tWidth), Random.Range(-tHeight, tHeight)); //Make Vector inside them
    }

    public static Vector2 RandomDirection() {
        //Ok, this is hard and I will explain in class
        //But read it back to front
        //Making a Up vector(0,1), which is already 1 long
        //Then rotating it by a random angle
        //the result is a normalised vector, in a random direction
        //to rotate with a Quaternion you multiply by it
        return Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.up;
    }

    private void Start() {
        mState = State.NewGame;
        StartCoroutine(StateMachine());
    }

    void MoreRocks(int vCount) {
        for (int i = 0; i < vCount; i++) {
            Instantiate(RockPrefab[0], RandomPosition(), Quaternion.identity); //Place Rocks onscreen when game starts
        }
    }



    IEnumerator StateMachine() {
        yield return new WaitForSeconds(1.0f); //Small delay to make sure all other object ready
        for (; ; ) {
                switch (mState) {

                case State.NewGame:
                    FindObjectOfType<GameOver>().Over = false;
                    PhysicsEntity[] tPhysicsObjects = FindObjectsOfType<PhysicsEntity>();
                    foreach (PhysicsEntity tPhysicsEntity in tPhysicsObjects) {
                        Destroy(tPhysicsEntity.gameObject);
                    }
                    Lives = StartLives;
                    PlayerScore = 0;
                    PlayerLevel = 0;
                    PlayerLife = 1;
                    BulletsFired = 0;
                    BulletsHit = 0;
                    TimeInGame = 0;

                    LevelStartRockCount = StartRockCount;
                    if (mPlayer != null) Destroy(mPlayer);  //Kill Old player
                    yield return new WaitForSeconds(1.5f);
                    MoreRocks(LevelStartRockCount++);
                    yield return new WaitForSeconds(1.5f);
                    mPlayer = Instantiate(PlayerShipPrefab, RandomPosition(), Quaternion.identity); //Place player onscreen when game starts
                    mState = State.PlayGame;
                    var tResult = AnalyticsEvent.GameStart(CaptureData()); //Start new session
                    AnalyticsEvent.LevelStart(PlayerLevel, CaptureData()); //Start New Level
                    break;

                case State.PlayGame:
                    if (RockBase.RockCount == 0) {
                        yield return new WaitForSeconds(1.5f);
                        mState = State.NewLevel;
                        AnalyticsEvent.LevelComplete(PlayerLevel, CaptureData()); //Start New Level
                    }
                    break;

                case State.NewLevel:
                    MoreRocks(StartRockCount++);
                    mState = State.PlayGame;
                    PlayerLevel++;    //Next Level
                    TimeInLevel = 0;
                    AnalyticsEvent.LevelStart(PlayerLevel, CaptureData()); //Start New Level
                    break;

                case State.NewPlayer:
                    mPlayer = Instantiate(PlayerShipPrefab, RandomPosition(), Quaternion.identity); //Place player onscreen when game starts
                    mState = State.PlayGame;
                    break;

                case State.GameOver:
                    if (Input.GetButtonDown("Fire1")) {
                        mState = State.NewGame;
                        FindObjectOfType<GameOver>().Over = false;
                        yield return new WaitForSeconds(1.5f);
                    }
                    break;

            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void PlayerDead() {
        mPlayer = null;
        if (Lives>0) {
            Lives--;
            PlayerLife++;
            AnalyticsEvent.LevelFail(PlayerLevel, CaptureData()); //Player Died
            mState = State.NewPlayer;
        } else {
            FindObjectOfType<GameOver>().Over = true;
            mState = State.GameOver;
            AnalyticsEvent.GameOver(PlayerLevel, "PlayerDead", CaptureData()); //Player Died
        }
    }

    private void Update() {
        TimeInGame += Time.deltaTime;
        TimeInLevel += Time.deltaTime;
    }


    Dictionary<string,object> CaptureData() {
        Dictionary<string, object> tData = new Dictionary<string, object>(); //Make a data packet to send with the event
        tData.Add("LivesUsed", PlayerLife);
        tData.Add("PlayerScore", PlayerScore);
        tData.Add("Accuracy", BulletsFired>0 ? (float)BulletsHit/(float)BulletsFired : 0);
        tData.Add("TimeInLastLevel", TimeInLevel);
        tData.Add("TimeInGame", TimeInGame);
        return tData;
    }

}

