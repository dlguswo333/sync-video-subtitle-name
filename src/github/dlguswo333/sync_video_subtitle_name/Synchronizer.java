package github.dlguswo333.sync_video_subtitle_name;

import java.io.File;
import java.io.IOException;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.regex.MatchResult;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Synchronizer {
    private ArrayList<File> videoFiles;
    private ArrayList<File> subtitleFiles;
    // TODO It will be more performant if indices structure is in hashmap.
    private ArrayList<Integer> videoIndices;
    private ArrayList<Integer> subtitleIndices;
    private ArrayList<String> newSubtitleNames = new ArrayList<>();

    public Synchronizer(ArrayList<File> videoFiles, ArrayList<File> subtitleFiles) {
        this.videoFiles = videoFiles;
        this.subtitleFiles = subtitleFiles;
    }

    private ArrayList<Integer> getIndices(int number, ArrayList<File> files) {
        ArrayList<Integer> indices = new ArrayList<>();
        Pattern pattern = Pattern.compile("(\\d+)");
        for (var file : files) {
            Matcher matcher = pattern.matcher(FileUtil.getFileNameWithoutExtension(file));
            int index = -1;
            var results = matcher.results();
            if (results != null) {
                var groups = results.map(MatchResult::group).toArray();
                if (groups.length <= number) {
                    throw new RuntimeException("Cannot find number in file: " + file.getName());
                }
                index = Integer.parseInt((String) groups[number]);
                indices.add(index);
            } else {
                throw new RuntimeException("Cannot find number in file: " + file.getName());
            }
        }
        return indices;
    }

    public boolean convert(int number) throws RuntimeException {
        var parentPath = Paths.get(subtitleFiles.get(0).getAbsolutePath()).getParent().toString();
        try {
            videoIndices = getIndices(number, videoFiles);
            subtitleIndices = getIndices(number, subtitleFiles);
        } catch (RuntimeException e) {
            return false;
        }
        for (int s = 0; s < subtitleFiles.size(); ++s) {
            var subtitleFile = subtitleFiles.get(s);
            for (int v = 0; v < videoIndices.size(); ++v) {
                if (subtitleIndices.get(s) == videoIndices.get(v)) {
                    var videoFile = videoFiles.get(v);
                    var newSubtitleFileName = FileUtil.getNewFileNameWithExtensionRetained(
                            subtitleFile,
                            FileUtil.getFileNameWithoutExtension(videoFile));
                    System.out.println(subtitleFile.getName() + " -> " + newSubtitleFileName);
                    newSubtitleNames.add(newSubtitleFileName);
                    break;
                }
            }
        }

        System.out.print("Do you want to continue? (y/n): ");
        char input = (char) -1;
        try {
            input = Main.readKey();
        } catch (IOException e) {
            System.out.println("Could not read input!");
        }
        if (input != 'y' && input != 'Y') {
            return false;
        }
        for (int s = 0; s < subtitleFiles.size() && s < newSubtitleNames.size(); s++) {
            var subtitleFile = subtitleFiles.get(s);
            var newSubtitleFileName = newSubtitleNames.get(s);
            if (!subtitleFile.renameTo(new File(parentPath, newSubtitleFileName))) {
                System.out.println("\nRenaming " + subtitleFile.getName() + " failed.");
                System.out.println("Check if there is an existing file with the same name.");
                return false;
            }
        }
        return true;
    }
}
