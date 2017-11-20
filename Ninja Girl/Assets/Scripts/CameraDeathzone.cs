using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeathzone : MonoBehaviour {
    
    [SerializeField]    private Transform character;
    [SerializeField]    private float offsetX;
    [SerializeField]    private float offsetY;
    
    private bool shouldMoveX = false;
    private bool shouldMoveY = false;
    private float movementX = 0;
    private float movementY = 0;
    
    
    
	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {

        getOffsets();

        //x, y, z
        this.transform.position = new Vector3(movementX, movementY, this.transform.position.z);
       
	}

    private void getOffsets() {

        if ((transform.position.x - offsetX) > character.transform.position.x) {
            shouldMoveX = true;
            movementX = character.transform.position.x + offsetX;
        }   else if ((transform.position.x + offsetX) < character.transform.position.x) {
            shouldMoveX = true;
            movementX = character.transform.position.x - offsetX;
        }   else {
            movementX = this.transform.position.x;
        }

        //  Movement Y
        if ((transform.position.y - offsetY) > character.transform.position.y) {
            shouldMoveY = true;
            movementY = character.transform.position.y + offsetY;
        }   else if ((transform.position.y + offsetY) < character.transform.position.y) {
            shouldMoveY = true;
            movementY = character.transform.position.y - offsetY;
        }   else  {
            movementY = this.transform.position.y;
        }

    }
}
