# clippy

A small .NET console utility for Linux that copies text from standard input to the system clipboard.

`clippy` reads a line from `stdin` and forwards it to the first available clipboard backend, making it easy to pipe or type text into the clipboard from the terminal.

## Features

- Reads input from standard input (`stdin`)
- Copies the provided text to the system clipboard
- Automatically tries common Linux clipboard utilities in order:
  1. `wl-copy` (Wayland)
  2. `xclip -selection clipboard` (X11)
  3. `xsel --clipboard --input` (X11 alternative)
- Prints a confirmation message on success
- Fails with a clear error if no supported clipboard utility is installed

## Requirements

### Runtime
- Linux
- One of the following clipboard utilities available on `PATH`:
  - [`wl-clipboard`](https://github.com/bugaevc/wl-clipboard) for Wayland sessions (`wl-copy`)
  - [`xclip`](https://github.com/astrand/xclip) for X11 sessions
  - [`xsel`](https://vergenet.net/~conrad/software/xsel/) for X11 sessions

### Development / building from source
- [.NET SDK 10.0](https://dotnet.microsoft.com/) (project targets `net10.0`)

## Project structure

- `clippy.slnx` – solution file
- `clippy/Program.cs` – application source
- `clippy/clippy.csproj` – project file
- `clippy/Properties/PublishProfiles/Release.pubxml` – publish profile for a self-contained Linux x64 build

## Build

From the repository root:

```bash
dotnet build clippy.slnx
```

Or build the project directly:

```bash
dotnet build clippy/clippy.csproj
```

## Run

Run from source:

```bash
dotnet run --project clippy/clippy.csproj
```

The program waits for a line on standard input, copies it to the clipboard, and prints:

```text
Value '<your text>' copied to the clipboard.
```

### Examples

Pipe text into `clippy`:

```bash
echo "hello world" | dotnet run --project clippy/clippy.csproj
```

Use with a built/published binary:

```bash
echo "hello world" | ./clippy
```

Type input interactively:

```bash
dotnet run --project clippy/clippy.csproj
# type text, then press Enter
```

## Publish

A `Release` publish profile is included at `clippy/Properties/PublishProfiles/Release.pubxml` with the following settings:

- `RuntimeIdentifier`: `linux-x64`
- `SelfContained`: `true`
- `PublishSingleFile`: `true`
- `PublishReadyToRun`: `true`
- `PublishTrimmed`: `true`

Publish using that profile:

```bash
dotnet publish clippy/clippy.csproj -p:PublishProfile=Release
```

This produces a self-contained single-file Linux x64 executable in the publish output directory.

## How it works

1. Reads a single line from `Console.ReadLine()`.
2. If the input is not empty/whitespace, it attempts to start a clipboard command:
   - `wl-copy`
   - `xclip -selection clipboard`
   - `xsel --clipboard --input`
3. Writes the input text to the selected command's standard input.
4. Returns success when the command exits with code `0`.
5. Throws an error if no supported clipboard utility is found.

## Notes

- Input is read as a single line from standard input (not command-line arguments).
- If no supported clipboard backend is installed, the app throws: `No clipboard utility found. Install one of: wl-clipboard, xclip, or xsel.`

## License

No license file is currently included in this repository.