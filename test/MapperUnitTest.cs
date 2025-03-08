namespace test;

public class MapperUnitTest {
    [Fact]
    public void Test1() {
        string[] videos = [
            "video01.mp4",
            "video02.mp4",
            "video03.mp4",
            "video04.mp4",
        ];
        string[] subtitles = [
            "subtitle1.smi",
            "subtitle2.smi",
            "subtitle3.smi",
            "subtitle4.smi",
        ];
        var files = new SVSN.Files() { Videos = videos, Subtitles = subtitles };
        var mapper = new SVSN.Mapper();
        var mapResult = mapper.Map(files);

        Assert.Equal(4, mapResult.Count);
        for (int i = 0; i < 4; ++i) {
            var video = videos[i];
            var subtitle = subtitles[i];
            Assert.Equal(mapResult[subtitle], video);
        }
    }

    [Fact]
    public void Test2() {
        string[] videos = [
            "video01.mp4",
            "video02.mp4",
            "video04.mp4",
        ];
        string[] subtitles = [
            "subtitle1.smi",
            "subtitle2.smi",
            "subtitle3.smi",
        ];
        var files = new SVSN.Files() { Videos = videos, Subtitles = subtitles };
        var mapper = new SVSN.Mapper();
        var mapResult = mapper.Map(files);

        Assert.Equal(2, mapResult.Count);
        for (int i = 0; i < 2; ++i) {
            var video = videos[i];
            var subtitle = subtitles[i];
            Assert.Equal(mapResult[subtitle], video);
        }
        Assert.False(mapResult.ContainsKey(subtitles[2]));
        Assert.False(mapResult.ContainsValue(videos[2]));
    }

    [Fact]
    public void Test3() {
        string[] videos = [
            "awesome drama S01E01.mp4",
            "awesome drama S01E02.mp4",
            "awesome drama S01E03.mp4",
        ];
        string[] subtitles = [
            "Awesome_drama season_1 episode_1.ass",
            "Awesome_drama season_1 episode_3.ass",
        ];
        var files = new SVSN.Files() { Videos = videos, Subtitles = subtitles };
        var mapper = new SVSN.Mapper();
        var mapResult = mapper.Map(files);

        Assert.Equal(2, mapResult.Count);
        Assert.Equal(mapResult[subtitles[0]], videos[0]);
        Assert.Equal(mapResult[subtitles[1]], videos[2]);
        Assert.False(mapResult.ContainsValue(videos[1]));
    }

    [Fact]
    public void Test4() {
        string[] videos = [
            "awesome drama_720p S04E01.mp4",
            "awesome drama_720p S04E02.mp4",
            "awesome drama_720p S04E03.mp4",
            "awesome drama_720p S04E04.mp4",
            "awesome drama_720p S04E05.mp4",
            "awesome drama_720p S04E06.mp4",
        ];
        string[] subtitles = [
            "Awesome_drama season_4 episode_1.ass",
            "Awesome_drama season_4 episode_2.ass",
            "Awesome_drama season_4 episode_3.ass",
            "Awesome_drama season_4 episode_4.ass",
            "Awesome_drama season_4 episode_5.ass",
            "Awesome_drama season_4 episode_6.ass",
        ];
        var files = new SVSN.Files() { Videos = videos, Subtitles = subtitles };
        var mapper = new SVSN.Mapper();
        var mapResult = mapper.Map(files);

        Assert.Equal(6, mapResult.Count);
        for (int i = 0; i < 6; ++i) {
            var video = videos[i];
            var subtitle = subtitles[i];
            Assert.Equal(mapResult[subtitle], video);
        }
    }

    [Fact]
    public void Test5() {
        string[] videos = [
            "nonumber.mp4",
        ];
        string[] subtitles = [
            "nonumber.ass",
        ];
        var files = new SVSN.Files() { Videos = videos, Subtitles = subtitles };
        var mapper = new SVSN.Mapper();
        var mapResult = mapper.Map(files);

        Assert.Empty(mapResult);
    }

    [Fact]
    public void Test6() {
        string[] videos = [
            "movie s01-e2.mp4",
            "movie s01-e1.mp4",
        ];
        string[] subtitles = [
            "subtitle s3-e1.ass",
            "subtitle s1-e1.ass",
        ];
        var files = new SVSN.Files() { Videos = videos, Subtitles = subtitles };
        var mapper = new SVSN.Mapper();
        var mapResult = mapper.Map(files);

        Assert.Single(mapResult);
        Assert.Equal(mapResult[subtitles[1]], videos[1]);
    }
}
