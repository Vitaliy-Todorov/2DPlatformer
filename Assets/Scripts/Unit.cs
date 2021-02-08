using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /*public - ����� ������������ ��� ������. ����� ������������ ��� ������������.
    private - ������ ������������ ��� ������. ������ ���������� ��� ������������.
    protected - ������ ������������ ��� ������. ����� ������������ ��� ������������.*/

    //virtual ������������ ��� ��������� ���������� �������, �������, ������������ � ������� � ���������� �� ��������������� � ����������� ������.
    public virtual void ReceiveDamage()
    {
        Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
