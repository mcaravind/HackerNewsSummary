﻿var edge = require('edge');
var path = require('path');
var dotNetFunction = edge.func({ assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll', methodName: 'GetTop100' });
function myFunction3() {
    dotNetFunction(10, function (error, result) {
        if (error) alert(error.message);
        alert('Reply:' + result);
    });
}

var top100Function = edge.func(
{
    assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll',
    typeName:'HiSum.Reader',
    methodName: 'GetFrontPage'
});

function GetTop100() {
    top100Function(100, function(error, result) {
        if (error) alert(error.message);
        result.forEach(function(entry)
        {
        console.log(entry);
        });
        
    });
}