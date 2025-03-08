using System.Text.RegularExpressions;

namespace SVSN;

interface IMapper {
    public SortedDictionary<string, string> Map(Files files);
}

public class Mapper : IMapper {
    public SortedDictionary<string, string> Map(Files files) {
        var numsArrayInVideos = files.Videos.Select(this.GetNumsFromFilePath).ToArray();
        var numsArrayInSubtitles = files.Subtitles.Select(this.GetNumsFromFilePath).ToArray();
        var distinctNumsInVideos = numsArrayInVideos.Select(
            nums => this.GetMostDistinctNum(nums, numsArrayInVideos)
        ).ToArray();
        var distinctNumsInSubtitles = numsArrayInSubtitles.Select(
            nums => this.GetMostDistinctNum(nums, numsArrayInSubtitles)
        ).ToArray();

        var mapResult = new SortedDictionary<string, string>();
        var isVideosMapped = new bool[files.Videos.Length];

        for(int i = 0;i < files.Subtitles.Length;++i) {
            var subtitle = files.Subtitles[i];
            var distinctNumInSubtitle = distinctNumsInSubtitles[i];
            for (int j = 0;j < files.Videos.Length;++j) {
                var video = files.Videos[j];
                var distinctNumInVideo = distinctNumsInVideos[j];
                if (distinctNumInVideo != distinctNumInSubtitle) {
                    continue;
                }
                var isVideoMapped = isVideosMapped[j];
                if (isVideoMapped) {
                    continue;
                }
                isVideosMapped[j] = true;
                mapResult.Add(subtitle, video);
            }
        }

        return mapResult;
    }

    private double[] GetNumsFromFilePath(string filePath) {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        return this.GetNumsFromStr(fileName);
    }

    private double[] GetNumsFromStr(string str) {
        var pattern = @"(\d+\.\d+)|(\d+)";
        var regex = new Regex(pattern);
        var matches = regex.Matches(str);
        return matches.Select(match => double.Parse(match.Value)).ToArray();
    }

    private double GetMostDistinctNum(double[] nums, double[][] numsArray) {
        int[] occurrenceCnt = new int[nums.Length];
        for (int i = 0;i < nums.Length;++i) {
            var num = nums[i];
            for (int j = 0;j < numsArray.Length;++j) {
                if (nums == numsArray[j]) {
                    continue;
                }
                occurrenceCnt[i] += numsArray[j].Count(value => value == num);
            }
        }
        var minOccurrenceInd = Array.IndexOf(occurrenceCnt, occurrenceCnt.Min());
        var mostDistinctNum = nums[minOccurrenceInd];
        return mostDistinctNum;
    }
}
