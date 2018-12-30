namespace HBCrestronLibrary.Common;
        // class declarations
         class HBCrestronEventArgs;
     class HBCrestronEventArgs 
    {
        // class delegates

        // class events

        // class functions
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING Name[];
        STRING Characteristic[];
        INTEGER Value;
    };

namespace HBCrestronLibrary.Accessories.Switch;
        // class declarations
         class HBSwitch;
    static class HBSwitch 
    {
        // class delegates

        // class events
        static EventHandler SwitchEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetOn ( STRING name , INTEGER value );
        static FUNCTION SetOnFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateOnFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.Fan;
        // class declarations
         class HBFan;
    static class HBFan 
    {
        // class delegates

        // class events
        static EventHandler FanEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , INTEGER supportsRotationSpeed , INTEGER supportsRotationDirection , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetOn ( STRING name , INTEGER value );
        static FUNCTION SetOnFeedback ( STRING name , INTEGER value );
        static FUNCTION SetRotationSpeedFeedback ( STRING name , INTEGER value );
        static FUNCTION SetRotationDirectionFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateOnFeedback ( STRING name );
        static FUNCTION UpdateRotationSpeedFeedback ( STRING name );
        static FUNCTION UpdateRotationDirectionFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.Outlet;
        // class declarations
         class HBOutlet;
    static class HBOutlet 
    {
        // class delegates

        // class events
        static EventHandler OutletEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetOn ( STRING name , INTEGER value );
        static FUNCTION SetOnFeedback ( STRING name , INTEGER value );
        static FUNCTION SetOutletInUseFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateOnFeedback ( STRING name );
        static FUNCTION UpdateOutletInUseFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.SecuritySystem;
        // class declarations
         class HBSecuritySystem;
    static class HBSecuritySystem 
    {
        // class delegates

        // class events
        static EventHandler SecuritySystemEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , INTEGER supportsStatusFault , INTEGER supportsStatusTampered , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetSecuritySystemTargetState ( STRING name , INTEGER value );
        static FUNCTION SetSecuritySystemCurrentState ( STRING name , INTEGER value );
        static FUNCTION SetSecuritySystemAlarmType ( STRING name , INTEGER value );
        static FUNCTION SetStatusFaultFeedback ( STRING name , INTEGER value );
        static FUNCTION SetStatusTamperedFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateSecuritySystemTargetState ( STRING name );
        static FUNCTION UpdateSecuritySystemCurrentState ( STRING name );
        static FUNCTION UpdateSecuritySystemAlarmType ( STRING name );
        static FUNCTION UpdateStatusFaultFeedback ( STRING name );
        static FUNCTION UpdateStatusTamperedFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.Lightbulb;
        // class declarations
         class HBLightbulb;
    static class HBLightbulb 
    {
        // class delegates

        // class events
        static EventHandler LightbulbEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , INTEGER supportsBrightness , INTEGER supportsHue , INTEGER supportsSaturation , INTEGER supportsColorTemperature , INTEGER supportsRGB , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetOn ( STRING name , INTEGER value );
        static FUNCTION SetOnFeedback ( STRING name , INTEGER value );
        static FUNCTION SetBrightness ( STRING name , INTEGER value );
        static FUNCTION SetBrightnessFeedback ( STRING name , INTEGER value );
        static FUNCTION SetHue ( STRING name , INTEGER value );
        static FUNCTION SetHueFeedback ( STRING name , INTEGER value );
        static FUNCTION SetSaturation ( STRING name , INTEGER value );
        static FUNCTION SetSaturationFeedback ( STRING name , INTEGER value );
        static FUNCTION SetColorTemperature ( STRING name , INTEGER value );
        static FUNCTION SetColorTemperatureFeedback ( STRING name , INTEGER value );
        static FUNCTION SetRGB ( STRING name , INTEGER red , INTEGER green , INTEGER blue );
        static FUNCTION SetRGBFeedback ( STRING name , INTEGER red , INTEGER green , INTEGER blue );
        static FUNCTION UpdateOnFeedback ( STRING name );
        static FUNCTION UpdateBrightnessFeedback ( STRING name );
        static FUNCTION UpdateHueFeedback ( STRING name );
        static FUNCTION UpdateSaturationFeedback ( STRING name );
        static FUNCTION UpdateColorTemperatureFeedback ( STRING name );
        static FUNCTION ConvertHSVtoRGB ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.Window;
        // class declarations
         class HBWindow;
    static class HBWindow 
    {
        // class delegates

        // class events
        static EventHandler WindowEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , INTEGER supportsHoldPosition , INTEGER supportsObstructionDetected , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetCurrentPositionFeedback ( STRING name , INTEGER value );
        static FUNCTION SetPositionStateFeedback ( STRING name , INTEGER value );
        static FUNCTION SetTargetPositionFeedback ( STRING name , INTEGER value );
        static FUNCTION SetObstructionDetectedFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateCurrentPositionFeedback ( STRING name );
        static FUNCTION UpdatePositionStateFeedback ( STRING name );
        static FUNCTION UpdateTargetPositionFeedback ( STRING name );
        static FUNCTION UpdateObstructionDetectedFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary;
        // class declarations
         class HBCrestron;
    static class HBCrestron 
    {
        // class delegates

        // class events
        static EventHandler HBWebSocketClientConnectionEvent ( EventArgs e );

        // class functions
        static FUNCTION hbWebSocketClient_Connect ( STRING url , INTEGER port );
        static FUNCTION SendWebSocketData ( STRING jsondata );
        static FUNCTION hbHttpClient_Connect ( STRING ip , INTEGER port , STRING code );
        static STRING_FUNCTION hbHttpClient_SendRequest ( STRING page , STRING parameters , STRING requestContent );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.Thermostat;
        // class declarations
         class HBThermostat;
    static class HBThermostat 
    {
        // class delegates

        // class events
        static EventHandler ThermostatEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , INTEGER supportsCurrentRelativeHumidity , INTEGER supportsTargetRelativeHumidity , INTEGER supportsCoolingThresholdTemperature , INTEGER supportsHeatingThresholdTemperature , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetTargetHeatingCoolingState ( STRING name , INTEGER value );
        static FUNCTION SetCurrentHeatingCoolingStateFeedback ( STRING name , INTEGER value );
        static FUNCTION SetTargetTemperature ( STRING name , INTEGER value );
        static FUNCTION SetCurrentTemperatureFeedback ( STRING name , INTEGER value );
        static FUNCTION SetTemperatureDisplayUnitsFeedback ( STRING name , INTEGER value );
        static FUNCTION SetTargetRelativeHumidityFeedback ( STRING name , INTEGER value );
        static FUNCTION SetCurrentRelativeHumidityFeedback ( STRING name , INTEGER value );
        static FUNCTION SetHeatingThresholdTemperatureFeedback ( STRING name , INTEGER value );
        static FUNCTION SetCoolingThresholdTemperatureFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateTargetHeatingCoolingStateFeedback ( STRING name );
        static FUNCTION UpdateCurrentHeatingCoolingStateFeedback ( STRING name );
        static FUNCTION UpdateTargetTemperatureFeedback ( STRING name );
        static FUNCTION UpdateCurrentTemperatureFeedback ( STRING name );
        static FUNCTION UpdateTemperatureDisplayUnitsFeedback ( STRING name );
        static FUNCTION UpdateTargetRelativeHumidityFeedback ( STRING name );
        static FUNCTION UpdateCurrentRelativeHumidityFeedback ( STRING name );
        static FUNCTION UpdateHeatingThresholdTemperatureFeedback ( STRING name );
        static FUNCTION UpdateCoolingThresholdTemperatureFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.GarageDoor;
        // class declarations
         class HBGarageDoor;
    static class HBGarageDoor 
    {
        // class delegates

        // class events
        static EventHandler GarageDoorEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , INTEGER supportsHoldPosition , INTEGER supportsObstructionDetected , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetCurrentDoorStateFeedback ( STRING name , INTEGER value );
        static FUNCTION SetTargetDoorStateFeedback ( STRING name , INTEGER value );
        static FUNCTION SetObstructionDetectedFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateCurrentDoorStateFeedback ( STRING name );
        static FUNCTION UpdateTargetDoorStateFeedback ( STRING name );
        static FUNCTION UpdateObstructionDetectedFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.Door;
        // class declarations
         class HBDoor;
    static class HBDoor 
    {
        // class delegates

        // class events
        static EventHandler DoorEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , INTEGER supportsHoldPosition , INTEGER supportsObstructionDetected , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetCurrentPositionFeedback ( STRING name , INTEGER value );
        static FUNCTION SetPositionStateFeedback ( STRING name , INTEGER value );
        static FUNCTION SetTargetPositionFeedback ( STRING name , INTEGER value );
        static FUNCTION SetObstructionDetectedFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateCurrentPositionFeedback ( STRING name );
        static FUNCTION UpdatePositionStateFeedback ( STRING name );
        static FUNCTION UpdateTargetPositionFeedback ( STRING name );
        static FUNCTION UpdateObstructionDetectedFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

namespace HBCrestronLibrary.Accessories.OccupancySensor;
        // class declarations
         class HBOccupancySensor;
    static class HBOccupancySensor 
    {
        // class delegates

        // class events
        static EventHandler OccupancySensorEvent ( HBCrestronEventArgs e );

        // class functions
        static FUNCTION AddDevice ( STRING name , INTEGER supportsStatusActive , INTEGER supportsStatusFault , INTEGER supportsStatusTampered , INTEGER supportsStatusLowBattery , STRING deviceLocation );
        static FUNCTION RemoveDevice ( STRING name );
        static INTEGER_FUNCTION CheckIfDeviceExists ( STRING name );
        static FUNCTION SetOccupancyDetectedFeedback ( STRING name , INTEGER value );
        static FUNCTION SetStatusActiveFeedback ( STRING name , INTEGER value );
        static FUNCTION SetStatusFaultFeedback ( STRING name , INTEGER value );
        static FUNCTION SetStatusTamperedFeedback ( STRING name , INTEGER value );
        static FUNCTION SetStatusLowBatteryFeedback ( STRING name , INTEGER value );
        static FUNCTION UpdateOccupancyDetectedFeedback ( STRING name );
        static FUNCTION UpdateStatusActiveFeedback ( STRING name );
        static FUNCTION UpdateStatusFaultFeedback ( STRING name );
        static FUNCTION UpdateStatusTamperedFeedback ( STRING name );
        static FUNCTION UpdateStatusLowBatteryFeedback ( STRING name );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

