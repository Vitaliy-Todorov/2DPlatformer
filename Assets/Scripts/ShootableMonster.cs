using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMonster : Monster
{
    //Частота стрельбы
    [SerializeField]
    private float rate = 2.0F;
    [SerializeField]
    private Color bulletColor = Color.black;

    Bullet bullet;

    protected override void Awake()
    {
        //Load(string path) - Загружает ресурс path, хранящийся в папке Resources.
        bullet = Resources.Load<Bullet>("Bullet");
    }

    protected override void Start()
    {
        //InvokeRepeating(string methodName, float time, float repeatRate) - Вызывает метод methodName в time секундах, а затем повторно каждые repeatRateсекунды.
        InvokeRepeating("Shoot", rate, rate);
    }

    private void Shoot()
    {
        //Поднимаем точку респаума пули
        Vector3 position = transform.position;         position.y += 0.5F;
        //Создаём копию объекта bullet в указанном месте и с указанным поворотом.
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        //по всей видимости привязываем пулю к монстру
        newBullet.Parent = gameObject;
        //Direction - направление вектора
        newBullet.Direction = -newBullet.transform.right;
        bulletColor = Color.black;
        newBullet.Color = bulletColor;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) < 0.4F) ReceiveDamage();
            else unit.ReceiveDamage();
        }
    }
}
