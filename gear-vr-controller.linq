<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Numerics.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.InteropServices.WindowsRuntime.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Numerics.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5\System.Runtime.WindowsRuntime.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Windows Kits\10\References\10.0.19041.0\Windows.Foundation.FoundationContract\4.0.0.0\Windows.Foundation.FoundationContract.winmd</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Windows Kits\10\References\10.0.19041.0\Windows.Foundation.UniversalApiContract\10.0.0.0\Windows.Foundation.UniversalApiContract.winmd</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Windows Kits\10\UnionMetadata\Facade\Windows.WinMD</Reference>
  <Namespace>System</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <AppConfig>
    <Content>
      <configuration>
        <Capabilities>
          <DeviceCapability Name="bluetooth" />
        </Capabilities>
        <appSettings>
          <add key="SendKeys" value="SendInput" />
        </appSettings>
      </configuration>
    </Content>
  </AppConfig>
</Query>

DumpContainer dc;
List<object> gdccontent;
// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-keybd_event
// https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
class GearVRController
{
	public static LINQPad.Controls.CheckBox DIAG, BTN1, BTN2, BTN3, BTN4, BTN5, BTN6, BTN7;
	public static LINQPad.Controls.TextBox TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, EXCEPTION;
	public static LINQPad.Controls.SelectBox SB;
	
	private static GearVRController instance;
	public static readonly Guid guid_controller_data_service         = Guid.Parse("4f63756c-7573-2054-6872-65656d6f7465");
	public static readonly Guid guid_controller_setup_characteristic = Guid.Parse("c8c51726-81bc-483b-a052-f7a14ea3d282");
	public static readonly Guid guid_controller_data_characteristic  = Guid.Parse("c8c51726-81bc-483b-a052-f7a14ea3d281");

	UInt64 i_vrcontrollerMAC = 49180498553505; // use calc to convert s_vrcontrollerMAC to decimal // string s_vrcontrollerMAC = "2C:BA:BA:25:6A:A1";

	private static Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic setup_characteristic = null;
	private static Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic data_characteristic = null;
	private static DateTime? time = null;
	
	[DllImport("user32.dll")]
	public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

	[DllImport("user32.dll")]
	public static extern int SetCursorPos(int x, int y);
	
	[DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);


	private const int MOUSEEVENTF_MOVE        = 0x0001; /* mouse move */
	private const int MOUSEEVENTF_LEFTDOWN    = 0x0002; /* left button down */
	private const int MOUSEEVENTF_LEFTUP      = 0x0004; /* left button up */
	private const int MOUSEEVENTF_RIGHTDOWN   = 0x0008; /* right button down */
	private const int MOUSEEVENTF_RIGHTUP     = 0x0010; /* right button up */
	private const int MOUSEEVENTF_MIDDLEDOWN  = 0x0020; /* middle button down */
	private const int MOUSEEVENTF_MIDDLEUP    = 0x0040; /* middle button up */
	private const int MOUSEEVENTF_XDOWN       = 0x0080; /* x button down */
	private const int MOUSEEVENTF_XUP         = 0x0100; /* x button down */
	private const int MOUSEEVENTF_WHEEL       = 0x0800; /* wheel button rolled */
	private const int MOUSEEVENTF_VIRTUALDESK = 0x4000; /* map to entire virtual desktop */
	private const int MOUSEEVENTF_ABSOLUTE    = 0x8000; /* absolute move */
	
	private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
    private const int APPCOMMAND_VOLUME_UP = 0xA0000;
    private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
    private const int WM_APPCOMMAND = 0x319;
	
	private const int KeysVolumeDown = 174; //Keys.VolumeDown
	private const int KeysVolumeUp   = 175; //Keys.VolumeUp
	private const int KeysVolumeMute = 173; //Keys.VolumeMute (?)
	
	private static int axisX   = 0;
    private static int axisY   = 0;
    private static int accelX  = 0;
    private static int accelY  = 0;
    private static int accelZ  = 0;
    private static int gyroX   = 0;
    private static int gyroY   = 0;
    private static int gyroZ   = 0;
    private static int magnetX = 0;
    private static int magnetY = 0;
    private static int magnetZ = 0;
	
    private static bool triggerButton    = false;
    private static bool homeButton       = false;
    private static bool backButton       = false;
    private static bool touchpadButton   = false;
    private static bool volumeUpButton   = false;
    private static bool volumeDownButton = false;
    private static bool NoButton         = false;
    private static int  idelta = 0;
    private static int  odelta = 0;
	
