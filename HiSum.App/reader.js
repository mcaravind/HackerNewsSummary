var edge = require('edge');
var path = require('path');
//var dotNetFunction = edge.func({ assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll', methodName: 'GetTop100' });
//function myFunction3() {
//    dotNetFunction(10, function (error, result) {
//        if (error) alert(error.message);
//        alert('Reply:' + result);
//    });
//}

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

var getArchiveFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'GetArchivedStories'
});

//var commentsFunction = edge.func(
//{
//    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
//    typeName: 'HiSum.Reader',
//    methodName: 'GetCommentTree'
//});

var tagCloudFunction = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName: 'HiSum.Reader',
    methodName: 'GetTagCloudTree'
});

//function getArchive() {
//    console.log("inside");
//    getArchiveFunction(10, function (error, result) {
//        if (error) alert(error.message);
//        console.log(result);
//    });
//}

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