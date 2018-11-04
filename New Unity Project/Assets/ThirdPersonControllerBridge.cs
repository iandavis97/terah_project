using Invector.vCharacterController;

using UnityEngine;



namespace ORKIntegrationLibrary.Invector

{    

    [RequireComponent(typeof(vThirdPersonInput))]

    public class ThirdPersonControllerBridge : MonoBehaviour

    {

        // Lock cursor when disabling controller input?

        public bool lockCursor = true;

        private vThirdPersonInput _thirdPersonInput = null;



        void Awake()

        {            

            _thirdPersonInput = gameObject.GetComponent<vThirdPersonInput>();

        }



        void OnEnable()

        {                        

            gameObject.GetComponent<vThirdPersonController>().gameObject.GetComponent<vThirdPersonInput>().SetLockBasicInput(false);
            if (lockCursor) { SetCursorLock(true); }

        }



        void OnDisable()

        {            

            gameObject.GetComponent<vThirdPersonController>().gameObject.GetComponent<vThirdPersonInput>().SetLockBasicInput(false);
            SetCursorLock(false);         

        }



        public void SetCursorLock(bool cursorLock = true)

        {

            Cursor.visible = !cursorLock;

            Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;

        }

    }

}
