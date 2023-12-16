namespace OS_kurs.OS
{
    internal class ProcessOS
    {
        public int ID;
        public int Time;
        public sbyte Priority;
        public char Status;
        public ProcessOS(int id, int time, sbyte priority)
        {
            ID = id;
            Time = time;
            Priority = priority;
            Status = 'W';
        }
    }
}
