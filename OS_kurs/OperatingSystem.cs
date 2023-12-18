using OS_kurs.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OS_kurs
{
    internal class OperatingSystem
    {
        private const int QuantumOfTime = 10;
        private int MaxID = 0;
        private volatile List<ProcessOS> ProcessQueue = new List<ProcessOS>();

        public OperatingSystem()
        {
            OperationsWithProcesses();
        }
        public bool IsNotEmpty() { return ProcessQueue.Count != 0; }
        private async void OperationsWithProcesses()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (ProcessQueue.Count != 0)
                    {
                        SortQueque();
                        ProcessOS process = ProcessQueue.First();
                        process.Status = 'R';
                        RunOperation(process);
                    }
                }
            });
        }
        private void SortQueque()
        {
            ProcessQueue = ProcessQueue
                .OrderBy(proc => proc.Priority)
                .ThenBy(proc => proc.Time)
                .ToList();
        }
        private void RunOperation(ProcessOS process)
        {
            if (QuantumOfTime > process.Time)
                ProcessQueue.Remove(process);
            else
                process.Time -= QuantumOfTime;

            Thread.Sleep(Math.Min(QuantumOfTime, process.Time) * 10);

            if (process.Time == 0)
                process.Status = 'Z';
        }
        public void AddNewProcess(int time, sbyte pri = 0)
        {
            int pid = MaxID++;
            ProcessQueue.Add(new ProcessOS(pid, time, pri));
        }
        private ProcessOS GetProcessByID(int id)
        {
            if (ProcessQueue.Count != 0)
                return ProcessQueue.Where(process => process.ID == id).First();
            return null;
        }
        public void ChangeTime(int id, int time) { GetProcessByID(id).Time = time; }
        public void ChangePriorety(int id, sbyte pri) { GetProcessByID(id).Priority = pri; }
        public string GetProcess() 
        {
            string res = "ID\tTime\tStatus\tPriorety\n";
            List<ProcessOS> plist = ProcessQueue.OrderBy(proc => proc.ID).ToList();
            foreach (var process in plist)
            {
                res += process.ID.ToString() + "\t";
                res += process.Time.ToString() + "\t";
                res += process.Status.ToString() + "\t";
                res += process.Priority.ToString() + "\n";
            }
            return res;
        }
        public void Remove(int id) { ProcessQueue.Remove(GetProcessByID(id)); }
    }
}
