namespace SVSN;

interface IFileReader {
    public Files? Read(string path);
}

public class FileReader : IFileReader {
    static private readonly string[] VideoExtensions = ["avi", "mp4", "mkv", "m4v", "mov"];
    static private readonly string[] SubtitleExtensions = ["smi", "ass"];

    public Files? Read(string path) {
        try {
            if (!Directory.Exists(path)) {
                return null;
            }
            var videos = new List<string>();
            var subtitles = new List<string>();
            var filePaths = Directory.GetFiles(path);
            foreach (var filePath in filePaths) {
                var fileExt = Path.GetExtension(filePath);
                var fileName = Path.GetFileName(filePath);
                if (VideoExtensions.Any(ext => fileExt.Replace(".", "").Equals(ext, StringComparison.OrdinalIgnoreCase))) {
                    videos.Add(fileName);
                } else if (SubtitleExtensions.Any(ext => fileExt.Replace(".", "").Equals(ext, StringComparison.OrdinalIgnoreCase))) {
                    subtitles.Add(fileName);
                }
            }
            var fileResult = new Files() { Videos = videos.ToArray(), Subtitles = subtitles.ToArray() };
            return fileResult;
        } catch (Exception e) {
            Console.WriteLine(e);
            return null;
        }
    }
}
