using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp
{
    public interface IAndroidForegroundService
    {
        void Start();
        void Stop();
    }
}
