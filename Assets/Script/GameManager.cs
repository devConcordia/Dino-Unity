using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	
	[SerializeField] private GameObject[] obstacleStyles;
	[SerializeField] private GameObject birdObstacle;
	
	[SerializeField] private GameObject[] groundStyles;
	[SerializeField] private GameObject ground;
	
	[SerializeField] private GameObject canvasGameOver;
	
	private float speed = 8.0f;
	
	private List<GameObject> groundList;
	private List<GameObject> obstacleList;
	
	private float distance = 0.0f;
	
	private Vector2 screenBounds;
	
	
	public static GameManager instance;

    private void Awake()
    {
        if( instance == null ) {
			
            instance = this;
            //DontDestroyOnLoad(gameObject);
			
        } else {
			
            Destroy(this);
			
        }
    }

	
    // Start is called before the first frame update
    void Start()
    {
		
		groundList = new List<GameObject>();
		obstacleList = new List<GameObject>();
		
		screenBounds = Camera.main.ScreenToWorldPoint(Vector2.zero);
		
	//	Debug.Log( screenBounds.x.ToString() +", "+ screenBounds.y.ToString() );
		
    }

    // Update is called once per frame
    void Update()
    {
        
		if( groundList.Count < 3 )
			appendGround();
		
		if( obstacleList.Count == 0 )
			appendObstacle();
		
		float delta = Time.deltaTime * speed;
		
		Vector3 delta_v3 = new Vector3(-1f,0f,0f) * delta;
		
		foreach( var g in groundList )
			g.transform.position = g.transform.position + delta_v3;
		
		foreach( var o in obstacleList )
			o.transform.position = o.transform.position + delta_v3;
		
		removeGround();
		removeObstacle();
		
		distance += delta;
		speed += 0.0001f;
		
    }
	
	
	
	private void appendGround() {
		
		float dx = 0f;
		
		if( groundList.Count > 0 ) {
			
			GameObject lastGround = groundList[ groundList.Count - 1 ];
			
			SpriteRenderer renderer = lastGround.GetComponent<SpriteRenderer>();
			Vector2 size = renderer.bounds.size;
			
			dx = lastGround.transform.position.x + size.x;
			
		}
		
		int i = (int) Random.Range(0f, (float)groundStyles.Length);
		
		Vector3 position = ground.transform.position + new Vector3(dx, 0f, 0f);
		
		GameObject g = Instantiate(groundStyles[i], position, Quaternion.identity);
		
		groundList.Add( g );
		
	}
	
	private void removeGround() {
		
		if( groundList.Count > 0 ) {
			
			GameObject g0 = groundList[0];
			
			SpriteRenderer renderer = g0.GetComponent<SpriteRenderer>();
			Vector2 size = renderer.bounds.size;
			
			/// 2f*size.x
		//	if( (g0.transform.position.x + size.x) < -size.x *.5f ) {
			if( (g0.transform.position.x + size.x) < screenBounds.x * .5f ) {
				Destroy(g0);
				groundList.RemoveAt(0);
			}
			
		}
		
	}
	
	///
	
	private void appendObstacle() {
		
		GameObject obstacle;
		
		if( Random.Range(0f, 1f) > .5f ) {
			
			/// bird
			float y = (float)((int) Random.Range(0f, 3f));
			
			Vector3 position = ground.transform.position + new Vector3(-1 * screenBounds.x, y,0f);
			
			obstacle = Instantiate(birdObstacle, position, Quaternion.identity);
			
		} else {
			
			/// cactus
			int i = (int) Random.Range(0f, (float)obstacleStyles.Length);
			
			Vector3 position = ground.transform.position + new Vector3(-1 * screenBounds.x,0f,0f);
			
			obstacle = Instantiate(obstacleStyles[i], position, Quaternion.identity);
			
		}
		
		obstacleList.Add( obstacle );

	}
	
	private void removeObstacle() {
		
		if( obstacleList.Count > 0 ) {
			
			GameObject obstacle = obstacleList[0];
			
			SpriteRenderer renderer = obstacle.GetComponent<SpriteRenderer>();
			Vector2 size = renderer.bounds.size;
			
			/// 2f*size.x
			if( (obstacle.transform.position.x + size.x) < screenBounds.x ) {
				Destroy(obstacle);
				obstacleList.RemoveAt(0);
			}
		}
		
	}
	
	
	///
	public void GameOver() {
    	canvasGameOver.SetActive(true);
        Time.timeScale = .0001f;
    }

    public void Restart() {
    	SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
	
}
