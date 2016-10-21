using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float enemySpeed;
    public float minDist = 1f;
    public Transform target;
    public float smooth = 2;



    void Start ()//в старте находим цель
    {
        if (target == null)
        {

            if (GameObject.Find("Player") != null)
            {
                target = GameObject.Find("Player").GetComponent<Transform>();
                transform.LookAt(target);
            }
        }

    }

    
    void Update()
    {
        if (target == null)
            return;


        SmoothLookAt(target.position, 10.0F);

        float distance = Vector3.Distance(transform.position, target.position);


        if (distance > minDist)
        {
            transform.position += transform.forward * enemySpeed * Time.deltaTime;
            
        }
    }

    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void SmoothLookAt(Vector3 target, float smooth)
    {
        Vector3 dir = target - transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        transform.forward = Vector3.Slerp(transform.forward, dir, Time.deltaTime * smooth);
    }

    public void DestroyEnemy()
    {

        gameObject.SetActive(false);
    }

   public void Create(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }

    

}





