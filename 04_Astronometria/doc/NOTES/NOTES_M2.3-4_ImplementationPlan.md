
```text
Astronometria.Desktop
│
├── App.xaml.cs
│     → CLI/GUI dispatch
│
└── ScientificRun/                 NEU
      ├── Cli/
      │     ├── CliArguments.cs
      │     └── CliParser.cs
      │
      └── Hosting/
            └── ScientificRunHost.cs
```

```text
Astronometria.Core
└── ScientificRun/                 NEU
      ├── Build/
      │     ├── BuildInfo.cs
      │     ├── BuildInfoLoader.cs
      │     └── buildinfo.json
      │
      ├── Planning/
      ├── Execution/
      ├── Diagnostics/
      ├── Hashing/
      ├── IO/
      └── Models/
```

```text
App.xaml.cs
→ detect CLI args
→ ScientificRunHost.Run(args)

ScientificRunHost
→ parse args
→ load buildinfo.json
→ print startup banner
→ later call planner
```


Der bestehende historische Bereich bleibt praktisch unangetastet:

```text
MainWindow.xaml
MainWindow.xaml.cs
Observation/
_CelObjects/
_Components/
StarMap.cs
...
```

Alles neue liegt sauber isoliert

```text
Astronometria.Core/ScientificRun/
Astronometria.Desktop/ScientificRun/
```