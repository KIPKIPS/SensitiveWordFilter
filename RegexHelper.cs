using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
namespace Lens.Gameplay.UI.UITools {
    public static class RegexHelper {
        //存储正则匹配数据
        public static List<PatternInfo> patternList = new List<PatternInfo>();
        public struct PatternInfo {
            public string tips;
            public string pattern;
            public PatternInfo(string _pattern, string _tips) {
                pattern = _pattern;
                tips = _tips;
            }
        }
        //插入正则串,需要进行转换
        public static void InsertPatternData(string pattern, string tips) {
            string patternConv = @pattern;
            // /pP => /p{P}
            MatchCollection mc1 = Regex.Matches(@pattern, @"/pP");
            if (mc1.Count > 0) {
                patternConv = Regex.Replace(patternConv, "/pP", "/p{P}");
            }
            
            // /x{15F3E} => /x15F3E
            MatchCollection mc2 = Regex.Matches(@pattern, @"/x\{[a-zA-Z0-9]*\}");
            if (mc2.Count > 0) {
                foreach (Match m in mc2) {
                    string t = m.ToString();
                    string p = m.ToString();
                    if (Regex.Matches(t, @"/x\{[0-9]*\}").Count > 0) {
                        t = Regex.Replace(t, @"{", "");
                        t = Regex.Replace(t, @"}", "");
                        p = Regex.Replace(p, @"{", "\\{");
                        p = Regex.Replace(p, @"}", "\\}");
                    } else {
                        t = Regex.Replace(t, @"{", "");
                        t = Regex.Replace(t, @"}", "");
                    }
                    patternConv = Regex.Replace(patternConv, @p, t);
                }
            }

            //Console.WriteLine(patternConv); Console.WriteLine("--------------");
            // /wsdbx.*+? => \wsdbx.*+?
            MatchCollection mc3 = Regex.Matches(@pattern, @"/[w|W|s|S|d|D|b|B|x|X|p|P|.|*|+|?]");
            if (mc3.Count > 0) {
                foreach (Match m in mc3) {
                    string ms = m.ToString();
                    string rp = m.ToString();
                    rp = rp.Replace("/", "");
                    if (Regex.Matches(ms[1].ToString(), "[.|*|+|?]").Count > 0) {
                        ms = "/\\" + ms[1];
                    }
                    patternConv = Regex.Replace(patternConv, @ms, @"\" + rp);
                }
            }
            Console.WriteLine(patternConv);
            patternList.Add(new PatternInfo(patternConv,tips));
        }

        public static string CheckFilter(string str,bool replace = false,string replaceChar = "*") {
            string res = str + "_nil_true";
            PatternInfo info;
            for (int i = 0; i < patternList.Count; i++) {
                info = patternList[i];
                MatchCollection mc = Regex.Matches(str, @info.pattern);
                if (mc.Count > 0) {
                    if (replace) {
                        foreach (Match m in mc) {
                            string t = "";
                            for (int j = 0; j < m.ToString().Length; j++) {
                                t += replaceChar;
                            }
                            str = Regex.Replace(str, @m.ToString(), t);
                        }
                    }
                    return str+"_"+info.tips+"_false";
                }
            }
            return res;
        }
    }
}