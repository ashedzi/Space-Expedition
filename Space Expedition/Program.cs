namespace Space_Expedition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Current directory: " + AppDomain.CurrentDomain.BaseDirectory);
            //Console.WriteLine("Current directory is: " + Environment.CurrentDirectory);
            //Console.WriteLine("Files in current directory:");
            //foreach (var f in Directory.GetFiles(Environment.CurrentDirectory)) {
            //    Console.WriteLine(f);
            //}
            int count;
            Artifact[] artifacts = Artifacts_Manager.ReadFile(out count);
            Artifacts_Manager.StartApp(artifacts, ref count);
        }
    }
}
