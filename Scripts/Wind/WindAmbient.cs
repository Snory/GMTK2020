using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindAmbient : MonoBehaviour
{

    public Image WindAmbientPointer;
    public GameObject CenterOfPointer;
    public GameObject PeakOfPointer;
    public List<Rigidbody2D> BodiesInWind;
    public float WindPower;
    private Vector2 _tempMousePositionInWorld = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomWindAngle());
        StartCoroutine(ApplyWindToBodies());
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    private void ManualWindApplication()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _tempMousePositionInWorld = Input.mousePosition;


        }

        if (Input.GetMouseButtonUp(1))
        {
            WindPower = Vector2.Distance(Input.mousePosition, _tempMousePositionInWorld);


            Vector2 directionOfWind = (PeakOfPointer.transform.position - CenterOfPointer.transform.position).normalized;
            foreach (Rigidbody2D body in BodiesInWind)
            {
                body.AddForce(directionOfWind * WindPower,ForceMode2D.Impulse);
            }

        }
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
            WindAmbientPointer.transform.rotation = Quaternion.RotateTowards(WindAmbientPointer.transform.rotation, newRotation, 30f * Time.deltaTime);
            yield return null;
        }


    }
}
