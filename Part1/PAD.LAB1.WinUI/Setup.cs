using MvvmCross.Platforms.Wpf.Core;
using Microsoft.Extensions.Logging;

namespace PAD.LAB1.WinUI
{
    internal class Setup : MvxWpfSetup<Core.App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return null;//new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            /* serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .CreateLogger();*/

            return null;// new SerilogLoggerFactory();
        }
    }
}
