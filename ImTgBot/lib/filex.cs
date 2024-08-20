global using static prjx.libx.filex;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static prjx.libx.bscEncdCls;
// prj202405.lib.filex
namespace prjx.libx
{
    public class filex
    {
        public static void AppendTozip(string zipFilePath, string fileName, string value)
        {
            // 追加到zip文件中
            using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    // 检查是否已存在相同文件名的条目
                    //def overwt
                    // 将ini文件添加到zip中
                    ZipArchiveEntry entry = archive.CreateEntry(fileName);
                    using (StreamWriter writer = new StreamWriter(entry.Open()))
                    {

                        writer.Write(value);
                    }
                }
            }
        }

        public static HashSet<string> ProcessFilesDep(string directoryPath)
        {
            var resultSet = new HashSet<string>();

            // Get all .cs files in the directory and subdirectories
            var csFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

            foreach (var file in csFiles)
            {
                // Read all lines from the current file
                var lines = System.IO.File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    // Use regular expression to find characters between dot and left parenthesis
                    var matches = Regex.Matches(line, @"\.(.*?)\(");
                    foreach (Match match in matches)
                    {
                        if (match.Groups.Count > 1)
                        {
                            resultSet.Add(match.Groups[1].Value);
                        }
                    }
                }
            }

            return resultSet;
        }


        public static void WriteFileWithTimestamp(string directoryPath, string content)
        {
            // 确保目录存在
            Directory.CreateDirectory(directoryPath);

            // 生成文件名，格式为 "yyyyMMddHHmmssfff.txt"
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".txt";
            string filePath = Path.Combine(directoryPath, fileName);

            // 写入文件
            System.IO.File.WriteAllText(filePath, content);

            // 处理文件数量
            ManageFileCount(directoryPath,50);
        }

        private static void ManageFileCount(string directoryPath,int MaxFiles)
        {
            // 获取所有文件路径
            var files = Directory.GetFiles(directoryPath);

            // 如果文件数量超过限制
            if (files.Length > MaxFiles)
            {
                // 按创建时间排序文件，从最早的文件开始删除
                var filesToDelete = files
                    .Select(file => new FileInfo(file))
                    .OrderBy(fileInfo => fileInfo.CreationTime)
                    .Take(files.Length - MaxFiles);

                foreach (var file in filesToDelete)
                {
                    file.Delete();
                }
            }
        }

        public static string[] ReadFileAndRemoveEmptyLines(string filePath)
        {
            // 读取文件所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            // 使用 LINQ 过滤掉空行
            string[] nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            return nonEmptyLines;
        }
    
        public static void MoveFileToDirectory(string sourceFilePath, string destinationDirectory)
        {
            try
            {
                // 检查目标文件夹是否存在，如果不存在则创建
                if (!Directory.Exists(destinationDirectory))
                {
                  //  mkdir_forFile()
                    Directory.CreateDirectory(destinationDirectory);
                }

                // 获取源文件的文件名
                string fileName = Path.GetFileName(sourceFilePath);

                // 构造目标文件的完整路径
                string destinationFilePath = Path.Combine(destinationDirectory, fileName);

                // 移动文件
                System.IO.File.Move(sourceFilePath, destinationFilePath);

                ConsoleWriteLine($"File moved to: {destinationFilePath}");
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"An error occurred: {ex.Message}");
            }
        }
        //public static string fl_ReadAllText(string f)
        //{
        //    return System.IO.File.ReadAllText(f);
        //}

        public static SortedList ReadJsonToSortedList(string filePath)
        {
            SortedList sortedList = new SortedList();

            try
            {
                // 读取JSON文件内容
                string jsonContent = System.IO.File.ReadAllText(filePath);

                // 解析JSON对象
                JObject jsonObject = JObject.Parse(jsonContent);

                // 遍历JSON对象并将键值对添加到SortedList
                foreach (var property in jsonObject.Properties())
                {
                    sortedList.Add(property.Name, property.Value.ToString());
                }
            }
            catch (Exception ex)
            {
               Print($"Error: {ex.Message}");
            }

            return sortedList;
        }

        public static void Copy2024(string sourceFilePath, string destination_newFileName)
        {
            filex.Mkdir4File(destination_newFileName);

            // 构造目标文件的完整路径
            // string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, newFileName);

            // 复制并重命名文件
            System.IO.File.Copy(sourceFilePath, destination_newFileName, true);
        }


        public static string InsertCurrentTimeToFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            // 获取文件名和扩展名
            string nameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string extension = System.IO.Path.GetExtension(fileName);

            // 获取当前时间并格式化
            string formattedTime = DateTime.Now.ToString("yyMMdd_HHmmss_fff");

            // 构造新的文件名
            string newFileName = $"{nameWithoutExtension}_{formattedTime}{extension}";

            return newFileName;
        }

        public static void CopyAndRenameFile(string sourceFilePath, string destinationFolderPath, string newFileName)
        {
            // 如果目标文件夹不存在，则创建它
            Directory.CreateDirectory(destinationFolderPath);

            // 构造目标文件的完整路径
            string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, newFileName);

            // 复制并重命名文件
            System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
        }

        public static void CopyFileToFolder(string sourceFilePath, string targetFolderPath)
        {
            // 检查目标文件夹是否存在，如果不存在则创建
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
               Print($"Created directory: {targetFolderPath}");
            }

            // 获取源文件名
            string fileName = System.IO.Path.GetFileName(sourceFilePath);

            // 构建目标文件路径
            string destinationFilePath = System.IO.Path.Combine(targetFolderPath, fileName);

            // 复制文件
            System.IO.File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
        }

        public static string GetBaseFileName(string filePath)
        {
            // 获取文件名（带扩展名）
            string fileName = Path.GetFileName(filePath);

            // 去掉扩展名，获取基本名字
            string baseFileName = Path.GetFileNameWithoutExtension(fileName);

            return baseFileName;
        }
        public static string GetFilePathsCommaSeparated(string directoryPath)
        {
            // 获取目录下的所有文件路径
            string[] filePaths = Directory.GetFiles(directoryPath);

            // 将文件路径数组转换为逗号分割的字符串
            string result = string.Join(",", filePaths);

            return result;
        }
     

        /// <summary>
        /// 创建一个新的目录，如果目录已存在，则不执行任何操作。
        /// </summary>
        /// <param name="directoryPath">要创建的目录的路径</param>
        public static void Mkdir(string directoryPath)
        {
            try
            {
                // 使用 Directory.CreateDirectory 创建目录
                Directory.CreateDirectory(directoryPath);
            }
            catch (Exception ex)
            {
               Print($"An error occurred: {ex.Message}");
            }
        }

        public static void Mkdir4File(string filePath )
        {
            // 获取文件目录
            string dir = System.IO.Path.GetDirectoryName(filePath);


            //rltv path
            if (dir == "")
                return;
            // 检查目录是否存在，如果不存在，则创建目录
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
          //  File.WriteAllText(filePath, txt);


        }
        public static string GetAbsolutePath(string relativePath)
        {
            // 获取当前工作目录
            string currentDirectory = Environment.CurrentDirectory;

            // 将相对路径转换为绝对路径
            string absolutePath = Path.GetFullPath(Path.Combine(currentDirectory, relativePath));

            return absolutePath;
        }
        /// <summary>
        /// 读取指定文件的内容，并返回字符串形式的内容。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件内容的字符串</returns>
        public static string file_get_contents(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    return System.IO.File.ReadAllText(filePath);
                }
                else
                {
                    throw new FileNotFoundException("The specified file does not exist.", filePath);
                }
            }
            catch (Exception ex)
            {
               Print($"An error occurred: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Ensures that the directory for the specified file path exists. If it does not exist, create it.
        /// </summary>
        /// <param name="filePath">The file path for which to ensure the directory exists.</param>
        public static void EnsureDirectoryExists(string filePath)
        {
            // Get the directory path from the file path
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                // Create the directory and any parent directories
                Directory.CreateDirectory(directoryPath);
            }
        }
        public static void wrtLgTypeDate(string logdir, object o)
        {
            // 创建目录
            Directory.CreateDirectory(logdir);
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"{logdir}/{timestamp}.json";
            file_put_contents(fileName, json_encode(o), false);

        }

        public static string FilenameBydtme( )
        {
            // 创建目录
          //  Directory.CreateDirectory(logdir);
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            return timestamp;

        }
        /// <summary>
        /// Removes consecutive extra newline characters from the input string.
        /// </summary>
        /// <param name="input">The input string with potential extra newlines.</param>
        /// <returns>A string with extra newlines removed.</returns>

        /// <summary>
        /// 将指定内容写入到文件中，如果文件不存在则创建文件。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">要写入的内容</param>
        /// <param name="append">是否追加内容到文件末尾，默认为 false</param>
        public static void file_put_contents(string filePath, string content, bool append = false)
        {
            EnsureDirectoryExists(filePath);
            try
            {
                // 根据 append 参数确定写入模式
                FileMode mode = append ? FileMode.Append : FileMode.Create;

                // 使用 FileStream 写入内容到文件
                using (FileStream fs = new FileStream(filePath, mode, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(content);
                }
            }
            catch (Exception ex)
            {
               Print($"An error occurred: {ex.Message}");
            }
        }

        public static HashSet<string> ReadWordsFromFile(string filePath)
        {
            var words = new HashSet<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 拆分行中的单词，按空格和回车拆分
                        var splitWords = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in splitWords)
                        {
                          var  word1= word.Trim();
                            words.Add(word1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               Print("Error reading file: " + ex.Message);
            }

            return words;
        }
        public static ArrayList rdWdsFromFileSplitComma(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);


            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        // { CallbackData = $"Merchant?id={guid}" }

                    }

                }


            }
            return wordList;
        }
     public   static HashSet<string> ReadFileToHashSet2024(string filePath)
        {
            HashSet<string> lines = new HashSet<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
        public static ArrayList rdWdsFromFile(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                     var   word1 = HttpUtility.UrlDecode(word);
                        wordList.Add(word1);
                        // { CallbackData = $"Merchant?id={guid}" }
                      
                    }

                }

                
            }
            return wordList;
        }

        public static InlineKeyboardButton[][] wdsFromFileRendrToBtnmenu(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<InlineKeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<InlineKeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        // { CallbackData = $"Merchant?id={guid}" }
                        InlineKeyboardButton btn = new InlineKeyboardButton(word) { CallbackData = $"cmd={word}" };
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }

        public static KeyboardButton[][] wdsFromFileRendrToTgBtmBtnmenuBycomma(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<KeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        KeyboardButton btn = new KeyboardButton(word);
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }

        public static KeyboardButton[][] wdsFromFileRendrToTgBtmBtnmenu(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<KeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        KeyboardButton btn = new KeyboardButton(word);
                        lineBtnArr.Add(btn);
                    }
                       
                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }
        static string GetParentDirectory(string filePath)
        {
            // 使用Path.GetDirectoryName方法获取文件路径的上一级目录
            // Path.GetDirectoryName方法返回目录路径字符串，如果路径无效，则返回null。
            string parentDirectory = Path.GetDirectoryName(filePath);

            if (parentDirectory != null)
            {
                // 使用Path.GetFileName获取上一级目录的名称
                string parentDirectoryName = Path.GetFileName(parentDirectory);
                return parentDirectoryName;
            }
            else
            {
                // 处理无效路径的情况
                return "无效路径";
            }
        }
        public   static HashSet<string> GetNotepadFilePaths(string rootDirectory, int depth)
        {
            HashSet<string> notepadFilePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            GetNotepadFilePathsRecursive(rootDirectory, depth, notepadFilePaths);
            return notepadFilePaths;
        }
        public static void GetNotepadFilePathsRecursive(string currentDirectory, int remainingDepth, HashSet<string> notepadFilePaths)
        {
            if (remainingDepth == 0)
                return;

            try
            {
                // 获取当前文件夹中的所有文件
                string[] files = Directory.GetFiles(currentDirectory, "*.txt");

                // 将记事本文件的路径添加到HashSet中
                foreach (string file in files)
                {
                    notepadFilePaths.Add(file);
                }

                // 获取当前文件夹中的所有子文件夹
                string[] subdirectories = Directory.GetDirectories(currentDirectory);

                // 递归调用每个子文件夹
                foreach (string subdirectory in subdirectories)
                {
                    GetNotepadFilePathsRecursive(subdirectory, remainingDepth - 1, notepadFilePaths);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // 处理无权访问的文件夹
               Print($"Access to {currentDirectory} is denied.");
            }
            catch (Exception ex)
            {
                // 处理其他异常
               Print($"An error occurred: {ex.Message}");
            }
        }
    }
}
