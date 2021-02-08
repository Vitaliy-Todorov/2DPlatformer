using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    //SerializeField - ����������� ��������� � ���� �������
    [SerializeField]
    private int lives = 5;
    [SerializeField]
    private float speed = 3.0F;
    [SerializeField]
    private float jumpForce = 15.0F;

    private bool isGrounded = false;

    private Bullet bullet;

    //������������ ������������ CharState
    private CharState State
    {
        //� ����� get �� ���������� �������� ����, � � ����� set �������������. https://metanit.com/sharp/tutorial/3.4.php
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int) value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        //������ ������
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        //������ �� ����
        bullet = Resources.Load<Bullet>("Bullet");
    }

    //FixedUpdate - ���������� ����� ������������� ���������� �������
    private void FixedUpdate()
    {
        ChecGround();
    }

    // Update is called once per frame
    private void Update()
    {
        //������ ��������� ������ �� �����
        if (isGrounded) State = CharState.Idle;
        //������� �����. Fire1 - ����� ctrl
        if (Input.GetButtonDown("Fire1")) Shoot();
        //������ ����� ����
        if (Input.GetButton("Horizontal")) Run();
        //��������� ������� �����, ������
        if (isGrounded && Input.GetButtonDown("Jump")) Junp();
    }

    private void Run()
    {
        //� ����������� �� ���� ���� ����� � ����� ��� � ���� ���������� 1 ��� -1
        //Transform ���������� Position (���������), Rotation (��������), � Scale (�������) ������� ������� � �����. � ������� GameObject�� ���� Transform.
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        //��������� ����� ��������. Time.deltaTime - ����� ����� ������� � ���������� ������� (������)
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        //������������� �������� ��������� ��� ��������� ����������� ��������
        sprite.flipX = direction.x < 0.0F;
        //������ ��������� ������ �� ����� 
        if (isGrounded) State = CharState.Ran;
    }

    private void Junp()
    {
        //������ ��������� ������ �� ������
        State = CharState.Jump;
        //����� ����������� ��������� ����. up- ������ �������
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    //�������
    private void Shoot()
    {
        Vector3 position = transform.position;
        //�������� ����� �������� ���� �� ��� ��
        position.y += 0.8F;
        //������ ����
        /*�������� ����� AS ������ ��� �������� ������� � ���������� ����, �� � ������� �� �������� ����������� [(���)������], 
         * � ������ ������������� �������� ������ � ���������� ���� �� ������ ���������� ������� null*/
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        //newBullet.transform.righ - ����������� ���� ������. sprite.flipX - ����������� ��
        //sprite.flipX - ������ �� -1.0F: ��� 1.0F
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);
        //��������� �������� ����
        newBullet.Parent = gameObject;
    }

    public override void ReceiveDamage()
    {
        lives--;

        //�������� ���������, ��� �� ���� ������������ ���� ������ ���� ����������
        rigidbody.velocity = Vector3.zero;
        //���������� �� �����
        rigidbody.AddForce(transform.up * 8.0F, ForceMode2D.Impulse);

        Debug.Log(lives);
    }

    //����� �� �� �����
    private void ChecGround()
    {
        //Physics2D.OverlapCapsuleAll(collidersX, r) - ��������� ������ � ������� r �� collidersX ������ Colliders
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.03F);
        //���� � colliders ����� 1-�� �������� (collider ������ �������), �� false
        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = CharState.Jump;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //�������� ����� ��� �������������� � worm
        //collision.gameObject - ������� ������, � ����������� �������� �� �������������
        Unit unit = collider.gameObject.GetComponent<Unit>();
        if (unit) ReceiveDamage();
    }
}

//��������� ������ (�����, �����, �������)
public enum CharState
{
    //State - ��������� ������������ � animation
    Idle,
    Ran,
    Jump
}
