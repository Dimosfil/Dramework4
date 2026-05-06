using IG.HappyCoder.Plugins.Dramework4.Runtime.Behaviours;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Requests;
using IG.HappyCoder.Plugins.Dramework4.Runtime.Messaging.Signals;

using Sirenix.OdinInspector;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Tests
{
    public class Tester : DBehaviour
    {
        #region ================================ FIELDS

        private int _counter;

        #endregion

        #region ================================ METHODS

        private void Start()
        {
#if UNITY_EDITOR
            Selection.activeGameObject = gameObject;
#endif
        }

        [Button(ButtonSizes.Medium)]
        private void TestRequests()
        {
            DWRequest.Subscribe("Test", () => "Test");
            DWRequest.Subscribe<int, int>("Test 1", value => value + 5);
            DWRequest.Subscribe<int, string>("Test 2", value => $"Value:{value}");

            Debug.Log($"Value = {DWRequest.Fire<string>("Test")}");
            Debug.Log($"Value = {DWRequest.Fire<int, int>("Test 1", 8)}");
            Debug.Log($"Value = {DWRequest.Fire<int, string>("Test 2", 10)}");
        }

        [Button(ButtonSizes.Medium)]
        private void TestSignals()
        {
            DWSignal.Subscribe<string>(s => { Debug.Log($"First = {s}_{2}"); }, 2);
            DWSignal.Subscribe<string>(s => { Debug.Log($"First = {s}_{1}"); }, 3);
            DWSignal.Subscribe<string>(s => { Debug.Log($"First = {s}_{3}"); }, 1);
            
            DWSignal.Subscribe<string, int>((s,  i) => { Debug.Log($"First = {s}_2. Second = {i}_2"); }, 3);
            DWSignal.Subscribe<string, int>((s,  i) => { Debug.Log($"First = {s}_2. Second = {i}_1"); }, 1);
            DWSignal.Subscribe<string, int>((s,  i) => { Debug.Log($"First = {s}_2. Second = {i}_3"); }, 2);

            // DWSignal.Subscribe<int>("Test", s => { Debug.Log($"Test = {s}"); });
            // DWSignal.Subscribe("No", () => Debug.Log("No"));

            DWSignal.Fire<string>("First");
            DWSignal.Fire<string, int>("First", 10);
            // DWSignal.Fire("Test", 987);
            // DWSignal.Fire("No");
        }
        
        [Button(ButtonSizes.Medium)]
        private void TestAsyncSignals()
        {
            DWSignalAsync.Subscribe<int>("First Async", num => Debug.LogError($"First Async {num}"));

            DWSignalAsync.Fire("First Async", 1);
            DWSignalAsync.Fire("First Async", 3);
            DWSignalAsync.Fire("First Async", 2);
        }

        #endregion
    }
}