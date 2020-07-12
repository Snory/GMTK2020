using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "FlyItem", menuName = "FlyItem", order = 51)]
public class FlyingItemData : ScriptableObject
{
    [SerializeField]
    private float _maxSpeed;
    public float MaxSpeed { get => _maxSpeed; }

    [SerializeField]
    private Sprite _texture;
    public Sprite Texture { get => _texture; }


    [SerializeField]
    private Color _color;
    public Color Color { get => _color; }


}
