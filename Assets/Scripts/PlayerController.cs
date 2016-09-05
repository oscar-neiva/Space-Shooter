using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour{
    private Quaternion calibrationQuaternion;

    public SimpleTouchPad touchPad;
    public float speed;
    public float tilt;
    public Boundary boundary;
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float nextFire;
    //public SimpleTouchPad touchPad;
    public SimpleTouchAreaButton areaButton;

    void Start(){
        CalibrateAccelerometer ();
    }

    void Update() {
        if (areaButton.CanFire() && Time.time > nextFire){
            nextFire = Time.time + fireRate;
            // GameObject clone = 
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);  // as GameObject
            GetComponent<AudioSource>().Play();
        }
    }

    void CalibrateAccelerometer() {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
    }

    Vector3 FixAcceleration(Vector3 acceleration) {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }

    void FixedUpdate() {
        //float moveHorizontal = Input.GetAxis ("Horizontal");
        //float moveVertical = Input.GetAxis ("Vertical");

        //Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        //Vector3 accelerationRaw = Input.acceleration;
        //Vector3 acceleration = FixAcceleration (accelerationRaw);
        //Vector3 movement = new Vector3(acceleration.x, 0.0f, acceleration.y);
        Vector2 direction = touchPad.GetDirection();
        Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
        //Vector3 movement = new Vector3 (direction.x, 0.0f, direction.y);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }
}
