using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private WindAmbient _windAmbinet;
    private GameObject _lampion;
    private GameObject _destination;

    [Header("Clouds")]
    public List<FlyingItemData> CloudsData;
    public List<FlyingItem> FlyingItems;
    public int CloudsPerBurst;

    [SerializeField]
    private GameObject _leftLevelBorder, _rightLevelBorder, _topLevelBorder, _bottomLevelBorded;

    [SerializeField]
    private float _minDistanceFromPlayer, _maxDistanceFromPlayer, _stepDistancePerBurst, _burstTime, _maxCountOfBurst;



    // Start is called before the first frame update
    void Start()
    {
        _windAmbinet = GameObject.FindGameObjectWithTag(MyTags.WIND_TAG).GetComponent<WindAmbient>();
        _lampion = GameObject.FindGameObjectWithTag(MyTags.LAMPION_TAG);
        _destination = GameObject.FindGameObjectWithTag(MyTags.DESTINATION_TAG);

        LevelManager.Instance.LevelStarted += OnLevelStarted;



    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnLevelStarted()
    {
        Invoke("InitAfterLevelStarted", 2f);
    }

    private void InitAfterLevelStarted()
    {
        StartCoroutine(RandomDestinationPosition());
        StartCoroutine(SpawnClouds());


    }

    private IEnumerator RandomDestinationPosition()
    {

        float offsetX = Mathf.Abs(_leftLevelBorder.transform.position.x - _rightLevelBorder.transform.position.x);
        float offsetY = Mathf.Abs(_bottomLevelBorded.transform.position.y - _topLevelBorder.transform.position.y);

        Vector3 destinationPosition = Vector3.zero;

        do
        {
            float positionX = Random.Range(_leftLevelBorder.transform.position.x + Random.Range(offsetX, offsetX + 10), _rightLevelBorder.transform.position.x - Random.Range(offsetX, offsetX + 10));
            float positionY = Random.Range(_bottomLevelBorded.transform.position.y + Random.Range(offsetY, offsetY + 10), _topLevelBorder.transform.position.y - Random.Range(offsetY, offsetY + 10));
            destinationPosition = new Vector3(positionX, positionY);
        } while (Vector2.Distance(_lampion.transform.position, destinationPosition) < offsetX / 2.5f);

        Vector3 velocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        _destination.transform.parent.transform.position = destinationPosition;

        _destination.GetComponentInParent<Rigidbody2D>().velocity = (velocity * 2.5f);

        yield return new WaitForSeconds(30f);
        StartCoroutine(RandomDestinationPosition());

    }

    private IEnumerator SpawnClouds()
    {

       

        float currentMinDistance = _minDistanceFromPlayer;
        float currentMaxDistance = _maxDistanceFromPlayer;

        for (int i = 0; i < _maxCountOfBurst; i++){
            for (int x = 0; x < CloudsPerBurst; x++)
            {
                if (!PoolManager.Instance.IsAvailableFlyingItem()) break;

                bool left = Random.Range(1, 2) == 1 ? true : false;

                float offsetX = Mathf.Abs(_leftLevelBorder.transform.position.x - _rightLevelBorder.transform.position.x);
                float offsetY = Mathf.Abs(_bottomLevelBorded.transform.position.y - _topLevelBorder.transform.position.y);

                Vector3 cloudPosition = Vector3.zero;
                float distanceOfCloudFromPlayer = Vector2.Distance(_lampion.transform.position, cloudPosition);
                do
                {
                    float positionX = Random.Range(_leftLevelBorder.transform.position.x + Random.Range(offsetX, offsetX + 10), _rightLevelBorder.transform.position.x - Random.Range(offsetX, offsetX + 10));
                    float positionY = Random.Range(_bottomLevelBorded.transform.position.y + Random.Range(offsetY, offsetY + 10), _topLevelBorder.transform.position.y - Random.Range(offsetY, offsetY + 10));
                    cloudPosition = new Vector3(positionX, positionY);
                    distanceOfCloudFromPlayer = Vector2.Distance(_lampion.transform.position, cloudPosition);

                } while (distanceOfCloudFromPlayer < currentMinDistance || distanceOfCloudFromPlayer > currentMaxDistance);


                Vector3 velocity = left == true ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);
                FlyingItem cloud = PoolManager.Instance.GetFlyingItem();
                cloud.FlyItemData = CloudsData[Random.Range(0, CloudsData.Count - 1)];
                cloud.transform.position = cloudPosition;
                cloud.transform.parent = this.transform;
                cloud.Body.mass = Random.Range(2, 10);
                cloud.Body.velocity = velocity;
                cloud.Spawned = true;
                cloud.Init();
                cloud.gameObject.name = $"Cloud| {currentMinDistance} | {currentMaxDistance} |{distanceOfCloudFromPlayer}";

                StartCoroutine(cloud.ReturnAfterDistanceFrom(_lampion.transform.position, _maxDistanceFromPlayer * 1.5f));

                _windAmbinet.BodiesInWind.Add(cloud.Body);
                FlyingItems.Add(cloud);
            }
        currentMinDistance = currentMaxDistance;
        currentMaxDistance += _stepDistancePerBurst;
        }


        yield return new WaitForSeconds(_burstTime);

        StartCoroutine(SpawnClouds());

    }




}
