namespace Space_Expedition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int count;
            Artifact[] artifacts = Artifacts_Manager.ReadFile(out count);
            Artifacts_Manager.StartApp(artifacts, ref count);
        }
    }
}
