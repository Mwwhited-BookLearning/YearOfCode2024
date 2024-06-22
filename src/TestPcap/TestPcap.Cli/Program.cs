
using SharpPcap;

var devices = CaptureDeviceList.Instance;
var targetDevice = @"\Device\NPF_Loopback";
var loopback = devices[targetDevice];
loopback.Open();

Console.WriteLine("Fixed!");
Console.ReadLine();
