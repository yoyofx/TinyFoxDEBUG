using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TinyfoxD.Debug;

namespace TinyfoxD
{
    class Program
    {
        static void Main(string[] args)
        {
            DTEObject dteObject = null;
            Process process = null;
            var processName = "TinyFox";
            string solutionName = string.Empty;

            if (args.Length > 0)
                solutionName = args[0];

            Process[] foundedProcess;
            if(ProcessExtension.FindProcess(processName, out foundedProcess))
            {
                ProcessExtension.KillProcess(foundedProcess);
            }

            var attachToPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, processName + ".exe");
            if (File.Exists(attachToPath))
            {
                var psi = new ProcessStartInfo(attachToPath, "-p 8088") { WindowStyle = ProcessWindowStyle.Minimized };
                process = new Process() { StartInfo = psi };
                process.Start();

                var dteList= DTEObject.GetOleInstances();
                if (!string.IsNullOrEmpty(solutionName))
                {
                    var dte = dteList.FirstOrDefault(o => Path.GetFileName(o.Solution.FileName) == solutionName);
                    dteObject = new DTEObject(dte);
                }
                else
                {
                    int index = 0;
                    SelectSolution:
                        index = 0;
                        Console.WriteLine("选择要调试的解决方案名称:");
                        
                        foreach (EnvDTE.DTE dte in dteList)
                        {
                            try
                            {
                                var solutionDisplayName = Path.GetFileName(dte.Solution.FileName);
                                Console.WriteLine($"{index + 1}. {solutionDisplayName}");
                            }
                            catch { }  index++;
                        }

                        try
                        {
                            Console.Write("请选择序号:");
                            int num = 0;
                            if (Int32.TryParse(Console.ReadLine(), out num))
                            {
                                if (num > 0 && num <= index)
                                    dteObject = new DTEObject(dteList.ToArray()[num - 1]);
                                else
                                    goto SelectSolution;
                            }
                            else goto Exit;
                                
                        }
                        catch { }
                }


                if (dteObject.Attach( process.ProcessName) )
                {
                    Console.WriteLine("Attach to TinyFox successfully!");
                }
                else
                {
                    Console.WriteLine("Visual Studio 2015 Not Found!");
                }
            }
            else
            {
                Console.WriteLine("TinyFox Not Found!");
            }

            Console.WriteLine("Press the any key to continue!");
            var key = Console.ReadKey();
            Exit:
            if (process != null)            //exit
            {
                process.Kill();
                process.WaitForExit();
            }

        }
    }
}
