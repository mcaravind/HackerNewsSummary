# HackerNewsSummary

Hacker News Summary is a client for Hacker News built with Node-Webkit, edge.js and C#. I am building this app to learn about Natural Language Processing.

Features:

See a preview of the important phrases in the comment thread for a given story

See the different users who have commented and see their comments for a given story

![Screenshot](screenshot.png?raw=true "Hacker News Summary")

Installation:

Supported platforms: Win64
Not yet supported: Win32, MacOSX, Linux

Currently the app works on Windows 64 bit machines but does not work on 32 bit machines. I have not been able to verify if it works on Mac or Linux. Getting the projects to work on the unsupported platforms is, at least on paper, only as complex as getting the correct edge.node files in place under the node_modules folder. Any help getting the node_modules fixed so that the app can become truly cross-platform would be greatly appreciated.

See this issue:
https://github.com/tjanczuk/edge/issues/61

To install and run the app:

Download the Zip file for the project, navigate to the folder called HiSum.App. Drag the file called app.nw and drop on the executable called nw.exe. Everything is self-contained (and thus the large install size)