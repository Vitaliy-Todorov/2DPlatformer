using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveableMonster : Monster
{
    [SerializeField]
    private float speed = 2.0F;

    private Vector3 direction;

    //private Bullet bullet;

    private SpriteRenderer sprite;

    protected override void Awake()
    {
        //непон€тно зачем это
        //sprite = GetComponentInChildren<SpriteRenderer>();
        //Debug.Log(sprite);
        // (это неправильно)пул€ унечтожаетс€
        //bullet = Resources.Load<Bullet>("Bullet");
    }
    protected override void Start()
    {
        //задаЄм направление движени€
        direction = transform.right;
    }

    protected override void Update()
    {
        Move();
    }

    //OnTriggerEnter2D - ќпредел€ем какой объект провзаимодействовал с данным
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        //если рассто€ние от центров колайдеров меньше 0.3, то есть взаимодействие произошло с боку, в противном случаи взаимодествие произошло сверху (но это работает если в высоту колайдеры больше чем в ширину)
        //≈сли истина умерает наподающий, если нет, умирает на кого напали (unit)
        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.3F)
                ReceiveDamage();
            else unit.ReceiveDamage();
        }
    }

    private void Move()
    {
        //ѕровер€ем наличие блока впереди (поднимаемс€ на 0.5 вверх и в сторону движени€ на 0.5, и провир€ем наличие блока в радиусе 0.1)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + direction * 0.5F, 0.1F);
        //есле впереди не 0 блоков и блок не гг, то мен€ем направление (дл€ проверки €вл€етс€ ли блок гг используем л€мда выражение)
        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>())) direction *= -1.0F;
        //движение вперЄд
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
