using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallControl : MonoBehaviour {
    private Rigidbody2D rb2d;
    private Vector2 vel;
    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;
    private static int PlayerGoal = 0;
    //todo: Add max and min speed 4 ball
    //todo: Ai
    public void GoBall()
    {
        float rand = Random.Range(0, 2);
        if (PlayerGoal == 1)
        {
            rb2d.AddForce(new Vector2(0, 20));
        }
        else if (PlayerGoal == 2)
        {
            rb2d.AddForce(new Vector2(0, -20));
        }
        else if (rand<1)
        {
            rb2d.AddForce(new Vector2(0, 20));
        }
        else
        {
            rb2d.AddForce(new Vector2(0, -20));
        }
    }
    public void ResetBall()
    {
        vel = Vector2.zero;
        rb2d.velocity = vel;
        transform.position = new Vector2(0, 1);
        UpdateScore(PlayerScore1, PlayerScore2);
        //ToDo: Make smooth animation for player reset
        //ToDo: Make pause menu and reset menu & move left and right walls
        //GameObject.Find("Player01.Cool").transform.position = new Vector2(0, 2.9f);
        //GameObject.Find("Player02.Cool").transform.position = new Vector2(0, -0.9f);
    }
    public void ResetGame()
    {
        PlayerScore1 = 0;
        PlayerScore2 = 0;
        ResetBall();
        Invoke("GoBall", 2);
    }
    public void RestartGame()
    {
        ResetBall();
        Invoke("GoBall", 1);
    }
    private static void UpdateScore(int player1=0,int player2=0)
    {
        GameObject.Find("Play1Score").GetComponent<Text>().text = PlayerScore1.ToString();
        GameObject.Find("Play2Score").GetComponent<Text>().text = PlayerScore2.ToString();
    }
    public static void Score(string wallID)
    {
        if (wallID == "TopWall")
        {
            PlayerScore1++;
            PlayerGoal = 1;
        }
        else
        {
            PlayerScore2++;
            PlayerGoal = 2;
        }
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
         if (coll.collider.CompareTag("Player"))
        {
            float rand = Random.Range(-2, 2);
            vel.y = rb2d.velocity.y;
            vel.x = (rb2d.velocity.x / 2.0f) + ((coll.collider.attachedRigidbody.velocity.x+rand) / 3.0f);
            rb2d.velocity = vel;
        }
    }
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke("GoBall", 2);
	}


    Vector2 predictRigidBodyPosInTime(Rigidbody2D sourceRigidbody, float timeInSec)
    {
        //Get current Position
        Vector2 defaultPos = sourceRigidbody.position;

        Debug.Log("Predicting Future Pos from::: x " + defaultPos.x + " y:"
            + defaultPos.y);

        //Simulate where it will be in x seconds
        while (timeInSec >= Time.fixedDeltaTime)
        {
            timeInSec -= Time.fixedDeltaTime;
            Physics.Simulate(Time.fixedDeltaTime);
        }

        //Get future position
        Vector2 futurePos = sourceRigidbody.position;

        Debug.Log("DONE Predicting Future Pos::: x " + futurePos.x + " y:"
            + futurePos.y);

        //Re-enable Physics AutoSimulation and Reset position
        Physics.autoSimulation = true;
        sourceRigidbody.velocity = Vector2.zero;
        sourceRigidbody.position = defaultPos;

        return futurePos;
    }

    public float fadeSpeed = 2f;
    Color oldColor = new Color(255, 255, 255, 255);
    Color newColor = new Color(144,180,144,255);
    float fadeAmount = 0;
    void ColorChange()
    {
        SpriteRenderer RightWall = GameObject.Find("RightWallImg").GetComponent<SpriteRenderer>();
        Debug.Log("Calling");
        fadeAmount += fadeSpeed * Time.deltaTime;
        RightWall.color = Color.Lerp(oldColor, newColor, fadeAmount);
        if (fadeAmount > 1f)
        {
            fadeAmount = 0;
            oldColor = RightWall.color;
            int random = Random.Range(1, 5);
            if (random == 1)
                newColor = Color.red;
            else if (random == 2)
                newColor = Color.cyan;
            else if (random == 3)
                newColor = Color.magenta;
            else if (random == 4)
                newColor = Color.green;
        }
    }
    // Update is called once per frame
    float directionSwitch = 1;
    void Update () {
        ColorChange();
        //Ai holder
        float AiMove = (Time.deltaTime * 0.2f)*directionSwitch;
        float ballDirection = transform.position.x - GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().transform.position.x;
        if (ballDirection>0)
        {
            if (AiMove<0)
            {
                directionSwitch *= -1;
                AiMove = (Time.deltaTime * 0.2f) * directionSwitch;
            }
            GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().transform.Translate(AiMove, 0, 0);
            if (GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().position.x >= 1.1f && AiMove >= 0)
            {
                directionSwitch *= -1;
            }
        }
        else
        {
            if (AiMove > 0)
            {
                directionSwitch *= -1;
                AiMove = (Time.deltaTime * 0.2f) * directionSwitch;
            }
            GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().transform.Translate(AiMove, 0, 0);
            if (GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().position.x <= -1.1f && AiMove <= 0)
            {
                directionSwitch *= -1;
            }
        }
        //GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().transform.Translate(GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().transform.position.x-rb2d.velocity.x);
        //Debug.Log(GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().transform.position.x-rb2d.velocity.x);

        /*Debug.Log(GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().position.x);
        if (GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().position.x >= 1.1f && AiMove>=0)
        {
            directionSwitch*=-1;
        }
        else if (GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().position.x <= -1.1f && AiMove<=0)
        {
            directionSwitch *= -1;
        }
        GameObject.Find("Player01.Cool").GetComponent<Rigidbody2D>().transform.Translate(AiMove, 0, 0);
        */
    }
}
