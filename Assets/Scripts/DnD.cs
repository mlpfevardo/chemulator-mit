using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnD : MonoBehaviour {

    // Use this for initialization
    float deltaX, deltaY;

    Rigidbody2D rb;

    bool moveAllowed = false;
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        PhysicsMaterial2D mat = new PhysicsMaterial2D();
        mat.bounciness = 0f;
        mat.friction = 0f;
        GetComponent<BoxCollider2D>().sharedMaterial = mat;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch(touch.phase)
            {
                case TouchPhase.Began:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        deltaX = touchPos.x - transform.position.x;
                        deltaY = touchPos.y - transform.position.y;
                        moveAllowed = true;
                        rb.freezeRotation = true;
                        rb.velocity = new Vector2(0, 0);
                        rb.gravityScale = 0;
                        GetComponent<BoxCollider2D>().sharedMaterial = null;
                    }
                    break;
                case TouchPhase.Moved:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos) && moveAllowed)
                        rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                    break;

                case TouchPhase.Ended:
                    moveAllowed = false;
                    rb.freezeRotation = false;
                    rb.gravityScale = 0;
                    PhysicsMaterial2D mat = new PhysicsMaterial2D();
                    mat.bounciness = 0f;
                    mat.friction = 0f;
                    GetComponent<BoxCollider2D>().sharedMaterial = mat;
                    break;
                    
            }
        }
	}
}
