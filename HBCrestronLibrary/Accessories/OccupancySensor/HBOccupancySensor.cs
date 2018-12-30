using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.OccupancySensor
{
    public delegate void OccupancySensorHandler(HBCrestronEventArgs e);

    class OccupancySensor
    {
        public string Name = string.Empty;
        public string Service = "OccupancySensor";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;

        public ushort OccupancyDetected = 0;
        public ushort RemoteDeviceOccupancyDetectedIID = 0;
        
        public ushort SupportsStatusActive = 0;
        public bool StatusActive = false;
        public ushort RemoteDeviceStatusActiveIID = 0;
        
        public ushort SupportsStatusFault = 0;
        public bool StatusFault = false;
        public ushort RemoteDeviceStatusFaultIID = 0;
        
        public ushort SupportsStatusTampered = 0;
        public bool StatusTampered = false;
        public ushort RemoteDeviceStatusTamperedIID = 0;
        
        public ushort SupportsStatusLowBattery = 0;
        public bool StatusLowBattery = false;
        public ushort RemoteDeviceStatusLowBatteryIID = 0;
    }

    class AddOccupancySensorPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string Service = "OccupancySensor";
        public string StatusActive = "default";
        public string StatusFault = "default";
        public string StatusTampered = "default";
        public string StatusLowBattery = "default";

        [JsonIgnore]
        public ushort SupportsStatusActive = 0;
        [JsonIgnore]
        public ushort SupportsStatusFault = 0;
        [JsonIgnore]
        public ushort SupportsStatusTampered = 0;
        [JsonIgnore]
        public ushort SupportsStatusLowBattery = 0;

        public bool ShouldSerializeStatusActive()
        {
            if (SupportsStatusActive == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeStatusFault()
        {
            if (SupportsStatusFault == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeStatusTampered()
        {
            if (SupportsStatusTampered == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeStatusLowBattery()
        {
            if (SupportsStatusLowBattery == 1)
            {
                return true;
            }
            return false;
        }
    }

    public static class HBOccupancySensor
    {
        public static event OccupancySensorHandler OccupancySensorEvent;
        private static List<OccupancySensor> OccupancySensors = new List<OccupancySensor>();
        public static void AddDevice(string name, ushort supportsStatusActive, ushort supportsStatusFault, ushort supportsStatusTampered, ushort supportsStatusLowBattery, string deviceLocation)
        {
            OccupancySensors.Add(new OccupancySensor { Name = name, SupportsStatusActive = supportsStatusActive, SupportsStatusFault = supportsStatusFault, SupportsStatusTampered = supportsStatusTampered, SupportsStatusLowBattery = supportsStatusLowBattery, DeviceLocation = deviceLocation});
            if (deviceLocation == "Local")
            {
                var tempAddOccupancySensorPayload = new AddOccupancySensorPayload();
                tempAddOccupancySensorPayload.Name = name;
                tempAddOccupancySensorPayload.SupportsStatusActive = supportsStatusActive;
                tempAddOccupancySensorPayload.SupportsStatusFault = supportsStatusFault;
                tempAddOccupancySensorPayload.SupportsStatusTampered = supportsStatusFault;
                tempAddOccupancySensorPayload.SupportsStatusLowBattery = supportsStatusLowBattery;
                var tempAddObject = new AddObject(tempAddOccupancySensorPayload);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject)); //Serialize the Data above
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            OccupancySensors.Remove(OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (OccupancySensors.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }
        public static void SetOccupancyDetectedFeedback(string name, ushort value)
        {
            if (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].OccupancyDetected != value)
            {
                OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].OccupancyDetected = value;
                UpdateOccupancyDetectedFeedback(name);
            }
        }
        public static void SetStatusActiveFeedback(string name, ushort value)
        {
            if ((OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusActive && value == 0) || (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusActive == false && value == 1))
            {
                if (value == 1)
                {
                    OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusActive = true;
                }
                else
                {
                    OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusActive = false;
                }
                UpdateStatusActiveFeedback(name);
            }
        }
        public static void SetStatusFaultFeedback(string name, ushort value)
        {
            if ((OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusFault && value == 0) || (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusFault == false && value == 1))
            {
                if (value == 1)
                {
                    OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusFault = true;
                }
                else
                {
                    OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusFault = false;
                }
                UpdateStatusFaultFeedback(name);
            }
        }
        public static void SetStatusTamperedFeedback(string name, ushort value)
        {
            if ((OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusTampered && value == 0) || (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusTampered == false && value == 1))
            {
                if (value == 1)
                {
                    OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusTampered = true;
                }
                else
                {
                    OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusTampered = false;
                }
                UpdateStatusTamperedFeedback(name);
            }
        }
        public static void SetStatusLowBatteryFeedback(string name, ushort value)
        {
            if ((OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusLowBattery && value == 0) || (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusLowBattery == false && value == 1))
            {
                if (value == 1)
                {
                    OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusLowBattery = true;
                }
                else
                {
                    OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusLowBattery = false;
                }
                UpdateStatusLowBatteryFeedback(name);
            }
        }
        public static void UpdateOccupancyDetectedFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "OccupancyDetected";
            tempUpdateValueObject.Payload.Value = OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].OccupancyDetected;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateStatusActiveFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "StatusActive";
            tempUpdateValueObject.Payload.Value = OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusActive;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateStatusFaultFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "StatusFault";
            tempUpdateValueObject.Payload.Value = OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusFault;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateStatusTamperedFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "StatusTampered";
            tempUpdateValueObject.Payload.Value = OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusTampered;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateStatusLowBatteryFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "StatusLowBattery";
            tempUpdateValueObject.Payload.Value = OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusLowBattery;
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
                    case "OccupancyDetected":
                        UpdateOccupancyDetectedFeedback(name);
                        break;
                    case "StatusActive":
                        UpdateStatusActiveFeedback(name);
                        break;
                    case "StatusFault":
                        UpdateStatusFaultFeedback(name);
                        break;
                    case "StatusTampered":
                        UpdateStatusTamperedFeedback(name);
                        break;
                    case "StatusLowBattery":
                        UpdateStatusLowBatteryFeedback(name);
                        break;
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort)accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string)characteristic["description"])
                    {
                        case "OccupancyDetected":
                            OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].RemoteDeviceOccupancyDetectedIID =
                                (ushort)characteristic["iid"];
                            if (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].OccupancyDetected != (ushort)characteristic["value"])
                            {
                                OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].OccupancyDetected = (ushort)characteristic["value"];
                                OccupancySensorEvent(new HBCrestronEventArgs(name, "OccupancyDetected", (ushort)characteristic["value"]));
                            }
                            break;
                        case "StatusActive":
                            OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].RemoteDeviceStatusActiveIID =
                                (ushort)characteristic["iid"];

                            if (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusActive != (bool)characteristic["value"])
                            {
                                OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusActive = (bool)characteristic["value"];
                                OccupancySensorEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "StatusActive", 1)
                                    : new HBCrestronEventArgs(name, "StatusActive", 0));
                            }
                            break;
                        case "StatusFault":
                            OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].RemoteDeviceStatusFaultIID =
                                (ushort)characteristic["iid"];

                            if (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusFault != (bool)characteristic["value"])
                            {
                                OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusFault = (bool)characteristic["value"];
                                OccupancySensorEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "StatusFault", 1)
                                    : new HBCrestronEventArgs(name, "StatusFault", 0));
                            }
                            break;
                        case "StatusTampered":
                            OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].RemoteDeviceStatusTamperedIID =
                                (ushort)characteristic["iid"];

                            if (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusTampered != (bool)characteristic["value"])
                            {
                                OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusTampered = (bool)characteristic["value"];
                                OccupancySensorEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "StatusTampered", 1)
                                    : new HBCrestronEventArgs(name, "StatusTampered", 0));
                            }
                            break;
                        case "StatusLowBattery":
                            OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].RemoteDeviceStatusLowBatteryIID =
                                (ushort)characteristic["iid"];

                            if (OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusLowBattery != (bool)characteristic["value"])
                            {
                                OccupancySensors[OccupancySensors.FindIndex(i => i.Name == name)].StatusLowBattery = (bool)characteristic["value"];
                                OccupancySensorEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "StatusLowBattery", 1)
                                    : new HBCrestronEventArgs(name, "StatusLowBattery", 0));
                            }
                            break;
                    }
                }
            }
        }
    }
}