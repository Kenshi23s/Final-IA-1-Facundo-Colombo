using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform target;
    int damage;
    float speed;
    int maxSpeed;
    Rigidbody rb;
    LayerMask layer;
    public void Initialize(Transform target,int maxSpeed,LayerMask layer, int damage=50,float speed =2)
    {
        this.damage = damage;
        this.speed  = speed;
        this.target = target;
        this.maxSpeed = maxSpeed;
        this.gameObject.layer = layer;
        this.layer = layer;
        rb = GetComponent<Rigidbody>();
        rb.useGravity= false;
        if (target==null)
        {
            Debug.LogError("el target es null");
        }
        Destroy(gameObject,15f);
    }

    private void Update()
    {
        Vector3 dir= target.transform.position - transform.position;
        transform.position+= dir * Time.deltaTime * speed;
        transform.forward = dir.normalized; 
    
    }
    private void OnTriggerEnter(Collider other)
    {
        var agent = other.GetComponent<Agent>();

        if (agent!=null)
        {
            agent.TakeDamage(damage);
            Debug.Log("le hice " + damage + " de daño a " + agent.name);
            Destroy(gameObject);
        }

        var bullet = other.GetComponent<Bullet>();
        if (bullet!=null)
        {
            if (bullet!=this&& this.layer!=bullet.layer)
            {
                Destroy(bullet);
            }
            return;
        }


      


    }

}
