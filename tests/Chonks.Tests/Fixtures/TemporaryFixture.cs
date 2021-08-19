using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Chonks.Tests.Fixtures {
    /// <summary>
    /// Temporary fixture files are copied into a temporary directory before use. You can use a using block around your tests, when the using block is exited the temporary directory and files will be deleted.
    /// </summary>
    internal class TemporaryFixture : TemporaryFixtureBase {
        /// <summary>
        /// Constructs a new TemporaryFixturesPath instance.
        /// </summary>
        /// <param name="pathRelativeToTestsProject">The path to the fixture, relative to the test project root.</param>
        /// <param name="tempDirectoryRootPath">Allows consumer to specify a specific root path as the location to store fixtures.</param>
        /// <param name="deleteOnDispose">Wether or not the temporary fixtures path should be cleaned up when this object is disposed. Default is true.</param>
        internal TemporaryFixture(string pathRelativeToTestsProject, string tempDirectoryRootPath = null, bool deleteOnDispose = true)
            : base(deleteOnDispose) {
            var permanentFixtureDirectory = GetFixturePath(pathRelativeToTestsProject);
            var temporaryDirectoryPathRoot = string.IsNullOrEmpty(tempDirectoryRootPath) ? System.IO.Path.GetTempPath() : tempDirectoryRootPath;
            var guid = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var temporaryDirectoryPath = System.IO.Path.Combine(temporaryDirectoryPathRoot, $"{guid}{System.IO.Path.DirectorySeparatorChar}");

            Directory.CreateDirectory(temporaryDirectoryPath);

            //Now Create all of the directories
            foreach (var dirPath in Directory.GetDirectories(permanentFixtureDirectory, "*", SearchOption.AllDirectories)) {
                Directory.CreateDirectory(dirPath.Replace(permanentFixtureDirectory, temporaryDirectoryPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (var src in Directory.GetFiles(permanentFixtureDirectory, "*.*", SearchOption.AllDirectories)) {
                var dest = src.Replace(permanentFixtureDirectory, temporaryDirectoryPath);
                File.Copy(src, dest, true);

                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Dos2Unix(dest);
            }

            Path = temporaryDirectoryPath;
        }

        public string GetFilePath(string fileName) {
            return System.IO.Path.Combine(Path, fileName);
        }

        private static string GetFixturePath(string pathRelativeToTestsProject) {
            // Location of the DLL, e.g., ...Tests/bin/Debug/netcoreapp1.1
            var binDebugDir = AppContext.BaseDirectory;
            var testsProjectDir = System.IO.Path.Combine(binDebugDir, "..", "..", "..");

            var fixturePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(testsProjectDir, pathRelativeToTestsProject));
            return fixturePath;
        }

        private static void Dos2Unix(string fileName) {
            const byte CR = 0x0D;
            const byte LF = 0x0A;
            byte[] data = File.ReadAllBytes(fileName);
            using (FileStream fileStream = File.OpenWrite(fileName)) {
                BinaryWriter bw = new BinaryWriter(fileStream);
                int position = 0;
                int index = 0;
                do {
                    index = Array.IndexOf<byte>(data, CR, position);
                    if ((index >= 0) && (data[index + 1] == LF)) {
                        // Write before the CR
                        bw.Write(data, position, index - position);
                        // from LF
                        position = index + 1;
                    }
                }
                while (index >= 0);
                bw.Write(data, position, data.Length - position);
                fileStream.SetLength(fileStream.Position);
            }
        }
    }
}
