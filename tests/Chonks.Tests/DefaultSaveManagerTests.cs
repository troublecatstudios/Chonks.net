using Chonks.Depots;
using Chonks.SaveManagement;
using Chonks.Tests.Fakes;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Chonks.Tests {
    public class DefaultSaveManagerTests {
        public class RegistrationTests : DefaultSaveManagerTests {
            [Fact]
            public void SaveStoresAreNotifiedWhenTheyAreRegistered() {
                var store = new FakeSaveStore();
                var storeRegistry = new Registry<ISaveStore>();

                var depot = TestHelpers.GenerateFakeSaveData("Save1", store);

                var manager = new DefaultSaveManager(storeRegistry, depot: depot);

                // register the store AFTER the snapshot is loaded.

                manager.LoadSnapshot(new SaveContainer() { Name = "Save1" });

                // our state inside the store should be empty right now
                Assert.Empty(store.ChunkData);

                // register the store
                storeRegistry.Register(store);

                // our store should have received the test data back as a save state
                Assert.Single(store.ChunkData);
                Assert.True(store.ChunkData.ContainsKey("global"));

                var restoredGlobalChunk = store.ChunkData["global"];
                Assert.True(restoredGlobalChunk.ContainsKey("Test"));
                Assert.Equal(10, restoredGlobalChunk.Get<int>("Test"));
            }
        }

        public class LoadSnapshot : DefaultSaveManagerTests {
            public class FakeStateData {
                public int Test { get; set; }
            }

            [Fact]
            public void SaveStoresArePassedCorrectChunkData() {
                var store = new FakeSaveStore();
                var storeRegistry = new Registry<ISaveStore>();
                var depot = TestHelpers.GenerateFakeSaveData("Save1", store);

                var manager = new DefaultSaveManager(storeRegistry, depot: depot);

                // register our store with the controller so it receives callbacks
                storeRegistry.Register(store);

                // our state inside the store should be empty right now
                Assert.Empty(store.ChunkData);

                manager.LoadSnapshot(new SaveContainer() { Name = "Save1" });

                // our store should have received the test data back as a save state
                Assert.Single(store.ChunkData);
                Assert.True(store.ChunkData.ContainsKey("global"));

                var restoredGlobalChunk = store.ChunkData["global"];
                Assert.True(restoredGlobalChunk.ContainsKey("Test"));
                Assert.Equal(10, restoredGlobalChunk.Get<int>("Test"));
            }

            [Fact]
            public void InterpretersAreCalledAfterSnapshot() {
                var storeRegistry = new Registry<ISaveStore>();
                var interpreterRegistry = new Registry<ISaveInterpreter>();

                var mockInterpreter = new Mock<ISaveInterpreter>(MockBehavior.Strict);
                mockInterpreter.Setup(m => m.ProcessChunks(It.IsAny<SaveChunk[]>())).Verifiable();

                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                storeRegistry.Register(store);
                interpreterRegistry.Register(mockInterpreter.Object);

                var depot = TestHelpers.GenerateFakeSaveData("Save1", store);

                var manager = new DefaultSaveManager(storeRegistry, interpreterRegistry, depot);

                manager.LoadSnapshot(new SaveContainer() { Name = "Save1" });

                mockInterpreter.Verify();
            }
        }

        public class ApplySnapshot : DefaultSaveManagerTests {
            [Fact]
            public void InterpretersAreCalledAfterSnapshot() {
                var storeRegistry = new Registry<ISaveStore>();
                var interpreterRegistry = new Registry<ISaveInterpreter>();

                var mockInterpreter = new Mock<ISaveInterpreter>(MockBehavior.Strict);
                mockInterpreter.Setup(m => m.ProcessChunks(It.IsAny<SaveChunk[]>())).Verifiable();
                mockInterpreter.Setup(m => m.IsDirty()).Returns(false).Verifiable();

                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                storeRegistry.Register(store);
                interpreterRegistry.Register(mockInterpreter.Object);

                var depot = TestHelpers.GenerateFakeSaveData("Save1", store);

                var manager = new DefaultSaveManager(storeRegistry, interpreterRegistry, depot);

                manager.MakeSnapshot();
                manager.ApplySnapshot(new SaveContainer() { Name = "Save1" });

                mockInterpreter.Verify();
            }

            [Fact]
            public void WhenInterpretersAreDirty_TheyCanModifyTheSnapshotBeforeSave() {
                var storeRegistry = new Registry<ISaveStore>();
                var interpreterRegistry = new Registry<ISaveInterpreter>();

                var modifiedChunks = new SaveChunk[] {
                    new SaveChunk("__test") {
                        Data = new Dictionary<string, string>() {
                            { "Testing", "25" }
                        }
                    }
                };
                Exception mockDepotException = null;

                var mockInterpreter = new Mock<ISaveInterpreter>(MockBehavior.Strict);
                mockInterpreter.Setup(m => m.ProcessChunks(It.IsAny<SaveChunk[]>())).Verifiable();
                mockInterpreter.Setup(m => m.IsDirty()).Returns(true).Verifiable();
                mockInterpreter.Setup(m => m.ApplyModifications(It.IsAny<SaveChunk[]>())).Returns(modifiedChunks).Verifiable();

                var mockDepot = new Mock<ISaveDepot>(MockBehavior.Strict);
                mockDepot.Setup(m => m.TryWriteSave("Save1", It.Is<SaveChunk[]>(i => i == modifiedChunks), out mockDepotException)).Returns(true);

                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                storeRegistry.Register(store);
                interpreterRegistry.Register(mockInterpreter.Object);

                var manager = new DefaultSaveManager(storeRegistry, interpreterRegistry, mockDepot.Object);

                manager.MakeSnapshot();
                manager.ApplySnapshot(new SaveContainer() { Name = "Save1" });

                mockInterpreter.Verify();
                mockDepot.Verify();
            }

            [Fact]
            public void SaveStoresAreCalledAfterSnapshot() {
                var storeRegistry = new Registry<ISaveStore>();
                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                storeRegistry.Register(store);

                var depot = TestHelpers.GenerateFakeSaveData("Save1", store);

                var manager = new DefaultSaveManager(storeRegistry, depot: depot);

                Assert.Empty(store.ChunkData);

                manager.MakeSnapshot();
                manager.ApplySnapshot(new SaveContainer() { Name = "Save1" });

                Assert.Single(store.ChunkData);
                Assert.Equal(10, store.ChunkData["global"].Get<int>("Test"));
            }
        }

        public class MakeSnapshot : DefaultSaveManagerTests {

            [Fact]
            public void NullSaveStatesAreIgnored() {
                var depot = new InMemorySaveDepot();
                var storeRegistry = new Registry<ISaveStore>();
                var store = new FakeSaveStore(new SaveState[] { null });
                storeRegistry.Register(store);

                var manager = new DefaultSaveManager(storeRegistry, depot: depot);
                manager.MakeSnapshot();
                manager.ApplySnapshot(new SaveContainer() { Name = "TestSave" });

                var storeId = store.GetStoreIdentifier();
                var expectedJSON = JsonConvert.SerializeObject(new { });
                var didSave = depot.TryLoadSave("TestSave", out var chunks, out _);

                Assert.True(didSave);
                Assert.Empty(chunks);
                Assert.NotNull(chunks);
            }

            [Fact]
            public void DataIsPulledFromStores() {
                var depot = new InMemorySaveDepot();
                var storeRegistry = new Registry<ISaveStore>();
                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                storeRegistry.Register(store);

                var manager = new DefaultSaveManager(storeRegistry, depot: depot);

                manager.MakeSnapshot();
                manager.ApplySnapshot(new SaveContainer() { Name = "TestSave" });

                var expectedJSON = JsonConvert.SerializeObject(new { Test = 10 });
                var storeId = store.GetStoreIdentifier();
                var didSave = depot.TryLoadSave("TestSave", out var chunks, out _);

                Assert.True(didSave);
                Assert.Single(chunks);
                Assert.Equal("global", chunks[0].Name);
                Assert.True(chunks[0].Data.ContainsKey(storeId));
                Assert.Equal(expectedJSON, chunks[0].Data[storeId]);
            }

            [Fact]
            public void InterpretersAreCalledAfterSnapshot() {
                var depot = new InMemorySaveDepot();
                var storeRegistry = new Registry<ISaveStore>();
                var interpreterRegistry = new Registry<ISaveInterpreter>();

                var mockInterpreter = new Mock<ISaveInterpreter>(MockBehavior.Strict);
                mockInterpreter.Setup(m => m.ProcessChunks(It.IsAny<SaveChunk[]>())).Verifiable();

                var manager = new DefaultSaveManager(storeRegistry, interpreterRegistry, depot);

                var store = new FakeSaveStore(new SaveState() { ChunkName = "global", Data = new { Test = 10 } });
                storeRegistry.Register(store);
                interpreterRegistry.Register(mockInterpreter.Object);

                manager.MakeSnapshot();

                mockInterpreter.Verify();
            }
        }
    }
}
