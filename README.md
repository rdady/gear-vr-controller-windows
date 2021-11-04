# gear-vr-controller-windows

Finally on windows!
The Samsung Gear VR Controller can be utilised as an emulated mouse (and gamepad).

This project is written in C#, but I decided not to provide any binaries.
Ever heard about LINQPad - The .NET Programmer's Playground https://www.linqpad.net ?
The supplied gear-vr-controller.linq can be opened and executed with LINQPad 5.
Works better with corporate machines.

Program is "AS IS", and I take no liabilities for any damage it causes.

This is an initial commit, WIP.

# Windows 10 - How to use

Bluetooth pairing + connection is done automatically by the program.
No unpairing is necessary. Next time automatic connection - already paired state.

Linqpad will offer to download missing nuget packages, say YES to it.

# LinqPad configuration

* App.config

<pre><code>
&lt;configuration&gt;
  &lt;Capabilities&gt;
    &lt;DeviceCapability Name="bluetooth" /&gt;
  &lt;/Capabilities&gt;
  &lt;appSettings&gt;
    &lt;add key="SendKeys" value="SendInput" /&gt;
  &lt;/appSettings&gt;
&lt;/configuration&gt;
</code></pre>

* Additional Namespace imports

<pre><code>
System
System.Linq
System.Runtime.InteropServices
</code></pre>

  
* Additional references

<pre><code>
<MyDocuments>\LINQPad Queries\GATT\AudioSwitcher.AudioApi.CoreAudio.dll<br />
<MyDocuments>\LINQPad Queries\GATT\AudioSwitcher.AudioApi.dll<br />
<RuntimeDirectory>\System.Linq.dll<br />
<RuntimeDirectory>\System.Numerics.dll<br />
<RuntimeDirectory>\System.Runtime.dll<br />
<RuntimeDirectory>\System.Runtime.InteropServices.WindowsRuntime.dll<br />
<RuntimeDirectory>\System.Runtime.Numerics.dll<br />
<ProgramFilesX86>\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5\System.Runtime.WindowsRuntime.dll<br />
<RuntimeDirectory>\System.Threading.Tasks.dll<br />
<RuntimeDirectory>\System.Windows.Forms.dll<br />
<ProgramFilesX86>\Windows Kits\10\References\10.0.19041.0\Windows.Foundation.FoundationContract\4.0.0.0\Windows.Foundation.FoundationContract.winmd<br />
<ProgramFilesX86>\Windows Kits\10\References\10.0.19041.0\Windows.Foundation.UniversalApiContract\10.0.0.0\Windows.Foundation.UniversalApiContract.winmd<br />
<ProgramFilesX86>\Windows Kits\10\UnionMetadata\Facade\Windows.WinMD<br />
</code></pre>
  
