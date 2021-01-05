# HL2-Scan-Demo
 Application to compare different scanning settings on the HoloLens 2.
 
 [See demo on YouTube here.](https://youtu.be/HznZ4sEB-UQ)

## Overview
This application manipulates the Spatial Awareness System and Mesh Observers in the Mixed Reality Tookit to explore the scanning capabilities of the HoloLens 2.

## Components
Unity v2019.4.17f1
MRTK v2.5.3

## Installation
Follow the download and installation instructions on the [Mixed Reality Toolkit github page](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases). 

Build settings are as pictured:
![HL2 Scan Demo Build Settings. Any device, ARM64, D3D Project](https://i.imgur.com/UE7ALqr.png)

Build, connect your HoloLens 2 via USB, then open the solution and select Configuration: Debug, Platform: ARM64, and Target: Device. Select "Start Without Debugging" and it should start on the HoloLens shortly.

Note: *Make sure that the MRTK configuration profile is based on the DefaultXRSDKConfigurationProfile, not MixedRealityToolkitConfigurationProfile.*  Otherwise, the app will launch in 2D. The "MixedRealityToolkitConfigurationProfile Custom 2" that I set up (and other "Custom 2" profiles) take this into account.

## Function

### Properties
The application uses the Scan Manager script to manipulate the following properties of the Spatial Mesh Observer:
- Update Interval - how frequently the scanner checks for updates, in seconds
- Level of Detail - what level of detail the scanner looks for. This can be set to Coarse, Medium, Fine, or Custom.
- Triangles per Cubic Meter - if the level of detail is set to custom, how many triangles per cubic meter to construct the mesh from. Higher = more detail. Typical numbers are Coarse - 100, Medium - 750, Fine - 2000. 
- Observation Extents - how far away from the user the observer tries to scan, in meters. 

### Modes
The application allows the following modes:

**Default**
The default scanning settings.
- Update Interval: 3.5
- Level of Detail: Coarse
- Observation extents: 3, 3, 3

**Quick**
Scans as quickly as possible.
- Update Interval: 0.1
- Level of Detail: Coarse
- Observation extents: 3, 3, 3

**Fine**
Scans at a fine level of detail.
- Update Interval: 3.5
- Level of Detail: Fine
- Observation extents: 3, 3, 3

**Max**
An experimental setting that seeks to max out scanning capabilities.
- Update Interval: 0.1
- Level of Detail: Custom
- Triangles per cubic meter: 3000
- Observation extents: 10, 10, 10

### Interface
The app starts with the scanner off and Default Mode selected. To start scanning, use the Start Scan button. To select a new mode, use one of the mode buttons on the right. The old mode must be stopped before the new mode can begin.

![HL2 Scan Demo Interface. Informs user of Current Mode and Selected Mode, and allows them to Start or Clear Scan, or switch to Default, Quick, Fine, or Max modes.](https://i.imgur.com/YF1a5e0.png)

Note that even though Clear Scan uses the Spatial Awareness' Clear Observations method, it will retain memories from the previous scan. To avoid this, you'll have to clear scan, exit the app, and use the Clear Environments or Clear Holograms command in Settings to delete local environment data.