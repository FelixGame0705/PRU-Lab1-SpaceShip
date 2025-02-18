using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField]
    int damage = 10;

    [SerializeField]
    string tag;

    RandomMove _randomMove;
    float _scale;

    public void Start()
    {
        if (gameObject.GetComponent<RandomMove>() != null)
            _randomMove = gameObject.GetComponent<RandomMove>();
    }

    public void OnEnable()
    {
        if (_randomMove != null)
            _scale = _randomMove.GetScale();
    }

    public int GetDamage()
    {
        if (_randomMove != null)
            return (int)(_scale * damage);
        return damage;
    }

    public void Hit()
    {
        ObjectPooler.Instance.ReturnToPool(tag, gameObject);
    }
}
