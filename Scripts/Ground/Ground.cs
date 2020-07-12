using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ground : MonoBehaviour
{

    public List<GameObject> Grounds;
    public GameObject _mostLeft;
    public GameObject _mostRight;

    // Start is called before the first frame update
    void Start()
    {
        GetEdges();
    }

    // Update is called once per frame
    void Update()
    {
        if((_mostRight.transform.position.x - Camera.main.transform.position.x) < 0) { //doparava
            _mostLeft.transform.position = _mostRight.transform.position + new Vector3(20, 0, 0);
            GetEdges();





        } else if ((_mostLeft.transform.position.x - Camera.main.transform.position.x) > 0) //doleva
        {
            _mostRight.transform.position = _mostLeft.transform.position + new Vector3(-20, 0, 0);
            GetEdges();


        }
    }

    private void GetEdges()
    {
        _mostLeft = Grounds.OrderBy(h => h.transform.position.x).FirstOrDefault();
        _mostRight = Grounds.OrderByDescending(h => h.transform.position.x).FirstOrDefault();
    }
}
