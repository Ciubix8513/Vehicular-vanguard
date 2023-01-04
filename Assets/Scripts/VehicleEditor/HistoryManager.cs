using UnityEngine;
using CarGame.Vehicle.Saving;
using System.Collections.Generic;
using System.Linq;

namespace CarGame.Vehicle.Editor
{
    public class HistoryManager : MonoBehaviour
    {
        private static List<HistoryVehicle> s_history;
        //Just for debugging
        [SerializeField]
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
                if (value == s_action_index_private) return;
                //Undo
                if (value < ActionIndex)
                    if (value < s_history.Count - 1)
                    {
                        s_action_index_private = value;
                        Generate(ActionIndex);
                        return;
                    }
                    else return;
                //Redo
                if (value > s_history.Count - 1) return;
                s_action_index_private = value;
                Generate(ActionIndex);
            }
        }
        static void Generate(int index)
        {
            print("Trying to generate for " + index + " length = " + s_history.Count);
            var res = VehicleSaver.GenerateHistoryVehicle(s_history[index], s_vehicleRoot);
            Root = res.Item1;
            if(s_history[index].EditorMode != EditorMode.input)return;
            UIManager.ActivateTab(1);
            InputMenu.s_this.OnMenuOpen();
            InputMenu.s_this.LoadData(res.Item2);
        }

        void Awake()
        {
            s_history = new();
            s_action_index_private = -1;
        }
        void Start()
        {
            Root = FindObjectsOfType<Part>().Where(_ => _.isRoot).FirstOrDefault();
            s_vehicleRoot = Root.transform.parent;
            //Save the initial state 
            ProcessChange();
        }
        //A function that saves the state of a vehicle
        public static void ProcessChange()
        {
            //If we're in the past clear all actions ahead
            if (ActionIndex < s_history.Count - 1)
                s_history.RemoveRange(ActionIndex + 1, s_history.Count - (ActionIndex + 1));
            s_history.Add(VehicleSaver.SerializeHistoryVehicle(Root));
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