    private static bool __useWheel  = false;
    private static bool __useTouch  = false;
	private static bool outerCircle = false;

	private static int __axisX = 0;
	private static int __axisY = 0;
    private static int __altX = 0;
	private static int __altY = 0;
    private static bool __reset  = false;
	private static bool __volbtn = false;
	private static bool __tchbtn = false;
	private static bool __trig   = false;
		
    private static int __c_numberOfWheelPositions = 64;
	private static IEnumerable<int> range = System.Linq.Enumerable.Range(0, __c_numberOfWheelPositions).ToArray();
	private static IEnumerable<int> __ror = range.Skip(__c_numberOfWheelPositions-8).Concat(range.Take(__c_numberOfWheelPositions-8));
    private static IEnumerable<int> __l_top    = __ror.Skip(0 * __c_numberOfWheelPositions/8).Take(__c_numberOfWheelPositions/8);
	private static IEnumerable<int> __l_right  = __ror.Skip(1 * __c_numberOfWheelPositions/8).Take(__c_numberOfWheelPositions/8);
	private static IEnumerable<int> __l_bottom = __ror.Skip(2 * __c_numberOfWheelPositions/8).Take(__c_numberOfWheelPositions/8);
	private static IEnumerable<int> __l_left   = __ror.Skip(3 * __c_numberOfWheelPositions/8).Take(__c_numberOfWheelPositions/8);
	//] = [list(x) for x in mit.divide(4, ror([i for i in range(0, self.__c_numberOfWheelPositions)], self.__c_numberOfWheelPositions // 8))]
    private static int __wheelMultiplier = 2;
    private static bool __dirUp   = false;
    private static bool __dirDown = false;
    private static bool __VR = false;

	private static bool T = false;
	private static bool R = false;
	private static bool B = false;
	private static bool L = false;
	private static int __max = 315;
	private static int __r = __max / 2;
	private static int delta_X = 0;
	private static int delta_Y = 0;
	private static int __wheelPos = 0;
	private static int wheelPos = 0;
 	

	private GearVRController() {}
	
	public static GearVRController getInstance() { if ( GearVRController.instance == null ) GearVRController.instance = new GearVRController(); return GearVRController.instance; }
	
	// # circle segments from 0 .. self.__c_numberOfWheelPositions clockwise
    private static int fwheelPos(int x, int y)
	{
        int pos = 0;
        if (x == 0 && y == 0) { pos = -1; }
		System.Numerics.Complex cnum = new System.Numerics.Complex(x-157, y-157);
        pos = (int) System.Math.Floor((cnum.Phase * 180 / System.Math.PI) / 360.0 * __c_numberOfWheelPositions);
        return pos;
	}

