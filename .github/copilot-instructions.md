# MOSA Project – Copilot Coding Agent Instructions

## What is MOSA?

MOSA (Managed Operating System Alliance) is an open-source project that natively executes .NET applications inside a virtual hypervisor or on bare-metal hardware. It consists of:

- **Compiler** – a high-quality, multithreaded, cross-platform, optimizing .NET compiler (CIL → IR → native code)
- **Kernel** – a small micro-kernel operating system (`Mosa.Kernel.BareMetal`)
- **Device Drivers Framework** – a modular device driver framework (`Mosa.DeviceSystem`, `Mosa.DeviceDriver`)
- **Tools** – GUI and CLI tools for launching, debugging, and exploring compiled code

The project targets **.NET 10** (`net10.0`), uses **C# 14**, and targets x86, x64, ARM32, and ARM64.

---

## Repository Layout

```
/
├── .github/workflows/   # CI definitions (builds.yml, docs.yml)
├── Demos/               # Demo project templates
├── Source/              # All C# source code
│   ├── Mosa.sln         # Main solution (Windows + Linux)
│   ├── Mosa.Linux.sln   # Linux-specific solution
│   ├── Directory.Build.props  # Shared build settings (TargetFramework, OutputPath, etc.)
│   ├── Mosa.Compiler.*  # Compiler core libraries
│   ├── Mosa.Kernel.BareMetal*  # Bare-metal kernel and platform-specific parts
│   ├── Mosa.Runtime*    # Runtime primitives (Pointer, GC, entry point)
│   ├── Mosa.Korlib      # MOSA's .NET standard library implementation
│   ├── Mosa.Plug.Korlib* # Compiler intrinsics / plugs for Korlib
│   ├── Mosa.DeviceSystem / Mosa.DeviceDriver  # Driver framework + drivers
│   ├── Mosa.FileSystem  # FAT12/FAT16 filesystem
│   ├── Mosa.Tool.*      # Developer tools (Launcher GUI, Explorer, Debugger, Compiler CLI)
│   ├── Mosa.Utility.*   # Shared utility libraries
│   ├── Mosa.UnitTests*  # Bare-metal unit test suites
│   ├── Mosa.BareMetal.* # Demo OS images (CoolWorld, TestWorld, Starter, etc.)
│   └── Docs/            # reStructuredText documentation (Sphinx)
├── Tests/               # Test runner scripts
├── Tools/               # External tools (QEMU, 7-Zip, NASM, etc.) used at runtime
└── bin/                 # Build output (all projects output here – gitignored)
```

All projects output to `../../bin` (relative to `Source/`) via `Directory.Build.props`.

---

## Key Project Groups

| Prefix | Purpose |
|---|---|
| `Mosa.Compiler.*` | Core compiler: `Common`, `Framework` (stages, IR, transforms), `MosaTypeSystem`, `x86`, `x64`, `ARM32`, `ARM64` |
| `Mosa.Kernel.BareMetal*` | Platform-agnostic kernel + platform-specific parts (`Intel`, `x86`, `x64`, `ARM32`, `ARM64`) |
| `Mosa.Runtime*` | Runtime primitives per platform |
| `Mosa.Korlib` + `Mosa.Plug.Korlib*` | Standard library + intrinsics/plugs |
| `Mosa.DeviceSystem` + `Mosa.DeviceDriver` | Abstraction + implementations |
| `Mosa.Tool.Launcher` | Avalonia-based GUI launcher |
| `Mosa.Tool.Launcher.Console` | CLI launcher (used in CI tests) |
| `Mosa.Tool.Explorer` + `.Avalonia` | IR/code explorer |
| `Mosa.Tool.Debugger` | QEMU-based GDB debugger |
| `Mosa.Utility.UnitTests` | Unit test runner utility |
| `Mosa.UnitTests` | Bare-metal unit tests (run via `Mosa.Utility.UnitTests`) |
| `Mosa.BareMetal.*` | Demo images: `CoolWorld`, `TestWorld`, `Starter` per platform |
| `Mosa.Workspace.*` | Experimental/playground projects |

