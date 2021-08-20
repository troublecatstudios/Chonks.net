using Chonks.Depots;
using Chonks.SaveManagement;
using Chonks.Tests.Fakes;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Chonks.Tests {
    public class DefaultSaveManagerTests {
        private readonly ISaveDepot _inMemoryDepot = new InMemorySaveDepot();
        private readonly ISaveController _controller = new DefaultSaveController();
        private readonly ISaveManager _saveManager;

        public DefaultSaveManagerTests() {
            _saveManager = new DefaultSaveManager(_controller, _inMemoryDepot);
        }

        public class LoadSnapshot : DefaultSaveManagerTests {
            public class FakeStateData {
                public int Test { get; set; }
            }

            [Fact]
            public void SaveStoresArePassedCorrectChunkData() {
                var store = new FakeSaveStore();

                var depot = TestHelpers.GenerateFakeSaveData("Save1", store);

                var manager = new DefaultSaveManager(_controller, depot);

                // register our store with the controller so it receives callbacks
                _controller.RegisterSaveStore(store);

                // our state inside the store should be empty right now
                Assert.Empty(store.ChunkData);

                manager.LoadSnapshot(new SaveContainer() { Name = "Save1" });

                // our store should have received the test data back as a save state
                Assert.Single(store.ChunkData);
                Assert.True(store.ChunkData.ContainsKey("global"));

                var restoredGlobalChunk = store.ChunkData["global"];
                Assert.True(restoredGlobalChunk.ContainsKey("Test"));
                Assert.Equal(10, restoredGlobalChunk.Get<int>("Test"));

                _controller.UnregisterSaveStore(store);
            }

            [Fact]
            public void InterpretersAreCalledAfterSnapshot() {
                var mockInterpreter = new Mock<ISaveInterpreter>(MockBehavior.Strict);
                mockInterpreter.Setup(m => m.ProcessChunks(It.IsAny<SaveChunk[]>())).Verifiable();

                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                _controller.RegisterSaveStore(store);
                _controller.RegisterSaveInterpreter(mockInterpreter.Object);

                var depot = TestHelpers.GenerateFakeSaveData("Save1", store);

                var manager = new DefaultSaveManager(_controller, depot);

                manager.LoadSnapshot(new SaveContainer() { Name = "Save1" });

                mockInterpreter.Verify();

                _controller.UnregisterSaveStore(store);
            }
        }

        public class ApplySnapshot : DefaultSaveManagerTests {

        }

        public class MakeSnapshot : DefaultSaveManagerTests {
            [Fact]
            public void DataIsPulledFromStores() {
                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                _controller.RegisterSaveStore(store);

                _saveManager.MakeSnapshot();
                _saveManager.ApplySnapshot(new SaveContainer() { Name = "TestSave" });

                var expectedJSON = JsonConvert.SerializeObject(new { Test = 10 });
                var storeId = store.GetStoreIdentifier();
                var didSave = _inMemoryDepot.TryLoadSave("TestSave", out var chunks, out _);

                Assert.True(didSave);
                Assert.Single(chunks);
                Assert.Equal("global", chunks[0].Name);
                Assert.True(chunks[0].Data.ContainsKey(storeId));
                Assert.Equal(expectedJSON, chunks[0].Data[storeId]);

                _controller.UnregisterSaveStore(store);
            }

            [Fact]
            public void InterpretersAreCalledAfterSnapshot() {
                var mockInterpreter = new Mock<ISaveInterpreter>(MockBehavior.Strict);
                mockInterpreter.Setup(m => m.ProcessChunks(It.IsAny<SaveChunk[]>())).Verifiable();

                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                _controller.RegisterSaveStore(store);
                _controller.RegisterSaveInterpreter(mockInterpreter.Object);

                _saveManager.MakeSnapshot();

                mockInterpreter.Verify();

                _controller.UnregisterSaveStore(store);
            }
        }
    }
}