	private void SelectedCharacteristic_ValueChanged(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic sender, Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
	{
		if (sender != GearVRController.data_characteristic) { return; }
		byte[] byte_values = new byte[args.CharacteristicValue.Length];
		Windows.Storage.Streams.DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(byte_values);		
		SendKeepAlive();
		
		axisX   = (((byte_values[54] & 0xF)  << 6) + ((byte_values[55] & 0xFC) >> 2)) & 0x3FF;
        axisY   = (((byte_values[55] & 0x3)  << 8) + ((byte_values[56] & 0xFF) >> 0)) & 0x3FF;
        accelX  = (int) (((byte_values[4]  << 8) + byte_values[5])  * 10000.0 * 9.80665 / 2048.0);
        accelY  = (int) (((byte_values[6]  << 8) + byte_values[7])  * 10000.0 * 9.80665 / 2048.0);
        accelZ  = (int) (((byte_values[8]  << 8) + byte_values[9])  * 10000.0 * 9.80665 / 2048.0);
        gyroX   = (int) (((byte_values[10] << 8) + byte_values[11]) * 10000.0 * 0.017453292 / 14.285);
        gyroY   = (int) (((byte_values[12] << 8) + byte_values[13]) * 10000.0 * 0.017453292 / 14.285);
        gyroZ   = (int) (((byte_values[14] << 8) + byte_values[15]) * 10000.0 * 0.017453292 / 14.285);
        magnetX = (int) (((byte_values[32] << 8) + byte_values[33]) * 0.06);
        magnetY = (int) (((byte_values[34] << 8) + byte_values[35]) * 0.06);
        magnetZ = (int) (((byte_values[36] << 8) + byte_values[37]) * 0.06);

        triggerButton    = ((byte_values[58] &  1) ==  1);
        homeButton       = ((byte_values[58] &  2) ==  2);
        backButton       = ((byte_values[58] &  4) ==  4);
        touchpadButton   = ((byte_values[58] &  8) ==  8);
        volumeUpButton   = ((byte_values[58] & 16) == 16);
        volumeDownButton = ((byte_values[58] & 32) == 32);
        NoButton         = ((byte_values[58] & 64) == 64);
		
		if (DIAG.Checked) {
			BTN1.Checked = triggerButton;
			BTN2.Checked = homeButton;
			BTN3.Checked = backButton;
			BTN4.Checked = touchpadButton;
			BTN5.Checked = volumeUpButton;
			BTN6.Checked = volumeDownButton;
			BTN7.Checked = NoButton;
			
			TB1.Text = $"{axisX}";
			TB2.Text = $"{axisY}";
			TB3.Text = $"{accelX}";
			TB4.Text = $"{accelY}";
			TB5.Text = $"{accelZ}";
			TB6.Text = $"{gyroX}";
			TB7.Text = $"{gyroY}";
			TB8.Text = $"{gyroZ}";
			TB9.Text = $"{magnetX}";
			TB10.Text = $"{magnetY}";
			TB11.Text = $"{magnetZ}";
		}
		
		switch (GearVRController.SB.SelectedIndex) {
			case 0: // Presentation
				break;
			case 1: // Browsing
				break;
			case 2: // Telco
				if ( touchpadButton) keybd_event((byte)KeysVolumeMute, 0, 0x0001, 0);
				if (!touchpadButton) keybd_event((byte)KeysVolumeMute, 0, 0x0002, 0);
				return;
				break;
			default:
				break;
		}
		
		if (touchpadButton && __trig) {
            __useWheel = ! __useWheel;
            __useTouch = ! __useTouch;
            __trig = false;
		}
        else if (!touchpadButton && !__trig) {
            __trig = true;
		}

        outerCircle = System.Math.Pow(axisX - __r, 2) + System.Math.Pow(axisY - __r, 2) > System.Math.Pow(__r - odelta, 2);
        wheelPos = GearVRController.fwheelPos(axisX, axisY);
        T = outerCircle && __l_top.Contains(__wheelPos)   ; // Top
        R = outerCircle && __l_right.Contains(__wheelPos) ; // Right
        B = outerCircle && __l_bottom.Contains(__wheelPos); // Bottom
        L = outerCircle && __l_left.Contains(__wheelPos)  ; // Left

        delta_X = axisX - __axisX;
        delta_Y = axisY - __axisY;
        delta_X = (int) System.Math.Round(delta_X * 1.2);
        delta_Y = (int) System.Math.Round(delta_Y * 1.2);
		
		if (__useWheel) {
	        if (System.Math.Abs(__wheelPos - wheelPos) > 1 && System.Math.Abs((__wheelPos + 1) % __c_numberOfWheelPositions - (wheelPos + 1) % __c_numberOfWheelPositions) > 1) {
	            __wheelPos = wheelPos;
	            return;
			}
	        if ((__wheelPos - wheelPos) == 1 || ((__wheelPos + 1) % __c_numberOfWheelPositions - (wheelPos + 1) % __c_numberOfWheelPositions) == 1) {
	            __wheelPos = wheelPos;
				foreach (int i in System.Linq.Enumerable.Range(0, __wheelMultiplier)) {
					keybd_event((byte)0x26, 0, 0, 0);
	                //System.Windows.Forms.SendKeys.Send("{UP}");
				}
	            return;
			}
	        if ((wheelPos - __wheelPos) == 1 || ((wheelPos + 1) % __c_numberOfWheelPositions - (__wheelPos + 1) % __c_numberOfWheelPositions) == 1) {
	            __wheelPos = wheelPos;
				foreach (int i in System.Linq.Enumerable.Range(0, __wheelMultiplier)) {
					keybd_event((byte)0x28, 0, 0, 0);
	                //System.Windows.Forms.SendKeys.Send("{DOWN}");
				}
	            return;
			}
	        return;
		}
		
		if (__useTouch) {
            if (System.Math.Abs(delta_X) < 50) {
                if (axisX == 0 && axisY == 0) {
                    __dirUp = false;
                    __dirDown = false;
                    __axisX = axisX;
                    __axisY = axisY;
                    return;
				}
                else if (!__dirUp && !__dirDown) {
                    if (delta_X > 0) {
                        __dirUp = true;
					}
                    else {
                        __dirDown = true;
					}
				}
                if (__dirUp && System.Math.Abs(delta_X) > 1) {
	                System.Windows.Forms.SendKeys.Send("{UP}");
				}
                else if (__dirDown && System.Math.Abs(delta_X) > 1) {
	                System.Windows.Forms.SendKeys.Send("{DOWN}");
				}
			}	
            __axisX = axisX;
            __axisY = axisY;
            //print(delta_X)
            return;
		}
		
		if (triggerButton)
            { System.Windows.Forms.SendKeys.Send("{LEFT}"); }
        else
            { /* System.Windows.Forms.SendKeys.Send("{LEFT}");*/ }

        if (homeButton && __volbtn) {
            System.Windows.Forms.SendKeys.Send("{%HOME}");
            __volbtn = false;
            return;
		}

        if (backButton && __volbtn) {
            System.Windows.Forms.SendKeys.Send("{%LEFT}");
            __volbtn = false;
            return;
		}
		
		if (volumeDownButton && __volbtn) {
			keybd_event((byte)KeysVolumeDown/*Keys.VolumeDown*/, 0, 0, 0); // decrease volume
            __volbtn = false;
            return;
		}

        if (volumeUpButton && __volbtn) {
            __volbtn = false;
			keybd_event((byte)KeysVolumeUp/*Keys.VolumeUp*/, 0, 0, 0); // increase volume
            __volbtn = false;
            return;
		}
		
		if (NoButton) {
            __volbtn = true;
            __tchbtn = true;
            __trig   = true;
		}

        // No standalone button handling behind this point

        if (axisX == 0 && axisY == 0) {
            __reset = true;
            return;
		}

        if (__reset) {
            __reset = false;
            __axisX = axisX;
            __axisY = axisY;
            __altX = gyroX;
            __altY = gyroY;
            return;
		}
		
		mouse_event(MOUSEEVENTF_MOVE, delta_X, delta_Y, 0, 0);

        __axisX = axisX;
        __axisY = axisY;
        __altX = gyroX;
        __altY = gyroY;
	} 
	
	private void movePointerREL(int dx, int dy) {
        int incx = (dx == 0)?0:(int)System.Math.Round((double)(0 - dx)/System.Math.Abs(dx));
        int incy = (dy == 0)?0:(int)System.Math.Round((double)(0 - dy)/System.Math.Abs(dy));

        while (dx != 0 || dy != 0) {
            if (dx != 0) {
                //self.__device.emit(uinput.REL_X, -incx, syn = True)
                dx += incx;
			}
            if (dy != 0) {
                //self.__device.emit(uinput.REL_Y, -incy, syn = True)
                dy += incy;
			}
		}
	}

	private async void SendToCharacteristic(byte[] value, int repeat)
	{
		if ( GearVRController.setup_characteristic == null ) return;
		try {
			while (repeat-- > 0) { await GearVRController.setup_characteristic.WriteValueAsync(Windows.Security.Cryptography.CryptographicBuffer.CreateFromByteArray(value)); }
		} catch (Exception e) {
			EXCEPTION.Text = "Exception: write to characteristic " + e.Message;
		}
	}
	
	private void SendKeepAlive()
	{
		/*if (DateTime.Now > GearVRController.time):
            self.__time = round(time.time()) + 10*/
		//SendToCharacteristic(new byte[] {0x04, 0x00}, 4);
	}

	public void Disconnect()
	{
		SendToCharacteristic(new byte[] {0x00, 0x00}, 3);
	}

	private async void Get_Characteristics(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService myService)
	{
		var accessRequestResponse = await myService.RequestAccessAsync();
	    Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicsResult CharResult = await myService.GetCharacteristicsAsync();// GetCharacteristicsAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached)
	    if (CharResult.Status == Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success)
	    {
	        foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic c in CharResult.Characteristics)
	        {
	            if (c.Uuid == guid_controller_setup_characteristic)
	            {
	                GearVRController.setup_characteristic = c;
					continue;
	            }
	            if (c.Uuid == guid_controller_data_characteristic)
	            {
	                GearVRController.data_characteristic = c;
					continue;
	            }
	        }
	        if ( GearVRController.setup_characteristic != null && GearVRController.data_characteristic != null ) try
	        {
				SendToCharacteristic(new byte[] {0x01, 0x00}, 3);
				SendToCharacteristic(new byte[] {0x06, 0x00}, 1);
				SendToCharacteristic(new byte[] {0x07, 0x00}, 1);
				SendToCharacteristic(new byte[] {0x08, 0x00}, 3);
	            // Write the ClientCharacteristicConfigurationDescriptor in order for server to send notifications.               
	            var result = await data_characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(Windows.Devices.Bluetooth.GenericAttributeProfile.GattClientCharacteristicConfigurationDescriptorValue.Notify);
	            if (result == Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success)
	            {
					//Debug.WriteLine("Successfully registered for notifications");
	                data_characteristic.ValueChanged += SelectedCharacteristic_ValueChanged;
	            }
	            else
	            {
					EXCEPTION.Text = $"Error registering for notifications: {result}";
	            }
	        }
	        catch (Exception ex)
	        {
	            // This usually happens when not all characteristics are found
	            // or selected characteristic has no Notify.
				//Debug.WriteLine("Exception:" + ex.Message);
	            await System.Threading.Tasks.Task.Delay(100);
	            Get_Characteristics(myService); //try again
	                                            //!!! Add a max try counter to prevent infinite loop!!!!!!!
	        }
	    }
	    else
	    {
			EXCEPTION.Text = "Restricted service. Can't read characteristics. Press Control+Shift+F5 before starting program again.";
	    }
	}


