
namespace IoT.ControlPanel.MVVM.Models;




// FLYTTA TILL SHARED LIBRARY om möjligt


public class DeviceItem
{
    public string DeviceId { get; set; }
    public string DeviceType { get; set; }
    public string Vendor { get; set; }
    public string Location { get; set; }
    public bool IsActive { get; set; }
}
