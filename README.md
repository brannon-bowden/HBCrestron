# HBCrestron
Crestron Simpl# Library for HomeBridge. 

Important to note: This is a proof of concept library written for my personal home. This does not do anything without HomeBridge running on a seperate device, and with certain plugins installed. Currently I am unable to act as a full HomeKit Accessory Server, so I am using HomeBridge as that piece. 

There are two types of control provided by this library, Local and Remote.

The first type of devices are Local Devices. There are devices controlled / hosted natively on the Crestron Processor. This form of control requires HomeBridge setup with the HomeBridge-Websocket Plugin installed. The plugin can be found @ https://github.com/cflurin/homebridge-websocket. The Simpl modules will create all the accessories based on what you have in your program. 
**Note if you change the parameters on a device module, you must trigger the reset device configuration signal.

The second type of devices are Remote Devices. These are devices controller by a third party plugin via HomeBridge. One example would be the HomeBridge-WEMO Plugin. When adding the remote device modules, please make sure the name matches EXACTLY to the existing HomeBridge device, and that the parameters match as well. 
**Note This type of control requires HomeBridge to be run in insecure mode. You can do this by launching HomeBridge with the -I flag. In addition, if you change the parameters on a device module, you must trigger the reset device configuration signal.

Currently there are no DIRECT ways to control native HomeKit devices, only HomeBridge. As a work around you can add "dummy" devices to HomeBridge and create Automation Links inside the HomeKit app to control a native HomeKit Device. You can also use this method to trigger things such as HomeKit Scenes from Crestron, or pass HomeKit Occupancy Information back to Crestron.

This project is still a work in progress. Not all device types have been created, the following page shows all the different device types possible (https://github.com/KhaosT/HAP-NodeJS/blob/master/lib/gen/HomeKitTypes.js). In addition, I do not have one of every device type, so I am unable to test every module myself. 

If you would like to help contribute, please provide a pull request.

