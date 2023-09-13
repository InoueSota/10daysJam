using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject camera;
    private float shakeRadius = 0.0f;
    [SerializeField] private float shakeRadiusMax = 1.0f;

    [SerializeField] private float shakeTime = 1.0f;
    [SerializeField] private float shakeETime = 0;
    [SerializeField] private bool isRunning = false;

    private Vector3 plusPos = Vector3.zero;
    private Vector3 prePlusPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            shakeETime += Time.deltaTime;
            float t = shakeETime / shakeTime;
            float ta = t;

            shakeRadius = shakeRadiusMax * (1.0f - ta);

            float rot = Random.Range(0.0f, 360.0f);

            float radian = Mathf.Deg2Rad * rot;

            plusPos.x = Mathf.Cos(radian) * shakeRadius;
            plusPos.y = Mathf.Sin(radian) * shakeRadius;

            camera.transform.position = (camera.transform.position - prePlusPos) + plusPos;

            prePlusPos = plusPos;

            if(ta >= 1.0f)
            {
                camera.transform.position -= prePlusPos;
                prePlusPos = Vector3.zero;
                isRunning = false;
                shakeETime = 0.0f;
            }
        }
    }

    public void Set()
    {
        isRunning = true;
    }
}
