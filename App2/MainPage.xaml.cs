using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /*
        UInt32 vid = 0x0403;
        UInt32 pid = 0x6001;
        string ID;
        DeviceWatcher superMuttWatcher;
        DeviceInformation DevInfo;
        */
        //FTDI myFTDIDevice = new FTDI();
        bool Login_clicked = false;
        public MainPage()
        {
            this.InitializeComponent();
            
           /* string aqs = UsbDevice.GetDeviceSelector(vid, pid);
            
            superMuttWatcher = DeviceInformation.CreateWatcher(aqs);

            superMuttWatcher.Added += new TypedEventHandler<DeviceWatcher, DeviceInformation>
                                      (this.usb_Added);

            superMuttWatcher.Removed += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>
                                    (this.usb_Removed);
            
            superMuttWatcher.Start();*//*
              UInt32 ftdiDeviceCount = 0;
  FTD2XX_NET.FTDI.FT_STATUS ftStatus = FTD2XX_NET.FTDI.FT_STATUS.FT_OK;

  // Create new instance of the FTDI device class
  FTD2XX_NET.FTDI myFtdiDevice = new FTD2XX_NET.FTDI();
  // Determine the number of FTDI devices connected to the machine
  ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);
  // Check status
            
            if (ftStatus != FTD2XX_NET.FTDI.FT_STATUS.FT_OK) {
                //Log.WriteLog("Failed to get number of FTDI devices [" + ftStatus.ToString() + "]");
                Background = new SolidColorBrush(Colors.Blue);
                //return false;
             }
  // If no devices available, return
  if (ftdiDeviceCount == 0) {
      Background = new SolidColorBrush(Colors.Red);
    //Log.WriteLog("Failed to find any FTDI devices [" + ftStatus.ToString() + "]");
  //  return false;
  }
  // Allocate storage for device info list
  FTD2XX_NET.FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTD2XX_NET.FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];
  // Populate our device list
  ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);
  if (ftStatus != FTD2XX_NET.FTDI.FT_STATUS.FT_OK) {
      Background = new SolidColorBrush(Colors.Blue);
    //Log.WriteLog("Failed enumerate FTDI devices [" + ftStatus.ToString() + "]");
  //  return false;
  }
  // Open first device in our list by serial number
  ftStatus = myFtdiDevice.OpenBySerialNumber(ftdiDeviceList[0].SerialNumber);
  if (ftStatus != FTD2XX_NET.FTDI.FT_STATUS.FT_OK) {
    //Log.WriteLog("Failed to open device [" + ftStatus.ToString() + "]");
      Background = new SolidColorBrush(Colors.Red);
  //  return false;
  }*/
  // Finally, reset the port
  //myFtdiDevice.CyclePort();
 // return true;


            //FPGA_Configuration();
        }
        //FTDI device = new FTDI();
        //string port;
       /* private void FPGA_Configuration()
        {           
            device.GetCOMPort(out port);
            //if(!string.IsNullOrEmpty(port) && port.Equals(target) && device.IsOpen)
            //{
                device.CyclePort();
                device.ResetDevice();
                device.ResetPort();
            //}











            FTD2XX_NET.FTDI.FT_STATUS ft_status;
            System.Byte[] writing_bytes = new Byte[4096];
            System.Byte[] write_byte = new Byte[104857600];
            byte EDX005_State;
            UInt32 written_byte_num;
            System.IO.BinaryReader bit_file;
            System.IO.FileInfo bit_file_info;
            string bit_file_name;
            bool result;
            long bit_file_size, send_byte_size;
            int send_times, send_times_tail, i, j, byte_position;
            byte bit_data;
            written_byte_num = 0;
            EDX005_State = 0;
            ft_status = myFTDIDevice.OpenByDescription("ED-CONFIGHuMANDATA LTD. A");

            if (ft_status == FTD2XX_NET.FTDI.FT_STATUS.FT_OK)
            {
                ft_status = myFTDIDevice.ResetDevice();
                ft_status = myFTDIDevice.Purge(FTDI.FT_PURGE.FT_PURGE_RX | FTDI.FT_PURGE.FT_PURGE_TX);
                ft_status = myFTDIDevice.SetTimeouts(0, 0);
                ft_status = myFTDIDevice.SetLatency(100);
                ft_status = myFTDIDevice.SetBaudRate(1000000);

                //Reset EDX-005
                
                ft_status = myFTDIDevice.SetBitMode((byte)2, 1);
                write_byte[0] = (byte)0xfd;
                ft_status = myFTDIDevice.Write(write_byte, 1, ref written_byte_num);
                write_byte[0] = (byte)0xff;
                ft_status = myFTDIDevice.Write(write_byte, 1, ref written_byte_num);

                //DownLoad bit data to EDX-005

                bit_file_name = "（bitファイルのパス）";
                result = System.IO.File.Exists(bit_file_name);

                if (result == true)
                {
                    bit_file_info = new System.IO.FileInfo(bit_file_name);
                    bit_file_size = bit_file_info.Length;
                    send_byte_size = (bit_file_size * 8 * 2) + (512 * 2);
                    send_times = (int)(send_byte_size / 4096);
                    send_times_tail = (int)(send_byte_size - (4096 * send_times));

                    bit_file = new System.IO.BinaryReader(System.IO.File.OpenRead(bit_file_name));

                    byte_position = 0;
                    
                    for (i = 0; i < bit_file_size; i++)
                    {
                        bit_data = bit_file.ReadByte();
                        for (j = 0; j < 8; j++) 
                        {
                            if ((bit_data & (byte)0×80)!=0)
                            {
                                write_byte[byte_position] = 0×04 | 0xFA;
                                byte_position++;
                                write_byte[byte_position] = 0×05 | 0xFA;
                                byte_position++;
                            }
                            else
                            {
                                write_byte[byte_position] = 0×00 | 0xFA;
                                byte_position++;
                                write_byte[byte_position] = 0×01 | 0xFA;
                                byte_position++;
                            }
                            bit_data = (byte)(bit_data << 1);
                        }
                    }
                    bit_file.Close();

                    for (i = 0; i < 512; i++)
                    {
                        write_byte[byte_position] = 0×04 | 0xFA;
                        byte_position++;
                        write_byte[byte_position] = 0×05 | 0xFA;
                        byte_position++;
                    }
                    ft_status = myFTDIDevice.SetBitMode((byte)0×07,1);
                    byte_position = 0;
                    for (i = 0; i < send_times; i++)
                    {
                        for (j = 0; j < 4096; j++)
                        {
                            writing_bytes[j] = write_byte[byte_position + j];
                        }
                        ft_status = myFTDIDevice.Write(writing_bytes, 4096, ref written_byte_num);
                        byte_position = byte_position + 4096;
                    }

                    if (send_times_tail > 0)
                    {
                        for (j = 0; j < send_times_tail; j++)
                        {   
                            writing_bytes[j] = write_byte[byte_position + j];
                        }
                        ft_status = myFTDIDevice.Write(writing_bytes, send_times_tail, ref written_byte_num);
                        byte_position = byte_position + send_times_tail;
                    }
                    ft_status = myFTDIDevice.GetPinStates(ref EDX005_State);
                    EDX005_State = (byte)(EDX005_State & 0×08);
                    if (EDX005_State == (byte)0×08)
                    {
                        //（コンフィグレーション成功なので、その時の処理を記述）
                    }
                    else
                    {
                        //（コンフィグレーション失敗なので、その時の処理を記述）
                    }
                }
                else
                {
                    //（bitファイル自体が見つかんなかったので、その時の処理を記述）
                }
                ft_status = myFTDIDevice.Close();
            }
        }

        /*
        void usb_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            superMuttWatcher.Stop();
            throw new NotImplementedException();
        }

        void usb_Added(DeviceWatcher sender, DeviceInformation args)
        {
            ID = args.Id;
         //   var match = Findde
        }
        public static async void Creating_USB(DeviceWatcher sender, DeviceInformation args)
        {
            UsbDevice usb = null;
            usb = await UsbDevice.FromIdAsync(args.Id);
        }
        */

        private void LoginBtn_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            Login_clicked = true;
        }

        private void LoginBtn_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            if (Login_clicked)
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(Storage));
                Window.Current.Content = frame;
            }
        }
    }

}
