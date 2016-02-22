using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Managers
{
	interface InputDevice
	{
		void Connect(string portName);
		void Disconnect();
		void TriggerDataRead();
	}
}
