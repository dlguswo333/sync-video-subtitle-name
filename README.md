## sync-video-subtitle-name
This project is a simple, lightweight console application
that synchronizes subtitle file names to video file names.

Built in `java` language, distributed in `JAR` executable format,
so you need Java Runtime Environment to run. Recommend JRE 11 or above.

## Logics
Even if video files and subtitle files are named different,
those names have one thing in common: **They have episode number in it**.

> - video files:
>     - awesome drama S**01**E**01**.mp4
>     - awesome drama S**01**E**02**.mp4
>     - awesome drama S**01**E**03**.mp4
>
> - subtitle files:
>     - Awesome_drama season_**1** episode_**1**.ass
>     - Awesome_drama season_**1** episode_**2**.ass
>     - Awesome_drama season_**1** episode_**3**.ass

The program asks user index starting from the left of the number to use,
uses it to synchronize subtitle file names.

In above example, since the number the program wants is
the second occurrence of the number in file name, insert **2**.

The program will then show the preview of synchronization results and
ask you if this is the result you want. Since the program cannot detect
duplicated episode video/subtitle files and 1 to N mapping errors,
watch the preview carefully.

> ⚠️ If the indices of the number in video/subtitle file names
> do not match (such as _\[1080p\]awesome drama E1.mp4_ and _awesome drama E1.ass_),
> this program will not work.
