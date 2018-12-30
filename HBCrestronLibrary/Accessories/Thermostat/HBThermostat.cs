using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.Thermostat
{
    public delegate void ThermostatHandler(HBCrestronEventArgs e);

    class Thermostat
    {
        public string Name = string.Empty;
        public string Service = "Thermostat";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;

        public ushort CurrentHeatingCoolingState = 0;
        public ushort RemoteDeviceCurrentHeatingCoolingStateIID = 0;

        public ushort TargetHeatingCoolingState = 0;
        public ushort RemoteDeviceTargetHeatingCoolingStateIID = 0;

        public double CurrentTemperature = 0;
        public ushort RemoteDeviceCurrentTemperatureIID = 0;

        public double TargetTemperature = 0;
        public ushort RemoteDeviceTargetTemperatureIID = 0;

        public ushort TemperatureDisplayUnits = 0;
        public ushort RemoteDeviceTemperatureDisplayUnitsIID = 0;

        public ushort SupportsCurrentRelativeHumidity = 0;
        public ushort CurrentRelativeHumidity = 0;
        public ushort RemoteDeviceCurrentRelativeHumidityIID = 0;

        public ushort SupportsTargetRelativeHumidity = 0;
        public ushort TargetRelativeHumidity = 0;
        public ushort RemoteDeviceTargetRelativeHumidityIID = 0;

        public ushort SupportsCoolingThresholdTemperature = 0;
        public double CoolingThresholdTemperature = 0;
        public ushort RemoteDeviceCoolingThresholdTemperatureIID = 0;

        public ushort SupportsHeatingThresholdTemperature = 0;
        public double HeatingThresholdTemperature = 0;
        public ushort RemoteDeviceHeatingThresholdTemperatureIID = 0;
    }

    class AddThermostatPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string Service = "Thermostat";
        public string CurrentRelativeHumidity = "default";
        public string TargetRelativeHumidity = "default";
        public string CoolingThresholdTemperature = "default";
        public string HeatingThresholdTemperature = "default";

        [JsonIgnore]
        public ushort SupportsCurrentRelativeHumidity = 0;
        [JsonIgnore]
        public ushort SupportsTargetRelativeHumidity = 0;
        [JsonIgnore]
        public ushort SupportsCoolingThresholdTemperature = 0;
        [JsonIgnore]
        public ushort SupportsHeatingThresholdTemperature = 0;

        public bool ShouldSerializeCurrentRelativeHumidity()
        {
            if (SupportsCurrentRelativeHumidity == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeTargetRelativeHumidity()
        {
            if (SupportsTargetRelativeHumidity == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeCoolingThresholdTemperature()
        {
            if (SupportsCoolingThresholdTemperature == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeHeatingThresholdTemperature()
        {
            if (SupportsHeatingThresholdTemperature == 1)
            {
                return true;
            }
            return false;
        }
    }

    public static class HBThermostat
    {
        public static event ThermostatHandler ThermostatEvent;
        private static List<Thermostat> Thermostats = new List<Thermostat>();
        public static void AddDevice(string name, ushort supportsCurrentRelativeHumidity, ushort supportsTargetRelativeHumidity, ushort supportsCoolingThresholdTemperature, ushort supportsHeatingThresholdTemperature, string deviceLocation)
        {
            Thermostats.Add(new Thermostat { Name = name, SupportsCurrentRelativeHumidity = supportsCurrentRelativeHumidity, SupportsTargetRelativeHumidity = supportsTargetRelativeHumidity, SupportsCoolingThresholdTemperature = supportsCoolingThresholdTemperature, SupportsHeatingThresholdTemperature = supportsHeatingThresholdTemperature, DeviceLocation = deviceLocation});
            if (deviceLocation == "Local")
            {
                var tempAddThermostatPayload = new AddThermostatPayload();
                tempAddThermostatPayload.Name = name;
                tempAddThermostatPayload.SupportsCurrentRelativeHumidity = supportsCurrentRelativeHumidity;
                tempAddThermostatPayload.SupportsTargetRelativeHumidity = supportsTargetRelativeHumidity;
                tempAddThermostatPayload.SupportsCoolingThresholdTemperature = supportsCoolingThresholdTemperature;
                tempAddThermostatPayload.SupportsHeatingThresholdTemperature = supportsHeatingThresholdTemperature;
                var tempAddObject = new AddObject(tempAddThermostatPayload);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject)); //Serialize the Data above
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            Thermostats.Remove(Thermostats[Thermostats.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (Thermostats.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }
        public static void SetTargetHeatingCoolingState(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetHeatingCoolingState != value)
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetHeatingCoolingState = value;
                UpdateTargetHeatingCoolingStateFeedback(name);
            }
        }
        public static void SetCurrentHeatingCoolingStateFeedback(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentHeatingCoolingState != value)
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentHeatingCoolingState = value;
                UpdateCurrentHeatingCoolingStateFeedback(name);
            }
        }
        public static void SetTargetTemperature(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetTemperature != HBThermostatExtension.FtoC(value))
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetTemperature = HBThermostatExtension.FtoC(value);
                UpdateTargetTemperatureFeedback(name);
            }
        }
        public static void SetCurrentTemperatureFeedback(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentTemperature != HBThermostatExtension.FtoC(value))
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentTemperature = HBThermostatExtension.FtoC(value);
                UpdateCurrentTemperatureFeedback(name);
            }
        }
        public static void SetTemperatureDisplayUnitsFeedback(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].TemperatureDisplayUnits != value)
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].TemperatureDisplayUnits = value;
                UpdateTemperatureDisplayUnitsFeedback(name);
            }
        }
        public static void SetTargetRelativeHumidityFeedback(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetRelativeHumidity != value)
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetRelativeHumidity = value;
                UpdateTargetRelativeHumidityFeedback(name);
            }
        }
        public static void SetCurrentRelativeHumidityFeedback(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentRelativeHumidity != value)
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentRelativeHumidity = value;
                UpdateCurrentRelativeHumidityFeedback(name);
            }
        }
        public static void SetHeatingThresholdTemperatureFeedback(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].HeatingThresholdTemperature != HBThermostatExtension.FtoC(value))
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].HeatingThresholdTemperature = HBThermostatExtension.FtoC(value);
                UpdateHeatingThresholdTemperatureFeedback(name);
            }
        }
        public static void SetCoolingThresholdTemperatureFeedback(string name, ushort value)
        {
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].CoolingThresholdTemperature != HBThermostatExtension.FtoC(value))
            {
                Thermostats[Thermostats.FindIndex(i => i.Name == name)].CoolingThresholdTemperature = HBThermostatExtension.FtoC(value);
                UpdateCoolingThresholdTemperatureFeedback(name);
            }
        }

        public static void UpdateTargetHeatingCoolingStateFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "TargetHeatingCoolingState";
            tempUpdateValueObject.Payload.Value = Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetHeatingCoolingState;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateCurrentHeatingCoolingStateFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "CurrentHeatingCoolingState";
            tempUpdateValueObject.Payload.Value = Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentHeatingCoolingState;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateTargetTemperatureFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "TargetTemperature";
            tempUpdateValueObject.Payload.Value = (int)Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetTemperature;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateCurrentTemperatureFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "CurrentTemperature";
            tempUpdateValueObject.Payload.Value = (int)Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentTemperature;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateTemperatureDisplayUnitsFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "TemperatureDisplayUnits";
            tempUpdateValueObject.Payload.Value = Thermostats[Thermostats.FindIndex(i => i.Name == name)].TemperatureDisplayUnits;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateTargetRelativeHumidityFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "TargetRelativeHumidity";
            tempUpdateValueObject.Payload.Value = Thermostats[Thermostats.FindIndex(i => i.Name == name)].TargetRelativeHumidity;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateCurrentRelativeHumidityFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "CurrentRelativeHumidity";
            tempUpdateValueObject.Payload.Value = Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentRelativeHumidity;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateHeatingThresholdTemperatureFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "HeatingThresholdTemperature";
            tempUpdateValueObject.Payload.Value = (int)Thermostats[Thermostats.FindIndex(i => i.Name == name)].HeatingThresholdTemperature;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateCoolingThresholdTemperatureFeedback(string name)
        {
            var tempUpdateValueObject = new SetIntValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "CoolingThresholdTemperature";
            tempUpdateValueObject.Payload.Value = (int)Thermostats[Thermostats.FindIndex(i => i.Name == name)].CoolingThresholdTemperature;
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
                    case "TargetHeatingCoolingState":
                        UpdateTargetHeatingCoolingStateFeedback(name);
                        break;
                    case "CurrentHeatingCoolingState":
                        UpdateCurrentHeatingCoolingStateFeedback(name);
                        break;
                    case "TargetTemperature":
                        UpdateTargetTemperatureFeedback(name);
                        break;
                    case "CurrentTemperature":
                        UpdateCurrentTemperatureFeedback(name);
                        break;
                    case "TemperatureDisplayUnits":
                        UpdateTemperatureDisplayUnitsFeedback(name);
                        break;
                    case "TargetRelativeHumidity":
                        UpdateTargetRelativeHumidityFeedback(name);
                        break;
                    case "CurrentRelativeHumidity":
                        UpdateCurrentRelativeHumidityFeedback(name);
                        break;
                    case "HeatingThresholdTemperature":
                        UpdateHeatingThresholdTemperatureFeedback(name);
                        break;
                    case "CoolingThresholdTemperature":
                        UpdateCoolingThresholdTemperatureFeedback(name);
                        break;

                }
            }
            else if (topic == "set")
            {
                string characteristic = (string)o["payload"]["characteristic"];
                switch (characteristic)
                {
                    case "TargetHeatingCoolingState":
                        ushort targetHeatingCoolingStateValue = (ushort)o["payload"]["value"];
                        ThermostatEvent(new HBCrestronEventArgs(name, characteristic, targetHeatingCoolingStateValue));
                        break;
                    case "TargetTemperature":
                        ushort targetTemperatureValue = (ushort)o["payload"]["value"];
                        ThermostatEvent(new HBCrestronEventArgs(name, characteristic, (ushort)HBThermostatExtension.CtoF(targetTemperatureValue)));
                        break;
                    case "TargetRelativeHumidity":
                        ushort targetRelativeHumidityValue = (ushort)o["payload"]["value"];
                        ThermostatEvent(new HBCrestronEventArgs(name, characteristic, targetRelativeHumidityValue));
                        break;
                    case "CoolingThresholdTemperature":
                        ushort coolingThresholdTemperatureValue = (ushort)o["payload"]["value"];
                        ThermostatEvent(new HBCrestronEventArgs(name, characteristic, (ushort)HBThermostatExtension.CtoF(coolingThresholdTemperatureValue)));
                        break;
                    case "HeatingThresholdTemperature":
                        ushort heatingThresholdTemperatureValue = (ushort)o["payload"]["value"];
                        ThermostatEvent(new HBCrestronEventArgs(name, characteristic, (ushort)HBThermostatExtension.CtoF(heatingThresholdTemperatureValue)));
                        break;
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            Thermostats[Thermostats.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort)accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string)characteristic["description"])
                    {
                        case "CurrentHeatingCoolingState":
                            Thermostats[Thermostats.FindIndex(i => i.Name == name)].RemoteDeviceCurrentHeatingCoolingStateIID =
                                (ushort)characteristic["iid"];
                            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentHeatingCoolingState != (ushort)characteristic["value"])
                            {
                                Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentHeatingCoolingState = (ushort)characteristic["value"];
                                ThermostatEvent(new HBCrestronEventArgs(name, "CurrentHeatingCoolingState", (ushort)characteristic["value"]));
                            }
                            break;
                        case "CurrentTemperature":
                            Thermostats[Thermostats.FindIndex(i => i.Name == name)].RemoteDeviceCurrentTemperatureIID =
                                (ushort)characteristic["iid"];
                            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentTemperature != (ushort)characteristic["value"])
                            {
                                Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentTemperature = (ushort)characteristic["value"];
                                ThermostatEvent(new HBCrestronEventArgs(name, "CurrentTemperature", (ushort)characteristic["value"]));
                            }
                            break;
                        case "TemperatureDisplayUnits":
                            Thermostats[Thermostats.FindIndex(i => i.Name == name)].RemoteDeviceTemperatureDisplayUnitsIID =
                                (ushort)characteristic["iid"];
                            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].TemperatureDisplayUnits != (ushort)characteristic["value"])
                            {
                                Thermostats[Thermostats.FindIndex(i => i.Name == name)].TemperatureDisplayUnits = (ushort)characteristic["value"];
                                ThermostatEvent(new HBCrestronEventArgs(name, "TemperatureDisplayUnits", (ushort)characteristic["value"]));
                            }
                            break;
                        case "CurrentRelativeHumidity":
                            Thermostats[Thermostats.FindIndex(i => i.Name == name)].RemoteDeviceCurrentRelativeHumidityIID =
                                (ushort)characteristic["iid"];
                            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentRelativeHumidity != (ushort)characteristic["value"])
                            {
                                Thermostats[Thermostats.FindIndex(i => i.Name == name)].CurrentRelativeHumidity = (ushort)characteristic["value"];
                                ThermostatEvent(new HBCrestronEventArgs(name, "CurrentRelativeHumidity", (ushort)characteristic["value"]));
                            }
                            break;
                        case "HeatingThresholdTemperature":
                            Thermostats[Thermostats.FindIndex(i => i.Name == name)].RemoteDeviceHeatingThresholdTemperatureIID =
                                (ushort)characteristic["iid"];
                            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].HeatingThresholdTemperature != (ushort)characteristic["value"])
                            {
                                Thermostats[Thermostats.FindIndex(i => i.Name == name)].HeatingThresholdTemperature = (ushort)characteristic["value"];
                                ThermostatEvent(new HBCrestronEventArgs(name, "HeatingThresholdTemperature", (ushort)characteristic["value"]));
                            }
                            break;
                        case "CoolingThresholdTemperature":
                            Thermostats[Thermostats.FindIndex(i => i.Name == name)].RemoteDeviceCoolingThresholdTemperatureIID =
                                (ushort)characteristic["iid"];
                            if (Thermostats[Thermostats.FindIndex(i => i.Name == name)].CoolingThresholdTemperature != (ushort)characteristic["value"])
                            {
                                Thermostats[Thermostats.FindIndex(i => i.Name == name)].CoolingThresholdTemperature = (ushort)characteristic["value"];
                                ThermostatEvent(new HBCrestronEventArgs(name, "CoolingThresholdTemperature", (ushort)characteristic["value"]));
                            }
                            break;
                    }
                }
            }
        }
    }
}