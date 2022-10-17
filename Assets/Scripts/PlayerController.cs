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

    public static UnityEvent<float> playerPastHalfScreen = new UnityEvent<float>();
    public static event UnityAction playerJumped;
    public static event UnityAction playerCollided;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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

        if (transform.position.y > 0) playerPastHalfScreen.Invoke(transform.position.y);

    }

    void OnCollisionEnter2D(Collision2D collision) {
        playerCollided.Invoke();
    }
}