	private async void PairingRequestedHandler(Windows.Devices.Enumeration.DeviceInformationCustomPairing sender, Windows.Devices.Enumeration.DevicePairingRequestedEventArgs args)
	{
	    switch (args.PairingKind)
	    {
	        case Windows.Devices.Enumeration.DevicePairingKinds.ConfirmOnly:
	            // Windows itself will pop the confirmation dialog as part of "consent" if this is running on Desktop or Mobile
	            // If this is an App for 'Windows IoT Core' where there is no Windows Consent UX, you may want to provide your own confirmation.
	            args.Accept();
	            break;

	        case Windows.Devices.Enumeration.DevicePairingKinds.ProvidePin:
	            var collectPinDeferral = args.GetDeferral();
	            string pin = "0000";
	            args.Accept(pin);
	            collectPinDeferral.Complete();
	            break;
	    }
	}
			
	async public void Pair_Connect() // https://stackoverflow.com/questions/35461817/uwp-bluetoothdevice-frombluetoothaddressasync-throws-0x80070002-exception-on-no
	{
		Windows.Devices.Bluetooth.BluetoothLEDevice device = await Windows.Devices.Bluetooth.BluetoothLEDevice.FromBluetoothAddressAsync(i_vrcontrollerMAC);
		Windows.Devices.Enumeration.DeviceInformation infDev = device.DeviceInformation; //We need this aux object to perform pairing
	    Windows.Devices.Enumeration.DevicePairingKinds ceremoniesSelected = Windows.Devices.Enumeration.DevicePairingKinds.ConfirmOnly | Windows.Devices.Enumeration.DevicePairingKinds.ProvidePin; //Only confirm pairing, we'll provide PIN from app
	    Windows.Devices.Enumeration.DevicePairingProtectionLevel protectionLevel = Windows.Devices.Enumeration.DevicePairingProtectionLevel.None; //Encryption; //Encrypted connection
	    Windows.Devices.Enumeration.DeviceInformationCustomPairing customPairing = infDev.Pairing.Custom; //Our app takes control of pairing, not OS
	    customPairing.PairingRequested += PairingRequestedHandler; //Our pairing request handler
	    Windows.Devices.Enumeration.DevicePairingResult result = await customPairing.PairAsync(ceremoniesSelected, protectionLevel); //launc pairing
	    customPairing.PairingRequested -= PairingRequestedHandler;
	    if ((result.Status == Windows.Devices.Enumeration.DevicePairingResultStatus.Paired) || (result.Status == Windows.Devices.Enumeration.DevicePairingResultStatus.AlreadyPaired))
	    {
			Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceServicesResult serviceResult = null;
			//Debug.WriteLine("success, now we are able to open a socket");
	        //success, now we are able to open a socket
			if (device != null)
			 {
			     int servicesCount = 3;//Fill in the amount of services from your device!!!!!
			     int tryCount = 0;
			     bool connected = false;
			     while (!connected)//This is to make sure all services are found.
			     {
			         tryCount++;
			         serviceResult = await device.GetGattServicesAsync();
			         if (serviceResult.Status == Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success && serviceResult.Services.Count >= servicesCount)
			         {
			             connected = true;
			             //Debug.WriteLine("Connected in " + tryCount + " tries");
			         }
			         if (tryCount > 5)//make this larger if faild
			         {
			             EXCEPTION.Text = "Failed to connect to device ";
			             return;
			         }
			     }
			     if (connected)
			     {
			         for (int i = 0; i < serviceResult.Services.Count; i++)
			         {
			             var service = serviceResult.Services[i];
			             //This must be the service that contains the Gatt-Characteristic you want to read from or write to !!!!!!!.
			             if (service.Uuid.ToString() == "4f63756c-7573-2054-6872-65656d6f7465")
			             {
						 	//Debug.WriteLine("Get_Characteristics(service);");
							Get_Characteristics(service);
			                break;
			             }
			         }
			     }
			 }
	    }
	    else
	    {
			EXCEPTION.Text = "pairing failed";
	    }
	}
}

