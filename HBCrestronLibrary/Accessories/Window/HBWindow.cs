using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.Window
{
    public delegate void WindowHandler(HBCrestronEventArgs e);

    class Window
    {
        public string Name = string.Empty;
        public string Service = "Window";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;

        public ushort CurrentPosition = 0;
        public ushort RemoteDeviceCurrentPositionIID = 0;

        public ushort PositionState = 0;
        public ushort RemoteDevicePositionStateIID = 0;

        public ushort TargetPosition = 0;
        public ushort RemoteDeviceTargetPositionIID = 0;

        public ushort SupportsHoldPosition = 0;
        public bool HoldPosition = false;
        public ushort RemoteDeviceHoldPositionIID = 0;

        public ushort SupportsObstructionDetected = 0;
        public bool ObstructionDetected = false;
        public ushort RemoteDeviceObstructionDetectedIID = 0;
    }

    class AddWindowPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string Service = "Window";
        public string HoldPosition = "default";
        public string ObstructionDetected = "default";

        [JsonIgnore]
        public ushort SupportsHoldPosition = 0;
        [JsonIgnore]
        public ushort SupportsObstructionDetected = 0;

        public bool ShouldSerializeHoldPosition()
        {
            if (SupportsHoldPosition == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeObstructionDetected()
        {
            if (SupportsObstructionDetected == 1)
            {
                return true;
            }
            return false;
        }
    }

    public static class HBWindow
    {
        public static event WindowHandler WindowEvent;
        private static List<Window> Windows = new List<Window>();
        public static void AddDevice(string name, ushort supportsHoldPosition, ushort supportsObstructionDetected, string deviceLocation)
        {
            Windows.Add(new Window { Name = name, SupportsHoldPosition = supportsHoldPosition, SupportsObstructionDetected = supportsObstructionDetected, DeviceLocation = deviceLocation});
            if (deviceLocation == "Local")
            {
                var tempAddWindowPayload = new AddWindowPayload();
                tempAddWindowPayload.Name = name;
                tempAddWindowPayload.SupportsHoldPosition = supportsHoldPosition;
                tempAddWindowPayload.SupportsObstructionDetected = supportsObstructionDetected;
                var tempAddObject = new AddObject(tempAddWindowPayload);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject)); //Serialize the Data above
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (Windows[Windows.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            Windows.Remove(Windows[Windows.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (Windows.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }
        public static void SetCurrentPositionFeedback(string name, ushort value)
        {
            if (Windows[Windows.FindIndex(i => i.Name == name)].CurrentPosition != value)
            {
                Windows[Windows.FindIndex(i => i.Name == name)].CurrentPosition = value;
                UpdateCurrentPositionFeedback(name);
            }
        }
        public static void SetPositionStateFeedback(string name, ushort value)
        {
            if (Windows[Windows.FindIndex(i => i.Name == name)].PositionState != value)
            {
                Windows[Windows.FindIndex(i => i.Name == name)].PositionState = value;
                UpdatePositionStateFeedback(name);
            }
        }
        public static void SetTargetPositionFeedback(string name, ushort value)
        {
            if (Windows[Windows.FindIndex(i => i.Name == name)].TargetPosition != value)
            {
                Windows[Windows.FindIndex(i => i.Name == name)].TargetPosition = value;
                UpdateTargetPositionFeedback(name);
            }
        }
        public static void SetObstructionDetectedFeedback(string name, ushort value)
        {
            if ((Windows[Windows.FindIndex(i => i.Name == name)].ObstructionDetected && value == 0) || (Windows[Windows.FindIndex(i => i.Name == name)].ObstructionDetected == false && value == 1))
            {
                if (value == 1)
                {
                    Windows[Windows.FindIndex(i => i.Name == name)].ObstructionDetected = true;
                }
                else
                {
                    Windows[Windows.FindIndex(i => i.Name == name)].ObstructionDetected = false;
                }
                UpdateObstructionDetectedFeedback(name);
            }
        }
        public static void UpdateCurrentPositionFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "CurrentPosition";
            tempUpdateValueObject.Payload.Value = Windows[Windows.FindIndex(i => i.Name == name)].CurrentPosition;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdatePositionStateFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "PositionState";
            tempUpdateValueObject.Payload.Value = Windows[Windows.FindIndex(i => i.Name == name)].PositionState;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateTargetPositionFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "TargetPosition";
            tempUpdateValueObject.Payload.Value = Windows[Windows.FindIndex(i => i.Name == name)].TargetPosition;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateObstructionDetectedFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "ObstructionDetected";
            tempUpdateValueObject.Payload.Value = Windows[Windows.FindIndex(i => i.Name == name)].ObstructionDetected;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void ProcessWebsocketResponse(JObject o)
        {
            string topic = (string)o["topic"];
            string name = (string)o["payload"]["name"];
            if (topic == "get")
            {
                string characteristic = (string)o["payload"]["characteristic"];
                switch (characteristic)
                {
                    case "CurrentPosition":
                        UpdateCurrentPositionFeedback(name);
                        break;
                    case "PositionState":
                        UpdatePositionStateFeedback(name);
                        break;
                    case "TargetPosition":
                        UpdateTargetPositionFeedback(name);
                        break;
                    case "ObstructionDetected":
                        UpdateObstructionDetectedFeedback(name);
                        break;
                }
            }
            else if (topic == "set")
            {
                string characteristic = (string)o["payload"]["characteristic"];
                switch (characteristic)
                {
                    case "TargetPosition":
                        ushort targetPositionValue = (ushort)o["payload"]["value"];
                        WindowEvent(new HBCrestronEventArgs(name, characteristic, targetPositionValue));
                        break;
                    case "CurrentPosition":
                        ushort currentPositionValue = (ushort)o["payload"]["value"];
                        WindowEvent(new HBCrestronEventArgs(name, characteristic, currentPositionValue));
                        break;
                    case "PositionState":
                        ushort positionStateValue = (ushort)o["payload"]["value"];
                        WindowEvent(new HBCrestronEventArgs(name, characteristic, positionStateValue));
                        break;
                    case "HoldPosition":
                        bool holdPositionValue = (bool)o["payload"]["value"];
                        if (holdPositionValue == false)
                        {
                            WindowEvent(new HBCrestronEventArgs(name, characteristic, 0));
                        }
                        else
                        {
                            WindowEvent(new HBCrestronEventArgs(name, characteristic, 1));
                        }
                        break;
                    case "ObstructionDetected":
                        bool obstructionDetectedValue = (bool)o["payload"]["value"];
                        if (obstructionDetectedValue == false)
                        {
                            WindowEvent(new HBCrestronEventArgs(name, characteristic, 0));
                        }
                        else
                        {
                            WindowEvent(new HBCrestronEventArgs(name, characteristic, 1));
                        }
                        break;
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (Windows[Windows.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            Windows[Windows.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort)accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string)characteristic["description"])
                    {
                        case "CurrentPosition":
                            Windows[Windows.FindIndex(i => i.Name == name)].RemoteDeviceCurrentPositionIID =
                                (ushort)characteristic["iid"];
                            if (Windows[Windows.FindIndex(i => i.Name == name)].CurrentPosition != (ushort)characteristic["value"])
                            {
                                Windows[Windows.FindIndex(i => i.Name == name)].CurrentPosition = (ushort)characteristic["value"];
                                WindowEvent(new HBCrestronEventArgs(name, "CurrentPosition", (ushort)characteristic["value"]));
                            }
                            break;
                        case "PositionState":
                            Windows[Windows.FindIndex(i => i.Name == name)].RemoteDevicePositionStateIID =
                                (ushort)characteristic["iid"];
                            if (Windows[Windows.FindIndex(i => i.Name == name)].PositionState != (ushort)characteristic["value"])
                            {
                                Windows[Windows.FindIndex(i => i.Name == name)].PositionState = (ushort)characteristic["value"];
                                WindowEvent(new HBCrestronEventArgs(name, "PositionState", (ushort)characteristic["value"]));
                            }
                            break;
                        case "HoldPosition":
                            Windows[Windows.FindIndex(i => i.Name == name)].RemoteDeviceHoldPositionIID =
                                (ushort)characteristic["iid"];

                            if (Windows[Windows.FindIndex(i => i.Name == name)].HoldPosition != (bool)characteristic["value"])
                            {
                                Windows[Windows.FindIndex(i => i.Name == name)].HoldPosition = (bool)characteristic["value"];
                                WindowEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "HoldPosition", 1)
                                    : new HBCrestronEventArgs(name, "HoldPosition", 0));
                            }
                            break;
                        case "ObstructionDetected":
                            Windows[Windows.FindIndex(i => i.Name == name)].RemoteDeviceObstructionDetectedIID =
                                (ushort)characteristic["iid"];

                            if (Windows[Windows.FindIndex(i => i.Name == name)].ObstructionDetected != (bool)characteristic["value"])
                            {
                                Windows[Windows.FindIndex(i => i.Name == name)].ObstructionDetected = (bool)characteristic["value"];
                                WindowEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "ObstructionDetected", 1)
                                    : new HBCrestronEventArgs(name, "ObstructionDetected", 0));
                            }
                            break;
                    }
                }
            }
        }
    }
}