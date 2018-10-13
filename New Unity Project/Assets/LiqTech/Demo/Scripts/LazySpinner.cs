using UnityEngine;

namespace LiqTech
{
    public class LazySpinner : MonoBehaviour
    {
        public float Timescale = 1;

        public bool SymmetricalDirectionSpeed;
        public float MinAxisDirectionSpeed = -90;
        public float MaxAxisDirectionSpeed = 90;
        public float AxisDirectionAcceleration = 5;

        public bool SymmetricalAxisSpeed;
        public float MinAxisSpeed = 0;
        public float MaxAxisSpeed = 180;
        public float AxisAcceleration = 10;

        public bool SymmetricalTurnSpeed;
        public float MinTurnSpeed = 0;
        public float MaxTurnSpeed = 360;
        public float TurnAcceleration = 20;

        private float axisDirectionPosition;
        private float axisDirectionSpeed;
        private Vector3 axis;
        private float axisSpeed;
        private float turnSpeed;

        void OnValidate()
        {
            MinAxisDirectionSpeed = Mathf.Clamp(MinAxisDirectionSpeed, -MaxAxisDirectionSpeed, MaxAxisDirectionSpeed);
            MinAxisSpeed = Mathf.Clamp(MinAxisSpeed, -MaxAxisSpeed, MaxAxisSpeed);
            MinTurnSpeed = Mathf.Clamp(MinTurnSpeed, -MaxTurnSpeed, MaxTurnSpeed);

            if (SymmetricalDirectionSpeed && MaxAxisDirectionSpeed < 0) MaxAxisDirectionSpeed = 0;
            if (SymmetricalDirectionSpeed) MinAxisDirectionSpeed = -MaxAxisDirectionSpeed;
            if (SymmetricalAxisSpeed && MaxAxisSpeed < 0) MaxAxisSpeed = 0;
            if (SymmetricalAxisSpeed) MinAxisSpeed = -MaxAxisSpeed;
            if (SymmetricalTurnSpeed && MinTurnSpeed < 0) MinTurnSpeed = 0;
            if (SymmetricalTurnSpeed) MinTurnSpeed = -MaxTurnSpeed;

            if (AxisAcceleration < 0) AxisAcceleration = 0;
            if (TurnAcceleration < 0) TurnAcceleration = 0;
        }

        void Start()
        {
            Scramble();
        }

        public void Scramble()
        {
            axisDirectionPosition = Random.Range(0, 360);
            axisDirectionSpeed = Random.Range(MinAxisDirectionSpeed, MaxAxisDirectionSpeed);
            axisSpeed = Random.Range(MinAxisSpeed, MaxAxisSpeed);
            turnSpeed = Random.Range(MinTurnSpeed, MaxTurnSpeed);
            axis = Random.onUnitSphere;
        }

        void Update()
        {
            var dt = Time.deltaTime * Timescale;

            axisDirectionSpeed += Random.Range(-1f, 1f) * dt * AxisDirectionAcceleration;
            axisSpeed += Random.Range(-1f, 1f) * dt * AxisAcceleration;
            turnSpeed += Random.Range(-1f, 1f) * dt * TurnAcceleration;

            axisDirectionSpeed = Mathf.Clamp(axisDirectionSpeed, MinAxisDirectionSpeed, MaxAxisDirectionSpeed);
            axisSpeed = Mathf.Clamp(axisSpeed, MinAxisSpeed, MaxAxisSpeed);
            turnSpeed = Mathf.Clamp(turnSpeed, MinTurnSpeed, MaxTurnSpeed);

            axisDirectionPosition += axisDirectionSpeed * dt;
            axisDirectionPosition = axisDirectionPosition % 360;




            var radp = Mathf.Deg2Rad * axisDirectionPosition;
            var v2 = new Vector2(Mathf.Cos(radp), Mathf.Sin(radp));
            v2 *= axisSpeed * dt;

            axis = Quaternion.Euler(v2) * axis;

            transform.rotation *= Quaternion.AngleAxis(turnSpeed * dt, Quaternion.Inverse(transform.rotation) * axis);

            //transform.rotation = rot;
            //transform.rotation *= Quaternion.AngleAxis(turnSpeed * dt, transform.InverseTransformDirection(Vector3.up));

            //Quaternion q = Quaternion.AngleAxis()
            //Quaternion q = Quaternion.AngleAxis(axisDirectionPosition, Vector3.up);
            //axis *= Quaternion 
            //axis *= Quaternion.AngleAxis(axisDirectionPosition, Vector3.forward)
            //if(axisDirectionPosition < )
        }
    }
}