using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LastEnemyMove : MonoBehaviour
{
    [SerializeField]
    private Transform playerTrans;
    [SerializeField]
    private Transform circlePoint;
    private Coroutine coroutine;
    private Vector3 position;
    private float circleRange = 5f;
    private void Start()
    {
       this.coroutine =this.StartCoroutine(this.CoMove());
    }
    private IEnumerator CoMove()
    {
        yield return new WaitForSeconds(1.0f);
        var position = circlePoint.position;
        while (true)
        {
            this.transform.LookAt(position);
            this.transform.Translate(Vector3.forward * 8f * Time.deltaTime);
            var dis = Vector3.Distance(position, this.transform.position);
            if (dis < 0.5f)
            {
                StartCoroutine(this.CoRandomMove());
                break;
            }
            yield return null;
        }
    }

    private IEnumerator CoRandomMove()
    {
        this.position = this.circlePoint.position;
        while (true)
        {
            if (Vector3.Distance(position, this.transform.position) < 0.2f)
            {
                yield return new WaitForSeconds(0.7f);
                Vector3 randPos = (Vector3)Random.insideUnitSphere * circleRange;
                randPos.y = 0;
                position = this.circlePoint.position + randPos;
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, position, 0.9f);
            }

            yield return null;
        }
    }
}
