namespace TraktNet.Objects.Get.Tests.Syncs.Activities.Json.Writer
{
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using Trakt.NET.Tests.Utility.Traits;
    using TraktNet.Extensions;
    using TraktNet.Objects.Get.Syncs.Activities;
    using TraktNet.Objects.Get.Syncs.Activities.Json.Writer;
    using Xunit;

    [TestCategory("Objects.Get.Syncs.Activities.JsonWriter")]
    public class SyncRecommendationsLastActivitiesObjectJsonWriter_Tests
    {
        private readonly DateTime UPDATED_AT = DateTime.UtcNow;

        [Fact]
        public async Task Test_SyncRecommendationsLastActivitiesObjectJsonWriter_Exceptions()
        {
            var traktJsonWriter = new SyncRecommendationsLastActivitiesObjectJsonWriter();
            Func<Task<string>> action = () => traktJsonWriter.WriteObjectAsync(default);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Test_SyncRecommendationsLastActivitiesObjectJsonWriter_Empty()
        {
            var traktJsonWriter = new SyncRecommendationsLastActivitiesObjectJsonWriter();
            string json = await traktJsonWriter.WriteObjectAsync(new TraktSyncRecommendationsLastActivities());
            json.Should().Be("{}");
        }

        [Fact]
        public async Task Test_SyncRecommendationsLastActivitiesObjectJsonWriter_Complete()
        {
            ITraktSyncRecommendationsLastActivities syncLastActivities = new TraktSyncRecommendationsLastActivities
            {
                UpdatedAt = UPDATED_AT
            };

            var traktJsonWriter = new SyncRecommendationsLastActivitiesObjectJsonWriter();
            string json = await traktJsonWriter.WriteObjectAsync(syncLastActivities);
            json.Should().Be($"{{\"updated_at\":\"{UPDATED_AT.ToTraktLongDateTimeString()}\"}}");
        }
    }
}
