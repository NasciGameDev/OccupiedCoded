using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private Camera mainCamera;

    [SerializeField] MMF_Player feedback;

    private bool lightTurn = true;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 rotation = mousePos - transform.position;
        
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotationZ);
        
        TurnFlashlight();
    
    }


    private void TurnFlashlight()
    {
        if (lightTurn == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                Light2D light = transform.GetComponentInChildren<Light2D>();
                /*foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }*/
                //light.gameObject.SetActive(false);
                light.GetComponent<Light2D>().enabled = false;

                lightTurn = false;
                feedback.PlayFeedbacks();
                Debug.Log("Turned On");
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Light2D light = transform.GetComponentInChildren<Light2D>();
                /*foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);

                }*/
                //light.gameObject.SetActive(true);
                light.GetComponent<Light2D>().enabled = true;

                lightTurn = true;
                //Debug.Log("Turned Off");

                feedback.PlayFeedbacks();
            }
        }
    }

   public bool GetFlashlightState() => lightTurn;
}
