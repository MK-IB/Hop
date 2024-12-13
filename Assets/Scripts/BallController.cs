using System;
using _Hop._Scripts.ControllerRelated;
using _Hop._Scripts.ElementRelated;
using Dreamteck.Splines;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController instance;
    
    private SplineFollower _follower;
    public float moveSpeed, xSpeed;
    private float xInput, yInput;

    private TileSpawner _tileSpawner;
    
    //Events for checkpoint to change color of tiles and speed of ball
    public delegate void  CheckPointAchieved();

    public event CheckPointAchieved OnCheckPoint;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _follower = GetComponent<SplineFollower>();
        _tileSpawner = TileSpawner.instance;
        _follower.followSpeed = moveSpeed;
    }
    private void OnEnable()
    {
        MainController.GameStateChanged += GameManager_GameStateChanged;
    }

    private void OnDisable()
    {
        MainController.GameStateChanged -= GameManager_GameStateChanged;
    }

    void GameManager_GameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.Gameplay)
        {
            _follower.follow = true;
        }
    }

    private bool _canRayCast;
    private void Update()
    {
        xInput = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * xSpeed * Mathf.Deg2Rad : 0;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            xInput = touchDeltaPosition.x * Mathf.Deg2Rad * xSpeed / 10;
        }
        _follower.motion.offset += Vector2.right * xInput;

        if (transform.position.y <= 0.5f) _canRayCast = true;
        else _canRayCast = false;

        if (_canRayCast && transform.position.z > 5)
        {
            Debug.DrawRay(transform.position, Vector3.down * 5, Color.green);
            RaycastHit hit;
            if(!Physics.Raycast(transform.position, Vector3.down, out hit, 5))
            {
                GameOver();
            }
        }
    }
    void GameOver()
    {
        if (MainController.instance.GameState == GameState.Levelfail) return;
        _follower.follow = false;
        _follower.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        _canRayCast = false;
        MainController.instance.SetActionType(GameState.Levelfail);
        Debug.Log("LEVEL FAIL...");
    }

    private int _hopCounter;
    private void OnTriggerEnter (Collider other)
    {
        TileElement tileElement = other.GetComponent<TileElement>();
        if(tileElement && tileElement.transform.CompareTag("Tile"))
        {
            tileElement.OnBallJumped();
            _hopCounter++;
            if(_hopCounter == _tileSpawner.tilesAhead - 4) _tileSpawner.AddMoreTiles();
            if (_hopCounter % 10 == 0) CheckPointReached();
            GameController.instance.SetCurrentScore();
            GameController.instance.StartCoroutine(GameController.instance.DumpUnUsed(tileElement.gameObject, 2.5f));
        }
        //Debug.Log("BALL COLLIDED...");
    }

    void CheckPointReached()
    {
        moveSpeed += 3;
        _follower.followSpeed = moveSpeed;
        _tileSpawner.SetCurrentTileColor();
        if(OnCheckPoint != null) 
            OnCheckPoint();
    }
    
}