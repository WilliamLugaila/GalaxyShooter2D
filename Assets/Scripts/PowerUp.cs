using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    // Start is called before the first frame update
    [SerializeField] // 0 = TripleShot
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    private AudioSource _audioSource;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            Player player = other.transform.GetComponent<Player>();
            switch(powerupID)
            {
                case 0:
                    player.tripleShotActive();
                    break;
                case 1:
                    
                    player.SpeedBoostActive();
                    break;
                case 2:
                    player.ShieldActive();
                    break;
                default:
                    Debug.Log("default");
                    break;

            }
            
            
            
            Destroy(this.gameObject );
        }
    }

        

}
