using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//Class detailing the rectangle for each hiddenWall - move to its own file
public class Anchor {
    public Vector3 cornerR { get; set; };
    public Vector3 cornerL { get; set; };

    public Anchor(Vector3 rightCorner, Vector3 leftCorner)
    {
        this.cornerR = rightCorner;
        this.cornerL = leftCorner;
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
    private List<Anchor> anchorList;
    private Anchor wallAnchor;

    GameObject newFrame;
    float distance;

    // Start is called before the first frame update
    void Start(){

        //Grabs the bottom right corner and top left corner of each wall in hiddenWalls
        foreach (GameObject wall in hiddenWalls)
        {
            this.wallAnchor = getWallAnchor(wall);
            this.anchorList.Add(this.wallAnchor);
        };

        //Creates a new frame for each hiddenWall 
        for (var i = 0; i < hiddenWalls.Count(); i++){
            //Instantiate parameters = (Template, position, rotation)
            //Snapping should be taken care of in the actual generation of the frame position
            var wall = hiddenWalls[i];
            var wallRotation = getRotation(wall);

            var newFrame = Instantiate(frameTemplate, generateFramePos(anchorList[i], wallRotation), Quaternion.identity);
            frameList.Add(newFrame);

            //Check distances between frames
            var xPositions = newFrame[i].transform.position.x - newFrame[i - 1].transform.position.x;
            var zPositions = newFrame[i].transform.position.z - newFrame[i - 1].transform.position.z;

            distance = Mathf.Sqrt( Mathf.Pow(xPositions, 2) + Mathf.Pow(zPositions, 2));

            if (distance > distanceBetween){
               // move ith frame away from i-1th frame
               // get vector position of i-1th frame
               // change ith frame position to Vector3(distance between x or z, y, other variable not changing)
                break;
            };
        };
    }

    //getRotation for seeing if the frame is in the correct rotation, can also be used with walls
    public Vector3 getRotation(GameObject frame){
        var rotation = frame.transform.eulerAngles;

        rotation.x = rotation.x <= 180f ? rotation.x : rotation.x -360f;
        rotation.y = rotation.y <= 180f ? rotation.y : rotation.y -360f;
        rotation.z = rotation.z <= 180f ? rotation.z : rotation.z - 360f;

        return (new Vector3(rotation.x, rotation.y, rotation.z)); 
    }

    //Create a random position within the anchor's range should take care of snapping
    public Vector3 generateFramePos(Anchor anchor, Vector3 wallRotation){
        var randomPosition = new Vector3(
            Random.Range(anchor.cornerR.x, anchor.cornerL.x),
            Random.Range(anchor.cornerR.z, anchor.cornerL.z),
            wall.transform.position.y
        );

        randomPosition.transform.eulerAngles = wallRotation;

        return randomPosition;
    }

    public Anchor getWallAnchor(GameObject wall)
    {
        float rightX, rightZ, leftX, leftZ;
        float y = wall!.transform.position.y;

        Vector3 bottomRightCorner;
        Vector3 topLeftCorner;

        Anchor anchorWithCorners;

        // need bottom right
        rightX = wall!.GetComponent<Renderer>().bounds.max.x;
        rightZ = wall!.GetComponent<Renderer>().bounds.min.z;

        bottomRightCorner = new Vector3(rightX, y, rightZ);

        //need top left 
        leftX = wall!.GetComponent<Renderer>().bounds.min.x;
        leftZ = wall!.GetComponent<Renderer>().bounds.max.z;

        topLeftCorner = new Vector3(leftX, y, leftZ);

        anchorWithCorners = new Anchor(bottomRightCorner, topLeftCorner);

        return anchorWithCorners;
    }
}

//I dont think we will need anything under this
//======================================================================

        //we will be redoing this in a less confusing way but good start
        //get rid of frameList.Count
        /* for (int i = 0; i < frameList.Count && frameList.Count != numRunTry; i++){
            Vector3 newFrame[i-1] = new Vector3(
                //possibly use this instead of the prefab frames (or maybe with) to squash to walls
                Random.Range(minGroundPos.x, maxGroundPos.x),
                1,
                Random.Range(minGroundPos.z, maxGroundPos.z)
            );

            //break these into two different for loops, have an array or list of randomSpawnPositions that are in same order as frames.
            foreach (GameObject frame in frameList)
            {
                distance = Mathf.Sqrt( Mathf.Pow( (frame.transform.position.x - newFrame[i-1].x), 2) + Mathf.Pow( (frame.transform.position.z - newFrame[i-1].z), 2));

                if (distance < distanceBetween){
                    break;
                }

                else if(distance >= distanceBetween && frame == frameList.Last()){
                    newFrame = Instantiate(frameTemplate, newFrame[i-1], Quaternion.identity);
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
