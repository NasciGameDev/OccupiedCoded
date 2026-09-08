using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Beartrap : MonoBehaviour 
{
    [SerializeField] private int damage = 1;
    [SerializeField] private AudioClip audioClip;
    [SerializeField, Range(0,1f)] private float volume = 1;
    [SerializeField, Space] private string trapId = "Beartrap";
    private void OnTriggerStay2D(Collider2D collider)
    {
        HealthSystem healthSystem = collider.GetComponentInParent<HealthSystem>();
        if (Input.GetKey(KeyCode.LeftShift) || healthSystem == null)
        {
            //Debug.Log($"The {name} is deactivated");
        }
        else
        {
            SoundManager.instance.playSound(audioClip, transform, volume);
            //Debug.Log($"{name} collided with {healthSystem.name}");
            healthSystem.DealDamage(damage, trapId);
            Debug.Log($"The player Health now is {healthSystem.getHealth}");
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            Destroy(gameObject, audioClip.length);
        }
    }

}
