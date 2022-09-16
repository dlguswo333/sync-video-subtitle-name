package github.dlguswo333.sync_video_subtitle_name;

import java.io.File;

public class FileUtil {

    public static String getFileNameWithoutExtension(File file) {
        var name = file.getName();
        name = name.substring(0, name.lastIndexOf('.'));
        return name;
    }

    public static String getFileExtension(File file) {
        var name = file.getName();
        var extension = name.substring(name.lastIndexOf('.') + 1);
        return extension;
    }

    public static String getNewFileNameWithExtensionRetained(File file, String newNameWithoutExtension) {
        var newName = newNameWithoutExtension + "." + getFileExtension(file);
        return newName;
    }
}
