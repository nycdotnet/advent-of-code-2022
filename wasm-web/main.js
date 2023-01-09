// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './dotnet.js'

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(true)
    .withApplicationArgumentsFromQuery()
    .create();

setModuleImports('main.js', {
    window: {
        location: {
            href: () => globalThis.window.location.href
        }
    }
});

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);
//const text = exports.MyClass.Greeting();
//console.log(text);

window.day1Data = exports.AdventOfCode.GetDay1Data();

document.getElementById('day1input').value = exports.AdventOfCode.GetDay1Data();
document.getElementById('solve-day-1-part-1').addEventListener('click', () => {
    const input = document.getElementById('day1input').value;
    console.log(`Attempting to solve via ${input}.`);
    const answer = exports.AdventOfCode.SolveDay1Part1(input);
    console.log(`got answer ${answer}.`)

    document.getElementById('day-1-part-1-answer').innerHTML = `Answer is ${answer}`;
});
await dotnet.run();