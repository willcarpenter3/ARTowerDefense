using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SW_Interfaces;

public class Bolt : MonoBehaviour
{
    [Tooltip("Prefab of the explosion that will play on collision")]
    public GameObject collisionExplosion;
    [Tooltip("Speed of the bolt in the air")]
    public float speed;
    [Tooltip("Whether the bolt can cause damage on impact")]
    public bool canDamage;
    [Tooltip("Damage of the bolt on collision")]
    public float damage;

    private GameObject ogBlaster;

    private void Start()
    {
        Rigidbody projRigid = gameObject.GetComponent<Rigidbody>();

        Vector3 direction = gameObject.transform.forward;
        Vector3 powVec = new Vector3(speed, speed, speed);
        direction.Scale(powVec);
        projRigid.velocity = direction;
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canDamage)
        {
            if (!ogBlaster.transform.IsChildOf(collision.collider.gameObject.transform))
            {
                MonoBehaviour[] list = collision.collider.gameObject.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour mb in list)
                {
                    if (mb is IDamageable)
                    {
                        IDamageable breakable = (IDamageable)mb;
                        breakable.Damage(damage);
                    }
                }

                GameObject explosion = Instantiate(collisionExplosion, transform.position, transform.rotation);
                Destroy(gameObject);
                Destroy(explosion, 1f);

                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Collisioned");
            if (collision.collider.gameObject.CompareTag("follow") || collision.collider.gameObject.CompareTag("corpse"))
            {
                Debug.Log("Follow tag found");
                //GameObject explosion = Instantiate(collisionExplosion, transform.position, transform.rotation);
                Destroy(gameObject);
                //Destroy(explosion, 1f);

                //Destroy(gameObject);
            }
        }
    }

    public void SetBlaster(GameObject blaster)
    {
        ogBlaster = blaster;
    }
}
