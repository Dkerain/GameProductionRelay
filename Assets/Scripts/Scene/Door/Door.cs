using System.Collections;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool permanent;
    public float clock;
    public Sprite lockedImage;
    public Sprite openedImage;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = lockedImage;
    }

    public void Unlocked()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = openedImage;
        if (!permanent)
        {
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(clock);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = lockedImage;
    }
}
