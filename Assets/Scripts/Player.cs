using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour
{


    private IEnumerator shootCoroutine;
    public float speed = 10.0F;
    public float smooth = 2.0F;    
    public GameObject bullet;
    float startTime;
    Transform targetToMove;
    [HideInInspector]
    public RaycastHit hit;
    private List<Bullet> bullets = new List<Bullet>();
          
    void Update()
    {
        TouchMove();
        if (targetToMove != null)
        {
            Move();
        }
    }

    void TouchMove() //проверяет манипуляции с тачем, совершает действия в зависимости от того, как палец коснулся(просто тап, тап на 2 сек)
    {
        if (Input.touchCount == 0) return;

        Touch current = Input.GetTouch(0);
        switch (current.phase)
        {
            case TouchPhase.Began:
                if (targetToMove != null)
                {
                    Destroy(targetToMove.gameObject);
                }
                if (shootCoroutine != null)
                {
                    StopCoroutine(shootCoroutine);
                }
                shootCoroutine = Shoot(current.position);
                StartCoroutine(shootCoroutine);
                startTime = Time.time;
                break;
            case TouchPhase.Stationary:
                if (Time.time - startTime > .2F && shootCoroutine != null)
                {
                    StopCoroutine(shootCoroutine);
                }
                if (Time.time - startTime > 2F)
                {
                    startTime = 0;
                    if (shootCoroutine != null)
                    {
                        StopCoroutine(shootCoroutine);
                    }
                    SetMoveTarget();
                }
                break;
            case TouchPhase.Canceled:
            case TouchPhase.Ended:
            case TouchPhase.Moved:
                startTime = 0;
                break;
        }

    }

    IEnumerator Shoot(Vector3 fingerPosition)
    {

        float angle = 20F;
        GameObject point = new GameObject();

        if (Physics.Raycast(Camera.main.ScreenPointToRay(fingerPosition), out hit))
        {
            point.transform.position = hit.point;
        }

        while (Vector3.Angle(transform.forward, point.transform.position - transform.position) > angle)

        {
            SmoothLookAt(point.transform.position, smooth);
            yield return new WaitForEndOfFrame();
        }

        CreateBullet(transform.position, point.transform);
        

        //GameObject b = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        //b.GetComponent<Bullet>().bulletTarget = point.transform;
    }

    void SmoothLookAt(Vector3 target, float smooth)
    {
        Vector3 dir = target - transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        transform.forward = Vector3.Slerp(transform.forward, dir, Time.deltaTime * smooth);
    }

    void SetMoveTarget()//плавный поворот
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (targetToMove != null)
            {
                Destroy(targetToMove.gameObject);
            }
            GameObject target = new GameObject();
            target.transform.position = hit.point;
            targetToMove = target.transform;
        }
    }

    void Move()
    {

        Vector3 direction = targetToMove.position;
        direction.y = transform.position.y;
        SmoothLookAt(direction, 4);
        transform.position = Vector3.MoveTowards(transform.position, direction, speed * Time.deltaTime);

        if (Vector3.Distance(targetToMove.position, transform.position) < 0.5F)
        {
            Destroy(targetToMove.gameObject);
        }
    }

    void CreateBullet(Vector3 pos, Transform bulletTarget)
    {


        if (bullets.Count < 15)
        {
            GameObject createdBullet = Instantiate(bullet, pos, transform.rotation) as GameObject;
            createdBullet.GetComponent<Bullet>().bulletTarget = bulletTarget;
            bullets.Add(createdBullet.GetComponent<Bullet>());

        }

        else
        {
            Bullet b = GetPooledBullet();
            if (b != null)
            {
                b.Create(pos, bulletTarget);
            }
        }

        

    }

    Bullet GetPooledBullet()//пул для пуль
    {
        List<Bullet> pooled = bullets.Where(b => !b.gameObject.activeInHierarchy).ToList();
        if (pooled.Count == 0)
        { return null; }
        return pooled[Random.Range(0, pooled.Count)];
    }
}

