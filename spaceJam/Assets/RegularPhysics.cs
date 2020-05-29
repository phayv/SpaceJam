using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularPhysics : MonoBehaviour
{
	public float gravityEffectOfPlanet = 1f;
	public float floorMinimumY = 0.65f;

	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	protected List<RaycastHit2D>hitBufferList = new List<RaycastHit2D>(16);
	protected ContactFilter2D contactFilter;
	protected Rigidbody2D rb2d;
	protected Vector2 velocity;
	protected Vector2 groundNormal;
	protected bool isGrounded;

	protected const float floor = 0.0001f;
	protected const float padding = 0.01f;

	void OnEnable()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

    // Start is called before the first frame update
    void Start()
    {
        // change project settings -> physics 2d layer to update layer collisions
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
    	velocity += gravityEffectOfPlanet * Physics2D.gravity * Time.deltaTime;
    	isGrounded = false;
    	Vector2 deltaPosition = velocity * Time.deltaTime;
    	Vector2 move = Vector2.up * deltaPosition.y;

    	Movement (move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
    	float distance = move.magnitude;

    	if (distance > floor)
    	{
    		int modifyVelocity = rb2d.Cast (move, contactFilter, hitBuffer, distance + padding);

    		// overlaps physics collision
    		hitBufferList.Clear ();
    		for (int i = 0; i < modifyVelocity; i++) 
    		{
    			hitBufferList.Add (hitBuffer [i]);
    		}
    		for (int i = 0; i < hitBufferList.Count; i++) 
    		{
    			Vector2 currentNormal = hitBufferList[i].normal;
    			if (currentNormal.y > floorMinimumY) 
				{
					isGrounded = true;
					if (yMovement)
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projectedMovement = Vector2.Dot(velocity, currentNormal);
				if (projectedMovement < 0)
				{
					velocity = velocity - (projectedMovement * currentNormal);
				}

				float modDistance = hitBufferList[i].distance - padding;
				distance = modDistance < distance ? modDistance : distance;
    		}
    	}
    	rb2d.position = rb2d.position + (move.normalized * distance);
    }
}
