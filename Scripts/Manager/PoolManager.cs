using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    public GameObject FlyingItemPrefab;
    List<FlyingItem> _flyingItems;
    public float CountOfPooledItems;

    private static PoolManager _instance;
    public static PoolManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {

            Destroy(this.gameObject);
        }

    }



    // Start is called before the first frame update
    void Start()
    {
        _flyingItems = new List<FlyingItem>();
        for(int i = 0; i < CountOfPooledItems; i++)
        {
            FlyingItem item = Instantiate(FlyingItemPrefab, this.transform.position, Quaternion.identity).GetComponent<FlyingItem>();
            item.transform.parent = this.transform;
            item.gameObject.SetActive(false);
            item.Spawned = false;
            _flyingItems.Add(item);
        }
    }


    public bool IsAvailableFlyingItem()
    {
        return _flyingItems.Count(f => !f.Spawned) > 0;
    }

    public FlyingItem GetFlyingItem()
    {
        return _flyingItems.Where(f => !f.Spawned).FirstOrDefault();
    }

    public void ReturnFlayingItem(FlyingItem item)
    {
        item.gameObject.SetActive(false);
        item.FlyItemData = null;
        item.Spawned = false;
        item.transform.parent = this.transform;
    }

    public void ReturAllFlyingItem()
    {

        foreach(var item in _flyingItems)
        {
            item.gameObject.SetActive(false);
            item.Spawned = false;
            item.FlyItemData = null;
            item.transform.parent = this.transform;
        }
    }

}
