using Chonks.Depots;
using System.IO;
using Xunit;

namespace Chonks.Tests {
    public class FileSaveDepotTests {
        private FileSaveDepot _depot;

        private string _temporaryWorkingDirectory = $"{Path.GetTempPath()}{Path.DirectorySeparatorChar}{System.Guid.NewGuid()}{Path.DirectorySeparatorChar}";

        public FileSaveDepotTests() {
            _depot = new FileSaveDepot(_temporaryWorkingDirectory);
        }

        public class ListSaves : FileSaveDepotTests {

            [Fact]
            public void GivenNoSavesExist_ReturnsAnEmptyListOfSaves() {
                var saves = _depot.ListSaves();

                Assert.Empty(saves);
            }

            [Fact]
            public void GivenSavesDirectoryDoesntExist_DirectoryIsCreatedAndReturnsAnEmptyListOfSaves() {
                Assert.False(Directory.Exists(_temporaryWorkingDirectory));

                var saves = _depot.ListSaves();

                Assert.Empty(saves);
                Assert.True(Directory.Exists(_temporaryWorkingDirectory));

                _depot.Cleanup();
            }

            [Fact]
            public void GivenSavesExist_ReturnsAMatchingArrayOfSaves() {
                Directory.CreateDirectory(_temporaryWorkingDirectory);
                File.WriteAllText(Path.Combine(_temporaryWorkingDirectory, "save1"), "test");

                var saves = _depot.ListSaves();

                Assert.Single(saves);
            }
        }

    }
}
