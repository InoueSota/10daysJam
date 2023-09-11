using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoScroll : MonoBehaviour
{
    [SerializeField] private GameObject cameraObj;
    private ScrollManager scrollManager;

    Vector3 initialPosition;

    void Start()
    {
        scrollManager = cameraObj.GetComponent<ScrollManager>();
        initialPosition = transform.position;
    }

    void Update()
    {
        float scrollValue = scrollManager.GetScrollValue();
        transform.position = new(initialPosition.x + scrollValue, initialPosition.y, initialPosition.z);
    }
}
