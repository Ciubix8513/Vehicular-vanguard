using UnityEngine;
using CarGame.Vehicle.Saving;
using System.Collections.Generic;
using System.Linq;

namespace CarGame.Vehicle.Editor
{
    using Vehicle = Saving.Vehicle;
    public class HistoryManager : MonoBehaviour
    {
        private static List<Vehicle> s_history;
        //Just for debugging
        [SerializeField]
        private List<Vehicle>history = new();
        private static bool s_needUpdate;
        private static Transform s_vehicleRoot;
        public static Part Root;
        //Index to current action
        private static int s_action_index_private;

        public static int ActionIndex
        {
            get => s_action_index_private; 
            set
            {
                if(value < 0)return;

                s_needUpdate = true;
                if (value == s_action_index_private) return;
                //Undo
                if (value < ActionIndex)
                    if (value < s_history.Count - 1)
                    {
                        s_action_index_private = value;
                        Root = VehicleSaver.GenerateVehicle(s_history[ActionIndex], s_vehicleRoot);
                        return;
                    }
                    else return;
                //Redo
                if (value > s_history.Count - 1) return;
                s_action_index_private = value;
                Root = VehicleSaver.GenerateVehicle(s_history[ActionIndex], s_vehicleRoot);
            }
        }

        void Awake()
        {
            s_history = new();
            history = new();
            s_action_index_private = -1;
        }
        void Start()
        {
            Root = FindObjectsOfType<Part>().Where(_ => _.isRoot).FirstOrDefault();
            s_vehicleRoot = Root.transform.parent;
            //Save the initial state 
            ProcessChange();
        }
        void Update()
        {
            if(!s_needUpdate)return;
            s_needUpdate = false;
            Debug.Log("Updating history");
            history = s_history;
        }
        //A function that saves the state of a vehicle
        public static void ProcessChange()
        {
            s_needUpdate = true;
            //If we're in the past clear all actions ahead
            if (ActionIndex < s_history.Count - 1)
                s_history.RemoveRange(ActionIndex + 1, s_history.Count - (ActionIndex + 1));
            s_history.Add(VehicleSaver.SerializeVehicle(Root));
            s_action_index_private++;
        }
        public static void ResetHistory()
        {
            s_action_index_private = -1;
            s_history.Clear();
            ProcessChange();
        }
    }
}