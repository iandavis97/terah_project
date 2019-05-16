//
//  Camera dissolve effect. Copyright 2017 Sebastian Strandberg
//
//  This script is just for giving some motion to the cameras in the demo scene. Feel free to use it for whatever you want though.

using UnityEngine;

namespace LiqTech
{
    public class CircleCamera : MonoBehaviour
    {
        /// <summary>
        /// The point to rotate around
        /// </summary>
        public Vector3 Origin;
        
        /// <summary>
        /// This distance from the point to rotate
        /// </summary>
        public float Radius = 5;
        
        /// <summary>
        /// The point to look towards
        /// </summary>
        public Vector3 FocalPoint;
        
        /// <summary>
        /// How fast to rotate
        /// </summary>
        public float SpinRate = 10;


        public float Angle;
        private float trueAngle;
        public float MinAngle;
        public float MaxAngle;
        public float Direction = 1;

        //Draw the circle to make things a little easier to edit.
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < 360; i += 5)
            {
                var a = Origin + Quaternion.AngleAxis(i, Vector3.up) * Vector3.forward * Radius;
                var b = Origin + Quaternion.AngleAxis(i + 5, Vector3.up) * Vector3.forward * Radius;

                Gizmos.DrawLine(a, b);
            }

            Gizmos.DrawLine(Origin + Vector3.forward * Radius, FocalPoint);
        }

        void Start()
        {
            trueAngle = Angle;
        }

        void Update()
        {
            Angle += Time.deltaTime * SpinRate * Direction;
            //just a bit of smoothing
            trueAngle = Mathf.Lerp(trueAngle, Angle, 0.05f);

            if(Angle < MinAngle)
            {
                Direction = 1;
            }
            if(Angle > MaxAngle)
            {
                Direction = -1;
            }

            //Place the transform based on the current angle.
            transform.position = Origin + Quaternion.AngleAxis(trueAngle, Vector3.up) * Vector3.forward * Radius;
            transform.LookAt(FocalPoint);
        }
    }
}