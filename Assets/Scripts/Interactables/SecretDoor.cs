using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDoor : MonoBehaviour
{
    [SerializeField] Vector3 _closedPos;
    [SerializeField] Vector3 _openPos;
    
    void OnEnable()
    {
        ItemManager.AllItemsDeliveredEvent += Open;
    }

    void OnDisable()
    {
        ItemManager.AllItemsDeliveredEvent -= Open;
    }
    
    void Start()
    {
        transform.position = _closedPos;
    }

    void Open()
    {
        transform.position = _openPos;
    }
}
