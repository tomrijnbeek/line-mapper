using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using Bearded.Utilities.IO;
using LineMapper;


ensureInvariants();

var logger = new Logger {MirrorToConsole = true};
var window = new ProgramWindow(logger);
window.Initialize();
window.Run();


static void ensureInvariants()
{
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    Directory.SetCurrentDirectory(exeDir ?? throw new InvalidOperationException());
}
