using System;
using HBCrestronLibrary.Accessories.Door;
using HBCrestronLibrary.Accessories.Fan;
using HBCrestronLibrary.Accessories.GarageDoor;
using HBCrestronLibrary.Accessories.Lightbulb;
using HBCrestronLibrary.Accessories.OccupancySensor;
using HBCrestronLibrary.Accessories.Outlet;
using HBCrestronLibrary.Accessories.SecuritySystem;
using HBCrestronLibrary.Accessories.Switch;
using HBCrestronLibrary.Accessories.Thermostat;
using HBCrestronLibrary.Accessories.Window;
using Newtonsoft.Json;

namespace HBCrestronLibrary.Common
{
    public class HBCrestronEventArgs : EventArgs
    {

        public string Name { get; set; }
        public string Characteristic { get; set; }
        public ushort Value { get; set; }
        public HBCrestronEventArgs(string name, string characteristic, ushort value)
        {
            Name = name;
            Characteristic = characteristic;
            Value = value;
        }

        public HBCrestronEventArgs()
        {

        }
    }

    class AddObject
    {
        [JsonProperty(PropertyName = "topic")]
        public string Topic = "add";
        [JsonProperty(PropertyName = "payload")]
        public object Payload = new object();

        public AddObject(AddLightbulbPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddDoorPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddFanPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddGarageDoorPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddOccupancySensorPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddOutletPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddSecuritySystemPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddSwitchPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddThermostatPayload payload)
        {
            Payload = payload;
        }
        public AddObject(AddWindowPayload payload)
        {
            Payload = payload;
        }
    }

    class RemoveObject
    {
        [JsonProperty(PropertyName = "topic")]
        public string Topic = "remove";
        [JsonProperty(PropertyName = "payload")]
        public RemovePayload Payload = new RemovePayload();
    }
    class RemovePayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
    }
    class SetIntValueObject
    {
        [JsonProperty(PropertyName = "topic")]
        public string Topic = "setValue";
        [JsonProperty(PropertyName = "payload")]
        public SetIntValuePayload Payload = new SetIntValuePayload();
    }
    class SetIntValuePayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "characteristic")]
        public string Characteristic = "";
        [JsonProperty(PropertyName = "value")]
        public int Value = 0;
    }
    class SetBoolValueObject
    {
        [JsonProperty(PropertyName = "topic")]
        public string Topic = "setValue";
        [JsonProperty(PropertyName = "payload")]
        public SetBoolValuePayload Payload = new SetBoolValuePayload();
    }
    class SetBoolValuePayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "characteristic")]
        public string Characteristic = "";
        [JsonProperty(PropertyName = "value")]
        public bool Value;
    }
    class SetStringValueObject
    {
        [JsonProperty(PropertyName = "topic")]
        public string Topic = "setValue";
        [JsonProperty(PropertyName = "payload")]
        public SetStringValuePayload Payload = new SetStringValuePayload();
    }
    class SetStringValuePayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "characteristic")]
        public string Characteristic = "";
        [JsonProperty(PropertyName = "value")]
        public string Value = "";
    }

    class ControlIntValuePayload
    {
        [JsonProperty(PropertyName = "aid")]
        public ushort AID = 0;
        [JsonProperty(PropertyName = "iid")]
        public ushort IID = 0;
        [JsonProperty(PropertyName = "value")]
        public ushort Value = 0;
    }
    class ControlIntValueObject
    {
        [JsonProperty(PropertyName = "characteristics")]
        public ControlIntValuePayload[] Characteristics = new ControlIntValuePayload[1];
    }

    class ControlBoolValuePayload
    {
        [JsonProperty(PropertyName = "aid")]
        public ushort AID = 0;
        [JsonProperty(PropertyName = "iid")]
        public ushort IID = 0;
        [JsonProperty(PropertyName = "value")]
        public bool Value = false;
    }
    class ControlBoolValueObject
    {
        [JsonProperty(PropertyName = "characteristics")]
        public ControlBoolValuePayload[] Characteristics = new ControlBoolValuePayload[1];
    }

    class ControlStringValuePayload
    {
        [JsonProperty(PropertyName = "aid")]
        public ushort AID = 0;
        [JsonProperty(PropertyName = "iid")]
        public ushort IID = 0;
        [JsonProperty(PropertyName = "value")]
        public string Value = "";
    }
    class ControlStringValueObject
    {
        [JsonProperty(PropertyName = "characteristics")]
        public ControlStringValuePayload[] Characteristics = new ControlStringValuePayload[1];
    }
}