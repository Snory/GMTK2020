using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        _windAmbinet = GameObject.FindGameObjectWithTag(MyTags.WIND_TAG).GetComponent<WindAmbient>();
        _lampion = GameObject.FindGameObjectWithTag(MyTags.LAMPION_TAG);
        _destination = GameObject.FindGameObjectWithTag(MyTags.DESTINATION_TAG);
        SpawnClouds();
   
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

        foreach (var item in FlyingItems)
        {
            item.Init();
        }
    }

    private IEnumerator RandomDestinationPosition()
    {


        Vector3 destinationPosition = Vector3.zero;
        do
        {
            float positionX = Random.Range(_leftLevelBorder.transform.position.x + Random.Range(20, 50), _rightLevelBorder.transform.position.x - Random.Range(20, 50));
            float positionY = Random.Range(_bottomLevelBorded.transform.position.y + Random.Range(20, 50), _topLevelBorder.transform.position.y - Random.Range(20, 50));
            destinationPosition = new Vector3(positionX, positionY);
        } while (Vector2.Distance(_lampion.transform.position, destinationPosition) < 300);

        Vector3 velocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        _destination.transform.parent.transform.position = destinationPosition;

        _destination.GetComponentInParent<Rigidbody2D>().velocity = (velocity * 2.5f);

        yield return new WaitForSeconds(30f);
        StartCoroutine(RandomDestinationPosition());

    }

    private void SpawnClouds()
    {
        for(int i = 0; i <= CloudsPerBurst;i++)
        {
            if (!PoolManager.Instance.IsAvailableFlyingItem()) break;

            bool left = Random.Range(1, 2) == 1 ? true : false;

            Vector3 cloudPosition = Vector3.zero;
            do
            {
                float positionX = Random.Range(_leftLevelBorder.transform.position.x + Random.Range(50, 200), _rightLevelBorder.transform.position.x - Random.Range(50, 200));
                float positionY = Random.Range(_bottomLevelBorded.transform.position.y + Random.Range(50, 200), _topLevelBorder.transform.position.y - Random.Range(50, 200));
                cloudPosition = new Vector3(positionX, positionY);
            } while (Vector2.Distance(_lampion.transform.position, cloudPosition) < 15);


            Vector3 velocity = left == true ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);



            FlyingItem cloud = PoolManager.Instance.GetFlyingItem();
            cloud.FlyItemData = CloudsData[Random.Range(0, CloudsData.Count - 1)];
            cloud.transform.position = cloudPosition;
            cloud.transform.parent = this.transform;
            cloud.Body.mass = Random.Range(2, 10);
            cloud.Body.velocity = velocity;
            cloud.Spawned = true;
            _windAmbinet.BodiesInWind.Add(cloud.Body);
            FlyingItems.Add(cloud);

        }

    }

 
}
