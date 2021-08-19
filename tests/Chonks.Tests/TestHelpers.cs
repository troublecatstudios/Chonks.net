using Chonks.Tests.Fakes;

namespace Chonks.Tests {
    public static class TestHelpers {
        public static ISaveDepot GenerateFakeSaveData(string saveFileName, ISaveStore store) {
            var depot = new FakeDepot();
            var globalChunk = new SaveChunk("global");
            globalChunk.AddToChunk(store.GetStoreIdentifier(), new { Test = 10 });

            // add our fake chunk to the "save file"
            depot.Storage.Add(saveFileName, new SaveChunk[] {
                globalChunk
            });

            return depot;
        }
    }
}
