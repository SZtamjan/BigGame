using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{

    [System.Serializable]
    private class posAndRot
    {
        public Vector3 pos;
        public Vector3 rot;
    }
    [SerializeField]
    List<posAndRot> destination = new List<posAndRot>();


    public float speed = 1f;
    public float angle;
    private Animator myAnim;

    public bool playWalk = true;
    public bool playIdle = false;

    public bool attack = false;
    public bool die = false;

    private void Start()
    {
        myAnim = GetComponent<Animator>();

    }

    void Update()
    {

        if (destination.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination[0].pos, Time.deltaTime * speed);



            if (destination.Count > 1 && Vector3.Distance(transform.position, destination[0].pos) < 1f && Vector3.Distance(transform.position, destination[0].pos) < 0.2f)
            {

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(destination[0].rot - transform.position), 8f * Time.deltaTime);




                if (Vector3.Distance(transform.position, destination[0].pos) < 0.01f)
                {
                    destination.RemoveAt(0);
                }
            }

            else if (Vector3.Distance(transform.position, destination[0].pos) < 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(destination[0].rot - transform.position), 10f * Time.deltaTime );


                angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(destination[0].rot - transform.position));

                if (playIdle && Vector3.Distance(transform.position, destination[0].pos) < 0.13f && angle >= 3f)
                {
                    playWalk = false;
                    myAnim.SetTrigger("idle");
                }

                if (angle < 0.1f && Vector3.Distance(transform.position, destination[0].pos) < 0.01f)
                {
                    destination.RemoveAt(0);
                }


            }

        }


        if (myAnim != null)
        {
            if (destination.Count > 0 && playWalk)
            {
                myAnim.SetTrigger("walk");
                playWalk = false;
                playIdle = true;

            }
            else if (destination.Count > 0)
            {

            }
            else if (playIdle)
            {
                myAnim.SetTrigger("idle");
                playWalk = true;
                playIdle = false;

            }
        }


        if (myAnim != null && die)
        {
            if (die)
            {
                myAnim.SetTrigger("die");

            }
            die = false;

        }

        if (myAnim != null && attack)
        {
            if (attack)
            {
                myAnim.SetTrigger("attack");

            }
            attack = false;

        }



    }

    public void AddToDestination(Vector3 place, Vector3 nextTile)
    {
        place.y = 0.14f;
        nextTile.y = 0.14f;
        posAndRot addToQ = new posAndRot() { pos = place, rot = nextTile };

        destination.Add(addToQ);

    }




}
