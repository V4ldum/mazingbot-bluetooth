using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BluetoothLibrary.Droid
{
    internal class AndroidEV3BluetoothController : AndroidBluetoothController
    {
r;

        internal void PollInput()
        {
            Task t = Task.Factory.StartNew(async () =>
            {
                while (!_tokenSource.IsCancellationRequested)
                {
                    Stream stream = _socket.InputStream;

                    // if the stream is valid and ready
                    if (stream != null && stream.CanRead)
                    {
                        await stream.ReadAsync(_sizeBuffer, 0, _sizeBuffer.Length);

                        short size = (short)(_sizeBuffer[0] | _sizeBuffer[1] << 8);
                        if (size == 0)
                            return;

                        byte[] report = new byte[size];
                        await stream.ReadAsync(report, 0, report.Length);
                        if (ReportReceived != null)
                            ReportReceived(this, new ReportReceivedEventArgs { Report = report });
                    }
                }
            }, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }
}