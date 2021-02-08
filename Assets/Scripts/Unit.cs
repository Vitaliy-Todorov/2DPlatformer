using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /*public - можно использовать вне класса. можно использовать при наследовании.
    private - нельзя использовать вне класса. нельзя обращаться при наследовании.
    protected - нельзя использовать вне класса. можно использовать при наследовании.*/

    //virtual используется для изменения объявлений методов, свойств, индексаторов и событий и разрешения их переопределения в производном классе.
    public virtual void ReceiveDamage()
    {
        Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
