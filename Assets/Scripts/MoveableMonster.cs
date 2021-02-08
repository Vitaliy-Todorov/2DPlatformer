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
        //��������� ����� ���
        //sprite = GetComponentInChildren<SpriteRenderer>();
        //Debug.Log(sprite);
        // (��� �����������)���� ������������
        //bullet = Resources.Load<Bullet>("Bullet");
    }
    protected override void Start()
    {
        //����� ����������� ��������
        direction = transform.right;
    }

    protected override void Update()
    {
        Move();
    }

    //OnTriggerEnter2D - ���������� ����� ������ ������������������� � ������
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        //���� ���������� �� ������� ���������� ������ 0.3, �� ���� �������������� ��������� � ����, � ��������� ������ ������������� ��������� ������ (�� ��� �������� ���� � ������ ��������� ������ ��� � ������)
        //���� ������ ������� ����������, ���� ���, ������� �� ���� ������ (unit)
        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.3F)
                ReceiveDamage();
            else unit.ReceiveDamage();
        }
    }

    private void Move()
    {
        //��������� ������� ����� ������� (����������� �� 0.5 ����� � � ������� �������� �� 0.5, � ��������� ������� ����� � ������� 0.1)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + direction * 0.5F, 0.1F);
        //���� ������� �� 0 ������ � ���� �� ��, �� ������ ����������� (��� �������� �������� �� ���� �� ���������� ����� ���������)
        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>())) direction *= -1.0F;
        //�������� �����
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
