package github.dlguswo333.sync_video_subtitle_name;

import java.io.File;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;

public class Main {
    final static String[] videoExtenstions = { "mp4", "mkv", "m4v", "mov" };
    final static String[] subtitleExtenstions = { "smi", "ass" };

    private static boolean endsWithAny(String str, String[] keywords) {
        for (var keyword : keywords) {
            if (str.endsWith(keyword)) {
                return true;
            }
        }
        return false;
    }

    private static ArrayList<File> getFilesThatEndWith(File folder, String[] keywords) {
        ArrayList<File> files = new ArrayList<>();
        for (var file : folder.listFiles()) {
            if (file.isFile() && file.canRead() && endsWithAny(file.getName(), keywords)) {
                files.add(file);
            }
        }
        return files;
    }

    private static ArrayList<File> getVideoFiles(File folder) {
        return getFilesThatEndWith(folder, videoExtenstions);
    }

    private static ArrayList<File> getSubtitleFiles(File folder) {
        return getFilesThatEndWith(folder, subtitleExtenstions);
    }

    public static char readKey() throws IOException {
        System.in.read(new byte[System.in.available()]);
        char input = (char) System.in.read();
        System.in.read(new byte[System.in.available()]);
        return input;
    }

    public static void main(String args[]) {
        Path path;
        if (args.length == 0) {
            System.out.print("argument not provided. Using current working directory: ");
            var cwdPath = new File(".").toPath().toAbsolutePath();
            System.out.println(cwdPath.toString());
            path = cwdPath;
        } else {
            try {
                path = Paths.get(args[0]);
                if (!path.toFile().exists() || !path.toFile().isDirectory()) {
                    throw new RuntimeException();
                }
            } catch (Exception e) {
                System.out.println("Argument path is not valid!");
                System.exit(1);
                return;
            }
            System.out.println("Target directory: " + path.toAbsolutePath().toString());
        }

        var videoFiles = getVideoFiles(path.toFile());
        var subtitleFiles = getSubtitleFiles(path.toFile());
        if (videoFiles.size() == 0 || subtitleFiles.size() == 0) {
            System.out.println("No " + (videoFiles.size() == 0 ? "video" : "subtitle") + " file detected.");
            System.exit(1);
            return;
        }

        Synchronizer logic = new Synchronizer(videoFiles, subtitleFiles);
        System.out.println(
                "Insert the index of number in file names starting from left. Input non-number key if you want to cancel.");
        int input = 1;
        try {
            input = readKey();
            input -= (int) '0';
            input -= 1;
        } catch (IOException e) {
            System.out.println("Could not read input!");
            e.printStackTrace();
            System.exit(1);
            return;
        }
        if (!(0 <= input && input <= 8)) {
            System.out.println("Cancelled synchronization.");
            return;
        }

        try {
            if (logic.convert(input)) {
                System.out.println("Synchronization completed.");
            } else {
                System.out.println("Synchronization cancelled.");
            }
        } catch (RuntimeException e) {
            System.out.println("Oops! Something went wrong!");
            System.exit(1);
            return;
        }
    }
}
