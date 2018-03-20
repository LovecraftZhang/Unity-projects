using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour
{
    public int offsetX = 10;
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;
    public bool reverseScale = false;
    private float spriteWidth = 0f;
    private float spriteLength = 0f;
    private Camera theCamera;
    private Transform myTransform;
    private int numberOfLeftBuddies = 0;
    private bool secondRun = false;
    private bool alreadySpawned = false;

    private void Awake()
    {
        theCamera = Camera.main;
        myTransform = transform;
    }
    
    
    // Use this for initialization
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
        spriteLength = sRenderer.sprite.bounds.size.y;
    }
   
    
    // Update is called once per frame
    void Update()
    {
        if (!alreadySpawned)
        {
            if (!hasALeftBuddy || !hasARightBuddy)
            {
                float camHorizontalExtend = theCamera.orthographicSize * Screen.width / Screen.height;
                float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
                float edgeVisiblePositionLeft = ((myTransform.position.x - spriteWidth / 2) + camHorizontalExtend);

                if (theCamera.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasARightBuddy)
                {
                    MakeNewBuddy(1);
                    hasARightBuddy = true;
                }
                else if (theCamera.transform.position.x <= (edgeVisiblePositionLeft + offsetX) && !hasALeftBuddy)
                {
                    MakeNewBuddy(-1);
                    hasARightBuddy = true;
                    alreadySpawned = true;
                }
                secondRun = true;
            }
        }
    }
   
    
    
    //a function that create a buddy on the side required
    void MakeNewBuddy(int rightOrLeft)
    {
        //calculating the new position for our new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = (Transform)Instantiate(myTransform, newPosition, myTransform.rotation);
        if (reverseScale)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }
        newBuddy.transform.parent = myTransform;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}