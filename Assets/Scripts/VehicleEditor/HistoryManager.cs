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
        private static Transform s_vehicleRoot;
        public static Part Root;
        //Index to current action
        private static int s_action_index_private;

        public static int s_action_index
        {
            get => s_action_index_private; set
            {
                if (value == s_action_index_private) return;
                //Undo
                if (value > s_action_index)
                    if (value < s_history.Count - 1)
                    {
                        s_action_index_private = value;
                        VehicleSaver.GenerateVehicle(s_history[s_action_index], s_vehicleRoot);
                        return;
                    }
                    else return;
                //Redo
                if (value > s_history.Count - 1) return;
                s_action_index_private = value;
                VehicleSaver.GenerateVehicle(s_history[s_action_index], s_vehicleRoot);
            }
        }

        void Awake()
        {
            s_history = new();
            s_action_index = -1;
        }
        void Start()
        {
            Root = FindObjectsOfType<Part>().Where(_ => _.isRoot).FirstOrDefault();
            s_vehicleRoot = Root.transform.parent;
        }
        //A function that saves the state of a vehicle
        public static void ProcessChange()
        {
            //If we're in the past clear all actions ahead
            if (s_action_index < s_history.Count - 1)
                s_history.RemoveRange(s_action_index + 1, s_history.Count - s_action_index + 1);
            s_history.Add(VehicleSaver.SerializeVehicle(Root));
        }
    }
}