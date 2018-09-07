Nano RPC Checker
==============

Checks for open RPC port on peers of your Nano node.

Manual:

1. Get peers list from your node: `curl -d '{"action":"peers"}' [::1]:7076`, save to peers.json
2. `dotnet run`

Program will parse peers list and try to connect to (port+1) and ask for version. Non-empty response will be written to console.


Requirements:

* [.NET Core 2.1](https://dot.net/)