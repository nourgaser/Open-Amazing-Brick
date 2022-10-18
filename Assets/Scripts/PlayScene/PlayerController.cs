using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float sideForceValue = 20f;
    public float upForceValue = 20f;

    private Rigidbody2D rb;

    public static UnityEvent<float> playerPastHalfScreen;
    public static UnityAction playerFellOutOfScreen;
    public static event UnityAction playerJumped;
    public static event UnityAction playerCollided;
    public static event UnityAction playerPaused;

    private float width;
    private float height;

    public bool controlsEnabled = true;

    private void Awake()
    {
        playerPastHalfScreen = new UnityEvent<float>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        width = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        height = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

    // Update is called once per frame
    void Update()
    {

        if (controlsEnabled)
        {
            bool touchStarted = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
            bool leftTouch = (touchStarted && Input.GetTouch(0).position.x <= Screen.width / 2);
            bool rightTouch = (touchStarted && Input.GetTouch(0).position.x > Screen.width / 2);

            if (Input.GetKeyDown(KeyCode.LeftArrow) || leftTouch)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(new Vector2(-sideForceValue, upForceValue), ForceMode2D.Impulse);
                playerJumped.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || rightTouch)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(new Vector2(sideForceValue, upForceValue), ForceMode2D.Impulse);
                playerJumped.Invoke();
            }

            // if (Input.GetKeyDown(KeyCode.Escape)) {
            //     playerPaused.Invoke();
            // }
        }

        if (transform.position.y > 0) playerPastHalfScreen.Invoke(transform.position.y);
        if (transform.position.y <= -Boundaries.screenBounds.y) playerFellOutOfScreen.Invoke();

        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -Boundaries.screenBounds.x + width, Boundaries.screenBounds.x - width);
        viewPos.y = Mathf.Clamp(viewPos.y, -Boundaries.screenBounds.y - 1f, 0);
        transform.position = viewPos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        playerCollided.Invoke();
    }
}
