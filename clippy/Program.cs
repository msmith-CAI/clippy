using System.Diagnostics;

//var arg = args[0];
var arg = Console.ReadLine() ?? null;

if (!string.IsNullOrWhiteSpace(arg))
{
    SetClipboard(arg);
    Console.WriteLine($"Value \'{arg}\' copied to the clipboard.");
}

static void SetClipboard(string text)
{
    (string fileName, string[] args)[] candidates =
    [
        ("wl-copy", []),                          // Wayland
        ("xclip", ["-selection", "clipboard"]),   // X11
        ("xsel", ["--clipboard", "--input"]),     // X11 alternative
    ];

    foreach (var (fileName, args) in candidates)
    {
        try
        {
            var psi = new ProcessStartInfo(fileName)
            {
                RedirectStandardInput = true,
                UseShellExecute = false,
            };
            foreach (var arg in args)
                psi.ArgumentList.Add(arg);

            using var process = Process.Start(psi);
            if (process is null)
                continue;

            process.StandardInput.Write(text);
            process.StandardInput.Close();
            process.WaitForExit();

            if (process.ExitCode == 0)
                return;
        }
        catch (Exception ex)
        {
            throw new Exception($"An exception occurred while copying the value - {ex.Message}", ex);
        }
    }

    throw new InvalidOperationException(
        "No clipboard utility found. Install one of: wl-clipboard, xclip, or xsel.");
}

