using UnityEngine;
using UnityEngine.Playables;

public class TimelineSequenceStarter : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector timeline;
    private bool firstTime;


    private void Start()
    {
        firstTime = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (firstTime)
        {
            timeline.Play();
            firstTime = false;
        }
    }
}
