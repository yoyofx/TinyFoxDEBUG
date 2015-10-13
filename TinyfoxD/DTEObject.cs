using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace TinyfoxD
{
    public class DTEObject
    {

        private const string progId = @"VisualStudio.DTE.14.0";     //2015 dte objects id

        private DTE mDTE = null;
        public DTEObject(DTE dte)
        {
            this.mDTE = dte;
        }


        public bool Attach(string processName)
        {
            var localProcess = GetLocalProcesses(proc => proc.Name.IndexOf(processName) != -1).FirstOrDefault();
            if (localProcess != null)
                localProcess.Attach();
            else return false;
            return true;
        }



        public void RebuildSolution()
        {
            this.mDTE.ExecuteCommand("Build.RebuildSolution", "");
        }


        public IEnumerable<Process> GetLocalProcesses(Func<Process,bool> whereExpress = null)
        {
            Processes processes = this.mDTE.Debugger.LocalProcesses;
            return processes.Cast<Process>().Where(whereExpress);
        }


        public static DTEObject GetCurrent()
        {
           return new DTEObject( GetOleInstance());
        }


        public static DTE GetOleInstance()
        {
            
            DTE dte;
            try
            {
                dte = (DTE)Marshal.GetActiveObject(progId);
            }
            catch (COMException)
            {
                return null;
            }

            return dte;
        }


        public static IEnumerable<DTE> GetOleInstances()
        {
            IRunningObjectTable rot;
            IEnumMoniker enumMoniker;
            int retVal = GetRunningObjectTable(0, out rot);

            if (retVal == 0)
            {
                rot.EnumRunning(out enumMoniker);

                IntPtr fetched = IntPtr.Zero;
                IMoniker[] moniker = new IMoniker[1];
                while (enumMoniker.Next(1, moniker, fetched) == 0)
                {
                    IBindCtx bindCtx;
                    CreateBindCtx(0, out bindCtx);
                    string displayName;
                    moniker[0].GetDisplayName(bindCtx, null, out displayName);
                    bool isVisualStudio = displayName.StartsWith("!VisualStudio");
                    if (isVisualStudio)
                    {
                        object outDte = null;
                        rot.GetObject(moniker[0],out outDte);
                        yield return (DTE)outDte;
                    }
                }
            }
        }

        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);


    }
}
