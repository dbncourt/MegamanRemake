using System.Collections.Generic;
using UnityEngine;

namespace PlayerController.InputController
{
    public class PlayerInputController : MonoBehaviour
    {
        private Dictionary<string, Key> actionKeysMap;
        private Dictionary<string, Axis> axisMap;

        public PlayerInputController()
        {
            actionKeysMap = new Dictionary<string, Key>();
            axisMap = new Dictionary<string, Axis>();
        }

        void Start() { }

        void Update()
        {
            foreach (KeyValuePair<string, Key> key in actionKeysMap)
            {
                key.Value.EvaluateActionKey();
            }
        }

        void FixedUpdate()
        {
            foreach (KeyValuePair<string, Axis> axis in axisMap)
            {
                axis.Value.EvaluateAxisEvent();
            }
        }

        public void BindAction(ActionInputConstants actionName, Key.ActionEvent eventMethod, Key.EKeyStatus keyStatus)
        {
            Key key;
            try
            {
                key = actionKeysMap[actionName.ToString()];
            }
            catch (KeyNotFoundException)
            {
                key = new Key(actionName.ToString());
                actionKeysMap.Add(actionName.ToString(), key);
            }
            key.BindAction(eventMethod, keyStatus);
        }

        public void BindAxis(AxisInputConstants axisName, Axis.AxisEvent axisEvent)
        {
            Axis axis;
            try
            {
                axis = axisMap[axisName.ToString()];
            }
            catch (KeyNotFoundException)
            {
                axis = new Axis(axisName.ToString());
                axisMap.Add(axisName.ToString(), axis);
            }
            axis.BindAxis(axisEvent);
        }

        public Key.KeyStatus QueryInputKeyStatus(string keyName)
        {
            return actionKeysMap[keyName].inputKeyStatus;
        }        
    }
}