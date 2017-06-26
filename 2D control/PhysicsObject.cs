using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);


    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;


    // rigid body variable stored
    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    void Start () 
    {   // don't check with triggers
        contactFilter.useTriggers = false;
        // check it these collide layers
        contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
    }
    
    void Update () 
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity (); 
    }

    protected virtual void ComputeVelocity()
    {
    
    }

    
    void FixedUpdate()
    {   // update position on y axis(gravity)
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        // update x
        velocity.x = targetVelocity.x;

        // until it hit something
        grounded = false;
        // y
        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;
        // first x, then y
        Movement (move, false);

        move = Vector2.up * deltaPosition.y;

        Movement (move, true);
    }


    // movement to object
    void Movement(Vector2 move, bool yMovement)
    {   
        // decide to check or not
        float distance = move.magnitude;

        if (distance > minMoveDistance) 
        {   
            // check the layers
            int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear ();

            // copy the layer the can be hited
            for (int i = 0; i < count; i++) {
                hitBufferList.Add (hitBuffer [i]);
            }

            // compute the normal(angle)
            for (int i = 0; i < hitBufferList.Count; i++) 
            {
                Vector2 currentNormal = hitBufferList [i].normal;
                // compare the angle with a min
                if (currentNormal.y > minGroundNormalY) 
                {
                    grounded = true;
                    if (yMovement) 
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot (velocity, currentNormal);
                if (projection < 0) 
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList [i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }
        // set position of rb
        rb2d.position = rb2d.position + move.normalized * distance;
    }

}