void Main()
{
	var dccontent = new List<object>();
	dc = new DumpContainer (Util.VerticalRun (dccontent)).Dump();
	dccontent.Add(new LINQPad.Controls.WrapPanel (
		GearVRController.BTN1 = new LINQPad.Controls.CheckBox("Trigger"),
		GearVRController.BTN2 = new LINQPad.Controls.CheckBox("Home"),
		GearVRController.BTN3 = new LINQPad.Controls.CheckBox("Back"),
		GearVRController.BTN4 = new LINQPad.Controls.CheckBox("Touch"),
		GearVRController.BTN5 = new LINQPad.Controls.CheckBox("VolumeUp"),
		GearVRController.BTN6 = new LINQPad.Controls.CheckBox("VolumeDown"),
		GearVRController.BTN7 = new LINQPad.Controls.CheckBox("NoButton"),
		new LINQPad.Controls.Button("Power OFF", onClick:(sender) => { GearVRController.getInstance().Disconnect(); }),
		GearVRController.DIAG = new LINQPad.Controls.CheckBox("Diagnostics" ) { Checked = true }
		));
	dccontent.Add(new LINQPad.Controls.WrapPanel (
		new LINQPad.Controls.TextBox("Axis") { Width = "60px" }, GearVRController.TB1 = new LINQPad.Controls.TextBox("") { Width = "60px" },
		GearVRController.TB2 = new LINQPad.Controls.TextBox("") { Width = "60px" }));
	dccontent.Add(new LINQPad.Controls.WrapPanel (
		new LINQPad.Controls.TextBox("Accel") { Width = "60px" }, GearVRController.TB3 = new LINQPad.Controls.TextBox("") { Width = "60px" },
		GearVRController.TB4 = new LINQPad.Controls.TextBox("") { Width = "60px" },
		GearVRController.TB5 = new LINQPad.Controls.TextBox("") { Width = "60px" }));
	dccontent.Add(new LINQPad.Controls.WrapPanel (
		new LINQPad.Controls.TextBox("Gyro") { Width = "60px" }, GearVRController.TB6 = new LINQPad.Controls.TextBox("") { Width = "60px" },
		GearVRController.TB7 = new LINQPad.Controls.TextBox("") { Width = "60px" },
		GearVRController.TB8 = new LINQPad.Controls.TextBox("") { Width = "60px" }));
	dccontent.Add(new LINQPad.Controls.WrapPanel (
		new LINQPad.Controls.TextBox("Magnet") { Width = "60px" }, GearVRController.TB9 = new LINQPad.Controls.TextBox("") { Width = "60px" },
		GearVRController.TB10 = new LINQPad.Controls.TextBox("") { Width = "60px" },
		GearVRController.TB11 = new LINQPad.Controls.TextBox("") { Width = "60px" }));
	dccontent.Add(GearVRController.SB = new LINQPad.Controls.SelectBox(new string[] {"Presentation", "Browsing", "Telco"}));
	dccontent.Add(GearVRController.EXCEPTION = new LINQPad.Controls.TextBox("") { Width = "1000px" });

	GearVRController.SB.Width = (3 + (float)GearVRController.SB.Options [0].ToString().Length / 2) + "em";
	GearVRController.SB.SelectionChanged += (sender, args) =>
		{
			Debug.WriteLine("New mode selected");
			//GearVRController.SB.SelectedIndex = 0;
		};
	gdccontent = new List<object>();
	gdccontent.Add(dc);
	dc.Refresh();
	
	GearVRController gvrc = GearVRController.getInstance();
	gvrc.Pair_Connect();/**/
}
