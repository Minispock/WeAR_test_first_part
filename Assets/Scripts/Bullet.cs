using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public Transform bulletTarget;

    

    void Update ()
    {
        
        if (bulletTarget != null)
        {
            BulletMove();
        }
	}

    public void BulletMove()
    {
        Vector3 direction = bulletTarget.position;
        direction.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, direction, 5.0F * Time.deltaTime);

        if (Vector3.Distance(transform.position, bulletTarget.position) < 2.0F)
        {
            Destroy(bulletTarget.gameObject);
            DestroyBullet();
        }
    }

    void OnTriggerEnter(Collider col)//пуля уничтожает врага
    {
        if (col.GetComponent<Enemy>())
        {
            col.GetComponent<Enemy>().DestroyEnemy();
            Destroy(bulletTarget.gameObject);
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {

        gameObject.SetActive(false);
    }

    public void Create(Vector3 pos, Transform bulletTarget)
    {
        this.bulletTarget = bulletTarget;
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
