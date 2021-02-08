using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //�������� ������������� ������ (���� ��� �������)
    private GameObject parent;
    public GameObject Parent { set { parent = value;} get { return parent; } }

    private float speed = 10.0F;
    private Vector3 direction;
    //���� ������� ��
    public Vector3 Direction { set { direction = value; } }

    //����� ����
    public Color Color { set { sprite.color = value; } get { return sprite.color; } }

    //����� ��� ��������� �����
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, 1.4F);

        Debug.Log(sprite.color);
        Debug.Log(Color);
    }

    private void Update()
    {
        //�������� ����
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //���� ���� ��������� ������� ������� ��� ��������
        Unit unit = collider.GetComponent<Unit>();
        MoveableMonster moveableMonster = collider.GetComponent<MoveableMonster>();

        //���� ����� �� ������������ (�� ��� ��� ��������) � ���� ��� �� moveableMonster
        if (unit && unit.gameObject != parent && unit != moveableMonster)
        {
            //��������� �����
            unit.ReceiveDamage();
            //�����������
            Destroy(gameObject);
        }
    }
}
