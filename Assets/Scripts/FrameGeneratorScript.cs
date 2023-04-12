using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//Class detailing the rectangle for each hiddenWall - move to its own file
public class Anchor{
    public Vector3 cornerR { get; set; }
    public Vector3 cornerL { get; set; }

    public Anchor(Vector3 rightCorner, Vector3 leftCorner)
    {
        this.cornerR = rightCorner;
        this.cornerL = leftCorner;
    }
}

public class FrameGeneratorScript : MonoBehaviour{

    public GameObject frameTemplate;
    public List<GameObject> frameList = new List<GameObject>();
    public GameObject cameraMan;

    [SerializeField]
    public int distanceBetween;

    public Transform[] hiddenWalls;
    private List<Anchor> anchorList = new List<Anchor>();
    private Anchor wallAnchor;

    float distance;

    public void Generate(string[] paths) {

        foreach (Transform wall in hiddenWalls)
        {
            this.wallAnchor = getWallAnchor(wall);
            this.anchorList.Add(this.wallAnchor);
        };

        var numOfPhotos = hiddenWalls.Count() / paths.Length;
        Debug.Log(numOfPhotos);

        for (var i = 0; i < hiddenWalls.Count(); i++){
            var wall = hiddenWalls[i];

            for (var j = 0; j < numOfPhotos; j++){
                var newFrame = Instantiate(frameTemplate, generateFramePos(anchorList[i], wall.transform.position.y), Quaternion.identity);
                newFrame.transform.eulerAngles = getRotation(wall);
                newFrame.transform.LookAt(cameraMan.transform);
                this.frameList.Add(newFrame);
            };

        };
        fixFrameDistance(this.frameList, this.distanceBetween, this.anchorList);
    }

    public void fixFrameDistance(List<GameObject> frameList, int distanceBetween, List<Anchor> anchorList){
        var rightFrameCorner = anchorList[0].cornerR.x;
        var leftFrameCorner = anchorList[0].cornerL.x;

        var frameDistance = Mathf.Sqrt(Mathf.Pow(rightFrameCorner, 2) + Mathf.Pow(leftFrameCorner, 2));

        for(var i = 1; i < frameList.Count(); i++){

            var currentFrame = frameList[i].transform.position;
            var lastFrame = frameList[i - 1].transform.position;

            var xPositions =  currentFrame.x - lastFrame.x;
            var zPositions = currentFrame.z - lastFrame.z;

            var distance = Mathf.Sqrt(Mathf.Pow(xPositions, 2) + Mathf.Pow(zPositions, 2));

            var pos = frameList[i].transform.position;

            if (distance < distanceBetween){
                currentFrame.x = currentFrame.x + distanceBetween + frameDistance;
                currentFrame.z = currentFrame.z + distanceBetween + frameDistance;
            }
        }
    }

    public Vector3 getRotation(Transform frame){
        var rotation = frame.transform.eulerAngles;

        rotation.x = rotation.x <= 180f ? rotation.x - 180f : rotation.x - 540f;
        rotation.y = rotation.y <= 180f ? rotation.y : rotation.y -360f;
        rotation.z = rotation.z <= 180f ? rotation.z : rotation.z - 360f;

        return (new Vector3(rotation.x, rotation.y, rotation.z)); 
    }

    public Vector3 generateFramePos(Anchor anchor, float wall){
        return new Vector3(
            Random.Range(anchor.cornerR.x, anchor.cornerL.x),
            wall,
            Random.Range(anchor.cornerR.z, anchor.cornerL.z)
        );
    }

    public Anchor getWallAnchor(Transform wall)
    {
        float rightX, rightZ, leftX, leftZ;
        float y = wall!.transform.position.y;

        Vector3 bottomRightCorner;
        Vector3 topLeftCorner;

        Anchor anchorWithCorners;

        // need bottom right
        rightX = wall.GetComponent<Renderer>().bounds.max.x;
        rightZ = wall.GetComponent<Renderer>().bounds.min.z;

        bottomRightCorner = new Vector3(rightX, y, rightZ);

        //need top left 
        leftX = wall.GetComponent<Renderer>().bounds.min.x;
        leftZ = wall.GetComponent<Renderer>().bounds.max.z;

        topLeftCorner = new Vector3(leftX, y, leftZ);

        anchorWithCorners = new Anchor(bottomRightCorner, topLeftCorner);

        return anchorWithCorners;
    }
}