---

## Building

### Prerequisites

- **.NET 9 SDK or later** – the project targets `net10.0` with `<RollForward>major</RollForward>` in `Directory.Build.props`. The CI installs the .NET 9 SDK and relies on roll-forward to target net10.0 when .NET 10 is available on the runner. For local development, install the .NET 9 SDK (minimum) or .NET 10 SDK if available.
- On Windows: MSBuild + NuGet CLI (for packaging/nuspec operations)
- On Linux/macOS: `dotnet` CLI only

### Build Commands

```bash
# Build everything (from repo root on Windows)
Compile.bat

# Build using dotnet CLI (cross-platform)
dotnet build Source/Mosa.sln

# Linux-specific (excludes Windows-only tools)
dotnet build Source/Mosa.Linux.sln

# Restore packages first if needed
dotnet restore Source/Mosa.sln
```

Build output goes to `bin/` at the repo root.

### Build Configuration Notes

- `Directory.Build.props` sets `TargetFramework=net10.0`, `LangVersion=14.0`, `Nullable=disable`, `ImplicitUsings=enable`
- `OutputPath` is `../../bin` for all projects (relative to `Source/`)
- `AppendTargetFrameworkToOutputPath` and `AppendRuntimeIdentifierToOutputPath` are both `false`
- `AssemblyVersion` is `2.6.1.0`

---

## Running Tests

### Unit Tests (dotnet xUnit)

Standard xUnit tests exist in `Mosa.Compiler.Common.xUnit` and `Mosa.Compiler.Framework.xUnit`:

```bash
dotnet test Source/Mosa.sln
```

### Bare-Metal Unit Tests (CI-style)

The bare-metal unit tests are run by `Mosa.Utility.UnitTests.dll` which compiles and runs code in QEMU:

```bash
# Run all unit tests at optimization level 0 (requires QEMU)
dotnet bin/Mosa.Utility.UnitTests.dll -check -o0 -counters artifact/counters.txt
```

### Demo/Functional Tests (CI-style)

Demo tests compile a bare-metal image and boot it in QEMU using `Mosa.Tool.Launcher.Console`:

```bash
# Linux example
dotnet bin/Mosa.Tool.Launcher.Console.dll bin/Mosa.BareMetal.TestWorld.x86.dll -o0 -check -test -output-counters

# Windows example  
bin\Mosa.Tool.Launcher.Console.exe bin\Mosa.BareMetal.TestWorld.x86.dll -o0 -check -test -output-counters
```

The `-check` flag verifies correctness; `-test` runs the built-in test suite; `-oN` sets the optimization level (0–9).

---

## CI Workflows

Defined in `.github/workflows/builds.yml`. Key jobs:

| Job | Platform | What it does |
|---|---|---|
| `windows-build` / `linux-build` / `macos-build` | All | `dotnet restore`, `dotnet build`, `dotnet test`, uploads artifacts |
| `windows-unit-testing` / `linux-unit-testing` / `macos-unit-testing` | All | Runs bare-metal unit tests at optimization levels 0–9 via QEMU |
| `windows-demo-testing` / `linux-demo-testing` / `macos-demo-testing` | All | Boots demo OS images in QEMU at optimization levels 0–9 |
| `linux-x64-compile-test` / `linux-arm32-compile-test` | Linux | Compilation-only tests for x64 and ARM32 targets |
| `windows-build-packaging` | Windows | Creates NuGet packages (only on push to `master`) |

The CI uses `dotnet-version: "9.0.x"` with `RollForward=major` enabling net10.0.

---

## Code Style

Defined in `Source/.editorconfig`:

