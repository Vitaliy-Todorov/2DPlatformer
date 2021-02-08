using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Получаем прикосновение юнита
        Unit unit = collision.GetComponent<Unit>();
        //Является ли юнит гг
        if (unit && unit is Character)
        {
            unit.ReceiveDamage();
        }
    }
}
