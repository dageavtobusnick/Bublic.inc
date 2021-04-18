using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public int _index;
    public string Name { get; }
    public Texture2D Icon { get; }
    public Transform AttackPosition;
    public int Damage { get; set; }
    public int Durability { get; set; }
    public float TimeBtwnAttack { get; set; }
    public float StartTimeAttack { get; set; }
    public float AttackRadius;



    private void Awake()
    {

        enabled = true;
        StartTimeAttack = 1f;
        TimeBtwnAttack = 0;

    }

    void Update()
    {
        if (TimeBtwnAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoDamage();
                //PickUp();
                TimeBtwnAttack = StartTimeAttack;

            }
        }
        else 
        {
            TimeBtwnAttack -= Time.deltaTime;
            print("���� �����������");
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (Input.GetKey(KeyCode.E) && collision.tag == "Player")
        {
            PickUp(collision);

        }
    }
    public void PickUp(Collider2D player)
    {
        GetComponent<ObjectNameView>().enabled = false;
        //player.GetComponent<Inventory>().AddItem(_index);//���� ������ �����, �� �� ������ ��������� �������
        Destroy(gameObject); //�������� ������� � �����
    }
    public void AddItem(int index)
    {
        //hasItems[index] = true;
    }
    public void DoDamage()
    {
            Collider2D[] enemysCollider = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRadius, LayerMask.GetMask("Enemy"));

            if (enemysCollider.Length == 0)
            {
                print("������ ��� � ������� ������");
                return;
            }
;
            foreach (Collider2D enemyCollider in enemysCollider)
            {
                enemyCollider.GetComponent<HP>().TakeDamage(15);
            }
            TimeBtwnAttack = StartTimeAttack; 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AttackPosition.position, AttackRadius);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AttackPosition.position, AttackRadius);
    }
}
