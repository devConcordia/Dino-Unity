using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Vector2 input;
	
	private bool jumping = true;
	//private bool downing = false;
	
	public Rigidbody2D body;
    [SerializeField] private Animator anim;
    [SerializeField] public float jumpForce = 30f;
	
	private PolygonCollider2D polygonCollider;
	private SpriteRenderer spriteRenderer;
	
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
		
		polygonCollider = GetComponent<PolygonCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float y = Input.GetAxisRaw("Vertical");
		
		if( !jumping ) {
			
			anim.SetFloat("y", y);
			
			if( y > 0 ) {
				
				jumping = true;
				body.velocity = new Vector2(0,jumpForce);
			
			}
			
			updateCollider();
			
		}
		
    }
	
	private void updateCollider() {
		
		Sprite sprite = spriteRenderer.sprite;
		int size = sprite.GetPhysicsShapeCount();
		
		List<Vector2> path = new List<Vector2>();
		
		for( int i = 0; i < size; i++ ) {
			path.Clear();
			sprite.GetPhysicsShape(i, path);
			polygonCollider.SetPath(i, path.ToArray());
		}
		
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.CompareTag("Ground") ) {
		
			jumping = false;
		   
        } else if( collision.gameObject.CompareTag("Obstacle") ) {
			
			anim.SetBool("dead", true);
			GameManager.instance.GameOver();
			
		}
		
		
    }
	
	
}
