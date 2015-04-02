var edge = require('edge');
var path = require('path');

var top100Function = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName:'HiSum.Reader',
    methodName: 'GetFrontPage'
});

var storyFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'GetStory'
});

var summaryFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'GetStoryTopNWords'
});

var archiveFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'ArchiveStory'
});

var storyDeleteFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'DeleteStory'
});

var checkForUpdatesFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'CheckForUpdates'
});

var getArchiveFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'GetArchivedStories'
});

var fullStoryFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'GetFullStory'
});

var sentenceCommentTreeFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'GetCommentTreeForId'
});

function archive(id) {
    archiveFunction(id, function (error, result) {
        if (error) alert(error.message);
        console.log(result);
    });
}

function GetTop100() {
    top100Function(100, function(error, result) {
        if (error) alert(error.message);
        result.forEach(function(entry)
        {
        console.log(entry);
        });
        
    });
}