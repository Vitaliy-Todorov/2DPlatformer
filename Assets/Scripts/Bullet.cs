using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Получаем родительсткий объект (того кто стрелял)
    private GameObject parent;
    public GameObject Parent { set { parent = value;} get { return parent; } }

    private float speed = 10.0F;
    private Vector3 direction;
    //куда смотрит гг
    public Vector3 Direction { set { direction = value; } }

    //задаём цвет
    public Color Color { set { sprite.color = value; } get { return sprite.color; } }

    //нужен для изменения цвета
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, 1.4F);
    }

    private void Update()
    {
        //движение пули
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //если пуля коснулась другого объекта она исчизает
        Unit unit = collider.GetComponent<Unit>();
        MoveableMonster moveableMonster = collider.GetComponent<MoveableMonster>();

        //Если обект не родительский (не тот кто стреляет) и если это не moveableMonster
        if (unit && unit.gameObject != parent && unit != moveableMonster)
        {
            //Нанесение урона
            unit.ReceiveDamage();
            //Уничтожение
            Destroy(gameObject);
        }
    }
}
