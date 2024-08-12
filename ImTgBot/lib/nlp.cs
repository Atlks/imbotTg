global using static prjx.libx.nlp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Porter2Stemmer;
using prjx.libx;

//using PorterStemmerAlgorithm;
namespace prjx.libx
{
    internal class nlp
    {


        private static void mergeTransWdlib()
        {
            string inif = $"{prjdir}/cfgNlp/word5000.ini";

            string jsonf = $"{prjdir}/cfgNlp/wd.engCns5k.json";

            inif = $"{prjdir}/cfgNlp/wdlib.enNcn5k.delKenLenLess3Fnl.ini";
            jsonf = "C:\\Users\\Administrator\\GolandProjects\\awesomeProject\\wd.tmp3k.json";
            SortedList<string, string> hs1 = LoadHashtabEsFrmIni(inif);
            SortedList<string, string> hs2 = LdHstbEsFrmJsonFile(jsonf);
            SortedList<string, string> hs4 = MergeSortedLists(hs1, hs2);
            //  CleanupSortedListKeysLenLessthan3(hs4);
            WriteAllText("wdlib.enNcn5k.delKenLenLess3.json", hs4);
            CleanupSortedListValuesStartWzAlphbt(hs4);
            WriteAllText("wdlib.enNcn5k.delKenLenLess3Fnl.json", hs4);
            ormIni.saveIni(hs4, "wdlib.enNcn5k.v2.ini");
        }
        //static string GetStem(string word)
        //{
        //    PorterStemmer stemmer = new PorterStemmer();
        //    return stemmer.StemWord(word);
        //}

        public   static string GetRoot(string word)
        {
            // 创建一个 Porter2Stemmer 实例
            var stemmer = new EnglishPorter2Stemmer();

           //  stemme
            // 获取单词的原始形式（词干）
            var stem = stemmer.Stem(word) ;
            return stem.Value.ToString();
          //  stemmer.
           // return stemmer.FindRoot(word);
        }
    }
}
