using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Microsoft.MixedReality.Toolkit { 
public class ScanManager : MonoBehaviour
{
        /// <summary>
        /// Controls the scanning modes and display panel.
        /// </summary>

        public GameObject scanPanel;
        public GameObject currentModeTextObject;
        public GameObject selectedModeTextObject;

        private bool isScanning = true;
        private string currentMode = "Default";

        private float updateInterval = 3.5f;
        private float updateIntervalNext;

        private SpatialAwarenessMeshLevelOfDetail levelOfDetail = SpatialAwarenessMeshLevelOfDetail.Coarse;
        private SpatialAwarenessMeshLevelOfDetail levelOfDetailNext;

        private int trianglesPerCubicMeter = 0;
        private int trianglesPerCubicMeterNext;

        private Vector3 observationExtents = new Vector3(3f, 3f, 3f);
        private Vector3 observationExtentsNext;

        public void ClearScan()
        {
            //Clears observer and stops scanning.
            if (isScanning == false)
            {
                Debug.Log("Scan has already stopped.");
            }
            
            else
            {
                var spatialAwarenessSystem = CoreServices.SpatialAwarenessSystem;
                if (spatialAwarenessSystem != null)
                {
                    Debug.Log("Clearing scan.");
                    spatialAwarenessSystem.SuspendObservers();
                    spatialAwarenessSystem.ClearObservations();
                    isScanning = false;

                    //Adjust Current Mode text
                    //currentModeTextObject.GetComponent<TextMesh>().text = "<size=\"32\"><b>Off</b></size>";
                    currentModeTextObject.GetComponent<TMP_Text>().text = "<size=\"32\"><b>Off</b></size>";

                }

            }

        }

        public void StartScan()
        {

            //Starts scan in currently selected mode. Only works if system has been cleared.
            if (isScanning == false)
            {
                Debug.Log("Starting scan in " + currentMode + " mode.");

                var spatialAwarenessSystem = CoreServices.SpatialAwarenessSystem;
                if (spatialAwarenessSystem != null)
                {
                    //Access mesh observer
                    var dataProviderAccess = spatialAwarenessSystem as IMixedRealityDataProviderAccess;
                    var meshObserver = dataProviderAccess.GetDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();


                    //Set mesh observer qualities
                    meshObserver.UpdateInterval = updateIntervalNext;
                    meshObserver.LevelOfDetail = levelOfDetailNext;

                    if (meshObserver.LevelOfDetail == SpatialAwarenessMeshLevelOfDetail.Custom)
                    {
                        meshObserver.TrianglesPerCubicMeter = trianglesPerCubicMeterNext;
                    }

                    meshObserver.ObservationExtents = observationExtentsNext;


                    //Update Current Mode panel text to match Selected Mode text
                    currentModeTextObject.GetComponent<TMP_Text>().text = selectedModeTextObject.GetComponent<TMP_Text>().text;

                    //Update current mode variables
                    updateInterval = updateIntervalNext;
                    levelOfDetail = levelOfDetailNext;
                    trianglesPerCubicMeter = trianglesPerCubicMeterNext;
                    observationExtents = observationExtentsNext;


                    //Resume observers
                    spatialAwarenessSystem.ResumeObservers();
                    isScanning = true;
                }

            }

            else Debug.Log("Can't start a new scan without clearing the old one.");
        }

        public void SelectMode(string selectedMode)
        {
            //Sets parameters for next mode based on given mode type.
            if (selectedMode == "Default")
            {
                currentMode = "Default";
                updateIntervalNext = 3.5f;
                levelOfDetailNext = SpatialAwarenessMeshLevelOfDetail.Coarse;
                observationExtentsNext = new Vector3(3f, 3f, 3f);

                Debug.Log("Selected mode: Default");
            }

            else if (selectedMode == "Quick")
            {
                currentMode = "Quick";
                updateIntervalNext = 0.1f;
                levelOfDetailNext = SpatialAwarenessMeshLevelOfDetail.Coarse;
                observationExtentsNext = new Vector3(3f, 3f, 3f);
                Debug.Log("Selected mode: Quick");

            }


            else if (selectedMode == "Fine")
            {
                currentMode = "Fine";
                updateIntervalNext = 3.5f;
                levelOfDetailNext = SpatialAwarenessMeshLevelOfDetail.Fine;
                observationExtentsNext = new Vector3(3f, 3f, 3f);
                Debug.Log("Selected mode: Fine");

            }

            else if (selectedMode == "Max")
            {
                currentMode = "Max";
                updateIntervalNext = 0.1f;
                levelOfDetailNext = SpatialAwarenessMeshLevelOfDetail.Custom;
                trianglesPerCubicMeterNext = 3000;
                observationExtentsNext = new Vector3(10f, 10f, 10f);
                Debug.Log("Selected mode: Max");

            }

            //Adjust panel text
            //string selectedPanelText = selectedModeTextObject.GetComponent<TextMesh>().text;

            Debug.Log("Adjusting Selected Mode panel text to " + selectedMode + " mode.");

            string selectedPanelText = "<size=\"32\"><b>" + selectedMode + "</b></size> \n" + updateIntervalNext.ToString() + "\n" + levelOfDetailNext.ToString();
            if (levelOfDetailNext == SpatialAwarenessMeshLevelOfDetail.Custom)
            {
                selectedPanelText += "\n" + trianglesPerCubicMeterNext.ToString();
            }
            else
            {
                selectedPanelText += "\nN/A";
            }
            selectedPanelText += "\n" + observationExtentsNext.ToString();

            selectedModeTextObject.GetComponent<TMP_Text>().text = selectedPanelText;
        }


        // Start is called before the first frame update
        void Start()
    {

            //Clears scan
            ClearScan();

            //Sets Default as next mode
            SelectMode("Default");
    }

        // Update is called once per frame
        void Update()
        {
            //Log current scan manager settings for debugging 
            
            /*
            var spatialAwarenessSystem = CoreServices.SpatialAwarenessSystem;

            if (isScanning)
            {
                Debug.Log("Scanning.");

                if (spatialAwarenessSystem != null)
                {
                    //Access mesh observer
                    var dataProviderAccess = spatialAwarenessSystem as IMixedRealityDataProviderAccess;
                    var meshObserver = dataProviderAccess.GetDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();

                    Debug.Log("Update interval: " + meshObserver.UpdateInterval.ToString());
                    Debug.Log("Level of Detail: " + meshObserver.LevelOfDetail.ToString());
                    Debug.Log("Triangles per cubic meter: " + meshObserver.TrianglesPerCubicMeter.ToString());
                    Debug.Log("Observer extents: " + meshObserver.ObservationExtents.ToString());

                    Debug.Log("Observer location: " + meshObserver.ObserverOrigin.ToString());
                    Debug.Log("Observer is stationary: " + meshObserver.IsStationaryObserver);
                    Debug.Log("Observer type: " + meshObserver.GetType().ToString());
                }
            }

            else
            {
                Debug.Log("Scanning stopped.");
            }
            */

        }
}

}