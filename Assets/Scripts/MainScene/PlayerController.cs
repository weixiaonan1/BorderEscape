using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float jumpForce;

    public float gravity;

    public float trackWidth;

    public float changeTrackSpeed;

    //use kinectInput or keyboardInput
    public bool useKinectInput;


    private bool grouded;
    private bool hasChangedPosition;
    private float target;

    public Animator animator;
    private Transform foot;
    private Rigidbody body;

    private GameController gameCtrl;
    private PlayerGestureListener gestureListener;
    private CapsuleCollider RunCollider;
    private CapsuleCollider SquatCollider;

    private bool arrived;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        foot = transform.Find("Foot");

        gameCtrl = GameObject.Find("GameController").GetComponent<GameController>();
        Component[] CapsuleColliders = GetComponents(typeof(CapsuleCollider));
        RunCollider = (CapsuleCollider)CapsuleColliders[0];
        SquatCollider = (CapsuleCollider)CapsuleColliders[1];
    }


    void Start()
    {
        grouded = false;

        hasChangedPosition = false;

        target = 0;

        //get mode(user selected) in menu scene and set useKinectInput
        int mode = PlayerPrefs.GetInt("mode");
        useKinectInput = mode == 0 ? true : false;
        gestureListener = PlayerGestureListener.Instance;

        animator.SetFloat("MoveSpeed", 1.5f);
    }

    void Update()
    {
        //set animator state
        animator.SetBool("Grounded", grouded);
        //check if in target track
        arrived = Mathf.Abs(transform.position.z - target) < 0.1;

        //use kinect input
        if (useKinectInput)
        {
            //player can turn left or turn right only if player is in target track
            if (!hasChangedPosition && arrived)
            {
                if (gestureListener.IsSwipeLeft())
                {
                    target = target + Mathf.Sign(-0.5f) * trackWidth;
                    target = Mathf.Clamp(target, -trackWidth, trackWidth);
                    hasChangedPosition = true;
                    GetComponent<AudioSource>().Play();
                }
                else if (gestureListener.IsSwipeRight())
                {
                    target = target + Mathf.Sign(0.5f) * trackWidth;
                    target = Mathf.Clamp(target, -trackWidth, trackWidth);
                    hasChangedPosition = true;
                    GetComponent<AudioSource>().Play();
                }
            }
            if (!gestureListener.IsSwipeLeft() && !gestureListener.IsSwipeRight() && hasChangedPosition)
            {
                hasChangedPosition = false;
            }

        }
        //use keyboard input
        else
        {
            float input = Input.GetAxis("Horizontal");

            if (input != 0 && !hasChangedPosition && arrived)
            {
                target = target + Mathf.Sign(input) * trackWidth;
                target = Mathf.Clamp(target, -trackWidth, trackWidth);
                hasChangedPosition = true;
                GetComponent<AudioSource>().Play();
            }

            if (input == 0 && hasChangedPosition)
            {

                hasChangedPosition = false;
            }
        }
        //check if the player is in ground according to its foot'sposition
        grouded = Physics.Linecast(transform.position, foot.position,
            1 << LayerMask.NameToLayer("Ground"));
    }

    private void FixedUpdate()
    {
        //add gravity to the player
        body.AddForce(Vector3.down * gravity);
        float zPos = Mathf.MoveTowards(transform.position.z, target, changeTrackSpeed * Time.deltaTime);
        transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                zPos
            );

        //player can jump or squat if he is in ground and at target tarck, 
        if (grouded && arrived)
        {
            if (useKinectInput)
            {
                if (gestureListener.IsJump())
                {
                    body.velocity = new Vector3(0, jumpForce, 0);
                    GetComponent<AudioSource>().Play();
                }
                if (gestureListener.IsSquat())
                {
                    Squat();
                }
                else
                {
                    RunCollider.isTrigger = true;
                    SquatCollider.isTrigger = false;
                }
            }
            else
            {
                if (Input.GetButton("Jump"))
                {
                    body.velocity = new Vector3(0, jumpForce, 0);
                    GetComponent<AudioSource>().Play();
                }

                if (Input.GetAxis("Vertical") < 0)
                {
                    Squat();
                }
                else
                {
                    RunCollider.isTrigger = true;
                    SquatCollider.isTrigger = false;
                }
            }

        }

    }


    public void Death()
    {
        //stop bgm and play death audio
        GameObject.Find("BGM").GetComponent<AudioSource>().Stop();
        GetComponents<AudioSource>()[1].Play();

    }
    public void Squat()
    {
        RunCollider.isTrigger = false;
        SquatCollider.isTrigger = true;
        animator.Play("SLIDE00");
    }


    private void OnTriggerEnter(Collider other)
    {
        //if player hit the obstacle, gameover
        if (other.CompareTag("Obstacle"))
        {
            Death();
            gameCtrl.Gameover();
        }
    }
}
