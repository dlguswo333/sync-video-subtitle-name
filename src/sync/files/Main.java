package sync.files;

import java.io.File;
import java.io.IOException;
import java.nio.file.InvalidPathException;
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
            } catch (InvalidPathException e) {
                System.out.println("Oops! Something went wrong!");
                System.exit(1);
                return;
            }
            System.out.println("Target directory: " + path.toAbsolutePath().toString());
        }

        var videoFiles = getVideoFiles(path.toFile());
        var subtitleFiles = getSubtitleFiles(path.toFile());
        Logic logic = new Logic(videoFiles, subtitleFiles);
        System.out.println("Insert a number starting from left. Input non-number key if you want to abort.");
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
        if (input < 0) {
            System.out.println("Input value not valid!");
            System.exit(1);
            return;
        }

        if (subtitleFiles.size() == 0) {
            System.out.println("No subtitle file detected.");
            return;
        }

        try {
            logic.convert(input);
        } catch (Exception e) {
            System.out.println("Oops! Something went wrong!");
            System.exit(1);
            return;
        }
    }
}