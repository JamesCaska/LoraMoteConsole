# LoraMoteConsole
Visual Studio 2015 C# Project for driving the microchip RN2483 Lora module using a Serial port.

Is intended to be a minimal configuration for connecting a Lora device to the virtualbreadboard.io IoT Lora Network Server

To use this application you should have
* An RN2483 Lora Device connected to serial port ( You may need to change the device COM3 in code )
* Be in range of a Lora gateway with Otaa support and have a registered AppEUI/DevEUI/AppKey with the Lora Server
* You can use any Lora Server but for testing you can use the virtualbreadboard.io network server at 

<code>vbbiot.cloudapp.net, 1680</code>
   
This network server supports he semtech packetforwarder data exchange format

See The Following Getting Started Video : https://youtu.be/VaaH8aBaaNY

 
