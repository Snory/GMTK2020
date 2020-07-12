using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindAmbient : MonoBehaviour
{
    [Header("Pointer")]
    public Image WindAmbientPointer;
    public GameObject CenterOfPointer;
    public GameObject PeakOfPointer;

    [Header("Bodies")]
    public List<Rigidbody2D> BodiesInWind;
    private Vector2 _tempMousePositionInWorld = Vector3.zero;
    
    [Header("Settings")]
    public float WindPower;
    public bool WindOnStart;
   


    // Start is called before the first frame update
    void Start()
    {
        if (WindOnStart)
        {
            this.EnableWind();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableWind()
    {
        StartCoroutine(RandomWindAngle());
        StartCoroutine(ApplyWindToBodies());
    }


    public void DisableWind()
    {
        StopAllCoroutines();
    }



    IEnumerator ApplyWindToBodies()
    {
        Vector2 directionOfWind = (PeakOfPointer.transform.position - CenterOfPointer.transform.position).normalized;
        foreach (Rigidbody2D body in BodiesInWind)
        {
            body.AddForce(directionOfWind * WindPower);
           

        }


        yield return new WaitForSeconds(Random.Range(0.5f, 0.8f));
       
        StartCoroutine(ApplyWindToBodies());
    }




    IEnumerator RandomWindAngle()
    {

        float _windAmbientAngle = Random.Range(-180, 180);
        Quaternion newRotation = Quaternion.AngleAxis(_windAmbientAngle, Vector3.forward);

        float startTime = Time.realtimeSinceStartup;
        float randomTime = Random.Range(1, 3);

        while (true)
        {
            if((Time.realtimeSinceStartup - startTime) > randomTime)
            {
                StartCoroutine(RandomWindAngle());
                yield break;
            }
            UpdateWindAmbientPointer(newRotation);
            yield return null;
        }


    }

    private void UpdateWindAmbientPointer(Quaternion newRotation)
    {
        if(WindAmbientPointer != null)
        {
            WindAmbientPointer.transform.rotation = Quaternion.RotateTowards(WindAmbientPointer.transform.rotation, newRotation, 30f * Time.deltaTime);
        }
    }
}
