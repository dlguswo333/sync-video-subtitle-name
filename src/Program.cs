// Get videos and subtitles from the current path.
SVSN.IFileReader fileReader = new SVSN.FileReader();

var files = fileReader.Read(".");
if (files is null) {
    Console.WriteLine("Could not read the current working directory.");
    return;
}

Console.WriteLine(string.Join(" ", files.Videos));
Console.WriteLine(string.Join(" ", files.Subtitles));

// Get Map<oldSubtitleName, newSubtitleName>
SVSN.IMapper mapper = new SVSN.Mapper();

var mapResult = mapper.Map(files);
Console.WriteLine($"mapResult count: {mapResult.Count()}");
foreach(var pair in mapResult) {
    Console.WriteLine($"{pair.Key}: {pair.Value}");
}

Console.WriteLine("The following changes will be applied.");
Console.WriteLine("Do you really want to continue? y/n");

var userInput = Console.ReadKey(false);
if (userInput.Key == ConsoleKey.Y) {
    Console.WriteLine("Okay");
}
