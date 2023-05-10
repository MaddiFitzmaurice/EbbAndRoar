using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject _interactUI;
    private Camera _mainCam;

    void Start()
    {
        _mainCam = Camera.main;
        _interactUI.gameObject.SetActive(false);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, _mainCam.transform.rotation.eulerAngles.y, 0f);
    }

    public void OnPlayerInteract(Collider other, bool isActive)
    {
        _interactUI.gameObject.SetActive(isActive);
    }
}
