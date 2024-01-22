using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBullet
{
    public void setLookDirection(Vector3 LookDirection);
    public void Rotate();
    public void Move();
    public IEnumerator LifeBullet();
}
public class Bullet : MonoBehaviour, IBullet
{
    public float speed;
    public float damage;
    public float lifeBullet;
    private Vector3 LookDirection;
    public void setLookDirection(Vector3 LookDirection)
    {
        this.LookDirection = LookDirection;
    }
    public void OnEnable()
    {
        StartCoroutine(LifeBullet());
    }
    public void FixedUpdate()
    {
        Rotate();
        Move();
    }
    public void Rotate()
    {
        transform.eulerAngles = LookDirection;
        
    }
    public void Move()
    {
        float rotationAngle = LookDirection.y;
        Vector3 forwardDirection = Quaternion.Euler(0, 0, rotationAngle) * new Vector3(0,0,1);
        gameObject.transform.Translate(forwardDirection* speed);
    }
    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Поподание!");
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Stat>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    public IEnumerator LifeBullet()
    {
        yield return new WaitForSeconds(lifeBullet);
        Destroy(gameObject);
    }
}
