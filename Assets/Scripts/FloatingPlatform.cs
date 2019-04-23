using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    public float movingSpeed, range;
    float startPosY;
    Vector3 newPos;

    // Use this for initialization
    void Start () {
        startPosY = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        Floating();
    }

    void Floating()
    {
        if (Mathf.Abs(newPos.y - transform.position.y) <= 0.2f)
            range *= -1;
        newPos = new Vector2(transform.position.x, startPosY + range);
        transform.position = Vector2.Lerp(transform.position, newPos, movingSpeed * Time.deltaTime);
    }
}
