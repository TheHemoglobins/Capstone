using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FrameGeneratorScript : MonoBehaviour
{
    public GameObject prefabFrame;

    public Vector3 minGroundPos;
    public Vector3 maxGroundPos;

    public List<GameObject> frameList = new List<GameObject>();

    [SerializeField]
    public int numRunTry;
    [SerializeField]
    public int distanceBetween;
    [SerializeField]
    public float SnapDistance = 2f;
    public Transform[] anchors;

    GameObject newFrame;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        newFrame = Instantiate(prefabFrame, new Vector3(71, 1, 62), Quaternion.identity);

        frameList.Add(newFrame);
    }

    void Update()
    {
        for (int i = 0; i < frameList.Count && frameList.Count != numRunTry; i++){
            Vector3 randomSpawnPos = new Vector3(
                Random.Range(minGroundPos.x, maxGroundPos.x),
                1,
                Random.Range(minGroundPos.z, maxGroundPos.z)
            );

            foreach (GameObject frame in frameList)
            {
                distance = Mathf.Sqrt( Mathf.Pow( (frame.transform.position.x - randomSpawnPos.x), 2) + Mathf.Pow( (frame.transform.position.z - randomSpawnPos.z), 2));

                if (distance < distanceBetween){
                    break;
                }

                else if(distance >= distanceBetween && frame == frameList.Last()){
                    newFrame = Instantiate(prefabFrame, randomSpawnPos, Quaternion.identity);
                    frameList.Add(newFrame);
                    //SnapOn(newFrame, randomSpawnPos);

                    break;
                }
            }
        }
    }

    void SnapOn(GameObject frame, Vector3 spawnPosition){
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
            //pos = new Vector3 (Random.Range(point.x * anchor.localScale.x, point.x), point.y, Random.Range(point.z * anchor.localScale.x, point.z));
            if (anchor.transform.eulerAngles.x > 0f && anchor.transform.eulerAngles.x < 360){
                pos = new Vector3(Random.Range(point.x - 4, point.x), point.y, Random.Range(point.z -4, point.z));
            }else if (anchor.transform.eulerAngles.y > 0f && anchor.transform.eulerAngles.y < 360){
                pos = new Vector3(Random.Range(point.z -4, point.z), point.y, Random.Range(point.x - 4, point.x));
            }else if (anchor.transform.eulerAngles.z > 0f && anchor.transform.eulerAngles.z < 360){
                pos = new Vector3(Random.Range(point.z -4, point.z), point.y, Random.Range(point.x - 4, point.x));
            }else{
                pos = point;
            }

            if (distance > SnapDistance){ // position is close enough to snap point put it there.
                frame.transform.position = spawnPosition;
            }
            else{
                frame.transform.position = pos;
            }

        }        
    }
}
