using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMonster : Monster
{
    //������� ��������
    [SerializeField]
    private float rate = 2.0F;
    [SerializeField]
    private Color bulletColor = Color.black;

    Bullet bullet;

    protected override void Awake()
    {
        //Load(string path) - ��������� ������ path, ���������� � ����� Resources.
        bullet = Resources.Load<Bullet>("Bullet");
    }

    protected override void Start()
    {
        //InvokeRepeating(string methodName, float time, float repeatRate) - �������� ����� methodName � time ��������, � ����� �������� ������ repeatRate�������.
        InvokeRepeating("Shoot", rate, rate);
    }

    private void Shoot()
    {
        //��������� ����� �������� ����
        Vector3 position = transform.position;         position.y += 0.5F;
        //������ ����� ������� bullet � ��������� ����� � � ��������� ���������.
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        //�� ���� ��������� ����������� ���� � �������
        newBullet.Parent = gameObject;
        //Direction - ����������� �������
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
