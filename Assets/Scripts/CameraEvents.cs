using UnityEngine;

public class CameraEvents : MonoBehaviour {

    public Transform character;
    public Transform LeftBound, RightBound, TopBound, BottomBound;
    private Vector3 target;
    Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        CameraFollowToTheCharacter();
    }

    void CameraFollowToTheCharacter()
    {
        target = character.position;
        target.x = Mathf.Clamp(character.position.x, LeftBound.position.x + 12.5f, RightBound.position.x - 12.5f);
        target.y = Mathf.Clamp(character.position.y, BottomBound.position.y, TopBound.position.y);
        target.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, 0.15f);
    }
}
