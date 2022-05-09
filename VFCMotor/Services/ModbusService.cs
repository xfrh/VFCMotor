using Code4Bugs.Utils.IO;
using Code4Bugs.Utils.IO.Modbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using VFCMotor.Models;

namespace VFCMotor
{
    public class ModbusService
    {
         public static string Portname { get; set; }

        public static bool SetValue(int dataAddress,int value)
        {
            try
            {
                using (var sp = new SerialPort())
                {
                    sp.PortName = Portname;
                    sp.BaudRate = 38400;
                    sp.Parity = Parity.None;
                    sp.DataBits = 8;
                    sp.StopBits = StopBits.One;
                    sp.ReadTimeout = 500;
                    sp.WriteTimeout = 500;
                    sp.Open();
                    var stream = new SerialStream(sp);
                    var responseBytes = stream.RequestFunc6(1, dataAddress, value);
                    int address = responseBytes.ToResponseFunc6().DataAddress;
                    sp.Close();
                    return address > 0;
                }
             
                
            }
            catch (Exception ex)
            {
                LogService.LogMessage("modbus setpower: " + ex.Message);
                return false;
            }
        }

      

        public static byte[] ReadMultiData(int addr, int count)
        {
            try
            {
                using (var sp = new SerialPort())
                {
                    sp.PortName = Portname;
                    sp.BaudRate = 38400;
                    sp.Parity = Parity.None;
                    sp.DataBits = 8;
                    sp.StopBits = StopBits.One;
                    sp.ReadTimeout = 500;
                    sp.WriteTimeout = 500;
                    sp.Open();
                    using (var stream = new SerialStream(sp))
                    {
                        var responseBytes = stream.RequestFunc3(1, addr, count);
                        var data = responseBytes.ToResponseFunc3().Data;
                        sp.Close();
                        return data;

                    }
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage("modbus readpower: " + ex.Message);
                return null;

            }
        }

    }
    
    }
