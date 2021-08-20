using Xunit;

namespace Chonks.Tests {
    public class SerializationTests {

        public class CustomConvertersTests : SerializationTests {
            [Fact]
            public void UsersShouldBeAbleToSpecifyCustomConverters() {
                SerializationUtility.Configure((cfg) => {
                    cfg.Converters.Add(new Vector3Converter());
                    return cfg;
                });

                var chunk = new SaveChunk();
                chunk.AddToChunk("testing", new {
                    position = new Vector3(1, 1, 1)
                });

                Assert.Equal("{\"position\":{\"x\":1.0,\"y\":1.0,\"z\":1.0}}", chunk.Data["testing"]);
            }

        }
    }
}
