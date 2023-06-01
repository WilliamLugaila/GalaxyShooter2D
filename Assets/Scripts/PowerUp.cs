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
    private AudioSource _audioSource;
   
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if ( _audioSource == null )
        {
            Debug.LogError("The audiosource is null.");
        }
    }

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
            _audioSource.Play();
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
                    player.ShieldsActive();
                    break;
                default:
                    Debug.Log("default");
                    break;

            }
            
            
            
            Destroy(this.gameObject );
        }
    }

        

}
