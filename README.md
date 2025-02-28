# MCProtocolTestSet - Server/Client Sample Program Communicating through Mitsubishi PLC MC Protocol

## Background
### Developing applications that read/write PLC data requires REAL PLC connected to your PC.
### You don't always have a PLC, however, whenever you need to develope one.
### Mitsubishi PLC provides you with some good Active-X control named ActUtil series.
### But when you have to develope an application that handles multiple PLCs with multi-threaded functionality, Active-X control sometimes can't be a good solution.
### Mitsubishi PLCs also support TCP/IP socket communication too. Actually, the ActUtil libraries use TCP/IP under layer.
### The name of the protocol they use is MC Protocol : Reference is here https://dl.mitsubishielectric.com/dl/fa/document/manual/plc/sh080008/sh080008ab.pdf
### I needed to write some codes for my application which should be light, simple and multithread-safe.
### But I didn't have a PLC, which means I could write my client codes but I couldn't test the codes.
### So, I made a server application which emulates PLC with MC Protocol, Yay!

## Limited Functionality
### The PLC Server supports MC3E message frame. (MC4E message frame looks to be easily added soon)
### The Server memory device is configured with only 'D' and 'W' with 20_000 words each.
### It's not enough though but ok to start to write your MC Protocol Communication Client.
### The most popular Mitsubishi PLC cpu these days is Q series and they support MC3E message frame.

# Thanks To:
## Reference1 : The server app uses TCP/IP library here : https://github.com/jchristn/SuperSimpleTcp
## Reference2 : The MC Protocol processing here : https://github.com/SecondShiftEngineer/McProtocol/blob/master/McProtocol/MCProtocol.cs
