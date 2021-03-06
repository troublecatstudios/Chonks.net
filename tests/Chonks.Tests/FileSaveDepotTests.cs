using Chonks.Depots;
using Chonks.Tests.Fixtures;
using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace Chonks.Tests {
    public class FileSaveDepotTests {
        private string _temporaryWorkingDirectory;

        public FileSaveDepotTests() {
            _temporaryWorkingDirectory = $"{Path.GetTempPath()}{Path.DirectorySeparatorChar}{System.Guid.NewGuid()}{Path.DirectorySeparatorChar}";
            if (!Directory.Exists(_temporaryWorkingDirectory)) {
                Directory.CreateDirectory(_temporaryWorkingDirectory);
            }
        }

        ~FileSaveDepotTests() {
            Directory.Delete(_temporaryWorkingDirectory);
        }

        public class TryClearSae : FileSaveDepotTests {
            [InlineData("SaveFile1")]
            [InlineData("MultipleChunks")]
            [Theory]
            public void SavedFilesCanBeDeleted(string fixtureName) {
                using (var fixtures = new TemporaryFixture("Fixtures")) {
                    var depot = new FileSaveDepot(fixtures.Path);
                    var saveFilePath = fixtures.GetFilePath(fixtureName + ".sav");
                    Assert.True(File.Exists(saveFilePath));

                    var result = depot.TryClearSave(fixtureName, out var ex);

                    Assert.Null(ex);
                    Assert.True(result);

                    Assert.False(File.Exists(saveFilePath));
                }
            }
        }

        public class TryLoadSave : FileSaveDepotTests {
            [InlineData("SaveFile1")]
            [InlineData("MultipleChunks")]
            [Theory]
            public void ChunksAreLoadedFromDiskCorrectly(string fixtureName) {
                using (var fixtures = new TemporaryFixture("Fixtures")) {
                    var json = File.ReadAllText(fixtures.GetFilePath(fixtureName + ".json"));
                    var savFileData = File.ReadAllText(fixtures.GetFilePath(fixtureName + ".sav"));

                    var expectedChunks = JsonConvert.DeserializeObject<SaveChunk[]>(json);

                    var depot = new FileSaveDepot(fixtures.Path);

                    var result = depot.TryLoadSave(fixtureName, out var chunks, out var ex);

                    Assert.True(result);
                    Assert.Null(ex);

                    Assert.Equal(expectedChunks.Length, chunks.Length);
                    for (var i = 0; i < chunks.Length; i++) {
                        var expectedChunk = expectedChunks[i];
                        var actualChunk = chunks[i];

                        Assert.Equal(expectedChunk.Name, actualChunk.Name);
                        Assert.Equal(expectedChunk.Id, actualChunk.Id);
                        Assert.Equal(expectedChunk.Data.Count, actualChunk.Data.Count);

                        foreach (var pair in actualChunk.Data) {
                            Assert.True(expectedChunk.Data.ContainsKey(pair.Key));
                            Assert.Equal(expectedChunk.Data[pair.Key], pair.Value);
                        }
                    }
                }
            }
        }

        public class TryWriteSave : FileSaveDepotTests {

            [InlineData("SaveFile1")]
            [InlineData("MultipleChunks")]
            [Theory]
            public void ChunksAreWrittenToSavFileCorrectly(string fixtureName) {
                using (var fixtures = new TemporaryFixture("Fixtures")) {
                    var json = File.ReadAllText(fixtures.GetFilePath(fixtureName + ".json"));
                    var expectedSavFile = File.ReadAllText(fixtures.GetFilePath(fixtureName + ".sav"));

                    var chunks = JsonConvert.DeserializeObject<SaveChunk[]>(json);

                    var depot = new FileSaveDepot(fixtures.Path);

                    var result = depot.TryWriteSave(fixtureName, chunks, out var ex);
                    Assert.True(result);
                    Assert.Null(ex);

                    var savFile = File.ReadAllText(fixtures.GetFilePath(fixtureName + ".sav"));
                    Assert.Equal(expectedSavFile, savFile);
                }
            }
        }

        public class ListSaves : FileSaveDepotTests {

            [Fact]
            public void GivenNoSavesExist_ReturnsAnEmptyListOfSaves() {
                var depot = new FileSaveDepot(_temporaryWorkingDirectory);
                var saves = depot.ListSaves();

                Assert.Empty(saves);
            }

            [Fact]
            public void GivenSavesDirectoryDoesntExist_DirectoryIsCreatedAndReturnsAnEmptyListOfSaves() {
                if (Directory.Exists(_temporaryWorkingDirectory)) {
                    Directory.Delete(_temporaryWorkingDirectory);
                }
                Assert.False(Directory.Exists(_temporaryWorkingDirectory));

                var depot = new FileSaveDepot(_temporaryWorkingDirectory);

                var saves = depot.ListSaves();

                Assert.Empty(saves);
                Assert.True(Directory.Exists(_temporaryWorkingDirectory));

                depot.Cleanup();
            }

            [Fact]
            public void GivenSavesExist_ReturnsAMatchingArrayOfSaves() {
                Directory.CreateDirectory(_temporaryWorkingDirectory);
                File.WriteAllText(Path.Combine(_temporaryWorkingDirectory, "save1"), "test");

                var depot = new FileSaveDepot(_temporaryWorkingDirectory);

                var saves = depot.ListSaves();

                Assert.Single(saves);
            }
        }

    }
}
