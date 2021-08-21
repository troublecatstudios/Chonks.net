using Chonks.Tests.Fakes;

namespace Chonks.Tests {
    public static class TestHelpers {
        public static ISaveDepot GenerateFakeSaveData(string saveFileName, ISaveStore store) {
            var depot = new FakeDepot();
            // add our fake chunk to the "save file"
            depot.Storage.Add(saveFileName, GenerateFakeSaveChunks(store.GetStoreIdentifier()));

            return depot;
        }

        public static SaveChunk[] GenerateFakeSaveChunks(string identifier) {
            var globalChunk = new SaveChunk("global");
            globalChunk.AddToChunk(identifier, new { Test = 10 });
            return new SaveChunk[] { globalChunk };
        }
    }
}
