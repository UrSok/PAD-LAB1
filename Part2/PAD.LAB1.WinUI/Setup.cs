﻿using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.WinUI
{
    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override ILoggerProvider? CreateLogProvider()
        {
            return null;
        }

        protected override ILoggerFactory? CreateLogFactory()
        {
            return null;
        }
    }
}
