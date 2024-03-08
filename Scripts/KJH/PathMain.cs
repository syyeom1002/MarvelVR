using CurvedPathGenerator;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PathMain : MonoBehaviour
{

    public class WayPoint {
        public Vector3 position;
        public float distanceFromStartPos;
        public Transform postrans;

        public WayPoint(Vector3 position, float distanceFromStartPos, Transform postrans)
        {
            this.position = position;
            this.distanceFromStartPos = distanceFromStartPos;
            this.postrans = postrans;
        }
    }

    public PathGenerator gen;
    public Transform[] arrTransforms;
    private List<Vector3> posList = new List<Vector3>();
    public Transform startpos;
    public List<WayPoint> wayPoints = new List<WayPoint>();

  
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gen.NodeList.Count);
        Debug.Log(gen.AngleList_World.Count);
        
        foreach(var node in gen.NodeList)
        {
            //Debug.LogFormat("{0},{1},{2}",node.x,node.y, node.z);
            //https://docs.unity3d.com/ScriptReference/Transform.TransformPoint.html
            var localpos = new Vector3(node.x, node.y, node.z);
            var worldpos = this.gen.transform.TransformPoint(localpos);
            this.posList.Add(worldpos);
        }
        foreach (var angle in gen.AngleList)
        {
            //Debug.LogFormat("{0},{1},{2}", angle.x, angle.y, angle.z);
            var localpos = new Vector3(angle.x, angle.y, angle.z);
            var worldpos = this.gen.transform.TransformPoint(localpos);
            this.posList.Add(worldpos);
        }

        Debug.LogFormat("pos list :  <color=yellow>{0}</color>", this.posList.Count);

        for(int i = 0; i < this.posList.Count; i++) {

            var pos = this.posList[i];
            Debug.LogFormat("=> {0}: <color=lime>{1}</color>", this.arrTransforms[i].name, pos);
            this.arrTransforms[i].position = pos;

            var dir = (pos - this.startpos.position).normalized;
            var dis = Vector3.Distance(pos, this.startpos.position);

            var ray = new Ray(this.startpos.position, dir * dis);
            Debug.DrawRay(ray.origin, ray.direction * dis, Color.red, 5);

            this.wayPoints.Add(new WayPoint(pos, dis, this.arrTransforms[i]));
        }


        //https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.list-1.sort?view=net-8.0
        //https://learn.microsoft.com/ko-kr/dotnet/api/system.comparison-1?view=net-8.0
        //Less than 0    x is less than y.
        //0   x equals y.
        //Greater than 0  x is greater than y.

        this.wayPoints.Sort((x, y)=>
        {

            if (x.distanceFromStartPos > y.distanceFromStartPos) return 1;
            else if (y.distanceFromStartPos > x.distanceFromStartPos) return -1;
            else return x.distanceFromStartPos.CompareTo(y.distanceFromStartPos);

        });


        for (int i = 0; i < this.wayPoints.Count; i ++) {
            Debug.LogFormat("=>  <color=yellow>{0}</color>, {1}", this.wayPoints[i].postrans, this.wayPoints[i].position);

          
        }


        

    }

    
}
