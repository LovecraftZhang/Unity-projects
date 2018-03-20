
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] background;
    private float[] parralaxScales;
    public float smoothing = 1f;

    private Transform cam;
    private Vector3 previousCamPosition;

    void Awake ()
    {
        cam = Camera.main.transform;
    }
	// Use this for initialization
	void Start () {
        previousCamPosition = cam.position;
        parralaxScales = new float[background.Length];

        for (int i = 1; i < background.Length;i++)
        {
            parralaxScales[i] = background[i].position.z * -1;
        }

    }
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < background.Length; i++)
        {
            float parallax = (previousCamPosition.x - cam.position.x) * parralaxScales[i];

            float backgroundTargetPosx = background[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosx, background[i].position.y, background[i].position.z);

            background[i].position = Vector3.Lerp(background[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

        }

        previousCamPosition = cam.position;
	}
}
