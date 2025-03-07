// Get videos and subtitles from the current path.
SVSN.IFileReader fileReader = new SVSN.FileReader();

var files = fileReader.Read(".");
if (files is null) {
    Console.WriteLine("Could not read the current working directory.");
    return;
}

Console.WriteLine($"{files.Videos.Length} video(s) and {files.Subtitles.Length} subtitle(s) found.");
if (files.Videos.Length == 0 || files.Subtitles.Length == 0) {
    return;
}

// Get Map<subtitle, video>
SVSN.IMapper mapper = new SVSN.Mapper();
var mapResult = mapper.Map(files);
if (mapResult.Count() == 0){
    Console.WriteLine("Could not find any feasible file name synchronization.");
    return;
}

Console.WriteLine("The following changes will be applied.");
foreach(var pair in mapResult) {
    Console.WriteLine($"{pair.Key} -> {pair.Value}");
}
Console.WriteLine("Do you really want to continue? y/n");

var userInput = Console.ReadKey(true);
if (userInput.Key == ConsoleKey.Y) {
    Console.WriteLine("Renaming subtitle(s)...");
    foreach(var pair in mapResult) {
        var subtitle = pair.Key;
        var video = pair.Value;
        var videoName = Path.GetFileNameWithoutExtension(video);
        var subtitleExt = Path.GetExtension(subtitle);
        var newSubtitleName = $"{videoName}{subtitleExt}";
        File.Move(subtitle, newSubtitleName);
    }
}
Console.WriteLine();
