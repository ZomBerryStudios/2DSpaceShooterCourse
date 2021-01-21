using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;

    
    public void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        if(transform.position.y <= -7.5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            
            if (player != null)
            {
                
                switch(powerupID)
                {
                    case 0:
                        player.OnTriplePickUp();
                        break;
                    case 1:
                        player.OnSpeedPickup();
                        break;
                    case 2:
                        player.OnShieldPickup();
                        break;
                    default:
                        break;

                }
            }
            
            Destroy(this.gameObject);
        }
    }

}
