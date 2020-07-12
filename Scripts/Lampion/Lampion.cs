using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void DestinationReachedDelegate();
public class Lampion : MonoBehaviour
{

    public Transform basewire1, basewire2, basketWire1, basketWire2;
    private LineRenderer _lineRendered;
    private Rigidbody2D _body;

    [SerializeField]
    private GameObject _wireLinePrefab, _arrow;
    private GameObject _currentLeftLine;
    private GameObject _currentRightLine;
    private GameObject _destination;
    public event DestinationReachedDelegate DestinationReached;


    private float _maxSpeed;



    private void Awake()
    {
        _body = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _destination = GameObject.FindGameObjectWithTag("Destination");
        _arrow.SetActive(false);
        _maxSpeed = 100;
        LevelManager.Instance.LevelStarted += OnGameStarted;
        
    }

    // Update is called once per frame
    void Update()
    {
        CreateWires();
        NavigateTo(_destination);
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        float currentSpeed = _body.velocity.sqrMagnitude;
        Vector2 currentVelocity = _body.velocity;
        if(currentSpeed > _maxSpeed)
        {
            _body.velocity = currentVelocity.normalized* _maxSpeed;
        }
        HUDManager.Instance.SetSpeedMeter(currentSpeed / _maxSpeed);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Destination")
        {
            RaiseReachedDestination();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LevelManager.Instance.LoseGame();
    }

    private void CreateWires()
    {
        if (_currentRightLine != null) Destroy(_currentRightLine);
        if (_currentLeftLine != null) Destroy(_currentLeftLine);


        _currentLeftLine = Instantiate(_wireLinePrefab, Vector3.zero, Quaternion.identity);
        _currentLeftLine.transform.parent = this.transform.parent;
        _lineRendered = _currentLeftLine.GetComponent<LineRenderer>();
        _lineRendered.SetPosition(0, basewire1.position);
        _lineRendered.SetPosition(1, basketWire1.position);

        _currentRightLine = Instantiate(_wireLinePrefab, Vector3.zero, Quaternion.identity);
        _currentRightLine.transform.parent = this.transform.parent;
        _lineRendered = _currentRightLine.GetComponent<LineRenderer>();
        _lineRendered.SetPosition(0, basewire2.position);
        _lineRendered.SetPosition(1, basketWire2.position);

    }

    private void OnGameStarted()
    {
        _arrow.SetActive(true);
    }

    private void RaiseReachedDestination()
    {
        if(DestinationReached != null)
        {
            DestinationReached();
        }
    }

    private void NavigateTo(GameObject navigateTo)
    {
        Vector3 directionToDestination = navigateTo.transform.position - _arrow.transform.position;
        var angle = Mathf.Atan2(directionToDestination.y, directionToDestination.x) * Mathf.Rad2Deg;

     
        _arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


    }



}
