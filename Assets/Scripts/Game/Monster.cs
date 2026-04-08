using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isRight;
    public float attackPower;
    public int flag;
    public float moveSpeed = 2f;
    Vector3 original;
    SpriteRenderer render;

    void OnEnable()
    {
        render = GetComponent<SpriteRenderer>();
        original = transform.position;
        int rand = UnityEngine.Random.Range(0,2);
        if(rand == 0)
        {
            flag =1;
        }
        else
        {
            flag = -1;
        }
        float r = UnityEngine.Random.Range(0, 4f);
        moveSpeed = r;
    }
    void Update()
    {
        if(flag == 1)
        {
            if(transform.position.x <= original.x + 2)
            {
                transform.Translate(new Vector3(1,0,0) * moveSpeed * Time.deltaTime);   
            }
            else
            {
                flag = -1;
                render.flipX = false;
            }
        }
        else
        {
            if(transform.position.x >= original.x -2)
            {
                transform.Translate(new Vector3(-1,0,0) * moveSpeed * Time.deltaTime);
            }
            else
            {
                flag = 1;
                render.flipX = true;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            MonsterSpawner.Instance.ReSpawn();
        }
    }
}
