using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;
    private bool touched;
    private int pointerID;

    public float smoothing;

    void Awake() {
        direction = Vector2.zero;
        touched = false;
    }

    public void OnPointerDown(PointerEventData data) {
        // Set our start point
        if (!touched) {
            touched = true;
            pointerID = data.pointerId;
            origin = data.position;
        }
    }

    public void OnDrag(PointerEventData data) {
        // Compare the difference between our start point and current pointer pos
        if (data.pointerId == pointerID) {
            Vector2 currentPosition = data.position;
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;
        }
    }

    public void OnPointerUp(PointerEventData data){
        //Reset Everything
        if (data.pointerId == pointerID) {
            direction = Vector2.zero;
            touched = false;
        }
    }

    public Vector2 GetDirection() {
        smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
        return direction;
    }

}
