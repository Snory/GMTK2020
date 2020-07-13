using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void DestinationReachedDelegate();
public class Lampion : MonoBehaviour
{
    [SerializeField]
    private Transform basewire1, basewire2, basketWire1, basketWire2;
    private LineRenderer _lineRendered;
    private Rigidbody2D _body;

    [SerializeField]
    private GameObject _wireLinePrefab, _destinationPointer, _directionPointer;

    [SerializeField]
    private Image _speedMeter;

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
        _destination = GameObject.FindGameObjectWithTag(MyTags.DESTINATION_TAG);
        _destinationPointer.SetActive(false);
        _directionPointer.SetActive(false);
        _speedMeter.enabled = false;
        _maxSpeed = 15;
        LevelManager.Instance.LevelStarted += OnGameStarted;
        
    }

    // Update is called once per frame
    void Update()
    {

        CreateWires();
        UpdateDestinationPointer(_destination);
        UpdateDirectionPointer();
        UpdateSpeed();


    }

    

    private void UpdateDirectionPointer()
    {
        Vector3 movementDirection = _body.velocity;
        var angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        _directionPointer.transform.rotation = Quaternion.RotateTowards(_directionPointer.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 180 * Time.deltaTime);
    }


    private void UpdateSpeed()
    {
        _body.velocity = Vector3.ClampMagnitude(_body.velocity, _maxSpeed);
        _speedMeter.fillAmount = (_body.velocity.magnitude / _maxSpeed);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == MyTags.DESTINATION_TAG)
        {
            RaiseReachedDestination();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.name}");
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
        _destinationPointer.SetActive(true);
        _directionPointer.SetActive(true);
        _speedMeter.enabled = true;
    }

    private void RaiseReachedDestination()
    {
        if(DestinationReached != null)
        {
            DestinationReached();
        }
    }

    private void UpdateDestinationPointer(GameObject navigateTo)
    {
        Vector3 directionToDestination = navigateTo.transform.position - _destinationPointer.transform.position;
        var angle = Mathf.Atan2(directionToDestination.y, directionToDestination.x) * Mathf.Rad2Deg;     
        _destinationPointer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }



}
