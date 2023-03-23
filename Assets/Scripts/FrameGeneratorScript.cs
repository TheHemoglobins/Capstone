using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//Class detailing the rectangle for each hiddenWall
public class anchors{
    public Vector3 cornerR;
    public Vector3 cornerL;
    public Vector3 corners(Vector3 cornerR, Vector3 cornerL){
        cornerR = cornerR; 
        cornerL = cornerL
    }

    private override Vector3 getCorner(String corner, GameObject wall){
        var x, z;
        var y = wall.Transform.position.y;
        switch (corner){
            case corner == 'right':
                //dummy variables until testing using this link as reference: 
                //https://stackoverflow.com/questions/22605683/unity3d-how-to-determine-the-corners-of-a-gameobject-in-order-to-position-other
                // Bottom right corner of the wall needed
                x = wall.renderer.bound.max.x;
                z = wall.renderer.bound.min.z;
                return Vector3(x, y, z);
                break;
            case corner == 'left':
                //dummy variables until testing
                //Top Left corner of the wall needed
                x = wall.renderer.bound.min.x;
                z = wall.renderer.bound.max.z;
                return Vector3(x, y, z);
                break;
        }
    }
}

public class FrameGeneratorScript : MonoBehaviour{

    public GameObject frameTemplate;
    public List<GameObject> frameList = new List<GameObject>();

    [SerializeField]
    public int numRunTry;
    [SerializeField]
    public int distanceBetween;
    [SerializeField]
    public float SnapDistance = 2f;

    //Input for hidden gameObjects next to walls
    public Transform[] hiddenWalls;
    private List<Vector3> anchorList;

    GameObject newFrame;
    float distance;

    // Start is called before the first frame update
    void Start(){
/*      Will we want the middle vector to be an input situation? Possibly a list or array
        What is a quaterion
        could we make it so that this loops through in following way:
             hiddenWalls = array (make a hidden game object rectangle that is the size of a wall and skinny)
             anchors = array (matches up to each game object above)
             for i in length prefabframe{
                 new frame = instantiate(hiddenWalls[i], anchors[i], quaterions[i])
                 frameList.Add(newFrame)
             } */

        //Grabs the bottom right corner and top left corner of each wall in hiddenWalls
        foreach (GameObject wall in hiddenWalls){
            anchorList.Add(anchors(getCorner('right', wall), getCorner('left', wall)));
        }

        //Creates a new frame for each hiddenWall 
        for (i = 0; i < hiddenWalls; i++){
            //Instantiate parameters = (Template, position, rotation)
            newFrame = Instantiate(frameTemplate, generateFramePos(anchorList[i]), Quaternion.identity);
            frameList.Add(newFrame);
            //Snap frame to wall
            SnapOn(newFrame);

        }
    }

    private Vector3 generateFramePos(Vector3 anchor){
        //Create a random position withing the anchor's range
        return randomSpawPos = new Vetor3(
            Random.Range(anchor.cornerR.x, anchor.cornerL.x),
            Random.Range(anchor.cornerR.z), Random.Range(anchor.cornerL.z),
            wall.transform.position.y
        )
    }

    //getRotation for seeing if the frame is in the correct rotation, can also be used with walls
    //one singular return is a float 
    private Vector3 Vector3Int getRotation(GameObject frame){
        frame.eulerAngles.x = frame.eulerAngles.x <= 180f ?? frame.eulerAngles.x : frame.eulerAngles.x -360f;
        frame.eulerAngles.y = frame.eulerAngles.y <= 180f ?? frame.eulerAngles.y : frame.eulerAngles.y -360f; 
        frame.eulerAngles.z = frame.eulerAngles.z <= 180f ?? frame.eulerAngles.z : frame.eulerAngles.z -360f; 
        return Vector3(frame.eulerAngles.x, frame.eulerAngles.y, frame.eulerAngler.z); 
    }

        //we will be redoing this in a less confusing way but good start
        //get rid of frameList.Count
        /* for (int i = 0; i < frameList.Count && frameList.Count != numRunTry; i++){
            Vector3 randomSpawnPos = new Vector3(
                //possibly use this instead of the prefab frames (or maybe with) to squash to walls
                Random.Range(minGroundPos.x, maxGroundPos.x),
                1,
                Random.Range(minGroundPos.z, maxGroundPos.z)
            );

            //break these into two different for loops, have an array or list of randomSpawnPositions that are in same order as frames.
            foreach (GameObject frame in frameList)
            {
                distance = Mathf.Sqrt( Mathf.Pow( (frame.transform.position.x - randomSpawnPos.x), 2) + Mathf.Pow( (frame.transform.position.z - randomSpawnPos.z), 2));

                if (distance < distanceBetween){
                    break;
                }

                else if(distance >= distanceBetween && frame == frameList.Last()){
                    newFrame = Instantiate(frameTemplate, randomSpawnPos, Quaternion.identity);
                    frameList.Add(newFrame);

                    break;
                }
            }
        }
    }

    private void SnapOn(GameObject frame, Vector3 spawnPosition){
        Vector3 point = Vector3.zero;
        Vector3 pos;
        foreach (var anchor in anchors)
        {
            var d = Vector3.Distance(spawnPosition, anchor.transform.position);
            if (d < distance) 
            {
            distance = d;
            point = anchor.transform.position;
            }
            pos = new Vector3 (Random.Range(point.x * anchor.localScale.x, point.x), point.y, Random.Range(point.z * anchor.localScale.x, point.z));

            frame.transform.position = distance > Snapdistance ?? spawnPosition : pos; 

        }*/

    //add more private functions to make this legible code. Bc currently this is a bit of a yikes.

}
