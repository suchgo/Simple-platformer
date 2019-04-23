using UnityEngine;

public class BackgroundMove : MonoBehaviour {

    public float speed;
    float offset;
    Vector3 cameraPrevPosition;

    void Start()
    {
        speed /= 5000;
        cameraPrevPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
    }

    void Update()
    {
        MoveBackgrounds();
    }

    void MoveBackgrounds()
    {
        if (cameraPrevPosition.x != GameObject.FindGameObjectWithTag("MainCamera").transform.position.x)
        {
            offset += Input.GetAxis("Horizontal") * speed;
            GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
        cameraPrevPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
    }
}
