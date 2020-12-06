using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public int speed = 3;
    public float boundX = 1.85f;
    private Rigidbody2D rb2d;
    private Vector2 vel;
    bool _rightButtonDown;
    bool _leftButtonDown;
    private void UserKeyControl()
    {
        vel = rb2d.velocity;
        if (Input.GetKey(moveLeft))
        {
            vel.x = speed;
        }
        else if (Input.GetKey(moveRight))
        {
            vel.x = -speed;
        }
        else if (!Input.anyKey)
        {
            vel.x = 0;
        }
        rb2d.velocity = vel;
        var pos = transform.position;
        if (pos.x > boundX)
        {
            pos.x = boundX;
        }
        else if (pos.x < -boundX)
        {
            pos.x = -boundX;
        }
        transform.position = pos;
    }
    public void OnRightButtonDown(bool state)
    {
        _rightButtonDown = state;
    }

    public void OnLeftButtonDown(bool state)
    {
        _leftButtonDown = state;
    }
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	// Update is called once per frame
	void Update () {
        /*UserKeyControl();*/
        vel = rb2d.velocity;
        if (_rightButtonDown)
        {
            float moveRight = (Time.deltaTime * speed);
            transform.Translate(moveRight, vel.y, 0);
            var pos = transform.position;
            if (pos.x > boundX)
            {
                pos.x = boundX;
            }
            else if (pos.x < -boundX)
            {
                pos.x = -boundX;
            }
            transform.position = pos;
        }
        if (_leftButtonDown)
        {
            float moveLeft = (Time.deltaTime * speed);
            transform.Translate(-moveLeft, vel.y, 0);
            var pos = transform.position;
            if (pos.x > boundX)
            {
                pos.x = boundX;
            }
            else if (pos.x < -boundX)
            {
                pos.x = -boundX;
            }
            transform.position = pos;

        }

    }
}
