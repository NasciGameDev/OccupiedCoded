using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightTrapShiner : MonoBehaviour
{
    [SerializeField]
    private float cooldownBetweenChecker;

    [SerializeField]
    private Transform lightEndingPoint;

    private Flashlight flashlight;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlight = GetComponent<Flashlight>();
        timer = cooldownBetweenChecker;
    }

    // Update is called once per frame
    void Update()
    {
        if(flashlight.GetFlashlightState()) //If on
        {
            if (CooldownTimerForSignal())
            {
                RaycastHit2D[] rays = Physics2D.LinecastAll(transform.position, lightEndingPoint.position);

                foreach (RaycastHit2D ray in rays)
                {
                    if (ray && ray.collider.GetComponent<TrapShine>() is TrapShine trap)
                    {
                        Debug.Log("AHHH");
                        trap.StartShineSequence();
                    }
                }
            }
        }
    }


    private bool CooldownTimerForSignal()
    {
        bool sendSignal = false;
        
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            sendSignal = true;
            timer = cooldownBetweenChecker;
        }

        return sendSignal;
    }
}
