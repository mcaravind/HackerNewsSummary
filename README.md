# SkimHN

SkimHN is a desktop client for Hacker News built with Node-Webkit, edge.js and C#. 

Features:

- See a preview of the important phrases in the comment thread for a given story

![Summary](screenshot_summary.PNG?raw=true "Sentence summary")

- See the different users who have commented and see their comments for a given story

![Commenters](screenshot_commenters.png?raw=true "See all commenters")

- See the top keywords in the current story

![Keywords](screenshot_keywords.PNG?raw=true "Keywords in the story")

- Follow users and keywords. When you choose to follow users, fetching a page will automatically notify you if any user you are following has made a comment in one of the stories on that page. (and similarly for keywords you are watching)

![Filters](screenshot_filters.PNG?raw=true "Follow, Watch, Filter")

- Get quick summary for individual stories

![Individual Stories](screenshot_individualstory.PNG?raw=true "Individual stories")

Installation:

Supported platforms: Win64  
Not yet supported: Win32, MacOSX, Linux

Currently the app works on Windows 64 bit machines but does not work on 32 bit machines. I have not been able to verify if it works on Mac or Linux. Getting the projects to work on the unsupported platforms is, at least on paper, only as complex as getting the correct edge.node files in place under the node_modules folder. Any help getting the node_modules fixed so that the app can become truly cross-platform would be greatly appreciated.

See this issue:
https://github.com/tjanczuk/edge/issues/61

To install and run the app:

Download the Zip file for the project, navigate to the folder called HiSum.App. Drag the file called app.nw and drop on the executable called nw.exe. Everything is self-contained (and thus the large install size)

This is really a toy app I am building to learn about Natural Language Processing, so please approach with extremely low expectations :-)
