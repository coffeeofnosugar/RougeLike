using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private GameObject attacker;
    public bool isFriendHit = false;
    private void Start()
    {
        attacker = transform.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attacker.CompareTag("Player"))
        {
            if(other.CompareTag("EnemyHitArea"))
            {
                if (transform.parent.localScale.x > 0)
                    other.transform.parent.GetComponent<MonsterController>().TakeDamage(Vector2.right);
                else if (transform.parent.localScale.x < 0)
                    other.transform.parent.GetComponent<MonsterController>().TakeDamage(Vector2.left);
            }
            if (other.CompareTag("PlayerHitArea") && isFriendHit)
            {
                if (transform.parent.localScale.x > 0)
                    other.transform.parent.GetComponent<PlayerController>().TakeDamage(Vector2.right);
                else if (transform.parent.localScale.x < 0)
                    other.transform.parent.GetComponent<PlayerController>().TakeDamage(Vector2.left);
            }
        }
        if (attacker.CompareTag("Enemy"))
        {
            if(other.CompareTag("EnemyHitArea") && isFriendHit)
            {
                if (transform.parent.localScale.x > 0)
                    other.transform.parent.GetComponent<MonsterController>().TakeDamage(Vector2.right);
                else if (transform.parent.localScale.x < 0)
                    other.transform.parent.GetComponent<MonsterController>().TakeDamage(Vector2.left);
            }
            if (other.CompareTag("PlayerHitArea"))
            {
                if (transform.parent.localScale.x > 0)
                    other.transform.parent.GetComponent<PlayerController>().TakeDamage(Vector2.right);
                else if (transform.parent.localScale.x < 0)
                    other.transform.parent.GetComponent<PlayerController>().TakeDamage(Vector2.left);
            }
        }
    }
}
