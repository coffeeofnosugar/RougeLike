using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("EnemyHitArea"))
        {
            // other.transform.parent.GetComponent<MonsterController>().TakeDamage();
            // //敌人受伤
            // if (transform.parent.localScale.x > 0)
            //     other.transform.parent.GetComponent<Ghoul>().IsHit(Vector2.right);
            // else if (transform.parent.localScale.x < 0)
            //     other.transform.parent.GetComponent<Ghoul>().IsHit(Vector2.left);
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
