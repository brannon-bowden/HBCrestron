using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.Switch
{
    public delegate void SwitchHandler(HBCrestronEventArgs e);

    class Switch
    {
        public string Name = string.Empty;
        public string Service = "Switch";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;

        public bool On = false;
        public ushort RemoteDeviceOnIID = 0;
    }

    class AddSwitchPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string Service = "Switch";
    }

    public static class HBSwitch
    {
        public static event SwitchHandler SwitchEvent;
        private static List<Switch> Switches = new List<Switch>();
        public static void AddDevice(string name, string deviceLocation)
        {

            Switches.Add(new Switch { Name = name, DeviceLocation = deviceLocation });
            if (deviceLocation == "Local")
            {
                var tempAddSwitchPayload = new AddSwitchPayload();
                tempAddSwitchPayload.Name = name;
                var tempAddObject = new AddObject(tempAddSwitchPayload);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject)); //Serialize the Data above
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (Switches[Switches.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            Switches.Remove(Switches[Switches.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (Switches.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }
        public static void SetOn(string name, ushort value)
        {
            ControlBoolValueObject tempControlBoolValueObject = new ControlBoolValueObject();
            tempControlBoolValueObject.Characteristics[0] = new ControlBoolValuePayload();
            tempControlBoolValueObject.Characteristics[0].AID = Switches[Switches.FindIndex(i => i.Name == name)].RemoteDeviceAID;
            tempControlBoolValueObject.Characteristics[0].IID = Switches[Switches.FindIndex(i => i.Name == name)].RemoteDeviceOnIID;
            if (value == 1)
                tempControlBoolValueObject.Characteristics[0].Value = true;
            var stringToSend = JsonConvert.SerializeObject(tempControlBoolValueObject);
            HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
        }
        public static void SetOnFeedback(string name, ushort value)
        {
            if ((Switches[Switches.FindIndex(i => i.Name == name)].On && value == 0) || (Switches[Switches.FindIndex(i => i.Name == name)].On == false && value == 1)) 
            {
                if (value == 1)
                {
                    Switches[Switches.FindIndex(i => i.Name == name)].On = true;
                }
                else
                {
                    Switches[Switches.FindIndex(i => i.Name == name)].On = false;
                }
                UpdateOnFeedback(name);                
            }
        }
        public static void SetOnFeedback(string name, bool value)
        {
            if (Switches[Switches.FindIndex(i => i.Name == name)].On != value)
            {
                Switches[Switches.FindIndex(i => i.Name == name)].On = value;
                UpdateOnFeedback(name);                
            }
        }        

        public static void UpdateOnFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "On";
            tempUpdateValueObject.Payload.Value = Switches[Switches.FindIndex(i => i.Name == name)].On;
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
                    case "On":
                        UpdateOnFeedback(name);
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
                        SwitchEvent(new HBCrestronEventArgs(name,characteristic,value));
                        break;                    
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (Switches[Switches.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            Switches[Switches.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort)accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string) characteristic["description"])
                    {
                        case "On":
                            Switches[Switches.FindIndex(i => i.Name == name)].RemoteDeviceOnIID =
                                (ushort) characteristic["iid"];

                            if (Switches[Switches.FindIndex(i => i.Name == name)].On != (bool) characteristic["value"])
                            {
                                Switches[Switches.FindIndex(i => i.Name == name)].On = (bool) characteristic["value"];
                                SwitchEvent((bool) characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "On", 1)
                                    : new HBCrestronEventArgs(name, "On", 0));
                            }
                            break;
                    }
                }
            }
        }
    }
}