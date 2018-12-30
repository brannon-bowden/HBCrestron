using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.GarageDoor
{
    public delegate void GarageDoorHandler(HBCrestronEventArgs e);
    
    class GarageDoor
    {
        public string Name = string.Empty;
        public string Service = "GarageDoorOpener";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;

        public ushort CurrentDoorState = 0;
        public ushort RemoteDeviceCurrentDoorStateIID = 0;

        public ushort TargetDoorState = 0;
        public ushort RemoteDeviceTargetDoorStateIID = 0;

        public bool ObstructionDetected = false;
        public ushort RemoteDeviceObstructionDetectedIID = 0;
    }

    class AddGarageDoorPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string service = "GarageDoorOpener";
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

    public static class HBGarageDoor
    {
        public static event GarageDoorHandler GarageDoorEvent;
        private static List<GarageDoor> GarageDoors = new List<GarageDoor>();
        public static void AddDevice(string name, ushort supportsHoldPosition, ushort supportsObstructionDetected, string deviceLocation)
        {
            GarageDoors.Add(new GarageDoor { Name = name,DeviceLocation = deviceLocation});
            if (deviceLocation == "Local")
            {
                var tempAddGarageDoorObject = new AddGarageDoorPayload();
                tempAddGarageDoorObject.Name = name;
                tempAddGarageDoorObject.SupportsHoldPosition = supportsHoldPosition;
                tempAddGarageDoorObject.SupportsObstructionDetected = supportsObstructionDetected;
                var tempAddObject = new AddObject(tempAddGarageDoorObject);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject)); //Serialize the Data above
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            GarageDoors.Remove(GarageDoors[GarageDoors.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (GarageDoors.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }
        public static void SetCurrentDoorStateFeedback(string name, ushort value)
        {
            if (GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].CurrentDoorState != value)
            {
                GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].CurrentDoorState = value;
                UpdateCurrentDoorStateFeedback(name);
            }
        }
        public static void SetTargetDoorStateFeedback(string name, ushort value)
        {
            if (GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].TargetDoorState != value)
            {
                GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].TargetDoorState = value;
                UpdateTargetDoorStateFeedback(name);
            }
        }
        public static void SetObstructionDetectedFeedback(string name, ushort value)
        {
            if ((GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].ObstructionDetected && value == 0) || (GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].ObstructionDetected == false && value == 1))
            {
                if (value == 1)
                {
                    GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].ObstructionDetected = true;
                }
                else
                {
                    GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].ObstructionDetected = false;
                }
                UpdateObstructionDetectedFeedback(name);
            }
        }
        public static void UpdateCurrentDoorStateFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "CurrentDoorState";
            tempUpdateValueObject.Payload.Value = GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].CurrentDoorState;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateTargetDoorStateFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "TargetDoorState";
            tempUpdateValueObject.Payload.Value = GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].TargetDoorState;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateObstructionDetectedFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "ObstructionDetected";
            tempUpdateValueObject.Payload.Value = GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].ObstructionDetected;
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
                    case "CurrentDoorState":
                        UpdateCurrentDoorStateFeedback(name);
                        break;
                    case "TargetDoorState":
                        UpdateTargetDoorStateFeedback(name);
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
                    case "TargetDoorState":
                        ushort targetPositionValue = (ushort)o["payload"]["value"];
                        GarageDoorEvent(new HBCrestronEventArgs(name, characteristic, targetPositionValue));
                        break;
                    case "CurrentDoorState":
                        ushort currentPositionValue = (ushort)o["payload"]["value"];
                        GarageDoorEvent(new HBCrestronEventArgs(name, characteristic, currentPositionValue));
                        break;
                    case "ObstructionDetected":
                        bool obstructionDetectedValue = (bool)o["payload"]["value"];
                        if (obstructionDetectedValue == false)
                        {
                            GarageDoorEvent(new HBCrestronEventArgs(name, characteristic, 0));
                        }
                        else
                        {
                            GarageDoorEvent(new HBCrestronEventArgs(name, characteristic, 1));
                        }
                        break;
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort)accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string)characteristic["description"])
                    {
                        case "CurrentDoorState":
                            GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].RemoteDeviceCurrentDoorStateIID =
                                (ushort)characteristic["iid"];
                            if (GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].CurrentDoorState != (ushort)characteristic["value"])
                            {
                                GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].CurrentDoorState = (ushort)characteristic["value"];
                                GarageDoorEvent(new HBCrestronEventArgs(name, "CurrentDoorState", (ushort)characteristic["value"]));
                            }
                            break;
                        case "ObstructionDetected":
                            GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].RemoteDeviceObstructionDetectedIID =
                                (ushort)characteristic["iid"];

                            if (GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].ObstructionDetected != (bool)characteristic["value"])
                            {
                                GarageDoors[GarageDoors.FindIndex(i => i.Name == name)].ObstructionDetected = (bool)characteristic["value"];
                                GarageDoorEvent((bool)characteristic["value"]
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