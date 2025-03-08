## sync-video-subtitle-name
This project is a simple, lightweight console application
that synchronizes subtitle file names to video file names.

Built in `dotnet` language, distributed in AOT executable format,
so you don't need any external runtime. Simply run the executable from your terminal.

## Logics
As long as we correctly sort video files and subtitle files,
there is high probability that they will be correctly matched.

> - video files:
>     - awesome drama S**01**E**01**.mp4
>     - awesome drama S**01**E**02**.mp4
>     - awesome drama S**01**E**03**.mp4
>
> - subtitle files:
>     - Awesome_drama season_**1** episode_**1**.ass
>     - Awesome_drama season_**1** episode_**3**.ass

The program finds out the most non-overlapping numbers from file names
and use them to index the files internally.

In above example, since the number the program wants is
the second occurrence of the number in file names;
the numbers from the videos are `1`, `2`, `3` and
the numbers from the subtitles are `1` and `3`.
There is no `2` from subtitles so there is no match for the second video file.

The program will then show the preview of synchronization results and
ask you if this is the result you want.
Since the program may have errors, watch the preview carefully.
