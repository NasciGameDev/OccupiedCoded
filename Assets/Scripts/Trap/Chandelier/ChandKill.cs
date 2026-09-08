using UnityEngine;

public class ChandKill : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField, Range(0,1f)]
    private float volumeHittingGround;
    
    [SerializeField, Space]
    private AudioClip audioClipHitting;
    [SerializeField, Range(0, 1f)]
    private float volumeHittingPerson;
    [SerializeField] private string trapId = "Chandelier";



    private void OnTriggerEnter2D(Collider2D collider)
    {
        HealthSystem healthSystem = collider.GetComponentInParent<HealthSystem>();
        if (healthSystem == null)
        {
            SoundManager.instance.playSound(audioClip, transform, volumeHittingGround);
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            Destroy(gameObject.GetComponent<SpriteRenderer>());
            Destroy(gameObject, audioClip.length);
        }
        else
        {
            SoundManager.instance.playSound(audioClipHitting, transform, volumeHittingPerson);
            healthSystem.DealDamage(damage, trapId);
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            Destroy(gameObject.GetComponent<SpriteRenderer>());
            Destroy(gameObject, audioClipHitting.length);

        }

    }
}
