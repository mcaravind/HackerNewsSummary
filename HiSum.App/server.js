var edge = require('edge');
var path = require('path');
var dotNetFunction = edge.func({ assemblyFile: path.dirname(process.execPath) + '/dll/HiSum.dll' });