- **Indentation**: tabs (size 4)
- **Line endings**: `insert_final_newline = true`
- **Trailing whitespace**: trimmed
- **Braces**: always on new line (`csharp_new_line_before_open_brace = all`)
- **var**: used for built-in types and when type is apparent
- **Modifier order**: `private, async, protected, public, internal, volatile, abstract, new, override, sealed, static, virtual, extern, readonly, unsafe, file, required`
- **File header**: every `.cs` file must start with: `// Copyright (c) MOSA Project. Licensed under the New BSD License.`
- **Nullable**: disabled project-wide
- **ImplicitUsings**: enabled

---

## Compiler Architecture

The compiler uses two nested pipelines:

1. **Compiler Pipeline** (per-application): sets up type system, iterates methods, links, emits binary
2. **Method Compiler Pipeline** (per-method): lowers CIL → IR → MIR → native opcodes

Intermediate representations:
- **CIL** – Common Intermediate Language (decoded from .NET assemblies via `dnlib`)
- **IR** – High-Level Intermediate Representation (platform-agnostic)
- **MIR** – Machine-specific IR (platform-specific instructions before register allocation)

Key namespaces/classes:
- `Mosa.Compiler.Framework.Compiler` – main compiler entry point
- `Mosa.Compiler.Framework.BaseCompilerStage` – base class for all compiler pipeline stages
- `Mosa.Compiler.Framework.BaseMethodCompilerStage` – base for method pipeline stages
- `Mosa.Compiler.Framework.BaseTransform` – base for IR/optimization transforms
- `Mosa.Compiler.Framework.Context` – represents a position in the instruction stream
- `Mosa.Compiler.MosaTypeSystem` – MOSA's type system (wraps `dnlib`)
- `Mosa.Compiler.Framework.IR` – IR instruction definitions

Optimization levels (set via `-oN`):
- `-o0` = no optimizations
- `-o1` through `-o9` = progressively more aggressive optimizations (SSA, SCCP, value numbering, inlining, bit tracking, loop-invariant code motion, devirtualization, etc.)

---

## Plugs

"Plugs" replace method implementations at compile time. They're used to:
- Provide platform-specific intrinsics for `Mosa.Korlib`
- Replace methods that can't be implemented in managed code (e.g., memory operations)

Plugs are decorated with `[MethodPlug]` attributes and live in `Mosa.Plug.Korlib*` projects.

---

## Adding New Code

When adding new files to existing projects:

1. Add the file to the appropriate `Source/Mosa.*` project directory — `.csproj` files usually include all `*.cs` files implicitly.
2. Ensure the file header is present: `// Copyright (c) MOSA Project. Licensed under the New BSD License.`
3. Use tabs for indentation, not spaces.
4. Build with `dotnet build Source/Mosa.sln` to verify no compilation errors.
5. If adding a new compiler transform, inherit from `BaseTransform` and register it in the appropriate stage.
6. If adding new unit tests (xUnit), add them to `Mosa.Compiler.Common.xUnit` or `Mosa.Compiler.Framework.xUnit`.
7. If adding new bare-metal test cases, add them to `Mosa.UnitTests` following the existing pattern with `[MosaUnitTest]` attributes.

---

## Known Caveats

- **Mosa.sln vs Mosa.Linux.sln**: On Linux, use `Mosa.Linux.sln` because some Windows-only tools (e.g., WinForms-based tools) are excluded.
- **QEMU required for functional/unit tests**: Bare-metal tests and demo tests require QEMU to be installed and in `PATH`.
- **bin/ directory**: All build outputs land in the repo-root `bin/` directory. This is intentional and gitignored.
- **Tools/ directory**: Contains pre-built third-party tools (QEMU, NASM, 7-zip, etc.) used by the launcher at runtime — do not confuse with `Mosa.Tool.*` source projects.
- **Nullable disabled**: The project deliberately disables nullable reference type checks (`<Nullable>disable</Nullable>`). Do not enable it per-file without a broader plan.
- **net10.0 targeting**: The project targets `net10.0` with `<RollForward>major</RollForward>` in `Directory.Build.props`. The CI installs .NET 9 SDK and relies on the hosted runner also having .NET 10 available. For local development, install .NET 9 SDK or later (preferably .NET 10 SDK).
