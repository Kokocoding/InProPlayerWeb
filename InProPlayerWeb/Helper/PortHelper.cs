using System.Diagnostics;
using System.IO.Ports;

namespace InProPlayerWeb.Helper
{
    public class PortHelper
    {
        private readonly SerialPort _sp;

        public static byte[] CmdFrequAndTemp = new byte[] { 0xAA, 0xA4, 0x00, 0x00, 0x00, 0x00, 0xBF };
        public static byte[] CmdPwrAndSwr = new byte[] { 0xAA, 0xA3, 0x00, 0x00, 0x00, 0x00, 0xEE };

        public string PortName = "Com3";
        public int BaudRate = 9600;

        public PortHelper()
        {
            _sp = new SerialPort();            
            OpenPort();
        }

        public void OpenPort()
        {
            if (_sp.IsOpen)
            {
                _sp.Close();
            }
            _sp.PortName = PortName;
            _sp.BaudRate = BaudRate;
            _sp.Parity = Parity.None;
            _sp.DataBits = 8;
            _sp.StopBits = StopBits.One;
            try
            {
                _sp.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void PortWrite(byte[] data)
        {
            if (!_sp.IsOpen)
            {
                try
                {
                    _sp.Open();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            if (_sp.IsOpen)
            {
                _sp.Write(data, 0, data.Length);
                _sp.Close();
                _sp.Dispose();
            }
        }

        public void PortRead(object sender, SerialDataReceivedEventArgs e)
        {
            int length = _sp.BytesToRead;
            byte[] buffer = new byte[length];
            _sp.Read(buffer, 0, length);
            var t = DateTime.Now.ToString();
            Task.Run(() =>
            {
                Debug.WriteLine($"{BitConverter.ToString(buffer)}");
            });
            _sp.Close();
            _sp.Dispose();
        }

        public byte[] CmdAllCall(bool isOn)
        {
            byte[] cmd = new byte[7] { 0xAA, 0xA1, 0xFF, 0xFF, 0xFF, 0xFF, 0xE0 };
            if (isOn) return cmd;
            else
            {
                cmd[2] = 0x00;
                cmd[3] = 0x00;
                cmd[4] = 0x00;
                cmd[5] = 0x00;
                cmd[6] = 0x6D;
                return cmd;
            }
        }
        public byte[] CreateCommand(bool[] boolList)
        {
            byte[] cmd = new byte[6];
            cmd[0] = 0xAA;
            cmd[1] = 0xA1;
            for (int i = 0; i < 20; i++)
            {
                if (boolList[i])
                {
                    int byteIndex = i / 8 + 2; // Calculate the byte index (offset by 3 for the header)
                    int bitIndex = i % 8;
                    cmd[byteIndex] |= (byte)(1 << bitIndex); // Set the appropriate bit in the byte
                }
            }
            return Crc32(cmd);
        }
        public byte[] Crc32(byte[] hex)
        {
            const byte polynomial = 0x8C;
            byte crc = 0x00;
            for (int i = 0; i < hex.Length; i++)
            {
                byte data = hex[i];
                for (int j = 0; j < 8; j++)
                {
                    bool bit = ((crc ^ data) & 0x01) != 0;
                    crc >>= 1;
                    if (bit)
                    {
                        crc ^= polynomial;
                    }
                    data >>= 1;
                }
            }
            byte[] result = new byte[hex.Length + 1];
            hex.CopyTo(result, 0);
            result[hex.Length] = crc;
            return result;
        }
    }
}
