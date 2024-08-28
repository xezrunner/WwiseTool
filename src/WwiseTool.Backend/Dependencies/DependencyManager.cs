namespace WwiseTool.Backend {
    public struct Dependency {
        public string         Name;             // used for local directory name as well
        public string         FriendlyName;
        public string         UsageDescription; 
        public bool           IsOptional;       // TODO: manual intervention components!    
        public DependencyType Type;

        public string ProjectURL;

        // TODO: array instead?
        public string PrimaryDownloadURL;   // from Source
        public string SecondaryDownloadURL; // from our project

        // TODO: combine with internal (abstract) type instead?
        public bool   IsPresent;
        public string FilePath; // Path on disk to the local dependency

        // TODO: should these be functions or getters?
        public string FriendlyNameOrName() => !String.IsNullOrEmpty(FriendlyName) ? FriendlyName : Name;
        public string TypeFriendlyName()   => Type.ToFriendlyName();
    }

    public enum DependencyType {
        Other, Asset,
        PortableApplication, PortableLibrary,
        InstallableApplication, InstallableLibrary,
    }
    static class DependencyTypeExtensions {
        public static string ToFriendlyName(this DependencyType dependencyType) {
            switch (dependencyType) {
                case DependencyType.PortableApplication:    return "Application (portable)";
                case DependencyType.PortableLibrary:        return "Library (portable)";
                case DependencyType.InstallableApplication: return "Application (installation)";
                case DependencyType.InstallableLibrary:     return "Library (installation)";
                case DependencyType.Asset:                  return "Asset";
                default:                                    return dependencyType.ToString();
            }
        }
    }

    // TODO: move out the dependency system from WwiseTool!

    public static class WwiseToolDependencies {
        // TODO: these could be dynamically assignable (such as from a file), but at the same time,
        // having it in code as constants would suffice for this project.
        public static readonly Dependency[] Dependencies = [
            new Dependency() {
                Name               = "QuickBMS",
                FriendlyName       = "QuickBMS",
                UsageDescription   = "QuickBMS is used for the extraction of .pck (Wwise file package) files.",
                Type               = DependencyType.PortableApplication,
                ProjectURL         = "https://aluigi.altervista.org/quickbms.htm",
                PrimaryDownloadURL = "https://aluigi.altervista.org/papers/quickbms.zip",
            },
            new Dependency() {
                Name               = "QuickBMS_pck_script",
                FriendlyName       = "QuickBMS .pck script",
                UsageDescription   = "This script is used with QuickBMS for the extraction of .pck (Wwise file package) files.",
                Type               = DependencyType.Asset,
            },
            new Dependency() {
                Name               = "dep_opt_test",
                FriendlyName       = "Optional dependency test",
              //UsageDescription   = "Test dependency - do not use!",
                IsOptional         = true,
            }
        ];
    }

    [Flags] public enum DependencyManagerAnswer {
        InvalidDependencyList       = 0,
        DependencyPathError         = 1 << 0,
        MissingRequiredDependencies = 1 << 1,
        MissingOptionalDependencies = 1 << 2,
        OK                          = 1 << 3,
    }

    public struct DependencyManagerResult {
        public DependencyManagerResult(DependencyManagerAnswer answer, Dependency[] dependencies, Dependency[]? missingDependencies = null)
            { this.answer = answer; this.dependencies = dependencies; this.missingDependencies = missingDependencies; }
        public DependencyManagerResult(DependencyManagerAnswer answer, string error)
            { this.answer = answer; this.error = error; }
        public DependencyManagerResult(DependencyManagerAnswer answer)
            { this.answer = answer;  }

        public DependencyManagerAnswer answer;
        public string? error;

        public Dependency[]? dependencies;
        public Dependency[]? missingDependencies;
    }

    public class DependencyManager {
        // Path to dependency files (where they're eventually downloaded to/used from)
        // TODO: separate download path?
        public string dependenciesDirPath;
        public const string DEPENDENCIES_DIRECTORY_NAME = "Dependencies";

        public Dependency[] dependencies;

        public DependencyManager(Dependency[] dependencies, string baseDirPath) {
            this.dependencies = dependencies; // copy!
            this.dependenciesDirPath = Path.Combine(baseDirPath, DEPENDENCIES_DIRECTORY_NAME);
        }
        public DependencyManager(Dependency[] dependencies) {
            this.dependencies = dependencies; // copy!
            var baseDirPath = AppDomain.CurrentDomain.BaseDirectory;
            this.dependenciesDirPath = Path.Combine(baseDirPath, DEPENDENCIES_DIRECTORY_NAME);
        }

        string[] GetLocalDependencyDirectories() => Directory.GetDirectories(dependenciesDirPath);

        public DependencyManagerResult CheckDependencyStatus() {
            if (dependencies == null)     return new(DependencyManagerAnswer.InvalidDependencyList, "No dependency array was specified.");
            if (dependencies.Length == 0) return new(DependencyManagerAnswer.InvalidDependencyList, "No dependencies in the specified array.");

            if (!Directory.Exists(dependenciesDirPath)) {
                try {
                    Directory.CreateDirectory(dependenciesDirPath);
                } catch (Exception ex) {
                    return new(DependencyManagerAnswer.DependencyPathError, ex.Message);
                }
            }

#if true
            int foundRequired = 0;
            int foundOptional = 0;
            for (int i = 0; i < dependencies.Length; ++i) {
                Dependency dependency = dependencies[i];
                string localPath = Path.Combine(dependenciesDirPath, dependency.Name);
                if (Directory.Exists(localPath)) {
                    dependency.IsPresent = true;
                    dependency.FilePath = localPath;
                    if (dependency.IsOptional) ++foundOptional; else ++foundRequired;
                }
            }

            DependencyManagerResult result = new() { dependencies = dependencies };

            if (foundRequired + foundOptional == dependencies.Length) result.answer = DependencyManagerAnswer.OK;
            else {
                result.missingDependencies = GetMissingDependencies();
                if (foundRequired != dependencies.Where(d => !d.IsOptional).Count()) result.answer  = DependencyManagerAnswer.MissingRequiredDependencies;
                if (foundOptional != dependencies.Where(d =>  d.IsOptional).Count()) result.answer |= DependencyManagerAnswer.MissingOptionalDependencies;
            }

            return result;
#else
            List<(string path, string name)> localDirs = new();
            try {
                string[]? localDependencyDirPaths = Directory.GetDirectories(dependenciesDirPath);
                foreach (var path in localDependencyDirPaths) {
                    localDirs.Add((path, path.Split(Path.PathSeparator).Last()));
                }
            } catch (Exception ex) {
                return new(DependencyManagerAnswer.DependencyPathError, ex.Message);
            }

            Debugger.Break(); // verify the names!

            foreach (var dir in localDirs) {
                
            }
#endif
        }

        public Dependency[] GetMissingDependencies() => dependencies.Where(d => !d.IsPresent).ToArray();

        // NOTE: DO NOT USE
        public void DEBUG_CreateTestDependencyDirectories() {
            Directory.CreateDirectory(Path.Combine(dependenciesDirPath, "QuickBMS"));
        }
    }
}
