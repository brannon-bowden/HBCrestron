using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.SecuritySystem
{
    public delegate void SecuritySystemHandler(HBCrestronEventArgs e);

    class SecuritySystem
    {
        public string Name = string.Empty;
        public string Service = "SecuritySystem";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;

        public ushort SecuritySystemCurrentState = 0;
        public ushort RemoteDeviceSecuritySystemCurrentStateIID = 0;

        public ushort SecuritySystemTargetState = 0;
        public ushort RemoteDeviceSecuritySystemTargetStateIID = 0;

        public ushort SupportsStatusFault = 0;
        public bool StatusFault = false;
        public ushort RemoteDeviceStatusFaultIID = 0;

        public ushort SupportsStatusTampered = 0;
        public bool StatusTampered = false;
        public ushort RemoteDeviceStatusTamperedIID = 0;

        public ushort SupportsSecuritySystemAlarmType = 0;
        public ushort SecuritySystemAlarmType = 0;
        public ushort RemoteDeviceSecuritySystemAlarmTypeIID = 0;        
    }

    class AddSecuritySystemPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string Service = "SecuritySystem";
        public string StatusFault = "default";
        public string StatusTampered = "default";
        public string SecuritySystemAlarmType = "default";

        [JsonIgnore]
        public ushort SupportsStatusFault = 0;
        [JsonIgnore]
        public ushort SupportsStatusTampered = 0;
        [JsonIgnore]
        public ushort SupportsSecuritySystemAlarmType = 0;

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
        public bool ShouldSerializeSecuritySystemAlarmType()
        {
            if (SupportsSecuritySystemAlarmType == 1)
            {
                return true;
            }
            return false;
        }

    }

    public static class HBSecuritySystem
    {
        public static event SecuritySystemHandler SecuritySystemEvent;
        private static List<SecuritySystem> SecuritySystems = new List<SecuritySystem>();
        public static void AddDevice(string name, ushort supportsStatusFault, ushort supportsStatusTampered, string deviceLocation)
        {
            SecuritySystems.Add(new SecuritySystem { Name = name, SupportsStatusFault = supportsStatusFault, SupportsStatusTampered = supportsStatusTampered, DeviceLocation = deviceLocation});
            if (deviceLocation == "Local")
            {
                var tempAddSecuritySystemPayload = new AddSecuritySystemPayload();
                tempAddSecuritySystemPayload.Name = name;
                tempAddSecuritySystemPayload.SupportsStatusFault = supportsStatusFault;
                tempAddSecuritySystemPayload.SupportsStatusTampered = supportsStatusFault;
                var tempAddObject = new AddObject(tempAddSecuritySystemPayload);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject)); //Serialize the Data above
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            SecuritySystems.Remove(SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (SecuritySystems.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }

        public static void SetSecuritySystemTargetState(string name, ushort value)
        {
            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemTargetState != value)
            {
                SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemTargetState = value;
                UpdateSecuritySystemTargetState(name);
            }
        }
        public static void SetSecuritySystemCurrentState(string name, ushort value)
        {
            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemCurrentState != value)
            {
                SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemCurrentState = value;
                UpdateSecuritySystemCurrentState(name);
            }
        }
        public static void SetSecuritySystemAlarmType(string name, ushort value)
        {
            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemAlarmType != value)
            {
                SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemAlarmType = value;
                UpdateSecuritySystemAlarmType(name);
            }
        }

        public static void SetStatusFaultFeedback(string name, ushort value)
        {
            if ((SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusFault && value == 0) || (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusFault == false && value == 1))
            {
                if (value == 1)
                {
                    SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusFault = true;
                }
                else
                {
                    SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusFault = false;
                }
                UpdateStatusFaultFeedback(name);
            }
        }
        public static void SetStatusTamperedFeedback(string name, ushort value)
        {
            if ((SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusTampered && value == 0) || (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusTampered == false && value == 1))
            {
                if (value == 1)
                {
                    SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusTampered = true;
                }
                else
                {
                    SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusTampered = false;
                }
                UpdateStatusTamperedFeedback(name);
            }
        }

        public static void UpdateSecuritySystemTargetState(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "SecuritySystemTargetState";
            tempUpdateValueObject.Payload.Value = SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemTargetState;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateSecuritySystemCurrentState(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "SecuritySystemTargetState";
            tempUpdateValueObject.Payload.Value = SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemCurrentState;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateSecuritySystemAlarmType(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "SecuritySystemAlarmType";
            tempUpdateValueObject.Payload.Value = SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemAlarmType;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateStatusFaultFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "StatusFault";
            tempUpdateValueObject.Payload.Value = SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusFault;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateStatusTamperedFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "StatusTampered";
            tempUpdateValueObject.Payload.Value = SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusTampered;
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
                    case "SecuritySystemCurrentState":
                        UpdateSecuritySystemCurrentState(name);
                        break;
                    case "SecuritySystemTargetState":
                        UpdateSecuritySystemTargetState(name);
                        break;
                    case "StatusFault":
                        UpdateStatusFaultFeedback(name);
                        break;
                    case "StatusTampered":
                        UpdateStatusTamperedFeedback(name);
                        break;
                    case "SecuritySystemAlarmType":
                        UpdateSecuritySystemAlarmType(name);
                        break;
                }
            }
            else if (topic == "set")
            {
                string characteristic = (string)o["payload"]["characteristic"];
                switch (characteristic)
                {
                    case "SecuritySystemTargetState":
                        ushort securitySystemTargetStateValue = (ushort)o["payload"]["value"];
                        SecuritySystemEvent(new HBCrestronEventArgs(name, characteristic, securitySystemTargetStateValue));
                        break;
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort)accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string)characteristic["description"])
                    {
                        case "SecuritySystemCurrentState":
                            SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].RemoteDeviceSecuritySystemCurrentStateIID =
                                (ushort)characteristic["iid"];
                            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemCurrentState != (ushort)characteristic["value"])
                            {
                                SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemCurrentState = (ushort)characteristic["value"];
                                SecuritySystemEvent(new HBCrestronEventArgs(name, "SecuritySystemCurrentState", (ushort)characteristic["value"]));
                            }
                            break;
                        case "StatusFault":
                            SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].RemoteDeviceStatusFaultIID =
                                (ushort)characteristic["iid"];

                            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusFault != (bool)characteristic["value"])
                            {
                                SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusFault = (bool)characteristic["value"];
                                SecuritySystemEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "StatusFault", 1)
                                    : new HBCrestronEventArgs(name, "StatusFault", 0));
                            }
                            break;
                        case "StatusTampered":
                            SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].RemoteDeviceStatusTamperedIID =
                                (ushort)characteristic["iid"];

                            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusTampered != (bool)characteristic["value"])
                            {
                                SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].StatusTampered = (bool)characteristic["value"];
                                SecuritySystemEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "StatusTampered", 1)
                                    : new HBCrestronEventArgs(name, "StatusTampered", 0));
                            }
                            break;
                        case "SecuritySystemAlarmType":
                            SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].RemoteDeviceSecuritySystemAlarmTypeIID =
                                (ushort)characteristic["iid"];
                            if (SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemAlarmType != (ushort)characteristic["value"])
                            {
                                SecuritySystems[SecuritySystems.FindIndex(i => i.Name == name)].SecuritySystemAlarmType = (ushort)characteristic["value"];
                                SecuritySystemEvent(new HBCrestronEventArgs(name, "SecuritySystemAlarmType", (ushort)characteristic["value"]));
                            }
                            break;
                    }
                }
            }
        }
    }
}