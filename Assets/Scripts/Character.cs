using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    //SerializeField - отображение параметра в меню объекта
    [SerializeField]
    private int lives = 5;
    [SerializeField]
    private float speed = 3.0F;
    [SerializeField]
    private float jumpForce = 15.0F;

    private bool isGrounded = false;

    private Bullet bullet;

    //Обрабатываем перечеснение CharState
    private CharState State
    {
        //В блоке get мы возвращаем значение поля, а в блоке set устанавливаем. https://metanit.com/sharp/tutorial/3.4.php
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int) value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        //Создаём ссылки
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        //ссылка на пулю
        bullet = Resources.Load<Bullet>("Bullet");
    }

    //FixedUpdate - вызывается через фиксированное количество времени
    private void FixedUpdate()
    {
        ChecGround();
    }

    // Update is called once per frame
    private void Update()
    {
        //Меняем состояние игрока на стоит
        if (isGrounded) State = CharState.Idle;
        //Выстрел пулей. Fire1 - левый ctrl
        if (Input.GetButtonDown("Fire1")) Shoot();
        //ходить право лево
        if (Input.GetButton("Horizontal")) Run();
        //Проверяем наличие земли, прыжок
        if (isGrounded && Input.GetButtonDown("Jump")) Junp();
    }

    private void Run()
    {
        //В зависимости от того куда бежим в право или в лево возвращаем 1 или -1
        //Transform определяет Position (положение), Rotation (вращение), и Scale (масштаб) каждого объекта в сцене. У каждого GameObject’а есть Transform.
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        //Насколько нужно сдвинуть. Time.deltaTime - время между текущим и предыдущим фреймом (кадром)
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        //Инвертировать модельку персонажа при изменении направления движения
        sprite.flipX = direction.x < 0.0F;
        //Меняем состояние игрока на бежит 
        if (isGrounded) State = CharState.Ran;
    }

    private void Junp()
    {
        //Меняем состояние игрока на прыжок
        State = CharState.Jump;
        //Обект позволяющий приложить силу. up- длинна вектора
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    //Выстрел
    private void Shoot()
    {
        Vector3 position = transform.position;
        //поднимим место создания пули от ног гг
        position.y += 0.8F;
        //создаём пулю
        /*Ключевое слово AS служит для перевода объекта к указанному типу, но в отличие от знакомой конструкции [(тип)объект], 
         * в случае невозможности привести объект к указанному типу мы вместо исключения получим null*/
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        //newBullet.transform.righ - Направление пули вправо. sprite.flipX - направление гг
        //sprite.flipX - истина то -1.0F: лож 1.0F
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);
        //Назначаем родителя пули
        newBullet.Parent = gameObject;
    }

    public override void ReceiveDamage()
    {
        lives--;

        //Обнуляем ускорение, что бы сила отталкивания была больше силы притяжения
        rigidbody.velocity = Vector3.zero;
        //оталкивать от врага
        rigidbody.AddForce(transform.up * 8.0F, ForceMode2D.Impulse);

        Debug.Log(lives);
    }

    //стоит ли на земле
    private void ChecGround()
    {
        //Physics2D.OverlapCapsuleAll(collidersX, r) - проверяем естьли в радиусе r от collidersX дркгие Colliders
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.03F);
        //Если в colliders более 1-го элемента (collider самого объекта), то false
        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = CharState.Jump;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //отнимаем жизни при взаимодействии с worm
        //collision.gameObject - Игровой объект, с коллайдером которого вы сталкиваетесь
        Unit unit = collider.gameObject.GetComponent<Unit>();
        if (unit) ReceiveDamage();
    }
}

//Состояние игрока (стоит, бежит, прыгает)
public enum CharState
{
    //State - связывает перечесление и animation
    Idle,
    Ran,
    Jump
}
