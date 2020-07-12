using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingItem : MonoBehaviour
{

    public Rigidbody2D Body { get; private set; }
    private SpriteRenderer _renderer;
    private Image _image;

    [SerializeField]
    private FlyingItemData _flyingData;
    public FlyingItemData FlyItemData { get => _flyingData; set { _flyingData = value;} }
    public bool InitFromStart;
    public bool Spawned;
  
    private float MaxSpeed;

    private void Awake()
    {
        Body = this.GetComponent<Rigidbody2D>();
        _renderer = this.GetComponent<SpriteRenderer>();
        _image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InitFromStart) Init();

        if (FlyItemData == null) return;

        UpdateSpeed();
    }

    public void Init()
    {

            if(_image != null)
            {
                _image.sprite = _flyingData.Texture;
            }

            if(_renderer != null)
            {
                _renderer.sprite = _flyingData.Texture;
            }
            MaxSpeed = _flyingData.MaxSpeed;
            this.gameObject.SetActive(true);

    }


    private void UpdateSpeed()
    {


        Body.velocity = Vector3.ClampMagnitude(Body.velocity, _flyingData.MaxSpeed);
 


    }
}
