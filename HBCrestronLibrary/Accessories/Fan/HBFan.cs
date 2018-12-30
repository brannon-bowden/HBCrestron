using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.Fan
{
    public delegate void FanHandler(HBCrestronEventArgs e);

    class Fan
    {
        public string Name = string.Empty;
        public string Service = "Fan";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;

        public bool On = false;
        public ushort RemoteDeviceOnIID = 0;

        public ushort SupportsRotationSpeed = 0;
        public ushort RotationSpeed = 0;
        public ushort RemoteDeviceRotationSpeedIID = 0;

        public ushort SupportsRotationDirection = 0;
        public ushort RotationDirection = 0;
        public ushort RemoteDeviceRotationDirectionIID = 0;
    }

    class AddFanPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string service = "Fan";
        public string RotationSpeed = "default";
        public string RotationDirection = "default";

        [JsonIgnore]
        public ushort SupportsRotationSpeed = 0;
        [JsonIgnore]
        public ushort SupportsRotationDirection = 0;

        public bool ShouldSerializeRotationSpeed()
        {
            if (SupportsRotationSpeed == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeRotationDirection()
        {
            if (SupportsRotationDirection == 1)
            {
                return true;
            }
            return false;
        }
    }

    public static class HBFan
    {
        public static event FanHandler FanEvent;
        private static List<Fan> Fans = new List<Fan>();
        public static void AddDevice(string name, ushort supportsRotationSpeed, ushort supportsRotationDirection, string deviceLocation)
        {
            Fans.Add(new Fan { Name = name, SupportsRotationSpeed = supportsRotationSpeed, SupportsRotationDirection = supportsRotationDirection, DeviceLocation = deviceLocation});
            if (deviceLocation == "Local")
            {
                var tempAddFanPayload = new AddFanPayload();
                tempAddFanPayload.Name = name;
                tempAddFanPayload.SupportsRotationSpeed = supportsRotationSpeed;
                tempAddFanPayload.SupportsRotationDirection = supportsRotationDirection;
                var tempAddObject = new AddObject(tempAddFanPayload);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject)); //Serialize the Data above
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (Fans[Fans.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            Fans.Remove(Fans[Fans.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (Fans.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }
        public static void SetOn(string name, ushort value)
        {
            ControlBoolValueObject tempControlBoolValueObject = new ControlBoolValueObject();
            tempControlBoolValueObject.Characteristics[0] = new ControlBoolValuePayload();
            tempControlBoolValueObject.Characteristics[0].AID = Fans[Fans.FindIndex(i => i.Name == name)].RemoteDeviceAID;
            tempControlBoolValueObject.Characteristics[0].IID = Fans[Fans.FindIndex(i => i.Name == name)].RemoteDeviceOnIID;
            if (value == 1)
                tempControlBoolValueObject.Characteristics[0].Value = true;
            var stringToSend = JsonConvert.SerializeObject(tempControlBoolValueObject);
            HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
        }
        public static void SetOnFeedback(string name, ushort value)
        {
            if ((Fans[Fans.FindIndex(i => i.Name == name)].On && value == 0) || (Fans[Fans.FindIndex(i => i.Name == name)].On == false && value == 1))
            {
                if (value == 1)
                {
                    Fans[Fans.FindIndex(i => i.Name == name)].On = true;
                }
                else
                {
                    Fans[Fans.FindIndex(i => i.Name == name)].On = false;
                }
                UpdateOnFeedback(name);
            }
        }
        public static void SetOnFeedback(string name, bool value)
        {
            if (Fans[Fans.FindIndex(i => i.Name == name)].On != value)
            {
                Fans[Fans.FindIndex(i => i.Name == name)].On = value;
                UpdateOnFeedback(name);
            }
        }
        public static void SetRotationSpeedFeedback(string name, ushort value)
        {
            if (Fans[Fans.FindIndex(i => i.Name == name)].SupportsRotationSpeed == 1 && Fans[Fans.FindIndex(i => i.Name == name)].RotationSpeed != value)
            {
                Fans[Fans.FindIndex(i => i.Name == name)].RotationSpeed = value;
                UpdateRotationSpeedFeedback(name);
            }
        }
        public static void SetRotationDirectionFeedback(string name, ushort value)
        {
            if (Fans[Fans.FindIndex(i => i.Name == name)].SupportsRotationDirection == 1 && Fans[Fans.FindIndex(i => i.Name == name)].RotationDirection != value)
            {
                Fans[Fans.FindIndex(i => i.Name == name)].RotationDirection = value;
                UpdateRotationDirectionFeedback(name);
            }
        }        
        public static void UpdateOnFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "On";
            tempUpdateValueObject.Payload.Value = Fans[Fans.FindIndex(i => i.Name == name)].On;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateRotationSpeedFeedback(string name)
        {
            if (Fans[Fans.FindIndex(i => i.Name == name)].SupportsRotationSpeed == 1)
            {
                var tempUpdateValueObject = new SetIntValueObject();
                tempUpdateValueObject.Payload.Name = name;
                tempUpdateValueObject.Payload.Characteristic = "RotationSpeed";
                tempUpdateValueObject.Payload.Value = Fans[Fans.FindIndex(i => i.Name == name)].RotationSpeed;
                var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
                HBCrestron.SendWebSocketData(stringToSend);
            }
        }
        public static void UpdateRotationDirectionFeedback(string name)
        {
            if (Fans[Fans.FindIndex(i => i.Name == name)].SupportsRotationDirection == 1)
            {
                var tempUpdateValueObject = new SetIntValueObject();
                tempUpdateValueObject.Payload.Name = name;
                tempUpdateValueObject.Payload.Characteristic = "RotationDirection";
                tempUpdateValueObject.Payload.Value = Fans[Fans.FindIndex(i => i.Name == name)].RotationDirection;
                var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
                HBCrestron.SendWebSocketData(stringToSend);
            }
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
                    case "On":
                        UpdateOnFeedback(name);
                        break;
                    case "RotationSpeed":
                        UpdateRotationSpeedFeedback(name);
                        break;
                    case "RotationDirection":
                        UpdateRotationDirectionFeedback(name);
                        break;
                }
            }
            else if (topic == "set")
            {
                string characteristic = (string)o["payload"]["characteristic"];
                switch (characteristic)
                {
                    case "On":
                        bool onValue = (bool)o["payload"]["value"];
                        ushort value;
                        if (onValue)
                        {
                            value = 1;
                        }
                        else
                        {
                            value = 0;
                        }
                        FanEvent(new HBCrestronEventArgs(name, characteristic, value));
                        break;
                    case "RotationSpeed":
                        ushort rotationSpeedValue = (ushort)o["payload"]["value"];
                        FanEvent(new HBCrestronEventArgs(name, characteristic, rotationSpeedValue));
                        break;
                    case "RotationDirection":
                        ushort rotationDirectionValue = (ushort)o["payload"]["value"];
                        FanEvent(new HBCrestronEventArgs(name, characteristic, rotationDirectionValue));
                        break;
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (Fans[Fans.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            Fans[Fans.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort)accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string)characteristic["description"])
                    {
                        case "On":
                            Fans[Fans.FindIndex(i => i.Name == name)].RemoteDeviceOnIID =
                                (ushort)characteristic["iid"];

                            if (Fans[Fans.FindIndex(i => i.Name == name)].On != (bool)characteristic["value"])
                            {
                                Fans[Fans.FindIndex(i => i.Name == name)].On = (bool)characteristic["value"];
                                FanEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "On", 1)
                                    : new HBCrestronEventArgs(name, "On", 0));
                            }
                            break;
                        case "RotationSpeed":
                            Fans[Fans.FindIndex(i => i.Name == name)].RemoteDeviceRotationSpeedIID =
                                (ushort)characteristic["iid"];
                            if (Fans[Fans.FindIndex(i => i.Name == name)].RotationSpeed != (ushort)characteristic["value"])
                            {
                                Fans[Fans.FindIndex(i => i.Name == name)].RotationSpeed = (ushort)characteristic["value"];
                                FanEvent(new HBCrestronEventArgs(name, "RotationSpeed", (ushort)characteristic["value"]));
                            }
                            break;
                        case "RotationDirection":
                            Fans[Fans.FindIndex(i => i.Name == name)].RemoteDeviceRotationDirectionIID =
                                (ushort)characteristic["iid"];
                            if (Fans[Fans.FindIndex(i => i.Name == name)].RotationDirection != (ushort)characteristic["value"])
                            {
                                Fans[Fans.FindIndex(i => i.Name == name)].RotationDirection = (ushort)characteristic["value"];
                                FanEvent(new HBCrestronEventArgs(name, "RotationDirection", (ushort)characteristic["value"]));
                            }
                            break;                        
                    }
                }
            }
        }
    }
}