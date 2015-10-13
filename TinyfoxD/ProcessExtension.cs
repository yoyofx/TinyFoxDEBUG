using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using Process = EnvDTE.Process;

namespace TinyfoxD.Debug
{

    public static class ProcessExtension
    {
       
     

        #region -- Public Methods --

        /// <summary>
        /// Attaches Visual Studio (2015) to the process.
        /// </summary>
        /// <param name="process">The process.</param>
        public static bool Attach(this System.Diagnostics.Process process)
        {




            try
            {

                
                //Processes processes = dte.Debugger.LocalProcesses;
                //foreach (Process proc in processes.Cast<Process>()
                //         .Where(proc => proc.Name.IndexOf(process.ProcessName) != -1))
                //{
                //    proc.Attach();
                //    break;
                //}
            }
            catch (COMException)
            {
                return false;
            }

            return true;
        }



        public static bool FindProcess(string processName,out System.Diagnostics.Process[] foundedProcess)
        {
            foundedProcess = System.Diagnostics.Process.GetProcessesByName(processName);
            return foundedProcess.Length > 0;
        }


        public static void KillProcess(System.Diagnostics.Process[] foundedProcess)
        {
            foreach (var item in foundedProcess)
            {
                item.Kill();
            }
        }
        #endregion

    }






}

