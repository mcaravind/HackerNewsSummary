var edge = require('edge');
var path = require('path');
var dotNetFunction = edge.func({ assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll' });
function myFunction3() {
    dotNetFunction(10, function (error, result) {
        if (error) alert(error.message);
        alert('Reply:' + result);
    });
}