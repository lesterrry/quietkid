# quietkid
![Platform](https://img.shields.io/badge/platform-Windows-blue)
## ‼️ DISCLAIMER
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.\
__THIS SOFTWARE IS A PROOF OF CONCEPT AND IS INTENDED FOR EDUCATIONAL PURPOSES ONLY.__
## What is it
Quietkid allows you to control remote PCs via Telegram bot. It provides following features:
- Hidden background activity
- Pressing keys & pasting text ([guidelines](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?redirectedfrom=MSDN&view=net-5.0))
- Launching processes & shell scripts
- Looking through files, downloading them straight into the Telegram chat
- Opening websites
- Dumping built-in ASCII graphics
- Playing built-in sounds
- Keyboard activity logging & sending
- Self-deletion on command
### Commands
- `/plain` - place text once
- `/rep` - place text multiple times
- `/tab` - send `TAB` click
- `/alttab` - send `ALT` + `TAB` click
- `/wipe` select all text in current field & delete it
- `/close` - close current window
- `/proc` - enter Process mode, after which all unknown commands will be treated as processes to launch (e.g. `notepad`)
- `/shell` - enter Shell mode, after which all unknown commands will be treated as shell commands to execute
- `/finder` - enter Finder mode (additional commands afterwards: `B` to go back, `S` to set drive, `U` to go to user home dir)
- `/webload` - enter Webload mode, after which all unknown commands will be treated as links to go to
- `/ascii` - enter Ascii mode, after which all unknown commands will be treated as known Ascii presets to print
- `/sound` - enter Sound mode, after which all unknown commands will be treated as known sound presets to play
- `/volup` - set system volume to max
- `/hook` - toggle keyboard actions recording
- `/hookdump` - print all recorded keyboard actions
- `/basic` - print basic info about the software
- `/kill` - Exit program
- `/destruct` - Exit & self-delete\
Multiple commands may be executed one by one if divided by semicolon (e.g. `/proc;notepad;/ascii;shrek` will open notepad and print beautiful shrek in it)
## How to use
1. Modify 'Secure.cs' contents
2. Build .NET app
