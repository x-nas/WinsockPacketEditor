using System;
using System.Management;

namespace ProcessInjector
{
    class ComputerInfo
    {
        //获取网卡Mac地址
        public string GetMACInfo()
        {
            string madAddr = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc2 = mc.GetInstances();

            foreach (ManagementObject mo in moc2)
            {
                if (Convert.ToBoolean(mo["IPEnabled"]) == true)
                {
                    madAddr = mo["MacAddress"].ToString();
                    madAddr = madAddr.Replace(':', '-');
                }

                mo.Dispose();
            }

            return madAddr;
        }

        //获取CPU型号
        public string GetCPUInfo()
        {
            string CPUName = "";

            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_Processor");

            foreach (ManagementObject mo in mos.Get())
            {
                CPUName += mo["Name"].ToString() + ",";
            }

            mos.Dispose();

            CPUName = "CPU：" + CPUName.TrimEnd(',');

            return CPUName;
        }

        //获取显卡数量，及显卡名称
        public string GetGPUInfo()
        {
            string DisplayName = "";           

            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_VideoController");
           
            foreach (ManagementObject mo in mos.Get())
            {                
                DisplayName += mo["Name"].ToString() + ",";
            }

            mos.Dispose();

            DisplayName = "显卡：" + DisplayName.TrimEnd(',');

            return DisplayName;
        }
        
        //获取内存条数量，及大小
        public string GetMEMInfo()
        {
            string PhysicalMemory = "内存：";

            ManagementClass m = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection mn = m.GetInstances();            

            double capacity = 0.0;          

            foreach (ManagementObject mo1 in mn)
            {                
                capacity += ((Math.Round(Int64.Parse(mo1.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024.0, 1)));                
            }

            mn.Dispose();
            m.Dispose();

            PhysicalMemory += capacity.ToString() + "G";

            return PhysicalMemory;
        }

        //硬盘大小
        public string GetHDInfo()
        {
            string DiskDrive = "硬盘：";

            ManagementClass m = new ManagementClass("win32_DiskDrive");
            ManagementObjectCollection mn = m.GetInstances();

            double capacity = 0.0;

            foreach (ManagementObject mo1 in mn)
            {
                capacity += Int64.Parse(mo1.Properties["Size"].Value.ToString()) / 1024 / 1024 / 1024;
            }

            mn.Dispose();
            m.Dispose();

            DiskDrive += capacity.ToString() + "G";

            return DiskDrive;
        }
    }
}
