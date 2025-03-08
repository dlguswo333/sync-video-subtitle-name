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

        for (int i = 0; i < files.Subtitles.Length; ++i) {
            var subtitle = files.Subtitles[i];
            var distinctNumInSubtitle = distinctNumsInSubtitles[i];
            for (int j = 0; j < files.Videos.Length; ++j) {
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
        int[] occurrenceInOtherNums = new int[nums.Length];
        double mostDistinctNum;
        for (int i = 0; i < nums.Length; ++i) {
            var num = nums[i];
            for (int j = 0; j < numsArray.Length; ++j) {
                if (nums == numsArray[j]) {
                    continue;
                }
                occurrenceInOtherNums[i] += numsArray[j].Count(value => value == num);
            }
        }
        // The # of numbers with minimum occurrence.
        var numsCntWithMinOccurrence = occurrenceInOtherNums.Count(
            value => value == occurrenceInOtherNums.Min()
        );
        if (numsCntWithMinOccurrence == 1) {
            var minOccurrenceInd = Array.IndexOf(occurrenceInOtherNums, occurrenceInOtherNums.Min());
            mostDistinctNum = nums[minOccurrenceInd];
            return mostDistinctNum;
        }

        // If there are multiple nums with minimum occurences, things get complicated.
        // It maybe because there are numbers in the base file name
        // such as number in the series or resolution;
        // and the number coincides with the number we actually want.
        // In this case, we get the number with the most occurrence in 'nums' array.
        var numsWithOccurrence = nums.Distinct().ToDictionary(v => v, v => nums.Count(_v => _v == v));
        var candidateNum = new KeyValuePair<double, int>(double.NaN, 0);
        for (int i = 0; i < nums.Length; ++i) {
            var num = nums[i];
            if (occurrenceInOtherNums[i] > occurrenceInOtherNums.Min()) {
                continue;
            }
            var occurrenceInNums = nums.Count(v => v == num);
            if (occurrenceInNums > candidateNum.Value) {
                candidateNum = new KeyValuePair<double, int>(num, occurrenceInNums);
            }
        }
        mostDistinctNum = candidateNum.Key;
        return mostDistinctNum;
    }
}
