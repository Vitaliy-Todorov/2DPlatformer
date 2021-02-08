using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0F;

    [SerializeField]
    private Transform target;

    private void Awake()
    {
        //Если объект на котором должна фиксироавться камера не задан, то мы фиксируемся на гг
        if (!target) target = FindObjectOfType<Character>().transform;
    }

    private void Update()
    {
        //камеру нужно сместить на, что бы она не попадала в тот же слой что и картинка
        Vector3 position = target.position; position.z = -10.0F;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
