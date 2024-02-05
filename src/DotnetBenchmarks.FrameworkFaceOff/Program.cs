using BenchmarkDotNet.Running;

namespace DotnetBenchmarks.FrameworkFaceOff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
